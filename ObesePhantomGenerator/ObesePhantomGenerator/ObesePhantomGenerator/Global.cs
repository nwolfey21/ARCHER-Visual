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
    // declare a struct to record the coordinate of vertices, vertexnomals and faces
    public struct Coordinate
    {
        public double x, y, z;
    }

    public struct EachMeshInfo
    {
        public int id;
        public double mass;
        public double density;
        public double volume;
    }

    public class GlobalConstant
    {
        public const double standardHeight = 1.76; // [m]
        public const double standardDensity = 1.07 * 1000.0; // [kg/m^3]
        public const double epsilon = 1e-6;
        public const double densitySAT = 0.92;
        public const double densityVAT = 0.92;
        public const double densityResidualTissue = 1.05;
        public const double densitySkin = 0.45; //0.36
        public const double thicknessSkin = 0.394513; //[cm]
        public const double standardWaistPlane = 107.5; //[cm] the plane (perpendicular to y axis) where WC is measured
        public const double standardHipPlane = 92.42; //[cm] the plane (perpendicular to y axis) where HC is measured

    }

}// end namespace