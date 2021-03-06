/*******************************************************************

Rensselaer Polytechnic Institute

********************************************************************

Discription:
  This is the main project code for the visualization of ARCHER-CT
  simulations.

Version: 1.0

Currently: 04/19/2014
  Initial Code base

*******************************************************************/
/* Propagation Code Headers */
#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <time.h>
#include <string.h>
#include <complex.h>
#include <fstream>
#include "classes.h"
/* OpenGL Code Headers */
#include <GL/glui.h>

/*************External Functions***************/
// Updated
float drawSpatial(int N, int flag);
void drawOBJs(meshOBJ *obj, float *colorTable, float *modelMassTable, float *modelVolumeTable, float *modelDensityTable, int colorMode, int colored, int dogFlag);
//void drawParticles(particle *particles, int numParticles); 
void drawParticles(particle *particles, particle *particles2, particle *particles3, particle *particles4, int numParticles);
void drawOffice(meshOBJ *obj);
void drawAxis(int nAxis, float startPointZ);
void animate(particle *particles, int *iter, int numParticles);

/*************Internal Functions***************/
void spatialReshape( int x, int y );
void spatialDisplay( void );
void animationSpatialDisplay( void );
void loadColorTable();
void loadModelFeatures();

float xy_aspect;
int   last_x, last_y;
float rotationX = 0.0, rotationY = 0.0;

/** These are the live variables passed into GLUI ***/
int   xray_view_enabled = 0;
int   model_view_enabled = 0;
int   std_view_enabled = 1;
int   colored_view_enabled = 0;
int   grey_enabled = 1;
int   black_white_enabled = 0;
int   bold_enabled = 0;
int   pastel_enabled = 0;
int   cool_enabled = 0;
int   warm_enabled = 0;
int   blind_enabled = 0;
int   background_enabled = 0;
int   light0_enabled = 1;
int   light1_enabled = 1;
int   light2_enabled = 1;
float light0_intensity = 0.01;
float light0_intensity2 = 1.4;
float light1_intensity = .4;
int   spatialWindow;
float scale = 1.0;
int   show_spatial=0;
float view_spatial_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float spatial_pos[] = { 0.0, 0.0, 0.0 };

/** Pointers to the windows and some of the controls we'll create **/
GLUI            *spatialSubWindow,*controlWindow,*spatialBottomSubWIndow;

/********** User IDs for callbacks ********/
#define LIGHT0_ENABLED_ID    200
#define LIGHT1_ENABLED_ID    201
#define LIGHT2_ENABLED_ID    202
#define LIGHT0_INTENSITY_ID  250
#define LIGHT0_INTENSITY2_ID 251
#define LIGHT1_INTENSITY_ID  260
#define ENABLE_ID            300
#define DISABLE_ID           301
#define SHOW_ID              302
#define HIDE_ID              303
#define PAUSE		     	 233
#define LOAD_OBJ 			 235
#define LOAD_PARTICLES		 237
#define LOAD_OFFICE			 240
#define DRAW_PARTICLES		 236
#define DRAW_OBJS			 238
#define DRAW_OFFICE			 241
#define ANIMATE				 242
#define DRAW_AXIS			 243
#define REMOVE_PARTICLES     239
#define XRAY_VIEW			 305
#define MODEL_VIEW			 306
#define STD_VIEW			 307
#define COLORED_VIEW		 309
#define DOG_INCLUDED		 310
#define GREYSCALE			 311
#define BLACKWHITE			 312
#define COOL			 	 313
#define WARM				 314
#define BOLD			 	 315
#define PASTEL				 316
#define COLORBLIND			 317
#define BACKGROUND			 318

/********** Miscellaneous global variables **********/
GLfloat light0_ambient[] =  {0.1f, 0.1f, 0.3f, 1.0f};
GLfloat light0_diffuse[] =  {.6f, .6f, 1.0f, 1.0f};
GLfloat light0_position[] = {.5f, .5f, 1.0f, 0.0f};
GLfloat light1_ambient[] =  {0.1f, 0.1f, 0.3f, 1.0f};
GLfloat light1_diffuse[] =  {.9f, .6f, 0.0f, 1.0f};
GLfloat light1_position[] = {-1.0f, -1.0f, 1.0f, 0.0f};
GLfloat light2_position[] = {-0.5f, -0.5f, 1.0f, 0.0f};
GLfloat lights_rotation[16] = {1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };

//-----Approved Variables-----//
const int numParticles = 900;
meshOBJ objs[139];
meshOBJ office[1];
particle particles[numParticles];
particle particles2[numParticles];	//particles in 2nd scan rotation
particle particles3[numParticles];	//particles in 3rd scan rotation
particle particles4[numParticles];	//particles in 4th scan rotation
int loadedOBJ = 0;					//0 objects have not been loaded, 1 objects have been loaded
int loadedParticles = 0;
int loadedOffice = 0;
int objFlag = 0;
int particleFlag = 0;
int officeFlag = 0;
int axisFlag = 0;
int animateFlag = 0;
int nAxis = 4;
int currentAxis = 10000;
int dogFlag = 1;
int colorMode = 0;
float startPointZ = 0.0;
int *animationIteration = (int*) calloc (numParticles+1,sizeof(int));
float colorTable[303];
float modelNameTable[138];
float modelMassTable[138];
float modelVolumeTable[138];
float modelDensityTable[138];

/***********************************************************************/
/*                          control_cb()                               */
/*                      GLUI control callback                          */
/***********************************************************************/
void control_cb( int control )
{
	printf("control_cb \n");
	if ( control == XRAY_VIEW ) 
	{
		glutSetWindow(spatialWindow);
		glDisable(GL_DEPTH_TEST);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_COLOR);
		glDisable( GL_LIGHT0 );
		glDisable( GL_LIGHT1 );
		glDisable( GL_LIGHT2 );
		light0_enabled = 0;
		light1_enabled = 0;
		light2_enabled = 0;
		model_view_enabled = 0;
		std_view_enabled = 0;
		controlWindow->sync_live();
		spatialDisplay();
	}
	if ( control == MODEL_VIEW ) 
	{
		glutSetWindow(spatialWindow);
		glDisable(GL_DEPTH_TEST);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_COLOR);
		glEnable(GL_DEPTH_TEST);
		glEnable( GL_LIGHT0 );
		glLightfv(GL_LIGHT0, GL_AMBIENT, light0_ambient);
		glLightfv(GL_LIGHT0, GL_DIFFUSE, light0_diffuse);
		glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
		glDisable( GL_LIGHT1 );
		glDisable( GL_LIGHT2 );
		light0_enabled = 1;
		light1_enabled = 0;
		light2_enabled = 0;
		xray_view_enabled = 0;
		std_view_enabled = 0;
		controlWindow->sync_live();
		spatialDisplay();
	}
	if ( control == STD_VIEW ) 
	{
		glutSetWindow(spatialWindow);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		glEnable(GL_DEPTH_TEST);
			glEnable( GL_LIGHT0 );
			glLightfv(GL_LIGHT0, GL_AMBIENT, light0_ambient);
			glLightfv(GL_LIGHT0, GL_DIFFUSE, light0_diffuse);
			glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
			glEnable( GL_LIGHT1 );
			glLightfv(GL_LIGHT1, GL_AMBIENT, light1_ambient);
			glLightfv(GL_LIGHT1, GL_DIFFUSE, light1_diffuse);
			glLightfv(GL_LIGHT1, GL_POSITION, light1_position);
			glEnable( GL_LIGHT2 );
			glLightfv(GL_LIGHT2, GL_AMBIENT, light0_ambient);
			glLightfv(GL_LIGHT2, GL_DIFFUSE, light0_diffuse);
			glLightfv(GL_LIGHT2, GL_POSITION, light2_position);
		light0_enabled = 1;
		light1_enabled = 1;
		light2_enabled = 1;
		xray_view_enabled = 0;
		model_view_enabled = 0;
		controlWindow->sync_live();
		spatialDisplay();
	}
	if( control == COLORED_VIEW )
	{
		grey_enabled = 0;
		black_white_enabled = 0;
		bold_enabled = 0;
		pastel_enabled = 0;
		cool_enabled = 0;
		warm_enabled = 0;
		blind_enabled = 0;
		colorMode = 8;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if( control == GREYSCALE )
	{
		colored_view_enabled = 0;
		grey_enabled = 1;
		black_white_enabled = 0;
		bold_enabled = 0;
		pastel_enabled = 0;
		cool_enabled = 0;
		warm_enabled = 0;
		blind_enabled = 0;
		colorMode = 1;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if( control == BLACKWHITE )
	{
		colored_view_enabled = 0;
		grey_enabled = 0;
		black_white_enabled = 1;
		bold_enabled = 0;
		pastel_enabled = 0;
		cool_enabled = 0;
		warm_enabled = 0;
		blind_enabled = 0;
		colorMode = 2;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if( control == BOLD )
	{
		colored_view_enabled = 0;
		grey_enabled = 0;
		black_white_enabled = 0;
		bold_enabled = 1;
		pastel_enabled = 0;
		cool_enabled = 0;
		warm_enabled = 0;
		blind_enabled = 0;
		colorMode = 3;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if( control == PASTEL )
	{
		colored_view_enabled = 0;
		grey_enabled = 0;
		black_white_enabled = 0;
		bold_enabled = 0;
		pastel_enabled = 1;
		cool_enabled = 0;
		warm_enabled = 0;
		blind_enabled = 0;
		colorMode = 4;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if( control == COOL )
	{
		colored_view_enabled = 0;
		grey_enabled = 0;
		black_white_enabled = 0;
		bold_enabled = 0;
		pastel_enabled = 0;
		cool_enabled = 1;
		warm_enabled = 0;
		blind_enabled = 0;
		colorMode = 5;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if( control == WARM )
	{
		colored_view_enabled = 0;
		grey_enabled = 0;
		black_white_enabled = 0;
		bold_enabled = 0;
		pastel_enabled = 0;
		cool_enabled = 0;
		warm_enabled = 1;
		blind_enabled = 0;
		colorMode = 6;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if( control == COLORBLIND )
	{
		colored_view_enabled = 0;
		grey_enabled = 0;
		black_white_enabled = 0;
		bold_enabled = 0;
		pastel_enabled = 0;
		cool_enabled = 0;
		warm_enabled = 0;
		blind_enabled = 1;
		colorMode = 7;
		controlWindow->sync_live();
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	if ( control == LIGHT0_ENABLED_ID ) 
	{
		printf("light0_enabled\n");
		if ( light0_enabled ) 
		{
			printf("enable\n");
			glutSetWindow(spatialWindow);
			glEnable( GL_LIGHT0 );
			glLightfv(GL_LIGHT0, GL_AMBIENT, light0_ambient);
			glLightfv(GL_LIGHT0, GL_DIFFUSE, light0_diffuse);
			glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
			spatialDisplay();
		}
		else 
		{
			printf("disable\n");
			glutSetWindow(spatialWindow);
			glDisable( GL_LIGHT0 );
			spatialDisplay();
		}
	}
	else if ( control == LIGHT1_ENABLED_ID ) 
	{
		if ( light1_enabled ) 
		{
			printf("enable\n");
			glutSetWindow(spatialWindow);
			glEnable( GL_LIGHT1 );
			glLightfv(GL_LIGHT1, GL_AMBIENT, light1_ambient);
			glLightfv(GL_LIGHT1, GL_DIFFUSE, light1_diffuse);
			glLightfv(GL_LIGHT1, GL_POSITION, light1_position);
			spatialDisplay();
		}
		else 
		{
			printf("disable\n");
			glutSetWindow(spatialWindow);
			glDisable( GL_LIGHT1 );
			spatialDisplay();
		}
	}
	if ( control == LIGHT2_ENABLED_ID ) 
	{
		printf("light0_enabled\n");
		if ( light0_enabled ) 
		{
			printf("enable\n");
			glutSetWindow(spatialWindow);
			glEnable( GL_LIGHT2 );
			glLightfv(GL_LIGHT2, GL_AMBIENT, light0_ambient);
			glLightfv(GL_LIGHT2, GL_DIFFUSE, light0_diffuse);
			glLightfv(GL_LIGHT2, GL_POSITION, light2_position);
			spatialDisplay();
		}
		else 
		{
			printf("disable\n");
			glutSetWindow(spatialWindow);
			glDisable( GL_LIGHT2 );
			spatialDisplay();
		}
	}
	else if ( control == LIGHT0_INTENSITY_ID ) 
	{
		glutSetWindow(spatialWindow);
		float v[] = {light0_diffuse[0], light0_diffuse[1], light0_diffuse[2], light0_diffuse[3]};
		v[0] *= light0_intensity;
		v[1] *= light0_intensity;
		v[2] *= light0_intensity;
		glLightfv(GL_LIGHT0, GL_DIFFUSE, v );
		spatialDisplay();
	}
	else if ( control == LIGHT0_INTENSITY2_ID ) 
	{
		glutSetWindow(spatialWindow);
		float v[] = {light0_diffuse[0], light0_diffuse[1], light0_diffuse[2], light0_diffuse[3]};
		v[0] *= light0_intensity2;
		v[1] *= light0_intensity2;
		v[2] *= light0_intensity2;
		glLightfv(GL_LIGHT0, GL_DIFFUSE, v );
		spatialDisplay();
	}
	else if ( control == LIGHT1_INTENSITY_ID ) 
	{
		glutSetWindow(spatialWindow);
		float v[] = {light1_diffuse[0], light1_diffuse[1], light1_diffuse[2], light1_diffuse[3]};
		v[0] *= light1_intensity;
		v[1] *= light1_intensity;
		v[2] *= light1_intensity;
		glLightfv(GL_LIGHT1, GL_DIFFUSE, v );
		spatialDisplay();
	}
	else if ( control == ENABLE_ID )
	{
		spatialBottomSubWIndow->enable();			// Enables temporalBottomSubWindow (controls)
	}
	else if ( control == DISABLE_ID )
	{
		spatialBottomSubWIndow->disable();			// disables temporalBottomSubWindow (controls)
	}
	else if ( control == SHOW_ID )
	{
		spatialBottomSubWIndow->show();			// disables temporalBottomSubWindow (controls)
	}
	else if ( control == HIDE_ID )
	{
		spatialBottomSubWIndow->hide();			// disables temporalBottomSubWindow (controls)
	}
	else if(control == LOAD_OBJ)
	{
		printf("Load objects call back\n");
		loadedOBJ = 1;
		char filepath[200];
		for(int i=0;i<138;i++)
		{
			sprintf(filepath,"data/73_adult_male/073kg/%03d.obj",i+1);
//			sprintf(filepath,"data/misc/%03d.obj",i+1);
			std::cout << "loading file: " << filepath << std::endl;
			objs[i].setPath(filepath);
			objs[i].load();
		}
		if(dogFlag)
		{
			sprintf(filepath,"data/dog.obj");
			std::cout << "loading file: " << filepath << std::endl;
			objs[138].setPath(filepath);
			objs[138].load();
		}
		loadModelFeatures();
		loadColorTable();
	}
	else if(control == LOAD_PARTICLES)
	{
		printf("Load particles call back\n");
		loadedParticles = 1;
		char particleFilepath[200];
		for(int i=1;i<numParticles;i++)
		{
//			sprintf(particleFilepath,"data/particles/particle%dposition.dat",i);
//			std::cout << "loading file: " << particleFilepath << std::endl;
//			particles[i].setPath(particleFilepath);
//			particles[i].load();
			sprintf(particleFilepath,"data/particles/scan15/particle%dposition.dat",i);
			std::cout << "loading file: " << particleFilepath << std::endl;
			particles[i].setPath(particleFilepath);
			particles[i].load();
			sprintf(particleFilepath,"data/particles/scan35/particle%dposition.dat",i);
			std::cout << "loading file: " << particleFilepath << std::endl;
			particles2[i].setPath(particleFilepath);
			particles2[i].load();
			sprintf(particleFilepath,"data/particles/scan55/particle%dposition.dat",i);
			std::cout << "loading file: " << particleFilepath << std::endl;
			particles3[i].setPath(particleFilepath);
			particles3[i].load();
			sprintf(particleFilepath,"data/particles/scan75/particle%dposition.dat",i);
			std::cout << "loading file: " << particleFilepath << std::endl;
			particles4[i].setPath(particleFilepath);
			particles4[i].load();
		}
	}
	else if(control == LOAD_OFFICE)
	{
		printf("Load office call back\n");
		loadedOffice = 1;
		char officeFilepath[200] = "data/misc/office.obj";
		std::cout << "loading file: " << officeFilepath << std::endl;
		office[0].setPath(officeFilepath);
		office[0].load();
	}
	else if(control == DRAW_PARTICLES)
	{	
		printf("Draw particles call back\n");
		particleFlag = 1;
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	else if(control == DRAW_OBJS)
	{	
		printf("Draw objects call back\n");
		objFlag = 1;
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	else if(control == DRAW_OFFICE)
	{	
		printf("Draw office call back\n");
		officeFlag = 1;
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	else if(control == DRAW_AXIS)
	{	
		printf("Draw axis call back\n");
		axisFlag = 1;
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	else if(control == REMOVE_PARTICLES)
	{	
		printf("Remove Particles call back\n");
		particleFlag = 0;
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
	else if(control == ANIMATE)
	{	
		printf("Animate call back\n");
		particleFlag = 0;						//Disable rendering of particles
		animateFlag = 1;
		currentAxis = 0;
		startPointZ = currentAxis*180.0/nAxis;
		animationIteration = (int*) calloc (numParticles+1,sizeof(int));
		glutSetWindow(spatialWindow);			//Set Window for rendering
		spatialDisplay();						//begin rendering process
	}
}

void loadModelFeatures()
{
	std::ifstream infile;
	infile.open("data/73_adult_male/073kg/model-features.txt", std::ios::in);
	if (infile.is_open()) 
	{
		int junk = 11;
		for(int i=0;i<138;i++)
		{
			infile >> junk >> modelMassTable[i] >> modelVolumeTable[i] >> modelDensityTable[i];
			printf("junk:%d modelMassTable[%d]:%f modelVolumeTable[%d]:%f modelDensityTable[%d]:%f\n",junk, i, modelMassTable[i], i, modelVolumeTable[i], i, modelDensityTable[i]);
		}
	}
	else 
	{
		std::cout << "Error opening model-features.csv file";
	}
	infile.close();
}

void loadColorTable()
{
	std::ifstream infile;
	infile.open("data/misc/colorTable.txt", std::ios::in);
	for(int i=0;i<100;i++)
	{
		infile >> colorTable[i*3] >> colorTable[i*3+1] >> colorTable[i*3+2];
		printf("colortable[%d]:%f colortable[%d]:%f colortable[%d]:%f\n",i*3, colorTable[i*3], i*3+1, colorTable[i*3+1], i*3+2, colorTable[i*3+2]);
	}
	infile.close();
}

/***********************************************************************/
/*                         myGlutKeyboard()                            */
/***********************************************************************/
void myGlutKeyboard(unsigned char Key, int x, int y)
{
	printf("myglutkeyboard \n");
	switch(Key)
	{
		case 27: 
		case 'q':
			exit(0);
		break;
	};
	glutPostRedisplay();
}
/***********************************************************************/
/*                           myGlutMenu()                              */
/***********************************************************************/
void myGlutMenu( int value )
{
	printf("myglutmenu \n");
	myGlutKeyboard( value, 0, 0 );
}
/***********************************************************************/
/*                           myGlutIdle()                              */
/***********************************************************************/
void myGlutIdle( void )
{
	printf("myglutidle \n");
	if ( glutGetWindow() != spatialWindow ) 
	{
		glutSetWindow(spatialWindow);  
	}
	if(animationIteration[numParticles] != numParticles-1)
	{
		animationSpatialDisplay();						//begin rendering process
	}
	else if(currentAxis < nAxis)
	{
		currentAxis++;
		animationIteration = (int*) calloc (numParticles+1,sizeof(int));
		startPointZ = currentAxis*180.0/nAxis;
		spatialDisplay();						//begin rendering process
	}
	else
	{
		glutPostRedisplay();
	}
}
/***********************************************************************/
/*                           myGlutMouse()                             */
/***********************************************************************/
void myGlutMouse(int button, int button_state, int x, int y )
{
	printf("myglutmouse \n");
}
/***********************************************************************/
/*                           myGlutMotion()                            */
/***********************************************************************/
void myGlutMotion(int x, int y )
{
	printf("myglutmotion \n");
	glutPostRedisplay(); 
}
/***********************************************************************/
/*                           spatialReshape()                           */
/***********************************************************************/
void spatialReshape( int x, int y )
{
	printf("spatialReshape \n");
	glutSetWindow(spatialWindow);
	int tx, ty, tw, th;
	GLUI_Master.get_viewport_area( &tx, &ty, &tw, &th );
	glViewport( tx, ty, tw, th );
	xy_aspect = (float)tw / (float)th;
	glutPostRedisplay();
}
/***********************************************************************/
/*                           draw_spatial_axes()                       */
/*               Disables lighting, then draws RGB axes                */
/***********************************************************************/
void draw_spatial_axes( float scale )
{
	printf("draw_spatial_axes\n");
	glDisable( GL_LIGHTING );
	glPushMatrix();
	glScalef( scale, scale, scale );
	glLineWidth(2.0); 
	glBegin( GL_LINES );
	/* Letter X */
	glColor3f( 1.0, 0.0, 0.0 );
	glVertex3f( 1.77f-1.0f, -0.05f, 0.0 );  glVertex3f( 1.9-1.0f, -0.25f, 0.0 ); 
	glVertex3f( 1.77f-1.0f, -.25f, 0.0 );  glVertex3f( 1.9-1.0f, -0.05f, 0.0 );
	/* Letter Y */
	glColor3f( 1.0, 0.0, 0.0 );
	glVertex3f( -0.05f-1.0f, 1.85f+0.25f, 0.0 );  glVertex3f( -0.1-1.0f, 1.74f+0.25f, 0.0 ); 
	glVertex3f( -0.15f-1.0f, 1.85f+0.25f, 0.0 );  glVertex3f( -0.1-1.0f, 1.74f+0.25f, 0.0 );
	glVertex3f( -0.1f-1.0f, 1.6f+0.25f, 0.0 );  glVertex3f( -0.1-1.0f, 1.74f+0.25f, 0.0 ); 
	/* X axis */
	glColor3f( 1.0, 0.0, 0.0 );
	glVertex3f( 0.0-1.0f, 0.0, 0.0 );  glVertex3f( 2.0-1.0f, 0.0, 0.0 ); 
	/* Y axis */
	glColor3f( 1.0, 0.0, 0.0 );
	glVertex3f( 0.0-1.0f, 0.0, 0.0 );  glVertex3f( 0.0-1.0f, 2.0+0.25f, 0.0 ); 
	glEnd();
	glPopMatrix();
	glEnable( GL_LIGHTING );
}
/***********************************************************************/
/*                           spatialDisplay()                          */
/***********************************************************************/
void spatialDisplay( void )
{
	printf("spatialDisplay \n");
	glClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
	glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
	glMatrixMode( GL_PROJECTION );

	glLoadIdentity();
	glFrustum( -xy_aspect*.04, xy_aspect*.04, -.04, .04, .1, 15.0 );
	glMatrixMode( GL_MODELVIEW );

	glLoadIdentity();
	glMultMatrixf( lights_rotation );
	glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
	glLightfv(GL_LIGHT2, GL_POSITION, light2_position);

	glLoadIdentity();
	glTranslatef( 0.0, -0.8f, -2.6f );
	glTranslatef( spatial_pos[0], spatial_pos[1], -spatial_pos[2] ); 
	glMultMatrixf( view_spatial_rotate );

	glScalef( scale, scale, scale );
	glPushMatrix();
	if(loadedOBJ && objFlag)
	{
		drawOBJs(objs,colorTable,modelMassTable,modelVolumeTable,modelDensityTable,colorMode,colored_view_enabled,dogFlag);
	}
	if(loadedOffice && officeFlag)
	{
		drawOffice(office);
	}
	if(axisFlag && animateFlag)
	{
		drawAxis(1, 0);
	}
	if(axisFlag && animateFlag==0)
	{
		drawAxis(nAxis, 0);
	}
	if(loadedParticles && animateFlag && animationIteration[numParticles]!=numParticles-1)
	{
		animate(particles, animationIteration, numParticles);
	}
	if(loadedParticles && particleFlag)
	{
//		drawParticles(particles, numParticles);
		drawParticles(particles, particles2, particles3, particles4, numParticles);
	}
	glPopMatrix();	
	glutSwapBuffers(); 
}
/***********************************************************************/
/*                  animationSpatialDisplay()                          */
/***********************************************************************/
void animationSpatialDisplay( void )
{
	printf("animationSpatialDisplay \n");
	glMatrixMode( GL_PROJECTION );

	glLoadIdentity();
	glFrustum( -xy_aspect*.04, xy_aspect*.04, -.04, .04, .1, 15.0 );
	glMatrixMode( GL_MODELVIEW );

	glLoadIdentity();
	glMultMatrixf( lights_rotation );
	glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
	glLightfv(GL_LIGHT2, GL_POSITION, light2_position);

	glLoadIdentity();
	glTranslatef( 0.0, -0.8f, -2.6f );
	glTranslatef( spatial_pos[0], spatial_pos[1], -spatial_pos[2] ); 
	glMultMatrixf( view_spatial_rotate );

	glScalef( scale, scale, scale );
	glPushMatrix();
	if(axisFlag)
	{
//		drawAxis(1, startPointZ);
	}
	if(loadedParticles && animateFlag && animationIteration[numParticles]!=numParticles-1)
	{
		if(currentAxis == 0)
		{
			animate(particles, animationIteration, numParticles);
		}
		else if(currentAxis == 1)
		{
			animate(particles2, animationIteration, numParticles);
		}
		else if(currentAxis == 2)
		{
			animate(particles3, animationIteration, numParticles);
		}
		else if(currentAxis == 3)
		{
			animate(particles4, animationIteration, numParticles);
		}
		printf("animationIteration:%d\n",animationIteration[numParticles]);
	}
	glPopMatrix();	
	glutSwapBuffers();
}
/**********************************************************************************/
/*                                    MAIN                                        */
/**********************************************************************************/
int main(int argc, char* argv[])
{
	/***********************************************************************/
	/*                 Initialize GLUT and Create Windows                  */
	/***********************************************************************/
	glutInit(&argc, argv);
	glutInitDisplayMode( GLUT_RGB | GLUT_DOUBLE | GLUT_DEPTH );


	/***********************************************************************/
	/*                      Setup Spatial Window                           */
	/***********************************************************************/
	/*** Create Window/Setup Callbacks***/
	glutInitWindowPosition( 0, 25 );		// Top Left
	glutInitWindowSize( 580, 500 );
	spatialWindow = glutCreateWindow( "Spatial Intensity" );

	glutDisplayFunc( spatialDisplay );		//Set display function
	GLUI_Master.set_glutReshapeFunc( spatialReshape );  
	GLUI_Master.set_glutKeyboardFunc( myGlutKeyboard );
	GLUI_Master.set_glutSpecialFunc( NULL );
	GLUI_Master.set_glutMouseFunc( myGlutMouse );
	glutMotionFunc( myGlutMotion );			//Set motion function
//	glutTimerFunc (1, animationTimer,0);	//Set animation timer function
	glutIdleFunc(myGlutIdle);			//Set Idle function
	/*** End Create Window/Setup Callbacks***/

	/*** Add Bottom SubWindow ***/
	spatialSubWindow = GLUI_Master.create_glui_subwindow( spatialWindow, GLUI_SUBWINDOW_BOTTOM );
	spatialSubWindow->set_main_gfx_window( spatialWindow );

	GLUI_Rotation *view_rot_spatial = new GLUI_Rotation(spatialSubWindow, "Rotate", view_spatial_rotate );
	view_rot_spatial->set_spin( 1.0 );
	new GLUI_Column( spatialSubWindow, false );
	GLUI_Translation *trans_xy_spatial = new GLUI_Translation(spatialSubWindow, "XY", GLUI_TRANSLATION_XY, spatial_pos );
	trans_xy_spatial->set_speed( .005 );
	new GLUI_Column( spatialSubWindow, false );
	GLUI_Translation *trans_x_spatial = new GLUI_Translation(spatialSubWindow, "X", GLUI_TRANSLATION_X, spatial_pos );
	trans_x_spatial->set_speed( .005 );
	new GLUI_Column( spatialSubWindow, false );
	GLUI_Translation *trans_y_spatial = new GLUI_Translation(spatialSubWindow, "Y", GLUI_TRANSLATION_Y, &spatial_pos[1]);
	trans_y_spatial->set_speed( .005 );
	new GLUI_Column( spatialSubWindow, false );
	GLUI_Translation *trans_z_spatial = new GLUI_Translation(spatialSubWindow, "Z", GLUI_TRANSLATION_Z, &spatial_pos[2]);
	trans_z_spatial->set_speed( .005 );
	/*** End Bottom SubWindow ***/

	/***** Setup OpenGL lights *****/
	glEnable(GL_LIGHTING);
	glEnable( GL_NORMALIZE );

	glEnable(GL_LIGHT0);
	glLightfv(GL_LIGHT0, GL_AMBIENT, light0_ambient);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, light0_diffuse);
	glLightfv(GL_LIGHT0, GL_POSITION, light0_position);

	glEnable(GL_LIGHT1);
	glLightfv(GL_LIGHT1, GL_AMBIENT, light1_ambient);
	glLightfv(GL_LIGHT1, GL_DIFFUSE, light1_diffuse);
	glLightfv(GL_LIGHT1, GL_POSITION, light1_position);

	glEnable(GL_LIGHT2);
	glLightfv(GL_LIGHT2, GL_AMBIENT, light0_ambient);
	glLightfv(GL_LIGHT2, GL_DIFFUSE, light0_diffuse);
	glLightfv(GL_LIGHT2, GL_POSITION, light2_position);

	/***** Setup OpenGL Material Color *****/
	glColorMaterial(GL_FRONT_AND_BACK, GL_EMISSION);
	glEnable(GL_COLOR_MATERIAL);

	/***** Setup OpenGL Transparency *****/
	glEnable( GL_BLEND );
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	/***** Enable z-buferring *****/
	glEnable(GL_DEPTH_TEST);


	/***********************************************************************/
	/*                      Create Control Window                          */
	/***********************************************************************/
	controlWindow = GLUI_Master.create_glui( "Configuration", 0, 600/*2500*/, 25 );

	new GLUI_Button( controlWindow, "Load Objects", LOAD_OBJ, control_cb );
	new GLUI_Button( controlWindow, "Load Particles", LOAD_PARTICLES, control_cb );
	new GLUI_Button( controlWindow, "Load Office", LOAD_OFFICE, control_cb );
	new GLUI_Button( controlWindow, "Draw Objects", DRAW_OBJS, control_cb );
	new GLUI_Button( controlWindow, "Draw Particles", DRAW_PARTICLES, control_cb );
	new GLUI_Button( controlWindow, "Draw Office", DRAW_OFFICE, control_cb );
	new GLUI_Spinner( controlWindow, "# of Axis:", GLUI_SPINNER_INT, &nAxis);

	new GLUI_Button( controlWindow, "Draw Axis", DRAW_AXIS, control_cb );
	new GLUI_Button( controlWindow, "Remove Particles", REMOVE_PARTICLES, control_cb );
	new GLUI_Button( controlWindow, "Animate", ANIMATE, control_cb );
	new GLUI_Checkbox( controlWindow, "Light 0",&light0_enabled,LIGHT0_ENABLED_ID,control_cb);
	new GLUI_Checkbox( controlWindow, "Light 1",&light1_enabled,LIGHT1_ENABLED_ID,control_cb);
	new GLUI_Checkbox( controlWindow, "Light 2",&light2_enabled,LIGHT2_ENABLED_ID,control_cb);
	new GLUI_Checkbox( controlWindow, "Standard View",&std_view_enabled,STD_VIEW,control_cb);
	new GLUI_Checkbox( controlWindow, "XRAY View",&xray_view_enabled,XRAY_VIEW,control_cb);
	new GLUI_Checkbox( controlWindow, "Model View",&model_view_enabled,MODEL_VIEW,control_cb);
	new GLUI_Checkbox( controlWindow, "Colored Model",&colored_view_enabled,COLORED_VIEW,control_cb);
	new GLUI_Checkbox( controlWindow, "Greyscale",&grey_enabled,GREYSCALE,control_cb);
	new GLUI_Checkbox( controlWindow, "Black White",&black_white_enabled,BLACKWHITE,control_cb);
	new GLUI_Checkbox( controlWindow, "Cool",&cool_enabled,COOL,control_cb);
	new GLUI_Checkbox( controlWindow, "Warm",&warm_enabled,WARM,control_cb);
	new GLUI_Checkbox( controlWindow, "Bold",&bold_enabled,BOLD,control_cb);
	new GLUI_Checkbox( controlWindow, "Pastel",&pastel_enabled,PASTEL,control_cb);
	new GLUI_Checkbox( controlWindow, "Color Blind",&blind_enabled,COLORBLIND,control_cb);
	new GLUI_Checkbox( controlWindow, "Light Background",&background_enabled,BACKGROUND,control_cb);
	new GLUI_Checkbox( controlWindow, "Include Dog",&dogFlag,DOG_INCLUDED,control_cb);
	new GLUI_Button( controlWindow, "Decrease Intensity 0", LIGHT0_INTENSITY_ID, control_cb );
	new GLUI_Button( controlWindow, "Increase Intensity 0", LIGHT0_INTENSITY2_ID, control_cb );

	/***********************************************************************/
	/*                     And The Rest....                                */
	/***********************************************************************/

	glutMainLoop();
	return EXIT_SUCCESS;
}
