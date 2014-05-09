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
    static class UpdateVertices
    {
        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        // ------------------------------------------------------------------------
        //                update vertices for SameH_DifferentBMI
        // ------------------------------------------------------------------------
        // input: bullet and hole vertices, specified scale factor
        // output: new vertices
        // external function: 
        public static List<Coordinate> SameH_DifferentBMI
            (List<Coordinate> bulletVertices,
            List<Coordinate> holeVertices,
            double scaleFactor)
        {
            List<Coordinate> newVertices = new List<Coordinate>();
            Coordinate point = new Coordinate();
            int vertexNumber = 0;

            vertexNumber = bulletVertices.Count;

            // derive new points memberwise, i.e, performing the same method on each component x, y, z
            for (int i = 0; i < vertexNumber; i++)
            {
                point.x = scaleFactor * holeVertices[i].x + (1 - scaleFactor) * bulletVertices[i].x;
                point.y = scaleFactor * holeVertices[i].y + (1 - scaleFactor) * bulletVertices[i].y;
                point.z = scaleFactor * holeVertices[i].z + (1 - scaleFactor) * bulletVertices[i].z;
                newVertices.Add(point);
            }
            return newVertices;
        }

        // ------------------------------------------------------------------------
        //                update vertices for arbitary x, y, z scale factors ( particularly SameBMI_DifferentH )
        // ------------------------------------------------------------------------
        // input: new vertices, specified scale factor
        // output: new vertices
        // external function:
        public static List<Coordinate> ProportionalScaler(List<Coordinate> vertices,
            double xScaleFactor, double yScaleFactor, double zScaleFactor)
        {
            List<Coordinate> newVertices = new List<Coordinate>();
            Coordinate point = new Coordinate();
            double xMax, yMax, zMax;
            double xMin, yMin, zMin;
            double tx, ty, tz;
            int vertexNumber;

            vertexNumber = vertices.Count;

            // get x, y, z maxima and minima
            xMin = vertices[0].x;
            xMax = vertices[0].x;
            yMin = vertices[0].y;
            yMax = vertices[0].y;
            zMin = vertices[0].z;
            zMax = vertices[0].z;

            // note i should start at 1
            for (int i = 1; i < vertexNumber; i++)
            {
                if (xMax < vertices[i].x)
                {
                    xMax = vertices[i].x;
                }
                if (xMin > vertices[i].x)
                {
                    xMin = vertices[i].x;
                }

                if (yMax < vertices[i].y)
                {
                    yMax = vertices[i].y;
                }
                if (yMin > vertices[i].y)
                {
                    yMin = vertices[i].y;
                }

                if (zMax < vertices[i].z)
                {
                    zMax = vertices[i].z;
                }
                if (zMin > vertices[i].z)
                {
                    zMin = vertices[i].z;
                }
            }



            for (int i = 0; i < vertexNumber; i++)
            {
                // get intrinsic coefficient tx, ty, tz
                tx = (vertices[i].x - xMin) / (xMax - xMin);
                ty = (vertices[i].y - yMin) / (yMax - yMin);
                tz = (vertices[i].z - zMin) / (zMax - zMin);
                // get new vertices
                point.x = tx * xScaleFactor * xMax + (1 - tx * xScaleFactor) * xMin;
                point.y = ty * yScaleFactor * yMax + (1 - ty * yScaleFactor) * yMin;
                point.z = tz * zScaleFactor * zMax + (1 - tz * zScaleFactor) * zMin;
                newVertices.Add(point);
            }

            return newVertices;
        }

    }// end class
}// end namespace