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
        //                    import obj file using a dialog
        // ------------------------------------------------
        public static List<List<Coordinate>> ImportObjWithDialog()
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
            importDataDialog.InitialDirectory = Application.StartupPath + @"\FreeInput"; //Gets the path for the executable file that started the application, not including the executable name. 
            //importDataDialog.InitialDirectory = @"D:\study\deformation\Obese Phantom Interpolation";

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

                    //mesh info
                    /*MessageBox.Show("totalLineNumber = " + totalLineNumber.ToString() + "\n" +
                        "vertexNumber = " + vertexNumber.ToString() + "\n" +
                        "faceNumber = " + faceNumber.ToString() + "\n" +
                        "vertexNumber in list = " + vertices.Count.ToString() + "\n" +
                        "faceNumber in list = " + faces.Count.ToString() + "\n" +
                        "vertices[0].x = " + vertices[0].x.ToString() + "\n" +
                        "vertex normals[0].x = " + vertexNormals[0].x.ToString() + "\n" +
                        "faces[0].x = " + faces[0].x.ToString() + "\n" +
                        "vertexNormalNumber = " + vertexNormalNumber.ToString());*/

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
        //                    import obj file not using a dialog
        // ------------------------------------------------
        public static List<List<Coordinate>> ImportObjWithoutDialog(string filePath)
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
                using (StreamReader importLineByLine = new StreamReader(filePath))
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
                                    point.x = Convert.ToDouble(matches[0].Value);
                                    point.y = Convert.ToDouble(matches[1].Value);
                                    point.z = Convert.ToDouble(matches[2].Value);
                                    vertices.Add(point);
                                    break;
                                }

                            // faces
                            case "f ":
                                {
                                    regularExpression = @"\d+"; // decimal numbers
                                    matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression);
                                    point.x = Convert.ToDouble(matches[0].Value);
                                    point.y = Convert.ToDouble(matches[2].Value);
                                    point.z = Convert.ToDouble(matches[4].Value);
                                    faces.Add(point);
                                    //MessageBox.Show(faces[0].x.ToString());
                                    break;
                                }

                            // for statistics only. vn is not needed
                            case "vn":
                                {
                                    regularExpression = @"[^(vn)]\S+";
                                    matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression);
                                    point.x = Convert.ToDouble(matches[0].Value);
                                    point.y = Convert.ToDouble(matches[1].Value);
                                    point.z = Convert.ToDouble(matches[2].Value);
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
                /*MessageBox.Show("number of lines = " + lineNumber.ToString() + "\n" +
                    "number of vertices = " + vertices.Count.ToString() + "\n" +
                    "number of faces = " + faces.Count.ToString() + "\n" +
                    "number of vertex normals = " + vertexNormals.Count.ToString() + "\n" +
                    "vertices[0].x = " + vertices[0].x.ToString() + "\n" +
                    "vertex normals[0].x = " + vertexNormals[0].x.ToString() + "\n" +
                    "faces[0].x = " + faces[0].x.ToString());*/

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
        }// end ImportObjWithoutDialog

        // ------------------------------------------------
        //                    export obj file
        // ------------------------------------------------
        public static void ExportObj(List<List<Coordinate>> mesh, string filePath)
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
            exportDataDialog.InitialDirectory = Application.StartupPath + @"\" + filePath;
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

                        //MessageBox.Show("File saved.");

                    }// end using
                }//end try

                catch (Exception exportDataException)
                {
                    MessageBox.Show("Failed to Export the Obj File\n" + exportDataException.Message);
                }
            }// end if
        }//end ExportObj

        //
        public static void ExportObjWithoutDialog(List<List<Coordinate>> mesh, string fileFullName)
        {
            // variables
            int i = 0;
            List<Coordinate> vertices = mesh[0]; // vertices
            List<Coordinate> faces = mesh[1]; //faces
            List<Coordinate> vertexNormals = mesh[2]; // vertex normals
            int vertexNumber = vertices.Count;
            int faceNumber = faces.Count;
            int vertexNormalNumber = vertexNormals.Count;

            // create file
            try
            {
                // export line by line
                using (StreamWriter exportLineByLine = new StreamWriter(fileFullName))
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

                    //MessageBox.Show("File saved.");

                }// end using
            }//end try

            catch (Exception exportDataException)
            {
                MessageBox.Show("Failed to Export the Obj File\n" + exportDataException.Message);
            }
        }//end ExportObjWithoutDialog

        //
        public static void ExportObjBatch(List<List<Coordinate>> skinMesh, double BMI, double height)
        {
            //select a folder
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.SelectedPath = Application.StartupPath + @"\Output";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                //generate a new directory under that folder
                string directoryName = "BMI_" + BMI.ToString("#.##") + " height_" + height.ToString("#.###") + " weight_" + (BMI * height * height).ToString("#.##");
                string filePath = folderBrowserDialog.SelectedPath + @"\" + directoryName;
                if (Directory.Exists(filePath)) //if the directory already exists
                {
                    MessageBox.Show("The path " + filePath + " already exists!");
                    return;
                }
                Directory.CreateDirectory(filePath); // Try to create the directory.

                //save each file under that directory
                //save skin
                string fileName = filePath + @"\Skin.obj";
                ExportObjWithoutDialog(skinMesh, fileName);

                //save bones

                //save muscles

                //save blood vessels

                //save lymphatic nodes

                //save VAT and SAT

                //save internal organs
            }//end if

        }

        // ------------------------------------------------
        // rename obj files. for example: from 001.obj to 1.obj
        // ------------------------------------------------
        /*public static void RenameObj()
        {
            try
            {
                string directory = @"D:\study\deformation\Obese Phantom Interpolation\task3\ObesePhantomGenerator\ObesePhantomGenerator\bin\Debug\Input2";
                string[] oldFileNameAndExts = Directory.GetFiles(directory);
                int numberOfFiles = oldFileNameAndExts.Length;
                for (int i = 0; i < numberOfFiles; i++)
                {
                    string oldFileNamePure = Path.GetFileNameWithoutExtension(oldFileNameAndExts[i]);
                    int number = 0;
                    if (Int32.TryParse(oldFileNamePure, out number) == true)
                    {
                        string newFileNamePure = number.ToString();
                        string newFilePath = @"D:\study\deformation\Obese Phantom Interpolation\task3\ObesePhantomGenerator\ObesePhantomGenerator\bin\Debug\Input\";
                        string newFileNameAndExt = newFilePath + newFileNamePure + ".obj";
                        File.Move(oldFileNameAndExts[i], newFileNameAndExt);
                    }
                }

            }
            catch
            { }
        }//end method*/

        // ------------------------------------------------
        // read the file "PhantomInfo"
        // ------------------------------------------------
        public static List<EachMeshInfo> GetPhantomInfo()
        {
            EachMeshInfo eachMeshInfo = new EachMeshInfo();
            List<EachMeshInfo> phantomInfo = new List<EachMeshInfo>();
            string fileName = Application.StartupPath + @"\Input\PhantomInfo";
            // import data into array
            using (StreamReader importLineByLine = new StreamReader(fileName))
            {
                string regularExpression = null;
                string line = null;
                while ((line = importLineByLine.ReadLine()) != null)
                {
                    if (line[0] == '#')
                    {
                        continue;
                    }

                    regularExpression = @"\S+"; // match conditions: no v, no white space, contiguous character
                    System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(line, regularExpression); // matches are a collection of objects
                    // must use a struct (value type) rather than an array(reference type)!!!
                    eachMeshInfo.id = Int32.Parse(matches[0].Value);
                    eachMeshInfo.mass = Double.Parse(matches[1].Value);
                    eachMeshInfo.density = Double.Parse(matches[2].Value);
                    eachMeshInfo.volume = Double.Parse(matches[3].Value);
                    phantomInfo.Add(eachMeshInfo);
                }//end while
            }//end using
            return phantomInfo;
        }//end method

        // ------------------------------------------------
        // get volume or mass from "PhantomInfo"
        // ------------------------------------------------
        public static double GetMeshInfo(List<EachMeshInfo> phantomInfo, int meshID_begin, int meshID_end, string indicator)
        {
            double quantity = 0;
            int number = meshID_end - meshID_begin + 1;
            int currentMeshID = meshID_begin;

            for (int i = 0; i < number; i++)
            {
                int index = phantomInfo.FindIndex(
                delegate(EachMeshInfo eachMeshInfo)
                {
                    return eachMeshInfo.id == currentMeshID;
                }
                );//end FindIndex

                if (index != -1)//if the pre-specified meshID is found
                {
                    if (indicator == "volume")
                    {
                        //update volume summation
                        quantity = quantity + phantomInfo[index].volume;
                    }
                    else if (indicator == "mass")
                    {
                        //update mass summation
                        quantity = quantity + phantomInfo[index].mass;
                    }
                }

                //update index
                currentMeshID = currentMeshID + 1;

            }//end for           

            return quantity;
        }//end method

    } //end class

}//end namespace