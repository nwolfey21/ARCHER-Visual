//
//  main.cpp
//  importOBJ
//
//  Created by Noah Wolfe on 5/8/14.
//  Copyright (c) 2014 Noah Wolfe. All rights reserved.
//

#include "classes.h"
/* OpenGL Code Headers */
#include <GL/glui.h>

using namespace std;

//These variables will need to be passed into the function

int main(int argc, const char * argv[])
{
	char* filepath;
	meshOBJ objs[138];
	for(int i=0;i<138;i++)
	{
		cout << "here" << endl;
		sprintf(filepath,"/home/noah/Documents/ARCHER/ARCHER-Visual/data/73_adult_male/073kg/%03d.obj",i+1);
		cout << "loading file: " << filepath << endl;
		objs[i].setPath(filepath);
		objs[i].loadOBJ();
	}
	cin >> filepath;
    return 0;
}

