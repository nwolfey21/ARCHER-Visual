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
    static class CalculateMesh
    {
        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        // ------------------------------------------------
        //                    calculate mesh volume
        // ------------------------------------------------
        // input: vertices and faces of the mesh
        // output: volume
        // external functions: none
        public static double CalculateVolume(List<Coordinate> vertices, List<Coordinate> faces)
        {
            double volume = 0;
            double determinant = 0;
            int point1_index = 0;
            int point2_index = 0;
            int point3_index = 0;
            Coordinate point1 = new Coordinate();
            Coordinate point2 = new Coordinate();
            Coordinate point3 = new Coordinate();
            double p11;
            double p12;
            double p13;
            double p21;
            double p22;
            double p23;
            double p31;
            double p32;
            double p33;

            for (int i = 0; i < faces.Count; i++)
            {
                // get the coordinates (the vertices number) of a certain face
                point1_index = Convert.ToInt32(faces[i].x);
                point2_index = Convert.ToInt32(faces[i].y);
                point3_index = Convert.ToInt32(faces[i].z);

                // get the coordinates of each vertex belonging to that face
                // vertex 1
                point1.x = vertices[point1_index - 1].x;
                point1.y = vertices[point1_index - 1].y;
                point1.z = vertices[point1_index - 1].z;

                // vertex 2
                point2.x = vertices[point2_index - 1].x;
                point2.y = vertices[point2_index - 1].y;
                point2.z = vertices[point2_index - 1].z;

                // vertex 3
                point3.x = vertices[point3_index - 1].x;
                point3.y = vertices[point3_index - 1].y;
                point3.z = vertices[point3_index - 1].z;

                // get the determinant
                p11 = point2.x - point1.x;
                p12 = point2.y - point1.y;
                p13 = point2.z - point1.z;
                p21 = point3.x - point2.x;
                p22 = point3.y - point2.y;
                p23 = point3.z - point2.z;
                p31 = point1.x;
                p32 = point1.y;
                p33 = point1.z;

                determinant = p31 * (p12 * p23 - p13 * p22)
                    - p32 * (p11 * p23 - p13 * p21)
                    + p33 * (p11 * p22 - p12 * p21);

                // update the volume
                volume = volume + 1.0 / 6.0 * determinant;
            }// end for

            return volume;
        }// end class

        //
        public static double CalculateArea(List<Coordinate> vertices, List<Coordinate> faces)
        {
            double area = 0;
            double determinant = 0;
            int point1_index = 0;
            int point2_index = 0;
            int point3_index = 0;
            Coordinate point1 = new Coordinate();
            Coordinate point2 = new Coordinate();
            Coordinate point3 = new Coordinate();
            double p11;
            double p12;
            double p13;
            double p21;
            double p22;
            double p23;

            for (int i = 0; i < faces.Count; i++)
            {
                // get the coordinates (the vertices number) of a certain face
                point1_index = Convert.ToInt32(faces[i].x);
                point2_index = Convert.ToInt32(faces[i].y);
                point3_index = Convert.ToInt32(faces[i].z);

                // get the coordinates of each vertex belonging to that face
                // vertex 1
                point1.x = vertices[point1_index - 1].x;
                point1.y = vertices[point1_index - 1].y;
                point1.z = vertices[point1_index - 1].z;

                // vertex 2
                point2.x = vertices[point2_index - 1].x;
                point2.y = vertices[point2_index - 1].y;
                point2.z = vertices[point2_index - 1].z;

                // vertex 3
                point3.x = vertices[point3_index - 1].x;
                point3.y = vertices[point3_index - 1].y;
                point3.z = vertices[point3_index - 1].z;

                // get the determinant
                p11 = point2.x - point1.x;
                p12 = point2.y - point1.y;
                p13 = point2.z - point1.z;
                p21 = point3.x - point2.x;
                p22 = point3.y - point2.y;
                p23 = point3.z - point2.z;

                determinant = Math.Sqrt((p12 * p23 - p13 * p22) * (p12 * p23 - p13 * p22) +
                    (p11 * p23 - p13 * p21) * (p11 * p23 - p13 * p21) +
                    (p11 * p22 - p12 * p21) * (p11 * p22 - p12 * p21));

                // update the volume
                area = area + 1.0 / 2.0 * determinant;
            }//end for

            return area;
        }//end class

        // ------------------------------------------------
        //                    calculate face normals
        // ------------------------------------------------
        // input: vertices and faces of the mesh
        // output: face normals
        // external functions: none
        public static List<Coordinate> CalculateFaceNormals(List<Coordinate> vertices, List<Coordinate> faces)
        {
            List<Coordinate> faceNormals = new List<Coordinate>();
            int point1_index = 0;
            int point2_index = 0;
            int point3_index = 0;
            Coordinate point1 = new Coordinate();
            Coordinate point2 = new Coordinate();
            Coordinate point3 = new Coordinate();
            Coordinate crossProduct = new Coordinate();
            double p11;
            double p12;
            double p13;
            double p21;
            double p22;
            double p23;
            double norm;

            for (int i = 0; i < faces.Count; i++)
            {
                // get the coordinates (the vertices number) of a certain face
                point1_index = Convert.ToInt32(faces[i].x);
                point2_index = Convert.ToInt32(faces[i].y);
                point3_index = Convert.ToInt32(faces[i].z);

                // get the coordinates of each vertex belonging to that face
                // vertex 1
                point1.x = vertices[point1_index - 1].x;
                point1.y = vertices[point1_index - 1].y;
                point1.z = vertices[point1_index - 1].z;

                // vertex 2
                point2.x = vertices[point2_index - 1].x;
                point2.y = vertices[point2_index - 1].y;
                point2.z = vertices[point2_index - 1].z;

                // vertex 3
                point3.x = vertices[point3_index - 1].x;
                point3.y = vertices[point3_index - 1].y;
                point3.z = vertices[point3_index - 1].z;

                // get the cross product
                p11 = point2.x - point1.x;
                p12 = point2.y - point1.y;
                p13 = point2.z - point1.z;
                p21 = point3.x - point2.x;
                p22 = point3.y - point2.y;
                p23 = point3.z - point2.z;

                crossProduct.x = p12 * p23 - p13 * p22;
                crossProduct.y = -(p11 * p23 - p13 * p21);
                crossProduct.z = p11 * p22 - p12 * p21;

                //normalize
                norm = Math.Sqrt(crossProduct.x * crossProduct.x
                    + crossProduct.y * crossProduct.y
                    + crossProduct.z * crossProduct.z);
                crossProduct.x = crossProduct.x / norm;
                crossProduct.y = crossProduct.y / norm;
                crossProduct.z = crossProduct.z / norm;

                faceNormals.Add(crossProduct);
            }

            return faceNormals;
        }//end class

        // ------------------------------------------------
        //                    calculate vertex normals
        // ------------------------------------------------
        // input: vertices and faces of the mesh
        // output: vertex normals
        // external functions: CalculateFaceNormals
        public static List<Coordinate> CalculateVertexNormals(List<Coordinate> vertices, List<Coordinate> faces)
        {
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<Coordinate> faceNormals = new List<Coordinate>();
            Coordinate point = new Coordinate();
            Coordinate temp = new Coordinate();
            double norm;

            // calculate face normals
            faceNormals.AddRange(CalculateFaceNormals(vertices, faces));

            // initialize vertex normals
            point.x = 0;
            point.y = 0;
            point.z = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                vertexNormals.Add(point);
            }

            for (int i = 0; i < faces.Count; i++)
            {
                // pay attention to the way of parameter passing!
                // vertex 1
                // faces[i].x: index of the 1st vetex of the ith face
                temp = vertexNormals[Convert.ToInt32(faces[i].x) - 1];
                temp.x = temp.x + faceNormals[i].x;
                temp.y = temp.y + faceNormals[i].y;
                temp.z = temp.z + faceNormals[i].z;
                vertexNormals[Convert.ToInt32(faces[i].x) - 1] = temp;

                // vertex 2
                temp = vertexNormals[Convert.ToInt32(faces[i].y) - 1];
                temp.x = temp.x + faceNormals[i].x;
                temp.y = temp.y + faceNormals[i].y;
                temp.z = temp.z + faceNormals[i].z;
                vertexNormals[Convert.ToInt32(faces[i].y) - 1] = temp;

                // vertex 3
                temp = vertexNormals[Convert.ToInt32(faces[i].z) - 1];
                temp.x = temp.x + faceNormals[i].x;
                temp.y = temp.y + faceNormals[i].y;
                temp.z = temp.z + faceNormals[i].z;
                vertexNormals[Convert.ToInt32(faces[i].z) - 1] = temp;
            }

            //normalize
            for (int i = 0; i < vertices.Count; i++)
            {
                temp = vertexNormals[i];
                norm = Math.Sqrt(temp.x * temp.x + temp.y * temp.y + temp.z * temp.z);
                temp.x = temp.x / norm;
                temp.y = temp.y / norm;
                temp.z = temp.z / norm;
                vertexNormals[i] = temp;
            }

            return vertexNormals;
        }//end class

        //
        public static Coordinate CalculateCentroid(List<Coordinate> vertices)
        {
            Coordinate centroid = new Coordinate();
            int vertexNumber = vertices.Count;

            //calculate centroid
            for (int i = 0; i < vertexNumber; i++)
            {
                centroid.x = centroid.x + vertices[i].x;
                centroid.y = centroid.y + vertices[i].y;
                centroid.z = centroid.z + vertices[i].z;
            }

            centroid.x = centroid.x / vertexNumber;
            centroid.y = centroid.y / vertexNumber;
            centroid.z = centroid.z / vertexNumber;

            return centroid;
        }//end method

        //
        public static List<Coordinate> CalculateCentroidVertexVectors(List<Coordinate> vertices, Coordinate centroid)
        {
            Coordinate temp = new Coordinate();
            List<Coordinate> centroidVector = new List<Coordinate>();

            int vertexNumber = vertices.Count;

            //calculate centroid vector
            for (int i = 0; i < vertexNumber; i++)
            {
                temp.x = vertices[i].x - centroid.x;
                temp.y = vertices[i].y - centroid.y;
                temp.z = vertices[i].z - centroid.z;
                centroidVector.Add(temp);
            }

            return centroidVector;
        }//end method

        //
        public static double CalculateCircumference(List<List<Coordinate>> mesh, double desiredHeight, string bodyPart)
        {
            double circumference = 0;
            int point1_index = 0;
            int point2_index = 0;
            int point3_index = 0;
            Coordinate point1 = new Coordinate();
            Coordinate point2 = new Coordinate();
            Coordinate point3 = new Coordinate();
            Coordinate pointCouple1 = new Coordinate();
            Coordinate pointCouple2 = new Coordinate();
            Coordinate pointSingle = new Coordinate();
            Coordinate pointIntersection1 = new Coordinate();
            Coordinate pointIntersection2 = new Coordinate();
            double delta1 = 0;
            double delta2 = 0;
            double delta3 = 0;
            double k1 = 0;
            double k2 = 0;
            double length = 0;
            double plane = 0;
            double limit = 30;

            // get ky
            double heightScale = desiredHeight / GlobalConstant.standardHeight;
            Coordinate centroid = CalculateCentroid(mesh[0]);

            List<Coordinate> vertices = mesh[0];
            List<Coordinate> faces = mesh[1];
            int faceNumber = faces.Count;

            if (bodyPart == "waist")
            {
                plane = heightScale * (GlobalConstant.standardWaistPlane - centroid.y) + centroid.y;
            }
            else if (bodyPart == "hip")
            {
                plane = heightScale * (GlobalConstant.standardHipPlane - centroid.y) + centroid.y;
            }

            for (int i = 0; i < faceNumber; i++)
            {
                // get the coordinates (the vertices number) of a certain face
                point1_index = Convert.ToInt32(faces[i].x);
                point2_index = Convert.ToInt32(faces[i].y);
                point3_index = Convert.ToInt32(faces[i].z);

                // get the coordinates of each vertex belonging to that face
                // vertex 1
                point1.x = vertices[point1_index - 1].x;
                point1.y = vertices[point1_index - 1].y;
                point1.z = vertices[point1_index - 1].z;

                // vertex 2
                point2.x = vertices[point2_index - 1].x;
                point2.y = vertices[point2_index - 1].y;
                point2.z = vertices[point2_index - 1].z;

                // vertex 3
                point3.x = vertices[point3_index - 1].x;
                point3.y = vertices[point3_index - 1].y;
                point3.z = vertices[point3_index - 1].z;

                //test if the triangle intersects with the plane
                delta1 = point1.y - plane;
                delta2 = point2.y - plane;
                delta3 = point3.y - plane;
                bool isTriangleIntersectingWithPlane = !((delta1 > 0 && delta2 > 0 && delta3 > 0)
                    || (delta1 < 0 && delta2 < 0 && delta3 < 0));

                if (isTriangleIntersectingWithPlane == true)//if it intersects with the plane
                {
                    if (delta1 * delta2 > 0) // point 1 and point 2 are on the same side
                    {
                        pointSingle = point3;
                        pointCouple1 = point1;
                        pointCouple2 = point2;
                    }
                    else if (delta1 * delta2 < 0) // point 1 and point 2 are on the different side
                    {
                        if (delta1 * delta3 > 0) // point 1 and point 3 are on the same side
                        {
                            pointSingle = point2;
                            pointCouple1 = point1;
                            pointCouple2 = point3;
                        }
                        else if (delta1 * delta3 < 0) // point 1 and point 3 are on the different side
                        {
                            pointSingle = point1;
                            pointCouple1 = point2;
                            pointCouple2 = point3;
                        }//end if
                    }//end if

                    //calculate the intersection point
                    k1 = (plane - pointCouple1.y) / (pointSingle.y - pointCouple1.y);
                    k2 = (plane - pointCouple2.y) / (pointSingle.y - pointCouple2.y);

                    pointIntersection1.x = k1 * (pointSingle.x - pointCouple1.x) + pointCouple1.x;
                    pointIntersection1.y = plane;
                    pointIntersection1.z = k1 * (pointSingle.z - pointCouple1.z) + pointCouple1.z;

                    pointIntersection2.x = k2 * (pointSingle.x - pointCouple2.x) + pointCouple2.x;
                    pointIntersection2.y = plane;
                    pointIntersection2.z = k2 * (pointSingle.z - pointCouple2.z) + pointCouple2.z;

                    //test if the intersection points are in the valid area
                    bool isValidArea = Math.Abs(pointIntersection1.x) < limit &&
                        Math.Abs(pointIntersection2.x) < limit;
                    if (isValidArea == true)
                    {
                        //calculate the length of the intersection
                        length = Math.Sqrt((pointIntersection1.x - pointIntersection2.x) * (pointIntersection1.x - pointIntersection2.x)
                            + (pointIntersection1.y - pointIntersection2.y) * (pointIntersection1.y - pointIntersection2.y)
                            + (pointIntersection1.z - pointIntersection2.z) * (pointIntersection1.z - pointIntersection2.z));

                        //update the wc calculation
                        circumference = circumference + length;
                    }

                }//end if

            }// end for
            return circumference;
        }
    }// end class
}// end namespace