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

void drawOBJs(meshOBJ *obj)
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
			glColor4f(0.0f, 0.0f, 1.0f, 0.5f);
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

void drawParticles(particle *particles)
{
	printf("drawing Particles\n");
	glScalef(1.0 / 100.0, 1.0 / 100.0, 1.0/100.0);

	vertex v;

	for(int i=1;i<100;i++)
	{
//		glColor3f(0.0, 0.0, 1.5*i);
		glColor4f(1.0f, 0.0f, 0.0f, 0.5f);
//		glColor4f(.23,.78,.32,0.1);
		for(unsigned int j=0;j<particles[i].getSize();j++)
		{
			v = particles[i].getPosition(j);
			glPushMatrix(); // GL_MODELVIEW is default
			glTranslatef(v.getX(),v.getZ(),v.getY());
			glutSolidSphere (0.5, 20, 16);
			glPopMatrix();
		}
//Trying to connect dots with lines
/*
		glBegin(GL_LINES); 
	  	glLineWidth(4.0);
		for(unsigned int j=0;j<particles[i].getSize();j++)
		{
//			glColor3f(1.0f,0.0f,0.0f);
//			v = particles[i].getPosition(j);
//			glVertex3f(v.getX(),v.getY(),v.getZ());
		}
		glEnd();
*/	}
//	glPopMatrix();
}
