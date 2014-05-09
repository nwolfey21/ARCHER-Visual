using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization; // for use of Double.Parse(String, NumberStyles)
using System.IO;


namespace ObesePhantomGenerator
{
    static class Interpolation
    {
        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        // -------------------------------------------------------------------------
        //                    same height, different BMI
        // -------------------------------------------------------------------------
        // input: bullet and hole mesh, desired BMI
        // output: new mesh
        // external functions: CalculateMeshVolume
        public static List<List<Coordinate>> SameHeight_DifferentBMI
            (List<List<Coordinate>> bulletMesh,
            List<List<Coordinate>> holeMesh,
            double desiredWeight)
        {
            // initialize variables
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();
            List<Coordinate> newVertices = new List<Coordinate>();
            double optimumFactor = 0;
            double minOptimumFactor = 0; // corresponding to bulletMesh
            double maxOptimumFactor = 1; // corresponding to holeMesh

            //import phantom infor
            List<EachMeshInfo> phantomInfo = FileIO.GetPhantomInfo();

            //import SAT and VAT mesh
            List<List<Coordinate>> SATMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\150.obj");
            List<List<Coordinate>> VATMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\151.obj");

            //correct constants
            double volume_holeMesh = CalculateMesh.CalculateVolume(holeMesh[0], holeMesh[1]); //skin
            double volume_bulletMesh = CalculateMesh.CalculateVolume(bulletMesh[0], bulletMesh[1]); //skin

            //constants for now, but for more precise modeling they should be weight- or height-depedent
            double v150_standard = CalculateMesh.CalculateVolume(SATMesh[0], SATMesh[1]); //SAT
            double v151_standard = CalculateMesh.CalculateVolume(VATMesh[0], VATMesh[1]); //VAT
            double v72_standard = FileIO.GetMeshInfo(phantomInfo, 72, 72, "volume"); //stomach wall
            double v73_standard = FileIO.GetMeshInfo(phantomInfo, 73, 73, "volume"); //stomach content
            double v95_standard = FileIO.GetMeshInfo(phantomInfo, 95, 95, "volume"); //liver
            double v76_86_standard = FileIO.GetMeshInfo(phantomInfo, 76, 86, "volume"); //large intestine
            double v75_standard = FileIO.GetMeshInfo(phantomInfo, 75, 75, "volume"); //small intestine

            //variables
            double vSAT_newMesh = 0;
            double vVAT_newMesh = 0;
            double vResidualTissue_newMesh = 0;
            double aSkin_newMesh = 0;
            double weight_newMesh = 0;
            double volume_newMesh = 0;

            // first guess of the optimumFactor
            optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;

            // update the coordinates of new vertices resulting from the optimumFactor
            newVertices = UpdateVertices.SameH_DifferentBMI(bulletMesh[0], holeMesh[0], optimumFactor);

            // update volume
            volume_newMesh = CalculateMesh.CalculateVolume(newVertices, holeMesh[1]);

            //calculate total weight
            vSAT_newMesh = 0.8 * (volume_newMesh - v150_standard);
            vVAT_newMesh = v151_standard - 0.8 * v72_standard - v73_standard - v95_standard - v76_86_standard - v75_standard;
            vResidualTissue_newMesh = volume_newMesh - vSAT_newMesh - vVAT_newMesh - FileIO.GetMeshInfo(phantomInfo, 1, 138, "volume");
            aSkin_newMesh = CalculateMesh.CalculateArea(newVertices, holeMesh[1]);
            weight_newMesh = FileIO.GetMeshInfo(phantomInfo, 1, 138, "mass")
                + GlobalConstant.densitySAT * vSAT_newMesh
                + GlobalConstant.densityVAT * vVAT_newMesh
                + GlobalConstant.densityResidualTissue * vResidualTissue_newMesh
                + GlobalConstant.densitySkin * aSkin_newMesh * GlobalConstant.thicknessSkin;
            weight_newMesh = weight_newMesh / 1000; //convert from g to kg

            // binary approximation method
            while (Math.Abs(weight_newMesh - desiredWeight) > GlobalConstant.epsilon)
            {
                //MessageBox.Show("delta = "+Math.Abs(newVolume - desiredVolume).ToString());
                if (weight_newMesh < desiredWeight)
                {
                    maxOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }
                else if (weight_newMesh > desiredWeight)
                {
                    minOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }

                // update the coordinates of new vertices resulting from the optimumFactor
                newVertices = UpdateVertices.SameH_DifferentBMI(bulletMesh[0], holeMesh[0], optimumFactor);

                // update volume
                volume_newMesh = CalculateMesh.CalculateVolume(newVertices, holeMesh[1]);

                //calculate total weight
                vSAT_newMesh = 0.8 * (volume_newMesh - v150_standard);
                vVAT_newMesh = v151_standard - 0.8 * v72_standard - v73_standard - v95_standard - v76_86_standard - v75_standard;
                vResidualTissue_newMesh = volume_newMesh - vSAT_newMesh - vVAT_newMesh - FileIO.GetMeshInfo(phantomInfo, 1, 138, "volume");
                aSkin_newMesh = CalculateMesh.CalculateArea(newVertices, holeMesh[1]);
                weight_newMesh = FileIO.GetMeshInfo(phantomInfo, 1, 138, "mass")
                    + GlobalConstant.densitySAT * vSAT_newMesh
                    + GlobalConstant.densityVAT * vVAT_newMesh
                    + GlobalConstant.densityResidualTissue * vResidualTissue_newMesh
                    + GlobalConstant.densitySkin * aSkin_newMesh * GlobalConstant.thicknessSkin;
                weight_newMesh = weight_newMesh / 1000; //convert from g to kg

            }// end while

            //add vertices
            newMesh.Add(newVertices);

            // add faces
            newMesh.Add(holeMesh[1]);

            // add vertex normals
            newMesh.Add(CalculateMesh.CalculateVertexNormals(newMesh[0], newMesh[1]));

            // display statistics
            /*MessageBox.Show("same heigh, different BMI interpolation:"
                + "\n\n" + "optimum factor = " + optimumFactor.ToString()
                + "\n" + "volume of the new mesh = " + newVolume.ToString());*/

            return newMesh;
        } // end method

        // -------------------------------------------------------------------------
        //                    same BMI, different height
        // -------------------------------------------------------------------------
        // input: bullet and hole mesh, desired BMI, tolerance epsilon
        // output: new mesh
        // external functions: CalculateMeshVolume
        public static List<List<Coordinate>> SameBMI_DifferentHeight
            (List<List<Coordinate>> mesh, double desiredWeight, double desiredHeight)
        {
            #region preprocess
            // initialize variables
            Coordinate centroid = new Coordinate();
            List<Coordinate> newVertices = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();

            // get ky
            double heightScale = desiredHeight / GlobalConstant.standardHeight;

            //calculate centroid
            centroid = CalculateMesh.CalculateCentroid(mesh[0]);

            //import phantom info
            List<EachMeshInfo> phantomInfo = FileIO.GetPhantomInfo();

            //import SAT and VAT mesh
            List<List<Coordinate>> SATMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\150.obj");
            List<List<Coordinate>> VATMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\151.obj");

            //constants for now, but for more precise modeling they should be weight- or height-dependent
            double v150_standard = CalculateMesh.CalculateVolume(SATMesh[0], SATMesh[1]); //SAT
            double v151_standard = CalculateMesh.CalculateVolume(VATMesh[0], VATMesh[1]); //VAT
            double v72_standard = FileIO.GetMeshInfo(phantomInfo, 72, 72, "volume"); //stomach wall
            double v73_standard = FileIO.GetMeshInfo(phantomInfo, 73, 73, "volume"); //stomach content
            double v76_86_standard = FileIO.GetMeshInfo(phantomInfo, 76, 86, "volume"); //large intestine
            double v75_standard = FileIO.GetMeshInfo(phantomInfo, 75, 75, "volume"); //small intestine
            double v95 = FileIO.GetMeshInfo(phantomInfo, 95, 95, "volume") * ReturnOrganVolumeScalingFactor("liver", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 95, 95, "mass"));

            //cortical bone
            double vCorticalBone = FileIO.GetMeshInfo(phantomInfo, 55, 55, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 58, 58, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 26, 26, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 39, 39, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 45, 45, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 24, 24, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 13, 13, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 16, 16, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 19, 19, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 41, 41, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 22, 22, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 28, 28, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 31, 31, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 34, 34, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 37, 37, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 47, 47, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 128, 128, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 49, 49, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 43, 43, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 51, 51, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 53, 53, "volume");
            double mCorticalBone = FileIO.GetMeshInfo(phantomInfo, 55, 55, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 58, 58, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 26, 26, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 39, 39, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 45, 45, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 24, 24, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 13, 13, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 16, 16, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 19, 19, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 41, 41, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 22, 22, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 28, 28, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 31, 31, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 34, 34, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 37, 37, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 47, 47, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 128, 128, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 49, 49, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 43, 43, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 51, 51, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 53, 53, "mass");
            //red and yellow marrow
            double vRedYellow = FileIO.GetMeshInfo(phantomInfo, 56, 56, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 17, 17, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 42, 42, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 23, 23, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 35, 35, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 27, 27, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 40, 40, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 46, 46, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 25, 25, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 14, 14, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 15, 15, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 18, 18, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 21, 21, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 29, 29, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 30, 30, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 33, 33, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 36, 36, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 38, 38, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 48, 48, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 50, 50, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 44, 44, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 52, 52, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 54, 54, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 32, 32, "volume")
                + FileIO.GetMeshInfo(phantomInfo, 20, 20, "volume");
            double mRedYellow = FileIO.GetMeshInfo(phantomInfo, 56, 56, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 17, 17, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 42, 42, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 23, 23, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 35, 35, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 27, 27, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 40, 40, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 46, 46, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 25, 25, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 14, 14, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 15, 15, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 18, 18, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 21, 21, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 29, 29, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 30, 30, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 33, 33, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 36, 36, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 38, 38, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 48, 48, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 50, 50, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 44, 44, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 52, 52, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 54, 54, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 32, 32, "mass")
                + FileIO.GetMeshInfo(phantomInfo, 20, 20, "mass");
            //muscles
            double vMuscle = FileIO.GetMeshInfo(phantomInfo, 106, 109, "volume");
            double mMuscle = FileIO.GetMeshInfo(phantomInfo, 106, 109, "mass");

            //for calculation convenience
            //type 1 mesh (internal organs): volume is a function of height
            double type1Volume = FileIO.GetMeshInfo(phantomInfo, 97, 97, "volume") * (ReturnOrganVolumeScalingFactor("left lung", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 97, 97, "mass")) - 1) //left lung
                + FileIO.GetMeshInfo(phantomInfo, 99, 99, "volume") * (ReturnOrganVolumeScalingFactor("right lung", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 99, 99, "mass")) - 1) //right lung
                + FileIO.GetMeshInfo(phantomInfo, 95, 95, "volume") * (ReturnOrganVolumeScalingFactor("liver", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 95, 95, "mass")) - 1) //liver
                + FileIO.GetMeshInfo(phantomInfo, 127, 127, "volume") * (ReturnOrganVolumeScalingFactor("spleen", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 127, 127, "mass")) - 1) //spleen
                + FileIO.GetMeshInfo(phantomInfo, 89, 91, "volume") * (ReturnOrganVolumeScalingFactor("left kidney", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 89, 91, "mass")) - 1) //left kidney
                + FileIO.GetMeshInfo(phantomInfo, 92, 94, "volume") * (ReturnOrganVolumeScalingFactor("right kidney", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 92, 94, "mass")) - 1); //right kidney;
            double type1Mass = FileIO.GetMeshInfo(phantomInfo, 97, 97, "mass") * (ReturnOrganVolumeScalingFactor("left lung", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 97, 97, "mass")) - 1) //left lung
                + FileIO.GetMeshInfo(phantomInfo, 99, 99, "mass") * (ReturnOrganVolumeScalingFactor("right lung", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 99, 99, "mass")) - 1) //right lung
                + FileIO.GetMeshInfo(phantomInfo, 95, 95, "mass") * (ReturnOrganVolumeScalingFactor("liver", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 95, 95, "mass")) - 1) //liver
                + FileIO.GetMeshInfo(phantomInfo, 127, 127, "mass") * (ReturnOrganVolumeScalingFactor("spleen", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 127, 127, "mass")) - 1) //spleen
                + FileIO.GetMeshInfo(phantomInfo, 89, 91, "mass") * (ReturnOrganVolumeScalingFactor("left kidney", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 89, 91, "mass")) - 1) //left kidney
                + FileIO.GetMeshInfo(phantomInfo, 92, 94, "mass") * (ReturnOrganVolumeScalingFactor("right kidney", desiredHeight, FileIO.GetMeshInfo(phantomInfo, 92, 94, "mass")) - 1); //right kidney;
            //type 2 mesh (blood vessels and lymphatic nodes): volume is scaled by ky only
            double type2Volume = FileIO.GetMeshInfo(phantomInfo, 100, 105, "volume") * (heightScale - 1)
                + FileIO.GetMeshInfo(phantomInfo, 9, 12, "volume") * (heightScale - 1);
            double type2Mass = FileIO.GetMeshInfo(phantomInfo, 100, 105, "mass") * (heightScale - 1)
                + FileIO.GetMeshInfo(phantomInfo, 9, 12, "mass") * (heightScale - 1);
            //type 3 mesh (bones and muscles): volume is scaled by ky, then by kx and kz
            double type3Volume = vCorticalBone + vRedYellow + vMuscle;
            double type3Mass = mCorticalBone + mRedYellow + mMuscle;

            //variables
            double vSAT_newMesh = 0;
            double vVAT_newMesh = 0;
            double vResidualTissue_newMesh = 0;
            double aSkin_newMesh = 0;
            double weight_newMesh = 0;
            double volume_newMesh = 0;

            #endregion preprocess

            #region iteration
            //set min and max of the optimumFactor
            double minOptimumFactor = 0; // 0: shrinking to a singularity, 1: same size
            double maxOptimumFactor = 1e10;

            // first guess of the optimumFactor
            double optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;

            // update the coordinates of new vertices resulting from the optimumFactor
            newVertices = UpdateVertices.AlongXYZ(mesh[0], optimumFactor, heightScale, optimumFactor, centroid);

            // update volume
            volume_newMesh = CalculateMesh.CalculateVolume(newVertices, mesh[1]);

            //calculate total weight
            //update SAT volume
            vSAT_newMesh = 0.8 * (volume_newMesh - v150_standard);
            //update VAT volume
            vVAT_newMesh = v151_standard - 0.8 * v72_standard - v73_standard - v95 - v76_86_standard - v75_standard;
            //update residual tissue volume
            vResidualTissue_newMesh = volume_newMesh - vSAT_newMesh - vVAT_newMesh - FileIO.GetMeshInfo(phantomInfo, 1, 138, "volume") - type1Volume - type2Volume - type3Volume * (optimumFactor * optimumFactor * heightScale - 1);
            aSkin_newMesh = CalculateMesh.CalculateArea(newVertices, mesh[1]);
            weight_newMesh = FileIO.GetMeshInfo(phantomInfo, 1, 138, "mass") + type1Mass + type2Mass + type3Mass * (optimumFactor * optimumFactor * heightScale - 1)
                + GlobalConstant.densitySAT * vSAT_newMesh
                + GlobalConstant.densityVAT * vVAT_newMesh
                + GlobalConstant.densityResidualTissue * vResidualTissue_newMesh
                + GlobalConstant.densitySkin * aSkin_newMesh * GlobalConstant.thicknessSkin;
            weight_newMesh = weight_newMesh / 1000; //convert from g to kg

            // binary approximation method
            while (Math.Abs(weight_newMesh - desiredWeight) > GlobalConstant.epsilon)
            {
                //MessageBox.Show("delta = "+Math.Abs(newVolume - desiredVolume).ToString());
                if (weight_newMesh > desiredWeight)
                {
                    maxOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }
                else if (weight_newMesh < desiredWeight)
                {
                    minOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }

                // update the coordinates of new vertices resulting from the optimumFactor
                newVertices = UpdateVertices.AlongXYZ(mesh[0], optimumFactor, heightScale, optimumFactor, centroid);

                // update volume
                volume_newMesh = CalculateMesh.CalculateVolume(newVertices, mesh[1]);

                //calculate total weight
                //update SAT volume
                vSAT_newMesh = 0.8 * (volume_newMesh - v150_standard);
                //update VAT volume
                vVAT_newMesh = v151_standard - 0.8 * v72_standard - v73_standard - v95 - v76_86_standard - v75_standard;
                //update residual tissue volume
                vResidualTissue_newMesh = volume_newMesh - vSAT_newMesh - vVAT_newMesh - FileIO.GetMeshInfo(phantomInfo, 1, 138, "volume") - type1Volume - type2Volume - type3Volume * (optimumFactor * optimumFactor * heightScale - 1);
                aSkin_newMesh = CalculateMesh.CalculateArea(newVertices, mesh[1]);
                weight_newMesh = FileIO.GetMeshInfo(phantomInfo, 1, 138, "mass") + type1Mass + type2Mass + type3Mass * (optimumFactor * optimumFactor * heightScale - 1)
                    + GlobalConstant.densitySAT * vSAT_newMesh
                    + GlobalConstant.densityVAT * vVAT_newMesh
                    + GlobalConstant.densityResidualTissue * vResidualTissue_newMesh
                    + GlobalConstant.densitySkin * aSkin_newMesh * GlobalConstant.thicknessSkin;
                weight_newMesh = weight_newMesh / 1000; //convert from g to kg

            }// end while

            #endregion iteration

            //add vertices
            newMesh.Add(newVertices);

            // add faces
            newMesh.Add(mesh[1]);

            // add vertex normals
            newMesh.Add(CalculateMesh.CalculateVertexNormals(newVertices, newMesh[1]));

            // display statistics
            /*MessageBox.Show("same heigh, different BMI interpolation:"
                + "\n\n" + "optimum factor = " + optimumFactor.ToString()
                + "\n" + "volume of the new mesh = " + newVolume.ToString());*/

            return newMesh;

        } // end method

        //
        public static List<List<Coordinate>> DeformAlongXYZ_SpecifyScaleFactor
            (List<List<Coordinate>> oldMesh, double scaleFactorX, double scaleFactorY, double scaleFactorZ)
        {
            Coordinate centroid = new Coordinate();
            List<Coordinate> centroidVector = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();

            //calculate centroid
            centroid = CalculateMesh.CalculateCentroid(oldMesh[0]);

            // scale the vertices
            newMesh.Add(UpdateVertices.AlongXYZ(oldMesh[0], scaleFactorX, scaleFactorY, scaleFactorZ, centroid));

            // add the faces
            newMesh.Add(oldMesh[1]);

            // calculate vertex normals
            List<Coordinate> vertexNormals = new List<Coordinate>();
            vertexNormals = CalculateMesh.CalculateVertexNormals(newMesh[0], oldMesh[1]);

            // add vertex normals
            newMesh.Add(vertexNormals);

            return newMesh;
        }//end method

        //
        public static List<List<Coordinate>> DeformAlongXYZ_SpecifyVolume
            (List<List<Coordinate>> oldMesh, double desiredVolume, out double scaleFactorX, out double scaleFactorY, out double scaleFactorZ)
        {
            Coordinate centroid = new Coordinate();
            List<Coordinate> oldVertices = new List<Coordinate>();
            List<Coordinate> newVertices = new List<Coordinate>();
            List<Coordinate> faces = new List<Coordinate>();
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();
            double k = 0;
            double currentVolume = 0;
            double minK = 0;
            double maxK = 1e10;

            oldVertices = oldMesh[0];
            faces = oldMesh[1];

            //calculate centroid
            centroid = CalculateMesh.CalculateCentroid(oldMesh[0]);

            // first guess of the optimumFactor
            k = (minK + maxK) / 2.0;

            // update the coordinates of new vertices resulting from the optimumFactor
            newVertices = UpdateVertices.AlongXYZ(oldMesh[0], k, k, k, centroid);

            // calculate the volume of new mesh
            currentVolume = CalculateMesh.CalculateVolume(newVertices, faces);

            // binary approximation method
            while (Math.Abs(currentVolume - desiredVolume) > GlobalConstant.epsilon)
            {
                //MessageBox.Show("delta = "+Math.Abs(newVolume - desiredVolume).ToString());
                if (currentVolume < desiredVolume)
                {
                    minK = k;
                    k = (minK + maxK) / 2.0;
                }
                else if (currentVolume > desiredVolume)
                {
                    maxK = k;
                    k = (minK + maxK) / 2.0;
                }

                // update the coordinates of new vertices resulting from the optimumFactor
                newVertices = UpdateVertices.AlongXYZ(oldMesh[0], k, k, k, centroid);

                // calculate the volume of new mesh
                currentVolume = CalculateMesh.CalculateVolume(newVertices, faces);
                //MessageBox.Show(currentVolume.ToString()+'\n'+k.ToString());

            }// end while

            //pass the final scale factors
            scaleFactorX = k;
            scaleFactorY = k;
            scaleFactorZ = k;

            // add vertices and faces
            newMesh.Add(newVertices);
            newMesh.Add(faces);

            // calculate vertex normals
            vertexNormals = CalculateMesh.CalculateVertexNormals(newVertices, faces);

            // add vertex normals
            newMesh.Add(vertexNormals);
            return newMesh;
        }//end method

        //
        public static List<List<Coordinate>> DeformAlongCentroidVector_SpecifyScaleFactor
            (List<List<Coordinate>> oldMesh, double scaleFactorK)
        {
            Coordinate centroid = new Coordinate();
            Coordinate temp = new Coordinate();
            List<Coordinate> oldVertices = new List<Coordinate>();
            List<Coordinate> newVertices = new List<Coordinate>();
            List<Coordinate> faces = new List<Coordinate>();
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<Coordinate> centroidVector = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();

            oldVertices = oldMesh[0];
            faces = oldMesh[1];
            int vertexNumber = oldVertices.Count;

            //calculate centroid
            for (int i = 0; i < vertexNumber; i++)
            {
                centroid.x = centroid.x + oldVertices[i].x;
                centroid.y = centroid.y + oldVertices[i].y;
                centroid.z = centroid.z + oldVertices[i].z;
            }

            centroid.x = centroid.x / vertexNumber;
            centroid.y = centroid.y / vertexNumber;
            centroid.z = centroid.z / vertexNumber;

            //calculate centroid vector
            for (int i = 0; i < vertexNumber; i++)
            {
                temp.x = oldVertices[i].x - centroid.x;
                temp.y = oldVertices[i].y - centroid.y;
                temp.z = oldVertices[i].z - centroid.z;
                centroidVector.Add(temp);
            }

            // scale the vertices
            newMesh.Add(UpdateVertices.AlongCentroidVector(centroid, centroidVector, scaleFactorK));

            // add the faces
            newMesh.Add(oldMesh[1]);

            // calculate vertex normals
            vertexNormals = CalculateMesh.CalculateVertexNormals(newMesh[0], oldMesh[1]);

            // add vertex normals
            newMesh.Add(vertexNormals);

            return newMesh;
        }//end method

        //
        public static List<List<Coordinate>> DeformAlongCentroidVector_SpecifyVolume
            (List<List<Coordinate>> oldMesh, double desiredVolume, out double scaleFactorK)
        {
            Coordinate centroid = new Coordinate();
            Coordinate temp = new Coordinate();
            List<Coordinate> oldVertices = new List<Coordinate>();
            List<Coordinate> newVertices = new List<Coordinate>();
            List<Coordinate> faces = new List<Coordinate>();
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<Coordinate> centroidVector = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();
            double k = 0;
            double currentVolume = 0;
            double minK = 0;
            double maxK = 0;

            oldVertices = oldMesh[0];
            faces = oldMesh[1];
            int vertexNumber = oldVertices.Count;

            //calculate centroid
            for (int i = 0; i < vertexNumber; i++)
            {
                centroid.x = centroid.x + oldVertices[i].x;
                centroid.y = centroid.y + oldVertices[i].y;
                centroid.z = centroid.z + oldVertices[i].z;
            }

            centroid.x = centroid.x / vertexNumber;
            centroid.y = centroid.y / vertexNumber;
            centroid.z = centroid.z / vertexNumber;

            //calculate centroid vector
            for (int i = 0; i < vertexNumber; i++)
            {
                temp.x = oldVertices[i].x - centroid.x;
                temp.y = oldVertices[i].y - centroid.y;
                temp.z = oldVertices[i].z - centroid.z;
                centroidVector.Add(temp);
            }

            //-------------deform along centroid vector-------------
            //initialize parameters
            minK = 0.0;
            maxK = 1e10;

            //calculate volume
            currentVolume = CalculateMesh.CalculateVolume(oldVertices, faces);

            // first guess of the optimumFactor
            k = (minK + maxK) / 2.0;

            // update the coordinates of new vertices resulting from the optimumFactor
            newVertices = UpdateVertices.AlongCentroidVector(centroid, centroidVector, k);

            // calculate the volume of new mesh
            currentVolume = CalculateMesh.CalculateVolume(newVertices, faces);

            // binary approximation method
            while (Math.Abs(currentVolume - desiredVolume) > GlobalConstant.epsilon)
            {
                //MessageBox.Show("delta = "+Math.Abs(newVolume - desiredVolume).ToString());
                if (currentVolume < desiredVolume)
                {
                    minK = k;
                    k = (minK + maxK) / 2.0;
                }
                else if (currentVolume > desiredVolume)
                {
                    maxK = k;
                    k = (minK + maxK) / 2.0;
                }

                // update the coordinates of new vertices corresponding to the optimumFactor
                newVertices = UpdateVertices.AlongCentroidVector(centroid, centroidVector, k);

                // calculate the volume of new mesh
                currentVolume = CalculateMesh.CalculateVolume(newVertices, faces);

            }// end while

            //k
            scaleFactorK = k;

            // add vertices and faces
            newMesh.Add(newVertices);
            newMesh.Add(faces);

            // calculate vertex normals
            vertexNormals = CalculateMesh.CalculateVertexNormals(newVertices, faces);

            // add vertex normals
            newMesh.Add(vertexNormals);

            // display statistics
            /*MessageBox.Show("same heigh, different BMI interpolation:"
                + "\n\n" + "optimum factor = " + optimumFactor.ToString()
                + "\n" + "volume of the new mesh = " + newVolume.ToString());*/

            return newMesh;
        }//end method

        //
        public static List<List<Coordinate>> DeformAlongVertexNormal_SpecifyScaleFactor
            (List<List<Coordinate>> oldMesh, double scaleFactorK)
        {
            List<Coordinate> oldVertexNormals = new List<Coordinate>();
            List<Coordinate> newVertexNormals = new List<Coordinate>();
            List<Coordinate> centroidVector = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();

            // calculate vertex normals
            oldVertexNormals = CalculateMesh.CalculateVertexNormals(oldMesh[0], oldMesh[1]);

            // scale the vertices
            newMesh.Add(UpdateVertices.AlongVertexNormal(oldMesh[0], oldVertexNormals, scaleFactorK));

            // add the faces
            newMesh.Add(oldMesh[1]);

            // calculate vertex normals
            newVertexNormals = CalculateMesh.CalculateVertexNormals(newMesh[0], oldMesh[1]);

            // add vertex normals
            newMesh.Add(newVertexNormals);

            return newMesh;
        }//end method

        //
        public static List<List<Coordinate>> DeformAlongVertexNormal_SpecifyVolume
            (List<List<Coordinate>> oldMesh, double desiredVolume, out double scaleFactorK)
        {
            List<Coordinate> oldVertices = new List<Coordinate>();
            List<Coordinate> newVertices = new List<Coordinate>();
            List<Coordinate> faces = new List<Coordinate>();
            List<Coordinate> oldVertexNormals = new List<Coordinate>();
            List<Coordinate> newVertexNormals = new List<Coordinate>();
            List<Coordinate> centroidVector = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();
            double k = 0;
            double currentVolume = 0;
            double minK = 0;
            double maxK = 0;

            oldVertices = oldMesh[0];
            faces = oldMesh[1];

            //-------------deform along vertex normal-------------
            //initialize parameters
            minK = -1e10;
            maxK = 1e10;

            //get vertex normals
            oldVertexNormals = CalculateMesh.CalculateVertexNormals(oldVertices, faces);

            //calculate volume
            currentVolume = CalculateMesh.CalculateVolume(oldVertices, faces);

            // first guess of the optimumFactor
            k = (minK + maxK) / 2.0;

            // update the coordinates of new vertices resulting from the optimumFactor
            newVertices = UpdateVertices.AlongVertexNormal(oldVertices, oldVertexNormals, k);

            // calculate the volume of new mesh
            currentVolume = CalculateMesh.CalculateVolume(newVertices, faces);

            // binary approximation method
            while (Math.Abs(currentVolume - desiredVolume) > GlobalConstant.epsilon)
            {
                //MessageBox.Show("delta = "+Math.Abs(newVolume - desiredVolume).ToString());
                if (currentVolume < desiredVolume)
                {
                    minK = k;
                    k = (minK + maxK) / 2.0;
                }
                else if (currentVolume > desiredVolume)
                {
                    maxK = k;
                    k = (minK + maxK) / 2.0;
                }

                // update the coordinates of new vertices resulting from the optimumFactor
                newVertices = UpdateVertices.AlongVertexNormal(oldVertices, oldVertexNormals, k);

                // calculate the volume of new mesh
                currentVolume = CalculateMesh.CalculateVolume(newVertices, faces);
                //MessageBox.Show(currentVolume.ToString()+'\n'+k.ToString());

            }// end while

            //k
            scaleFactorK = k;

            // add vertices and faces
            newMesh.Add(newVertices);
            newMesh.Add(faces);

            // calculate vertex normals
            newVertexNormals = CalculateMesh.CalculateVertexNormals(newVertices, faces);

            // add vertex normals
            newMesh.Add(newVertexNormals);

            // display statistics
            /*MessageBox.Show("same heigh, different BMI interpolation:"
                + "\n\n" + "optimum factor = " + optimumFactor.ToString()
                + "\n" + "volume of the new mesh = " + newVolume.ToString());*/

            return newMesh;
        }//end method

        //
        public static double ReturnOrganVolumeScalingFactor(string meshName, double height, double originalMass)
        {
            double scalingFactor = 0;
            double slope = 0;
            double intercept = 0;

            height = height * 100; //convert from m to cm

            if (meshName == "left lung")
            {
                slope = 6.75;
                intercept = -570.166667;
            }
            else if (meshName == "right lung")
            {
                slope = 6.25;
                intercept = -401.833333;
            }
            else if (meshName == "liver")
            {
                slope = 18.8;
                intercept = -1555;
            }
            else if (meshName == "spleen")
            {
                slope = 3;
                intercept = -360;
            }
            else if (meshName == "left kidney")
            {
                slope = 1;
                intercept = -5.333333;
            }
            else if (meshName == "right kidney")
            {
                slope = 1;
                intercept = -11;
            }
            else
            {
                MessageBox.Show("error occurred at ReturnOrganVolumeScalingFactor!!");
            }

            double currentMass = slope * height + intercept;

            scalingFactor = currentMass / originalMass;

            return scalingFactor;

        }

    } // end class
}// end namespace