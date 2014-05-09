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
    //Use a static class as a unit of organization for methods not associated with particular objects. 
    //Also, a static class can make your implementation simpler and faster because you do not have to create an object in order to call its methods.
    //Note: variables are private by default. This holds in a public method.
    static class FileIO
    {
        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        // ------------------------------------------------
        //                    import obj file
        // ------------------------------------------------
        public static List<List<Coordinate>> ImportObj()
        {
            // variables
            int totalLineNumber = 0;
            int lineNumber = 0;
            // double progressRatio = 0;
            string line = null;
            int vertexNumber = 0;
            int faceNumber = 0;
            int vertexNormalNumber = 0; // test
            bool noData;
            Coordinate point = new Coordinate();
            List<Coordinate> vertices = new List<Coordinate>();
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<Coordinate> faces = new List<Coordinate>();
            List<List<Coordinate>> mesh = new List<List<Coordinate>>();
            System.Text.RegularExpressions.MatchCollection matches = null;

            // create dialog
            OpenFileDialog importDataDialog = new OpenFileDialog();
            importDataDialog.Title = "Import Obj";
            importDataDialog.Filter = "Wavefront File (*.obj)|*.obj|All Files (*.*)|*.*";
            importDataDialog.InitialDirectory = @"D:\study\deformation\Obese Phantom Interpolation";

            // open file
            if (importDataDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // count the number of lines
                    using (StreamReader importLineByLine = new StreamReader(importDataDialog.FileName))
                    {
                        while ((line = importLineByLine.ReadLine()) != null)
                        {
                            totalLineNumber++;
                        }
                    }

                    #region progressForm, need to be packaged into a separate class

                    //// create a new form
                    //Form progressForm = new Form();
                    //progressForm.StartPosition = FormStartPosition.CenterScreen;
                    //progressForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    //progressForm.Size = new Size(600, 250);
                    //progressForm.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    //// create a label
                    //Label progressLabel = new Label();
                    //progressLabel.Parent = progressForm;
                    //progressLabel.AutoSize = true;
                    //progressLabel.Text = "Importing Obj File...";
                    //progressLabel.TextAlign = ContentAlignment.MiddleCenter;
                    //progressLabel.Location = new Point((progressLabel.Parent.ClientSize.Width - progressLabel.Width) / 2, 50);

                    //// create a progressbar
                    //ProgressBar showProgress = new ProgressBar();
                    //showProgress.Parent = progressForm;
                    ////progressForm.Controls.Add(showProgress); //also okay
                    //showProgress.Top = 120;
                    //showProgress.Left = 50;
                    //showProgress.Height = 50;
                    //showProgress.Width = showProgress.Parent.ClientSize.Width - 2 * showProgress.Left;
                    //showProgress.Style = ProgressBarStyle.Continuous;
                    //showProgress.Minimum = 1;
                    //showProgress.Maximum = totalLineNumber;
                    //showProgress.Value = 1;
                    //showProgress.Step = 1;

                    //// display the entire new form
                    //progressForm.Show();

                    #endregion

                    // import data into array
                    using (StreamReader importLineByLine = new StreamReader(importDataDialog.FileName))
                    {
                        // string formatString = null;
                        string regularExpression = null;
                        while ((line = importLineByLine.ReadLine()) != null)
                        {
                            lineNumber++;
                            //progressRatio = Convert.ToDouble(lineNumber) / Convert.ToDouble(totalLineNumber) * 100.0;
                            //formatString = "Importing Obj File: {0:#0.00} % Completed";
                            //progressLabel.Text = string.Format(formatString, progressRatio);
                            //showProgress.PerformStep();

                            // if the line does not contain data, jump to the next loop
                            noData = line.Length <= 2;
                            if (noData == true)
                            {
                                continue;
                            }

                            // collect information on vertices and faces
                            switch (line[0].ToString() + line[1].ToString())
                            {
                                // vertices
                                case "v ":
                                    {
                                        vertexNumber++;
                                        regularExpression = @"[^v]\S+"; // match conditions: no v, no white space, contiguous character
                                        matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression); // matches are a collection of objects
                                        // must use a struct (value type) rather than an array(reference type)!!!
                                        point.x = Double.Parse(matches[0].Value);
                                        point.y = Double.Parse(matches[1].Value);
                                        point.z = Double.Parse(matches[2].Value);
                                        vertices.Add(point);
                                        //MessageBox.Show(vertices[0].x.ToString());
                                        break;
                                    }

                                // faces
                                case "f ":
                                    {
                                        faceNumber++;
                                        regularExpression = @"\d+"; // decimal numbers
                                        matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression);
                                        point.x = Double.Parse(matches[0].Value);
                                        point.y = Double.Parse(matches[2].Value);
                                        point.z = Double.Parse(matches[4].Value);
                                        faces.Add(point);
                                        //MessageBox.Show(faces[0].x.ToString());
                                        break;
                                    }

                                // test
                                case "vn":
                                    {
                                        vertexNormalNumber++;
                                        regularExpression = @"[^(vn)]\S+";
                                        matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression);
                                        point.x = Double.Parse(matches[0].Value);
                                        point.y = Double.Parse(matches[1].Value);
                                        point.z = Double.Parse(matches[2].Value);
                                        vertexNormals.Add(point);
                                        break;
                                    }

                                // empty lines, comment lines, vn, g, etc...
                                default:
                                    {
                                        break;
                                    }
                            }// end switch
                        }// end while
                    }// end using

                    // delete the progress form
                    //progressForm.Dispose();
                    MessageBox.Show("totalLineNumber = " + totalLineNumber.ToString() + "\n" +
                        "vertexNumber = " + vertexNumber.ToString() + "\n" +
                        "faceNumber = " + faceNumber.ToString() + "\n" +
                        "vertexNumber in list = " + vertices.Count.ToString() + "\n" +
                        "faceNumber in list = " + faces.Count.ToString() + "\n" +
                        "vertices[0].x = " + vertices[0].x.ToString() + "\n" +
                        "vertex normals[0].x = " + vertexNormals[0].x.ToString() + "\n" +
                        "faces[0].x = " + faces[0].x.ToString() + "\n" +
                        "vertexNormalNumber = " + vertexNormalNumber.ToString());
                    mesh.Add(vertices);
                    mesh.Add(faces);
                    mesh.Add(vertexNormals);
                }//end try

                catch (Exception importDataException)
                {
                    MessageBox.Show("Failed to Import the Obj File\n" + importDataException.Message + lineNumber);
                }

            }// end if
            return mesh;
        }// end ImportObj

        // ------------------------------------------------
        //                    import sample obj file
        // ------------------------------------------------
        public static List<List<Coordinate>> ImportSampleObj(string filepath)
        {
            // variables
            int lineNumber = 0;
            string line = null;
            bool noData;
            Coordinate point = new Coordinate();
            List<Coordinate> vertices = new List<Coordinate>();
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<Coordinate> faces = new List<Coordinate>();
            List<List<Coordinate>> mesh = new List<List<Coordinate>>();
            System.Text.RegularExpressions.MatchCollection matches = null;

            try
            {
                // import data into array
                using (StreamReader importLineByLine = new StreamReader(filepath))
                {
                    // string formatString = null;
                    string regularExpression = null;
                    while ((line = importLineByLine.ReadLine()) != null)
                    {
                        lineNumber++;

                        // if the line does not contain data, jump to the next loop
                        noData = line.Length <= 2;
                        if (noData == true)
                        {
                            continue;
                        }

                        // collect information on vertices and faces
                        switch (line[0].ToString() + line[1].ToString())
                        {
                            // vertices
                            case "v ":
                                {
                                    regularExpression = @"[^v]\S+"; // match conditions: no v, no white space, contiguous character
                                    matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression);
                                    // must use a struct (value type) rather than an array(reference type)!!!
                                    point.x = Double.Parse(matches[0].Value);
                                    point.y = Double.Parse(matches[1].Value);
                                    point.z = Double.Parse(matches[2].Value);
                                    vertices.Add(point);
                                    //MessageBox.Show(vertices[0].x.ToString());
                                    break;
                                }

                            // faces
                            case "f ":
                                {
                                    regularExpression = @"\d+"; // decimal numbers
                                    matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression);
                                    point.x = Double.Parse(matches[0].Value);
                                    point.y = Double.Parse(matches[2].Value);
                                    point.z = Double.Parse(matches[4].Value);
                                    faces.Add(point);
                                    //MessageBox.Show(faces[0].x.ToString());
                                    break;
                                }

                            // for statistics only. vn is not needed
                            case "vn":
                                {
                                    regularExpression = @"[^(vn)]\S+";
                                    matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression);
                                    point.x = Double.Parse(matches[0].Value);
                                    point.y = Double.Parse(matches[1].Value);
                                    point.z = Double.Parse(matches[2].Value);
                                    vertexNormals.Add(point);
                                    break;
                                }

                            // empty lines, comment lines, vn, g, etc...
                            default:
                                {
                                    break;
                                }
                        }// end switch
                    }// end while
                }// end using

                // delete the progress form
                //progressForm.Dispose();
                MessageBox.Show("number of lines = " + lineNumber.ToString() + "\n" +
                    "number of vertices = " + vertices.Count.ToString() + "\n" +
                    "number of faces = " + faces.Count.ToString() + "\n" +
                    "number of vertex normals = " + vertexNormals.Count.ToString() + "\n" +
                    "vertices[0].x = " + vertices[0].x.ToString() + "\n" +
                    "vertex normals[0].x = " + vertexNormals[0].x.ToString() + "\n" +
                    "faces[0].x = " + faces[0].x.ToString());

                // concatenate vertices, faces and vertexnormals
                mesh.Add(vertices);
                mesh.Add(faces);
                mesh.Add(vertexNormals); // vertex normals are never used

            }//end try

            catch (Exception importDataException)
            {
                MessageBox.Show("Failed to Import the Obj File\n" + importDataException.Message + lineNumber);
            }

            return mesh;
        }// end ImportSampleObj

        // ------------------------------------------------
        //                    export obj file
        // ------------------------------------------------
        public static void ExportObj(List<List<Coordinate>> mesh)
        {
            // variables
            int i = 0;
            List<Coordinate> vertices = mesh[0]; // vertices
            List<Coordinate> faces = mesh[1]; //faces
            List<Coordinate> vertexNormals = mesh[2]; // vertex normals
            int vertexNumber = vertices.Count;
            int faceNumber = faces.Count;
            int vertexNormalNumber = vertexNormals.Count;

            // create dialog
            SaveFileDialog exportDataDialog = new SaveFileDialog();
            exportDataDialog.Title = "Export Obj";
            exportDataDialog.Filter = "Wavefront File (*.obj)|*.obj|All Files (*.*)|*.*";
            exportDataDialog.InitialDirectory = @"D:\study\deformation\Obese Phantom Interpolation";
            exportDataDialog.DefaultExt = "obj";
            exportDataDialog.OverwritePrompt = true;

            // create file
            if (exportDataDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // export line by line
                    using (StreamWriter exportLineByLine = new StreamWriter(exportDataDialog.FileName))
                    {
                        string formatString = null;
                        // write the starting comment line and blank line
                        formatString = "# Rhino\n";
                        exportLineByLine.WriteLine(formatString);

                        // write vertices
                        for (i = 0; i < vertexNumber; i++)
                        {
                            formatString = "v {0} {1} {2}";
                            exportLineByLine.WriteLine(formatString, vertices[i].x, vertices[i].y, vertices[i].z);
                        }

                        // write vertex normals
                        for (i = 0; i < vertexNormalNumber; i++)
                        {
                            formatString = "vn {0} {1} {2}";
                            exportLineByLine.WriteLine(formatString, vertexNormals[i].x, vertexNormals[i].y, vertexNormals[i].z);
                        }

                        // write faces
                        for (i = 0; i < faceNumber; i++)
                        {
                            formatString = "f {0}//{0} {1}//{1} {2}//{2}";
                            exportLineByLine.WriteLine(formatString, faces[i].x, faces[i].y, faces[i].z);
                        }

                        MessageBox.Show("File saved.");

                    }// end using
                }//end try

                catch (Exception exportDataException)
                {
                    MessageBox.Show("Failed to Export the Obj File\n" + exportDataException.Message);
                }
            }// end if
        }//end ExportObj

    } //end class

}//end namespace