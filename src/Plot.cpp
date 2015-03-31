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

void drawOBJs(meshOBJ *obj, float *colorTable, int colored)
{
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
				glVertex3f(vertex[0].getX(),vertex[0].getY(),vertex[0].getZ());
				glNormal3f(normal[1].getX(),normal[1].getY(),normal[1].getZ());
				glVertex3f(vertex[1].getX(),vertex[1].getY(),vertex[1].getZ());
				glNormal3f(normal[2].getX(),normal[2].getY(),normal[2].getZ());
				glVertex3f(vertex[2].getX(),vertex[2].getY(),vertex[2].getZ());
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
