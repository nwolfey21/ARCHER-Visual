//++++++++++++++++++++++++++++++++++++
// HEADER
//++++++++++++++++++++++++++++++++++++
#include "Kernel.h"
//++++++++++++++++++++++++++++++++++++
// METHOD
//++++++++++++++++++++++++++++++++++++

// TALLY_PERTHREAD_BUFFER==1 is ensured for host CPU code

//------------------------------------------------------------
//------------------------------------------------------------
void TransportKernel(HostPlan& hostPlan, HostReductionPlan& hostReductionPlan)
{
//openmp region
#pragma omp parallel shared(hostPlan, hostReductionPlan)
    {
        //initialize PRN
        int threadIdx=omp_get_thread_num();
        curandStateXORWOW state=hostPlan.state[threadIdx];//copy state to register for efficiency

        //determine history per thread
        unsigned long long quotient=hostPlan.totalHistoryPerProcess/hostPlan.threadSizePerProcess;
        unsigned long long modulus=hostPlan.totalHistory%hostPlan.threadSizePerProcess;
        unsigned int historyPerThread=(threadIdx<modulus?quotient+1:quotient);

//barrier
#pragma omp barrier
        //single variable
        bool killed; //flag to reflect if the particle has already been killed
        CustomPrecision energy; //particle energy [MeV]
        CustomPrecision fictitiousCrossSection;
        CustomPrecision xi; //random number
        Position position;
        Position oldPosition;
        Direction direction;
        Index index;
        CustomPrecision s; //distance
        int universeIndex;
        int universeInnerIndex;
        int oldUniverseIndex; //universe index recorder
        int atomicNumberInnerIndex;
        CustomPrecision mu;
        CustomPrecision phi;
        CustomPrecision energyTemp;
        CustomPrecision oldEnergy;
        CustomPrecision cosAngle;
        CustomPrecision sinAngle;
        CustomPrecision zTranslation;
        int tubeIndex;
        bool isInsideBowtie;
        bool isInsidePhantom;
        CustomPrecision universeMacroCrossSectionTT;
        CrossSection materialAtomMacroCrossSection;
        int voxelDoseListIndex;
        int isCurrentParticleFluorescence=0;
        int isNextParticleFluorescence=0;

        //array of variables
        HostKernelPlan hostKernelPlan;

        AllocateHostKernelPlan(hostPlan, hostKernelPlan);
        //per-thread
        ResetHostKernelPlanPerThread(hostPlan, hostKernelPlan);

FILE *positionFile;
        int ii=0;
        while(ii<historyPerThread)
        {
            ++ii;
			if(ii<10)
			{
				if(ii>1)
				{
//					fclose(positionFile);
				}
				//My OutputFile
				char filename[100];
				sprintf(filename,"particle%dPosition.dat",ii);
				if((positionFile = fopen(filename,"a")) == NULL) 
				{
						fprintf(stdout,"Couldn't open 'positionFile.dat' output file\n");
				}
			}

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //initialize particle attributes
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ResetHostKernelPlanPerParticle(hostPlan, hostKernelPlan);
            killed=false;
            isInsideBowtie=false;
            isInsidePhantom=false;
            oldUniverseIndex=-1;
            oldEnergy=-1.;

            //initialize energy
            if(isCurrentParticleFluorescence)
            {
            }
            else
            {
#if APPLICATION()==EXTERNAL_DOSE()
                energy=300e-3;
#else
                do
                {
                    energy=SampleSourceEnergy(hostPlan, state);
                }
                while(energy<1.00001e-3);
#endif
            }

            //update fictitious cross-section
            UpdateFictitiousCrossSection(energy,
                                         fictitiousCrossSection,
                                         hostPlan,
                                         hostKernelPlan);

            //initialize position and direction
            if(isCurrentParticleFluorescence)
            {
            }
            else
            {
#if APPLICATION()==EXTERNAL_DOSE()
                //sample position
                position.x=hostPlan.voxelSizeX*hostPlan.dimX*GenerateRandomNumber(state)-hostPlan.voxelSizeX*hostPlan.dimX/2.;
                position.y=hostPlan.voxelSizeY*hostPlan.dimY/2.+10.;
                position.z=hostPlan.voxelSizeZ*hostPlan.dimZ*GenerateRandomNumber(state)-hostPlan.voxelSizeZ*hostPlan.dimZ/2.;

                //sample direction
                direction.x=0;
                direction.y=-1.;
                direction.z=0;
#else
                //example: tcm
                if (hostPlan.scannerMovementSimulationMode==perProjection)
                {
                    tubeIndex=hostPlan.externalInstanceIndex;
                    cosAngle=cos(tubeIndex*hostPlan.rotationAngleDelta+hostPlan.initialRotationAngle);
                    sinAngle=sin(tubeIndex*hostPlan.rotationAngleDelta+hostPlan.initialRotationAngle);
                    zTranslation=tubeIndex*hostPlan.zTranslationDelta+hostPlan.zInitialTranslationBowtie;
                }
                //example: axial scan, slice by slice
                else if (hostPlan.scannerMovementSimulationMode==perRotation)
                {
                    tubeIndex=SampleTubeIndex(0, hostPlan.projectionSizePerRotation, state);
                    cosAngle=cos(tubeIndex*hostPlan.rotationAngleDelta+hostPlan.initialRotationAngle);
                    sinAngle=sin(tubeIndex*hostPlan.rotationAngleDelta+hostPlan.initialRotationAngle);
                    zTranslation=hostPlan.externalInstanceIndex*hostPlan.zTranslationDelta+hostPlan.zInitialTranslationBowtie;
                }
                //example: helical scan, one instance
                else if (hostPlan.scannerMovementSimulationMode==randomSelection)
                {
                    tubeIndex=SampleTubeIndex(0, hostPlan.totalProjectionSize, state); //a sequence of projections
                    cosAngle=cos(tubeIndex*hostPlan.rotationAngleDelta+hostPlan.initialRotationAngle);
                    sinAngle=sin(tubeIndex*hostPlan.rotationAngleDelta+hostPlan.initialRotationAngle);
                    zTranslation=tubeIndex*hostPlan.zTranslationDelta+hostPlan.zInitialTranslationBowtie;
                }

                SampleSourcePositionDirection(cosAngle, sinAngle, zTranslation, position, direction, hostPlan, state);
#endif
			}
			
fprintf(positionFile,"%f %f %f\n",position.x,position.y,position.z);

            do //implicit loop for photon transport, or rather, scattering
            {
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //sample distance
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //sample distance
                xi=GenerateRandomNumber(state);
                s=-log(xi)/fictitiousCrossSection;
                //update position
                oldPosition=position;
                UpdatePosition(position,direction, s);
fprintf(positionFile,"%f %f %f\n",position.x,position.y,position.z);
                //kill the particle if it is out of ROI
                if(IsInsideROI(position, hostPlan)==false)
                {
                    killed=true;
                    break;
                }

                //update cross-section
#if APPLICATION()==EXTERNAL_DOSE()
                isInsideBowtie=0;
#else
                IsInsideBowtie(isInsideBowtie, cosAngle, sinAngle, zTranslation, position, hostPlan);
#endif
                IsInsidePhantom(isInsidePhantom, position, hostPlan);
//printf("particle:%d position(%f,%f,%f) xi:%f s:%f fictitiousCrossSection:%f direction(%f,%f,%f) Bowtie:%d Phantom:%d\n",ii,position.x,position.y,position.z,xi,s,fictitiousCrossSection,direction.x,direction.y,direction.z,isInsideBowtie,isInsidePhantom);

                if(isInsideBowtie==true)
                {
                    universeIndex=hostPlan.universeBowtie;
                }
                //prioritize bowtie if it intersects with the phantom where both isInsideBowtie and isInsidePhantom are true
                else if(isInsidePhantom==true && isInsideBowtie==false)
                {
                    //update index
                    UpdateIndex(index, position, hostPlan);
                    //REFER TO README.TXT
                    //TEMP SOLUTION
                    if(index.x>=hostPlan.dimX)
                    {
                        index.x=hostPlan.dimX-1;
                    }
                    if(index.y>=hostPlan.dimY)
                    {
                        index.y=hostPlan.dimY-1;
                    }
                    if(index.z>=hostPlan.dimZ)
                    {
                        index.z=hostPlan.dimZ-1;
                    }
                    voxelDoseListIndex=hostPlan.dimX*hostPlan.dimY*index.z+hostPlan.dimX*index.y+index.x;
                    universeIndex=hostPlan.phantom[voxelDoseListIndex];
                }
                else if(isInsidePhantom==false && isInsideBowtie==false)
                {
                    universeIndex=hostPlan.universeAir;
                }

                UpdateCrossSection(hostPlan,
                                   hostKernelPlan,
                                   universeIndex,
                                   universeInnerIndex,
                                   oldUniverseIndex,
                                   energy,
                                   oldEnergy,
                                   hostPlan.universeSize,
                                   hostPlan.universeList,
                                   universeMacroCrossSectionTT);

                //if the collision is virtual, continue to sample the distance
                if(GenerateRandomNumber(state)>universeMacroCrossSectionTT/fictitiousCrossSection)
                {
//					printf("particle:%d virtual collision\n",ii);
                    continue;
                }

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //sample collision atom
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if AccurateCrossSection
                SampleCollisionAtom(hostKernelPlan,
                                    materialAtomMacroCrossSection,
                                    universeInnerIndex,
                                    atomicNumberInnerIndex,
                                    hostPlan,
                                    state);
#endif

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //score the tally
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                if (hostPlan.tallyType==CollisionEstimator)
                {
#if AccurateCrossSection
                    TallyDoseCollisionEstimator(universeIndex,
                                                atomicNumberInnerIndex,
                                                energy,
                                                hostPlan,
                                                hostKernelPlan);
#else
                    TallyDoseCollisionEstimator(universeIndex,
                                                universeInnerIndex,
                                                energy,
                                                hostPlan,
                                                hostKernelPlan);
#endif
                }

                if(hostPlan.isFluxToDoseTally)
                {
                    TallyFluxToDose(universeIndex,
                                    universeMacroCrossSectionTT,
                                    energy,
                                    hostPlan,
                                    hostKernelPlan);
                }

#if !AccurateCrossSection
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //sample interaction type
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                materialAtomMacroCrossSection.PE=InterpolateDataType2(hostPlan.crossSectionListPE, universeInnerIndex*hostPlan.energyListSize, hostPlan.energyListSize, energy, hostPlan);
                materialAtomMacroCrossSection.CS=InterpolateDataType2(hostPlan.crossSectionListCS, universeInnerIndex*hostPlan.energyListSize, hostPlan.energyListSize, energy, hostPlan);
                materialAtomMacroCrossSection.TT=universeMacroCrossSectionTT;
#endif

                xi=GenerateRandomNumber(state);
//printf("xi:%f materialAtomMacroCrossSection.PE:%f materialAtomMacroCrossSection.TT:%f materialAtomMacroCrossSection.CS:%f\n",xi,materialAtomMacroCrossSection.PE,materialAtomMacroCrossSection.TT,materialAtomMacroCrossSection.CS);
                if(xi<materialAtomMacroCrossSection.PE/materialAtomMacroCrossSection.TT)
                {
//					printf("particle:%d photoelectric effect\n",ii);
                    //Photoelectric effect

                    // primary fluorescence subroutine
                    // if fluorescence is considered
                    // if the photon collides with thyroid
                    // if the photon collides with iodine
                    if(hostPlan.isFluorescence &&
                       hostPlan.universeThyroid==universeInnerIndex &&
                       GenerateRandomNumber(state)<InterpolateDataType3(hostPlan.iodineCollisionProbabilityList,
                                                                        0,
                                                                        hostPlan.iodineCollisionProbabilityListSize,
                                                                        energy,
                                                                        hostPlan))
                    {
//printf("particle:%d hopefully does not print here in flourescence effect\n",ii);
                        CustomPrecision energyRecord=energy;
                        //EL<=E<EK
                        if(energy>=hostPlan.iodineLEdgeEnergy && energy<hostPlan.iodineKEdgeEnergy)
                        {
                            xi=GenerateRandomNumber(state);
                            //if fluorescence emission occurs as opposed to Augur electron or self-absorption
                            if(xi<hostPlan.iodineYieldList[1]/hostPlan.iodineLRelativeProbability)
                            {
                                isNextParticleFluorescence=1;
                                energyTemp=hostPlan.iodineLEdgeEnergy;
                                energy=hostPlan.iodineFluorescenceEnergyList[1];
                            }

                        }
                        //E>=EK
                        else if(energy>=hostPlan.iodineKEdgeEnergy)
                        {
                            xi=GenerateRandomNumber(state);
                            //if fluorescence emission occurs as opposed to Augur electron or self-absorption
                            if(xi<hostPlan.iodineYieldList[hostPlan.iodineFluorescenceEnergyListSize-1]/hostPlan.iodineKRelativeProbability)
                            {
                                int lowerBound=0;
                                for(lowerBound=0; lowerBound<hostPlan.iodineFluorescenceEnergyListSize-1; ++lowerBound)
                                {
                                    if(xi< hostPlan.iodineYieldList[lowerBound+1]/hostPlan.iodineKRelativeProbability &&
                                       xi>=hostPlan.iodineYieldList[lowerBound]  /hostPlan.iodineKRelativeProbability)
                                    {
                                        break;
                                    }
                                }

                                isNextParticleFluorescence=1;
                                energyTemp=(lowerBound==0)?hostPlan.iodineLEdgeEnergy:hostPlan.iodineKEdgeEnergy;
                                energy=hostPlan.iodineFluorescenceEnergyList[lowerBound+1];

                                //secondary fluorescence for Kalpha1 and Kalpha2
                                if(lowerBound==1      // Kalpha1
                                   || lowerBound==2)  // Kalpha2
                                {
                                    xi=GenerateRandomNumber(state);
                                    if(xi<=hostPlan.secondaryFluorescenceProbability)
                                    {
                                        //bank the fluorescence
                                        TallyDoseRealistic(universeIndex,
                                                           hostPlan.secondaryFluorescenceEnergy,
                                                           hostPlan,
                                                           hostKernelPlan);
                                    }
                                }
                            }
                        }

                        if(1==isNextParticleFluorescence)
                        {
                            //position
                            //direction
                            mu=2.*GenerateRandomNumber(state)-1.;
                            phi=AzimuthalAngle(state);
                            UpdateDirection(direction, mu, phi);
                            ++historyPerThread;

                            //deposit energy
                            //electron kinetic energy (E-Eb) has been considered in the previous average heating number
                            //all aftermath (fluorescence, Augur, self-absorption): Eb
                            //now we assume the local deposited energy to be: Eb-F
                            //TallyDoseRealistic(universeIndex,
                            //                   hostPlan.iodineKEdgeEnergy-energy,
                            //                   hostPlan,
                            //                   hostKernelPlan);
                        }//end if fluorescence occurs
                    }//end if collision with iodine

                    //tally
                    if (hostPlan.tallyType==Realistic)
                    {
                        TallyDoseRealistic(universeIndex,
                                           energy,
                                           hostPlan,
                                           hostKernelPlan);
                    }
//					printf("particle:%d killed\n",ii);
                    break; //kill the current particle, terminate do loop, start a new particle
                }
                else if(xi>=materialAtomMacroCrossSection.PE/materialAtomMacroCrossSection.TT\
                        && xi<(materialAtomMacroCrossSection.PE+materialAtomMacroCrossSection.CS)/materialAtomMacroCrossSection.TT)
                {
//					printf("compton Scattering\n");
                    //Compton scattering occurs
                    //back up energyIn
                    energyTemp=energy;
                    //sample polar angle cosine using Kahn sampling method
                    //sample energyOut by Doppler broadening
                    ComptonScatteringPolarAngleCosine(mu,
                                                      energy,
                                                      atomicNumberInnerIndex,
                                                      hostPlan,
                                                      universeInnerIndex,
                                                      state);

                    //tally
                    if (hostPlan.tallyType==Realistic)
                    {
                        TallyDoseRealistic(universeIndex,
                                           energyTemp-energy,
                                           hostPlan,
                                           hostKernelPlan);
                    }
                    //energy cutoff. The remaining energy is also deposited. So the total energy deposited is oldEnergy.
                    if (energy<=1.00001e-3)
                    {
                        TallyDoseRealistic(universeIndex,
                                           energy,
                                           hostPlan,
                                           hostKernelPlan);
                        break;
                    }

                    //update crossSection
                    UpdateFictitiousCrossSection(energy,
                                                 fictitiousCrossSection,
                                                 hostPlan,
                                                 hostKernelPlan);

                    //sample azimuthal angle
                    phi=AzimuthalAngle(state);
                    //update flight direction
                    UpdateDirection(direction,mu,phi);
                }
                else
                {
//					printf("rayleigh scattering\n");
                    //Rayleigh scattering
                    //sample polar angle cosine
                    mu=RayleighScatteringPolarAngleCosine(energy, atomicNumberInnerIndex, hostPlan, universeInnerIndex, state);
                    //sample azimuthal angle
                    phi=AzimuthalAngle(state);
                    //update flight direction
                    UpdateDirection(direction,mu,phi);
                }
            }while(true);

            if(hostPlan.isFluorescence)
            {
                //update fluorescence status
                isCurrentParticleFluorescence=isNextParticleFluorescence;
                isNextParticleFluorescence=0;
            }

fclose(positionFile);

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //add local result to global
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //per-thread
            UpdatePerParticleDose(hostPlan, hostKernelPlan);
        }//end for, do-while

        //per-thread
        BufferDose(hostPlan,
                   hostKernelPlan,
                   hostReductionPlan,
                   threadIdx);

        //free memory
        FreeHostKernelPlan(hostPlan, hostKernelPlan);
    }//end parallel region
}
//------------------------------------------------------------
//------------------------------------------------------------
void AllocateHostKernelPlan(const HostPlan& hostPlan, HostKernelPlan& hostKernelPlan)
{
    if(!hostPlan.isPretabulateFictitious)
    {
        hostKernelPlan.universeMacroCrossSectionTTList=(CustomPrecision*)malloc(hostPlan.universeSize*sizeof(CustomPrecision));
    }
    hostKernelPlan.tallyCombinedDoseListPerParticle=(AtomicPrecision*)malloc(hostPlan.tallyUniverseLocationListSize*sizeof(AtomicPrecision));
#if AccurateCrossSection
    hostKernelPlan.atomMicroCrossSectionByWeightList=(CrossSection*)malloc(hostPlan.atomicNumberListSize*sizeof(CrossSection));
    hostKernelPlan.materialAtomMacroCrossSectionList=(CrossSection*)malloc(hostPlan.materialLineSize*sizeof(CrossSection));
    hostKernelPlan.materialMacroCrossSectionTTList=(CustomPrecision*)malloc(hostPlan.materialSize*sizeof(CustomPrecision));
#endif
    if(hostPlan.isFluxToDoseTally)
    {
        hostKernelPlan.tallyFluxToDoseListPerParticle=(AtomicPrecision*)malloc(hostPlan.tallyFluxToDoseUniverseListSize*sizeof(AtomicPrecision));
    }

    //per-thread
    hostKernelPlan.tallyCombinedDoseListPerThread=(CustomPrecision*)malloc(hostPlan.tallyUniverseLocationListSize*sizeof(CustomPrecision));
    hostKernelPlan.tallyCombinedDoseSquareListPerThread=(CustomPrecision*)malloc(hostPlan.tallyUniverseLocationListSize*sizeof(CustomPrecision));
    if(hostPlan.isFluxToDoseTally)
    {
        hostKernelPlan.tallyFluxToDoseListPerThread=(CustomPrecision*)malloc((hostPlan.tallyFluxToDoseUniverseListSize+1)*sizeof(CustomPrecision));
        hostKernelPlan.tallyFluxToDoseSquareListPerThread=(CustomPrecision*)malloc((hostPlan.tallyFluxToDoseUniverseListSize+1)*sizeof(CustomPrecision));
    }
}
//------------------------------------------------------------
//per-thread
//------------------------------------------------------------
void ResetHostKernelPlanPerThread(const HostPlan& hostPlan, HostKernelPlan& hostKernelPlan)
{
    for(int i=0; i<hostPlan.tallyUniverseLocationListSize; ++i)
    {
        hostKernelPlan.tallyCombinedDoseListPerThread[i]=0;
        hostKernelPlan.tallyCombinedDoseSquareListPerThread[i]=0;
    }
    if(hostPlan.isFluxToDoseTally)
    {
        for(int i=0; i<=hostPlan.tallyFluxToDoseUniverseListSize; ++i)
        {
            hostKernelPlan.tallyFluxToDoseListPerThread[i]=0;
            hostKernelPlan.tallyFluxToDoseSquareListPerThread[i]=0;
        }
    }
}

//------------------------------------------------------------
//------------------------------------------------------------
void ResetHostKernelPlanPerParticle(const HostPlan& hostPlan, HostKernelPlan& hostKernelPlan)
{
    for(int i=0; i<hostPlan.tallyUniverseLocationListSize; ++i)
    {
        hostKernelPlan.tallyCombinedDoseListPerParticle[i]=0;
    }
    if(hostPlan.isFluxToDoseTally)
    {
        for(int i=0; i<hostPlan.tallyFluxToDoseUniverseListSize; ++i)
        {
            hostKernelPlan.tallyFluxToDoseListPerParticle[i]=0;
        }
    }
}
//------------------------------------------------------------
//------------------------------------------------------------
void FreeHostKernelPlan(const HostPlan& hostPlan, HostKernelPlan& hostKernelPlan)
{
    if(!hostPlan.isPretabulateFictitious)
    {
        free(hostKernelPlan.universeMacroCrossSectionTTList); hostKernelPlan.universeMacroCrossSectionTTList=NULL;
    }
    free(hostKernelPlan.tallyCombinedDoseListPerParticle); hostKernelPlan.tallyCombinedDoseListPerParticle=NULL;
#if AccurateCrossSection
    free(hostKernelPlan.atomMicroCrossSectionByWeightList); hostKernelPlan.atomMicroCrossSectionByWeightList=NULL;
    free(hostKernelPlan.materialAtomMacroCrossSectionList); hostKernelPlan.materialAtomMacroCrossSectionList=NULL;
    free(hostKernelPlan.materialMacroCrossSectionTTList); hostKernelPlan.materialMacroCrossSectionTTList=NULL;
#endif
    if(hostPlan.isFluxToDoseTally)
    {
        free(hostKernelPlan.tallyFluxToDoseListPerParticle); hostKernelPlan.tallyFluxToDoseListPerParticle=NULL;
    }

    //per-thread
    free(hostKernelPlan.tallyCombinedDoseListPerThread); hostKernelPlan.tallyCombinedDoseListPerThread=NULL;
    free(hostKernelPlan.tallyCombinedDoseSquareListPerThread); hostKernelPlan.tallyCombinedDoseSquareListPerThread=NULL;
    if(hostPlan.isFluxToDoseTally)
    {
        free(hostKernelPlan.tallyFluxToDoseListPerThread); hostKernelPlan.tallyFluxToDoseListPerThread=NULL;
        free(hostKernelPlan.tallyFluxToDoseSquareListPerThread); hostKernelPlan.tallyFluxToDoseSquareListPerThread=NULL;
    }
}
//------------------------------------------------------------
// per-thread
//------------------------------------------------------------
void UpdatePerParticleDose(const HostPlan& hostPlan, HostKernelPlan& hostKernelPlan)
{
    for(int i=0; i<hostPlan.tallyUniverseLocationListSize; ++i)
    {
        hostKernelPlan.tallyCombinedDoseListPerThread[i]+=hostKernelPlan.tallyCombinedDoseListPerParticle[i];
        hostKernelPlan.tallyCombinedDoseSquareListPerThread[i]+=hostKernelPlan.tallyCombinedDoseListPerParticle[i]*hostKernelPlan.tallyCombinedDoseListPerParticle[i];
    }

    //combine flux-to-dose result
    if(hostPlan.isFluxToDoseTally)
    {
        CustomPrecision doseSum=0;
        for(int i=0; i<hostPlan.tallyFluxToDoseUniverseListSize; ++i)
        {
            doseSum+=hostKernelPlan.tallyFluxToDoseListPerParticle[i];
            hostKernelPlan.tallyFluxToDoseListPerThread[i]+=hostKernelPlan.tallyFluxToDoseListPerParticle[i];
            hostKernelPlan.tallyFluxToDoseSquareListPerThread[i]+=hostKernelPlan.tallyFluxToDoseListPerParticle[i]*hostKernelPlan.tallyFluxToDoseListPerParticle[i]; //correct because each entry contains only 1 universe
        }
        hostKernelPlan.tallyFluxToDoseListPerThread[hostPlan.tallyFluxToDoseUniverseListSize]+=doseSum;
        hostKernelPlan.tallyFluxToDoseSquareListPerThread[hostPlan.tallyFluxToDoseUniverseListSize]+=doseSum*doseSum;
    }
}

//------------------------------------------------------------
// per-thread
//------------------------------------------------------------
void BufferDose(const HostPlan& hostPlan,
                const HostKernelPlan& hostKernelPlan,
                HostReductionPlan hostReductionPlan,
                const int threadIdx)
{
    //at the end, save the temp array by means of the persistent array
    for (int i=0; i<hostPlan.tallyUniverseLocationListSize; ++i)
    {
        //coalesce memory access
        int globalDataIdx=i*hostPlan.threadSizePerProcess+threadIdx;
        hostReductionPlan.tallyCombinedDoseList[globalDataIdx]=hostKernelPlan.tallyCombinedDoseListPerThread[i];
        hostReductionPlan.tallyCombinedDoseSquareList[globalDataIdx]=hostKernelPlan.tallyCombinedDoseSquareListPerThread[i];
    }
    if(hostPlan.isFluxToDoseTally)
    {
        for(int i=0; i<=hostPlan.tallyFluxToDoseUniverseListSize; ++i)
        {
            //coalesce memory access
            int globalDataIdx=i*hostPlan.threadSizePerProcess+threadIdx;
            hostReductionPlan.tallyFluxToDoseList[globalDataIdx]=hostKernelPlan.tallyFluxToDoseListPerThread[i];
            hostReductionPlan.tallyFluxToDoseSquareList[globalDataIdx]=hostKernelPlan.tallyFluxToDoseSquareListPerThread[i];
        }
    }
}
