/*******************************************************************
                  Rensselaer Polytechnic Institute
********************************************************************
Discription:
  functions for plotting and rendering objects and particles
*******************************************************************/
/* Class Header */
#include "classes.h"
/* OpenGL Code Headers */
#include <GL/glui.h>

#include <cmath>

//      Mass		Volume		Density		
//min	0.2			0.18		0.25
//max	15006.82	14292.21	2.75

//GREYSCALE 1
//BLACKWHITE 2
//COOL 5
//WARM 6
//BOLD 3
//PASTEL 4
//COLORBLIND 7

void drawOBJs(meshOBJ *obj, float *colorTable, float *modelMassTable, float *modelVolumeTable, float *modelDensityTable, int colorMode, int colored, int dogFlag)
{
	float maxMass = 15006.82;
	float maxVolume = 14292.21;
	float maxDensity = 2.75;
	printf("drawing Objects\n");
	glPushMatrix(); // GL_MODELVIEW is default
	glScalef(1.0 / 100.0, 1.0 / 100.0, 1.0/100.0);
	vertex* normal;
	vertex* vertex;
	glBegin(GL_TRIANGLES); 
	for(int i=0;i<138;i++)
	{
		if(obj[i].isValidated())
		{
			int color;
			if(colorMode == 1)
			{
				color = floor(255.0*((int)modelMassTable[i]%425)/425);
printf("color:%d\n",color);
				glColor4f(color/255,color/255,color/255,0.3f);
			}
			else if(colorMode == 2)
			{
				if(modelMassTable[i] <= 425)
				{
					color = 1.0;
				}
				else 
				{
					color = 0.0;
				}
printf("color:%d\n",color);
//				glColor4f(colorTable[i%100*3]/255,colorTable[i%100*3+1]/255,colorTable[i%100*3+2]/255,0.3f);	
				glColor4f(color,color,color,0.3f);
			}
			else if(colorMode == 3)
			{
				if(modelMassTable[i] <= 350)
					glColor4f(18.0/255,18.0/255,18.0/255,0.3f);
				if(modelMassTable[i] > 350 && modelMassTable[i] <= 400)
					glColor4f(0.0/255,64.0/255,224.0/255,0.3f);
				if(modelMassTable[i] > 400 && modelMassTable[i] <= 450)
					glColor4f(64.0/255,227.0/255,0.0/255,0.3f);
				if(modelMassTable[i] > 450 && modelMassTable[i] <= 600)
					glColor4f(219.0/255,33.0/255,2.0/255,0.3f);
				if(modelMassTable[i] > 600)
					glColor4f(255.0/255,115.0/255,0.0/255,0.3f);
			}
			else if(colorMode == 4)
			{
				if(modelMassTable[i] <= 350)
					glColor4f(196.0/255,242.0/255,200.0/255,0.3f);
				if(modelMassTable[i] > 350 && modelMassTable[i] <= 400)
					glColor4f(242.0/255,216.0/255,203.0/255,0.3f);
				if(modelMassTable[i] > 400 && modelMassTable[i] <= 450)
					glColor4f(197.0/255,226.0/255,227.0/255,0.3f);
				if(modelMassTable[i] > 450 && modelMassTable[i] <= 600)
					glColor4f(252.0/255,255.0/255,117.0/255,0.3f);
				if(modelMassTable[i] > 600)
					glColor4f(255.0/255,204.0/255,204.0/255,0.3f);
			}
			else if(colorMode == 5)
			{
				if(modelMassTable[i] <= 200)
					glColor4f(241.0/255,238.0/255,246.0/255,0.3f);
				if(modelMassTable[i] > 200 && modelMassTable[i] <= 350)
					glColor4f(208.0/255,209.0/255,230.0/255,0.3f);
				if(modelMassTable[i] > 350 && modelMassTable[i] <= 400)
					glColor4f(166.0/255,189.0/255,219.0/255,0.3f);
				if(modelMassTable[i] > 400 && modelMassTable[i] <= 450)
					glColor4f(116.0/255,169.0/255,207.0/255,0.3f);
				if(modelMassTable[i] > 450 && modelMassTable[i] <= 600)
					glColor4f(54.0/255,144.0/255,192.0/255,0.3f);
				if(modelMassTable[i] > 600 && modelMassTable[i] <= 1000)
					glColor4f(5.0/255,112.0/255,176.0/255,0.3f);
				if(modelMassTable[i] > 1000)
					glColor4f(3.0/255,78.0/255,123.0/255,0.3f);
			}
			else if(colorMode == 6)
			{
				if(modelMassTable[i] <= 200)
					glColor4f(255.0/255,255.0/255,178.0/255,0.3f);
				if(modelMassTable[i] > 200 && modelMassTable[i] <= 350)
					glColor4f(254.0/255,217.0/255,118.0/255,0.3f);
				if(modelMassTable[i] > 350 && modelMassTable[i] <= 400)
					glColor4f(254.0/255,178.0/255,76.0/255,0.3f);
				if(modelMassTable[i] > 400 && modelMassTable[i] <= 450)
					glColor4f(253.0/255,141.0/255,60.0/255,0.3f);
				if(modelMassTable[i] > 450 && modelMassTable[i] <= 600)
					glColor4f(252.0/255,78.0/255,42.0/255,0.3f);
				if(modelMassTable[i] > 600 && modelMassTable[i] <= 1000)
					glColor4f(227.0/255,26.0/255,28.0/255,0.3f);
				if(modelMassTable[i] > 1000)
					glColor4f(177.0/255,0.0/255,38.0/255,0.3f);
			}
			else if(colorMode == 7)
			{
				if(modelMassTable[i] <= 200)
					glColor4f(178.0/255,24.0/255,43.0/255,0.3f);
				if(modelMassTable[i] > 200 && modelMassTable[i] <= 350)
					glColor4f(239.0/255,138.0/255,98.0/255,0.3f);
				if(modelMassTable[i] > 350 && modelMassTable[i] <= 400)
					glColor4f(253.0/255,219.0/255,199.0/255,0.3f);
				if(modelMassTable[i] > 400 && modelMassTable[i] <= 450)
					glColor4f(247.0/255,247.0/255,247.0/255,0.3f);
				if(modelMassTable[i] > 450 && modelMassTable[i] <= 600)
					glColor4f(209.0/255,229.0/255,240.0/255,0.3f);
				if(modelMassTable[i] > 600 && modelMassTable[i] <= 1000)
					glColor4f(103.0/255,169.0/255,207.0/255,0.3f);
				if(modelMassTable[i] > 1000)
					glColor4f(33.0/255,102.0/255,172.0/255,0.3f);
			}
			else if(colorMode == 8)
			{
				glColor4f(colorTable[i%100*3]/255,colorTable[i%100*3+1]/255,colorTable[i%100*3+2]/255,0.3f);	
			}
			else
			{
				glColor4f(0.0f, 0.0f, 1.0f, 0.2f);
			}

			for(unsigned int j=0;j<obj[i].getSize();j++)			
			{
				normal = obj[i].getNormals(j);
				vertex = obj[i].getVertices(j);
				//Render 1 Face
				glNormal3f(normal[0].getX(),normal[0].getY(),normal[0].getZ());
				glVertex3f(vertex[0].getX(),vertex[0].getY(),vertex[0].getZ());
				glNormal3f(normal[1].getX(),normal[1].getY(),normal[1].getZ());
				glVertex3f(vertex[1].getX(),vertex[1].getY(),vertex[1].getZ());
				glNormal3f(normal[2].getX(),normal[2].getY(),normal[2].getZ());
				glVertex3f(vertex[2].getX(),vertex[2].getY(),vertex[2].getZ());
			}
		}
	}

	//Print the dog
	if(dogFlag)
	{printf("printing dog\n");
		int i=138;
		if(obj[i].isValidated())
		{
			if(colored)
			{
				glColor4f(colorTable[i%100*3]/255,colorTable[i%100*3+1]/255,colorTable[i%100*3+2]/255,0.3f);	
			}
			else
			{
				glColor4f(0.0f, 0.0f, 1.0f, 0.2f);
			}

			for(unsigned int j=0;j<obj[i].getSize();j++)
			{
				normal = obj[i].getNormals(j);
				vertex = obj[i].getVertices(j);
				//Render 1 Face
				glNormal3f(normal[0].getX(),normal[0].getY(),normal[0].getZ());
				glVertex3f(vertex[0].getX()+70,vertex[0].getY(),vertex[0].getZ());
				glNormal3f(normal[1].getX(),normal[1].getY(),normal[1].getZ());
				glVertex3f(vertex[1].getX()+70,vertex[1].getY(),vertex[1].getZ());
				glNormal3f(normal[2].getX(),normal[2].getY(),normal[2].getZ());
				glVertex3f(vertex[2].getX()+70,vertex[2].getY(),vertex[2].getZ());
			}
		}
	}

	glEnd();
	glPopMatrix();
}

//void drawParticles(particle *particles, int numParticles)
void drawParticles(particle *particles, particle *particles2, particle *particles3, particle *particles4, int numParticles)
{
	printf("drawing Particles\n");
	glPushMatrix(); // GL_MODELVIEW is default
	glScalef(1.0 / 100.0, 1.0 / 100.0, 1.0/100.0);

	vertex v;

	for(int i=1;i<numParticles;i++)
	{
		glColor4f(1.0f, 0.0f, 0.0f, 0.5f);
		for(unsigned int j=0;j<particles[i].getSize();j++)
		{
			v = particles[i].getPosition(j);
			glPushMatrix(); // GL_MODELVIEW is default
			glTranslatef(v.getX(),v.getZ(),v.getY());
//			glutSolidSphere (0.12, 4, 4);
			glutSolidCube (0.12);
			glPopMatrix();
		}
		for(unsigned int j=0;j<particles2[i].getSize();j++)
		{
			v = particles2[i].getPosition(j);
			glPushMatrix(); // GL_MODELVIEW is default
			glTranslatef(v.getX(),v.getZ(),v.getY());
//			glutSolidSphere (0.12, 4, 4);
			glutSolidCube (0.12);
			glPopMatrix();
		}
		for(unsigned int j=0;j<particles3[i].getSize();j++)
		{
			v = particles3[i].getPosition(j);
			glPushMatrix(); // GL_MODELVIEW is default
			glTranslatef(v.getX(),v.getZ(),v.getY());
//			glutSolidSphere (0.12, 4, 4);
			glutSolidCube (0.12);
			glPopMatrix();
		}
		for(unsigned int j=0;j<particles4[i].getSize();j++)
		{
			v = particles4[i].getPosition(j);
			glPushMatrix(); // GL_MODELVIEW is default
			glTranslatef(v.getX(),v.getZ(),v.getY());
//			glutSolidSphere (0.12, 4, 4);
			glutSolidCube (0.12);
			glPopMatrix();
		}
	}
	glPopMatrix();
}

void drawOffice(meshOBJ *obj)
{
	printf("drawing Office\n");
	glPushMatrix(); // GL_MODELVIEW is default
	glScalef(1.0 / 100.0, 1.0 / 100.0, 1.0/100.0);
	vertex* normal;
	vertex* vertex;
	glBegin(GL_TRIANGLES); 
	glColor4f(1.0f, 1.0f, 1.0f, 0.5f);
	for(int i=0;i<1;i++)
	{
		for(unsigned int j=0;j<obj[i].getSize();j++)
		{
			normal = obj[i].getNormals(j);
			vertex = obj[i].getVertices(j);
			//Render 1 Face
			glNormal3f(normal[0].getX(),normal[0].getY(),normal[0].getZ());
			glVertex3f(vertex[0].getX(),vertex[0].getY(),vertex[0].getZ());
			glNormal3f(normal[1].getX(),normal[1].getY(),normal[1].getZ());
			glVertex3f(vertex[1].getX(),vertex[1].getY(),vertex[1].getZ());
			glNormal3f(normal[2].getX(),normal[2].getY(),normal[2].getZ());
			glVertex3f(vertex[2].getX(),vertex[2].getY(),vertex[2].getZ());
		}
	}
	glEnd();
	glPopMatrix();
}

void drawAxis(int nAxis, float startPointZ)	//Prints nAxis many axis starting at point startPointZ
{
	printf("drawing Axis\n");
	glPushMatrix(); // GL_MODELVIEW is default
	glScalef(1.0 / 100.0, 1.0 / 100.0, 1.0/100.0);
	float z = 0.0;
//	float dz = 180.0/nAxis;
//	float dz = 180.0/90.0;
	float dz = 40.0;
	for(int i=0;i<nAxis;i++)
	{
//		z = dz*i + startPointZ;
//		z = dz*66.0;
		z = dz*i + 15.0*2.0;
		glColor4f(0.9f, 0.0f, 0.0f, 1.0f);
		glPushMatrix(); // GL_MODELVIEW is default
		glTranslatef(0.0,z,0.0);
		glRotatef(90.0,1.0,0.0,0.0);
		glutSolidTorus(0.75, 52.7, 100, 100);
		glPopMatrix();
	}
	glPopMatrix();
}

void animate(particle *particles, int *iter, int numParticles)
{
	printf("animating\n");
//	drawAxis(1,startPointZ);
	glPushMatrix(); // GL_MODELVIEW is default
	glScalef(1.0 / 100.0, 1.0 / 100.0, 1.0/100.0);
	vertex v;

	for(int i=1;i<numParticles;i++)
	{
		glColor4f(1.0f, 0.0f, 0.0f, 0.5f);
		if(iter[i] != -1)
		{
			if((unsigned int)iter[i] < particles[i].getSize() && iter[i] < 100)		//Remove && iter[i] < 80 if want to do entire particle paths
			{
				for(int j=0;j<iter[i];j++)	//Plot all locations along path of particle i from j=0 to j=iter[i]
				{
					v = particles[i].getPosition(j);
					glPushMatrix(); // GL_MODELVIEW is default
					glTranslatef(v.getX(),v.getZ(),v.getY());
//					glutSolidSphere (0.12, 5, 5);
					glutSolidCube (0.2);
					glPopMatrix();
				}
				iter[i]++;					//increase so the next run plots one more location along particle i's path
			}
			else
			{
				iter[i] = -1;			//Lets us know that particle i's path has been plotted
				iter[numParticles]++;	//Keep track of how many particles have been plotted
			}
		}
	}
	glPopMatrix();
}
