//
//  vertex.h
//  importOBJ
//
//  Created by Noah Wolfe on 5/8/14.
//  Copyright (c) 2014 Noah Wolfe. All rights reserved.
//

#ifndef __importOBJ__classes__
#define __importOBJ__classes__

#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>

//---------------------vertex Class-------------------------//
class vertex
{
private:
    float p[3];
public:
    vertex();
    vertex(float p1, float p2, float p3);   //Initialization constructor
    vertex(vertex const &v);                //Copy constructor
    void print();
};

//--------------------face Class----------------------------//
class face
{
private:
    vertex v[3];
    vertex vN[3];
public:
    face();
    face(vertex v1, vertex v2, vertex v3, vertex vN1, vertex vN2, vertex vN3);
    face(face const&f);
    void printVertex();
    void printVertexNormal();
};

//---------------------meshOBJ Class------------------------//
class meshOBJ
{
private:
    std::vector<face> faces;
    std::string filePath;
	int valid;					//1 then obj loaded correctly, 0 didn't load correctly
public:
	meshOBJ();
    meshOBJ(std::string path);
	void setPath(std::string path);
    void addFace(face newFace);
    unsigned long getSize();
    void print();
    void loadOBJ();
};

#endif /* defined(__importOBJ__vertex__) */
