/*******************************************************************

Rensselaer Polytechnic Institute

********************************************************************

Discription:
  functions for plotting and rendering the surfaces

Version: 1.0

Currently: 04/19/2014
  Initial Code base

*******************************************************************/

//Includes, System
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <complex.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>
// includes
#include "queue.h"
#include "classes.h"
/* OpenGL Code Headers */
#include <GL/glui.h>

void drawOBJs(meshOBJ *obj)
{
	printf("drawing Objects\n");
  	glDisable( GL_LIGHTING );
	glPushMatrix(); // GL_MODELVIEW is default
//	glScalef(1.0 / 114.0, 1.0 / 93.0, 1.0/509.0);

	vertex* normal;
	vertex* vertex;

	glBegin(GL_TRIANGLES); 
	for(int i=43;i<44;i++)
	{
		if(obj[i].isValidated())
		{
			glColor3f(0.0, 0.0, 1.5*i);
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
	printf("crap\n");
	glEnd();
	glPopMatrix();
 	glEnable( GL_LIGHTING );
}

////////////////////////////////////////////////////////////////////////////////
//! Spatial Window Drawing Function
////////////////////////////////////////////////////////////////////////////////
float drawSpatial(int mt, int flag)
{
	float intensity = 0.0;
	printf("drawingSpatial\n");
/*	if(flag == 1)
	{
		double nTheta = 72.0;
		double theta = 0.0;
		double dTheta = (2.0*pi)/nTheta;
		int val0 = 0;
		int val1 = 0;
		int val2 = 0;
		double xcal[3], ycal[3], zcal[3];
	  	glDisable( GL_LIGHTING );
		glPushMatrix(); // GL_MODELVIEW is default
		glScalef(1.0 / (r[nr-1] - r[0]), 1.0 / (r[nr-1] - r[0]), 1.0);
	  	glLineWidth(4.0);
		glBegin(GL_TRIANGLES); 
		glColor3f(0.0, 0.0, 255.0);
		printf("_____________________________\n");
		for(j=0; j<nTheta; j++)
		{
			for(i=0; i<nr-1; i++)
			{
				//---First Triangle---//
	//			if(i==nr-2)
				{
	//				printf("first\n");
	//				printf("j:%d nTheta:%f, theta:%f, i:%d, nr:%d\n", j, nTheta, theta, i, nr);
				}
	//			glNormal3f(0.0,0.0,255.0);
				xcal[0] = r[i]*cos(theta);
				ycal[0] = r[i]*sin(theta);
				zcal[0] = pow(cuCabs(fSpatial3D[i])/E0,2);
	//			printf("xcal[0]:%f, ycal[0]:%f, zcal[0]:%f\n", xcal[0], ycal[0], zcal[0]);
				xcal[1] = r[i+1]*cos(theta);
				ycal[1] = r[i+1]*sin(theta);
				zcal[1] = pow(cuCabs(fSpatial3D[i+1])/E0,2);
	//			printf("xcal[1]:%f, ycal[1]:%f, zcal[1]:%f\n", xcal[1], ycal[1], zcal[1]);
				xcal[2] = r[i+1]*cos(theta+dTheta);
				ycal[2] = r[i+1]*sin(theta+dTheta);
				zcal[2] = pow(cuCabs(fSpatial3D[i+1])/E0,2);
	//			printf("xcal[2]:%f, ycal[2]:%f, zcal[2]:%f\n", xcal[2], ycal[2], zcal[2]);
				val0 = floor(zcal[0] * 205 + .5);
				val1 = floor(zcal[1] * 205 + .5);
				val2 = floor(zcal[2] * 205 + .5);
	//			printf("first\n");
				glColor3f(rt[val0], g[val0], b[val0]);
				glVertex3f(xcal[0], ycal[0], zcal[0]);
	//			printf("first\n");
				glColor3f(rt[val1], g[val1], b[val1]);
				glVertex3f(xcal[1], ycal[1], zcal[1]);
	//			printf("first\n");
				glColor3f(rt[val2], g[val2], b[val2]);
				glVertex3f(xcal[2], ycal[2], zcal[2]);
				//---Sencond Triangle---//
	//			if(i==nr-2)
				{
	//				printf("second\n");
				}
				xcal[1] = r[i]*cos(theta+dTheta);
				ycal[1] = r[i]*sin(theta+dTheta);
				zcal[1] = pow(cuCabs(fSpatial3D[i])/E0,2);
				val1 = floor(zcal[1] * 205 + .5);
				glColor3f(rt[val0], g[val0], b[val0]);
				glVertex3f(xcal[0], ycal[0], zcal[0]);
				glColor3f(rt[val1], g[val1], b[val1]);
				glVertex3f(xcal[1], ycal[1], zcal[1]);
				glColor3f(rt[val2], g[val2], b[val2]);
				glVertex3f(xcal[2], ycal[2], zcal[2]);
				//---Third Triangle---//
	//			if(i==nr-2)
				{
	//				printf("third\n");
				}
				xcal[2] = r[i+1]*cos(theta);
				ycal[2] = r[i+1]*sin(theta);
				zcal[2] = pow(cuCabs(fSpatial3D[i+1])/E0,2);
				val2 = floor(zcal[2] * 205 + .5);
				glColor3f(rt[val0], g[val0], b[val0]);
				glVertex3f(xcal[0], ycal[0], zcal[0]);
				glColor3f(rt[val1], g[val1], b[val1]);
				glVertex3f(xcal[1], ycal[1], zcal[1]);
				glColor3f(rt[val2], g[val2], b[val2]);
				glVertex3f(xcal[2], ycal[2], zcal[2]);
				//---Fourth Triangle---//
	//			if(i==nr-2)
				{
	//				printf("fourth\n");
				}
				xcal[0] = r[i+1]*cos(theta+dTheta);
				ycal[0] = r[i+1]*sin(theta+dTheta);
				zcal[0] = pow(cuCabs(fSpatial3D[i+1])/E0,2);
				val0 = floor(zcal[0] * 205 + .5);
				glColor3f(rt[val0], g[val0], b[val0]);
				glVertex3f(xcal[0], ycal[0], zcal[0]);
				glColor3f(rt[val1], g[val1], b[val1]);
				glVertex3f(xcal[1], ycal[1], zcal[1]);
				glColor3f(rt[val2], g[val2], b[val2]);
				glVertex3f(xcal[2], ycal[2], zcal[2]);
			}
			theta = theta + dTheta;
		}
		printf("crap\n");
		glEnd();
		glPopMatrix();
	 	glEnable( GL_LIGHTING );
	}
*/	return 2;
};
