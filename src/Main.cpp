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
#include "classes.h"
/* OpenGL Code Headers */
#include <GL/glui.h>

/*************External Functions***************/
// Updated
float drawSpatial(int N, int flag);
void drawOBJs(meshOBJ *obj);

/*************Internal Functions***************/
void spatialReshape( int x, int y );
void spatialDisplay( void );

float xy_aspect;
int   last_x, last_y;
float rotationX = 0.0, rotationY = 0.0;

/** These are the live variables passed into GLUI ***/
int   light0_enabled = 1;
int   light1_enabled = 1;
float light0_intensity = 1.0;
float light1_intensity = .4;
int   spatialWindow;
float scale = 1.0;
int   show_spatial=0;
float spatial_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_spatial_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float spatial_pos[] = { 0.0, 0.0, 0.0 };
float sphere_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float torus_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float obj_pos[] = { 0.0, 0.0, 0.0 };

/** Pointers to the windows and some of the controls we'll create **/
GLUI            *spatialSubWindow,*controlWindow,*spatialBottomSubWIndow;
GLUI_Spinner    *light0_spinner, *light1_spinner;

/********** User IDs for callbacks ********/
#define LIGHT0_ENABLED_ID    200
#define LIGHT1_ENABLED_ID    201
#define LIGHT0_INTENSITY_ID  250
#define LIGHT1_INTENSITY_ID  260
#define ENABLE_ID            300
#define DISABLE_ID           301
#define SHOW_ID              302
#define HIDE_ID              303
#define PAUSE		     233
#define LOAD 				 235
#define DRAW 				 236

/********** Miscellaneous global variables **********/
GLfloat light0_ambient[] =  {0.1f, 0.1f, 0.3f, 1.0f};
GLfloat light0_diffuse[] =  {.6f, .6f, 1.0f, 1.0f};
GLfloat light0_position[] = {.5f, .5f, 1.0f, 0.0f};

GLfloat light1_ambient[] =  {0.1f, 0.1f, 0.3f, 1.0f};
GLfloat light1_diffuse[] =  {.9f, .6f, 0.0f, 1.0f};
GLfloat light1_position[] = {-1.0f, -1.0f, 1.0f, 0.0f};

GLfloat lights_rotation[16] = {1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };


//-----Approved Variables-----//
meshOBJ objs[138];
int loaded = 0;					//0 objects have not been loaded, 1 objects have been loaded

/***********************************************************************/
/*                          control_cb()                               */
/*                      GLUI control callback                          */
/***********************************************************************/
void control_cb( int control )
{
	printf("control_cb \n");
	if ( control == LIGHT0_ENABLED_ID ) 
	{
		if ( light0_enabled ) 
		{
			glEnable( GL_LIGHT0 );
			light0_spinner->enable();
		}
		else 
		{
			glDisable( GL_LIGHT0 ); 
			light0_spinner->disable();
		}
	}
	else if ( control == LIGHT1_ENABLED_ID ) 
	{
		if ( light1_enabled ) 
		{
			glEnable( GL_LIGHT1 );
			light1_spinner->enable();
		}
		else 
		{
			glDisable( GL_LIGHT1 ); 
			light1_spinner->disable();
		}
	}
	else if ( control == LIGHT0_INTENSITY_ID ) 
	{
		float v[] = {light0_diffuse[0], light0_diffuse[1], light0_diffuse[2], light0_diffuse[3]};
		v[0] *= light0_intensity;
		v[1] *= light0_intensity;
		v[2] *= light0_intensity;
		glLightfv(GL_LIGHT0, GL_DIFFUSE, v );
	}
	else if ( control == LIGHT1_INTENSITY_ID ) 
	{
		float v[] = {light1_diffuse[0], light1_diffuse[1], light1_diffuse[2], light1_diffuse[3]};
		v[0] *= light1_intensity;
		v[1] *= light1_intensity;
		v[2] *= light1_intensity;
		glLightfv(GL_LIGHT1, GL_DIFFUSE, v );
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
	else if(control == LOAD)
	{
		printf("Load call back\n");
		loaded = 1;
		char filepath[200];
		for(int i=0;i<138;i++)
		{
			sprintf(filepath,"/home/noah/Documents/ARCHER/ARCHER-Visual/data/73_adult_male/073kg/%03d.obj",i+1);
			std::cout << "loading file: " << filepath << std::endl;
			objs[i].setPath(filepath);
			objs[i].loadOBJ();
		}
	}
	else if(control == DRAW)
	{	
		printf("Draw call back\n");
		glutSetWindow(spatialWindow);
		spatialDisplay();
	}
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
		glutSetWindow(spatialWindow);  
	glutPostRedisplay();
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
	glVertex3f( 1.77f, -0.05f, 0.0 );  glVertex3f( 1.9, -0.25f, 0.0 ); 
	glVertex3f( 1.77f, -.25f, 0.0 );  glVertex3f( 1.9, -0.05f, 0.0 );
	/* Letter Y */
	glColor3f( 0.0, 0.0, 0.0 );
	glVertex3f( -0.05f, 1.85f, 0.0 );  glVertex3f( -0.1, 1.74f, 0.0 ); 
	glVertex3f( -0.15f, 1.85f, 0.0 );  glVertex3f( -0.1, 1.74f, 0.0 );
	glVertex3f( -0.1f, 1.6f, 0.0 );  glVertex3f( -0.1, 1.74f, 0.0 ); 
	/* X axis */
	glColor3f( 1.0, 0.0, 0.0 );
	glVertex3f( 0.0, 0.0, 0.0 );  glVertex3f( 2.0, 0.0, 0.0 ); 
	/* Y axis */
	glColor3f( 0.0, 0.0, 0.0 );
	glVertex3f( 0.0, 0.0, 0.0 );  glVertex3f( 0.0, 2.0, 0.0 ); 
	glEnd();
	glPopMatrix();
	glEnable( GL_LIGHTING );
}
/***********************************************************************/
/*                           spatialDisplay()                           */
/***********************************************************************/
void spatialDisplay( void )
{
	float intensity = 0;
	printf("spatialDisplay \n");
	glClearColor( .9f, .9f, .9f, 1.0f );
	glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
	glMatrixMode( GL_PROJECTION );

	glLoadIdentity();
	glFrustum( -xy_aspect*.04, xy_aspect*.04, -.04, .04, .1, 15.0 );
	glMatrixMode( GL_MODELVIEW );

	glLoadIdentity();
	glMultMatrixf( lights_rotation );
	glLightfv(GL_LIGHT0, GL_POSITION, light0_position);

	glLoadIdentity();
	glTranslatef( 0.0, 0.0, -2.6f );
	glTranslatef( spatial_pos[0], spatial_pos[1], -spatial_pos[2] ); 
	glMultMatrixf( view_spatial_rotate );

	glScalef( scale, scale, scale );

	glPushMatrix();
	glTranslatef( 0.0, 0.0, 0.0 );
	glMultMatrixf( spatial_rotate );

	draw_spatial_axes(.52f);

	glTranslatef( 0.0, 0.0, 0.0 );
	glMultMatrixf( spatial_rotate );

//	drawSpatial(0, show_spatial);
	printf("I passed through here\n");
	if(loaded)
	{
		printf("and here\n");
		drawOBJs(objs);
	}

	glEnable( GL_LIGHTING );

	glutSwapBuffers(); 
}
/**********************************************************************************/
/*                                    MAIN                                        */
/**********************************************************************************/
int main(int argc, char* argv[])
{
	int i;
	for(i = 1; i < argc; ++i) 
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

		glutDisplayFunc( spatialDisplay );
		GLUI_Master.set_glutReshapeFunc( spatialReshape );  
		GLUI_Master.set_glutKeyboardFunc( myGlutKeyboard );
		GLUI_Master.set_glutSpecialFunc( NULL );
		GLUI_Master.set_glutMouseFunc( myGlutMouse );
		glutMotionFunc( myGlutMotion );
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
		/***** End OpenGL lights *****/

		/***** Enable z-buferring *****/
		glEnable(GL_DEPTH_TEST);


		/***********************************************************************/
		/*                      Create Control Window                          */
		/***********************************************************************/
		controlWindow = GLUI_Master.create_glui( "Configuration", 0, 600/*2500*/, 25 );

		new GLUI_Button( controlWindow, "Load Objects", LOAD, control_cb );
		new GLUI_Button( controlWindow, "Draw Objects", DRAW, control_cb );

		/***********************************************************************/
		/*                     And The Rest....                                */
		/***********************************************************************/

		glutMainLoop();
		return EXIT_SUCCESS;
		break;
	}
}
