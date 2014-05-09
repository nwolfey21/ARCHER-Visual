namespace ObesePhantomGenerator
{
    partial class FormObesePhantomGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormObesePhantomGenerator));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageFreeDeformer = new System.Windows.Forms.TabPage();
            this.buttonTrial = new System.Windows.Forms.Button();
            this.textBoxTrial = new System.Windows.Forms.TextBox();
            this.groupBoxData2 = new System.Windows.Forms.GroupBox();
            this.labelScaleFactorK = new System.Windows.Forms.Label();
            this.labelComputationTime2 = new System.Windows.Forms.Label();
            this.textBoxComputationTime2 = new System.Windows.Forms.TextBox();
            this.textBoxScaleFactorK = new System.Windows.Forms.TextBox();
            this.textBoxDeformedVolume = new System.Windows.Forms.TextBox();
            this.labelDeformedVolume = new System.Windows.Forms.Label();
            this.textBoxScaleFactorZ = new System.Windows.Forms.TextBox();
            this.labelScaleFactorZ = new System.Windows.Forms.Label();
            this.textBoxScaleFactorY = new System.Windows.Forms.TextBox();
            this.labelScaleFactorY = new System.Windows.Forms.Label();
            this.textBoxScaleFactorX = new System.Windows.Forms.TextBox();
            this.labelScaleFactorX = new System.Windows.Forms.Label();
            this.textBoxOriginalVolume = new System.Windows.Forms.TextBox();
            this.labelOriginalVolume = new System.Windows.Forms.Label();
            this.groupBoxMethod = new System.Windows.Forms.GroupBox();
            this.radioButtonAlongXYZ = new System.Windows.Forms.RadioButton();
            this.radioButtonAlongVertexNormal = new System.Windows.Forms.RadioButton();
            this.radioButtonAlongCentroidVertexVector = new System.Windows.Forms.RadioButton();
            this.groupBoxTask = new System.Windows.Forms.GroupBox();
            this.radioButtonSpecifyVolume = new System.Windows.Forms.RadioButton();
            this.radioButtonSpecifyScaleFactor = new System.Windows.Forms.RadioButton();
            this.buttonReset2 = new System.Windows.Forms.Button();
            this.buttonExportObj2 = new System.Windows.Forms.Button();
            this.buttonImportObj2 = new System.Windows.Forms.Button();
            this.buttonDeform = new System.Windows.Forms.Button();
            this.tabPageStandardDeformer = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelWaistHipRatio = new System.Windows.Forms.Label();
            this.textBoxWaistHipRatio = new System.Windows.Forms.TextBox();
            this.textBoxComputationTime = new System.Windows.Forms.TextBox();
            this.labelComputationTime = new System.Windows.Forms.Label();
            this.textBoxWaistCircumference = new System.Windows.Forms.TextBox();
            this.labelWaistCircumference = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxWeight = new System.Windows.Forms.TextBox();
            this.labelWeight = new System.Windows.Forms.Label();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.labelMaxWeight = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelMinWeight = new System.Windows.Forms.Label();
            this.labelMaxHeight = new System.Windows.Forms.Label();
            this.labelMinHeight = new System.Windows.Forms.Label();
            this.trackBarWeight = new System.Windows.Forms.TrackBar();
            this.labelMaxBMI = new System.Windows.Forms.Label();
            this.trackBarHeight = new System.Windows.Forms.TrackBar();
            this.labelMinBMI = new System.Windows.Forms.Label();
            this.labelBMI = new System.Windows.Forms.Label();
            this.textBoxBMI = new System.Windows.Forms.TextBox();
            this.trackBarBMI = new System.Windows.Forms.TrackBar();
            this.buttonExportObj = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPageFreeDeformer.SuspendLayout();
            this.groupBoxData2.SuspendLayout();
            this.groupBoxMethod.SuspendLayout();
            this.groupBoxTask.SuspendLayout();
            this.tabPageStandardDeformer.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBMI)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageFreeDeformer);
            this.tabControl.Controls.Add(this.tabPageStandardDeformer);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1069, 641);
            this.tabControl.TabIndex = 21;
            // 
            // tabPageFreeDeformer
            // 
            this.tabPageFreeDeformer.BackColor = System.Drawing.Color.Transparent;
            this.tabPageFreeDeformer.Controls.Add(this.buttonTrial);
            this.tabPageFreeDeformer.Controls.Add(this.textBoxTrial);
            this.tabPageFreeDeformer.Controls.Add(this.groupBoxData2);
            this.tabPageFreeDeformer.Controls.Add(this.groupBoxMethod);
            this.tabPageFreeDeformer.Controls.Add(this.groupBoxTask);
            this.tabPageFreeDeformer.Controls.Add(this.buttonReset2);
            this.tabPageFreeDeformer.Controls.Add(this.buttonExportObj2);
            this.tabPageFreeDeformer.Controls.Add(this.buttonImportObj2);
            this.tabPageFreeDeformer.Controls.Add(this.buttonDeform);
            this.tabPageFreeDeformer.Location = new System.Drawing.Point(4, 37);
            this.tabPageFreeDeformer.Name = "tabPageFreeDeformer";
            this.tabPageFreeDeformer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFreeDeformer.Size = new System.Drawing.Size(1061, 600);
            this.tabPageFreeDeformer.TabIndex = 1;
            this.tabPageFreeDeformer.Text = "Free Deformer";
            this.tabPageFreeDeformer.UseVisualStyleBackColor = true;
            // 
            // buttonTrial
            // 
            this.buttonTrial.Location = new System.Drawing.Point(771, 141);
            this.buttonTrial.Name = "buttonTrial";
            this.buttonTrial.Size = new System.Drawing.Size(149, 67);
            this.buttonTrial.TabIndex = 25;
            this.buttonTrial.Text = "Trial";
            this.buttonTrial.UseVisualStyleBackColor = true;
            this.buttonTrial.Click += new System.EventHandler(this.buttonTrial_Click);
            // 
            // textBoxTrial
            // 
            this.textBoxTrial.Location = new System.Drawing.Point(771, 89);
            this.textBoxTrial.Name = "textBoxTrial";
            this.textBoxTrial.Size = new System.Drawing.Size(100, 35);
            this.textBoxTrial.TabIndex = 24;
            // 
            // groupBoxData2
            // 
            this.groupBoxData2.Controls.Add(this.labelScaleFactorK);
            this.groupBoxData2.Controls.Add(this.labelComputationTime2);
            this.groupBoxData2.Controls.Add(this.textBoxComputationTime2);
            this.groupBoxData2.Controls.Add(this.textBoxScaleFactorK);
            this.groupBoxData2.Controls.Add(this.textBoxDeformedVolume);
            this.groupBoxData2.Controls.Add(this.labelDeformedVolume);
            this.groupBoxData2.Controls.Add(this.textBoxScaleFactorZ);
            this.groupBoxData2.Controls.Add(this.labelScaleFactorZ);
            this.groupBoxData2.Controls.Add(this.textBoxScaleFactorY);
            this.groupBoxData2.Controls.Add(this.labelScaleFactorY);
            this.groupBoxData2.Controls.Add(this.textBoxScaleFactorX);
            this.groupBoxData2.Controls.Add(this.labelScaleFactorX);
            this.groupBoxData2.Controls.Add(this.textBoxOriginalVolume);
            this.groupBoxData2.Controls.Add(this.labelOriginalVolume);
            this.groupBoxData2.Location = new System.Drawing.Point(49, 181);
            this.groupBoxData2.Name = "groupBoxData2";
            this.groupBoxData2.Size = new System.Drawing.Size(692, 416);
            this.groupBoxData2.TabIndex = 23;
            this.groupBoxData2.TabStop = false;
            this.groupBoxData2.Text = "Data";
            // 
            // labelScaleFactorK
            // 
            this.labelScaleFactorK.AutoSize = true;
            this.labelScaleFactorK.Location = new System.Drawing.Point(30, 323);
            this.labelScaleFactorK.Name = "labelScaleFactorK";
            this.labelScaleFactorK.Size = new System.Drawing.Size(163, 28);
            this.labelScaleFactorK.TabIndex = 20;
            this.labelScaleFactorK.Text = "Scale Factor k";
            // 
            // labelComputationTime2
            // 
            this.labelComputationTime2.AutoSize = true;
            this.labelComputationTime2.Location = new System.Drawing.Point(24, 375);
            this.labelComputationTime2.Name = "labelComputationTime2";
            this.labelComputationTime2.Size = new System.Drawing.Size(253, 28);
            this.labelComputationTime2.TabIndex = 21;
            this.labelComputationTime2.Text = "Computation Time [s]";
            // 
            // textBoxComputationTime2
            // 
            this.textBoxComputationTime2.Location = new System.Drawing.Point(380, 368);
            this.textBoxComputationTime2.Name = "textBoxComputationTime2";
            this.textBoxComputationTime2.Size = new System.Drawing.Size(200, 35);
            this.textBoxComputationTime2.TabIndex = 22;
            this.textBoxComputationTime2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPreventEnteringCharacters_KeyPress);
            // 
            // textBoxScaleFactorK
            // 
            this.textBoxScaleFactorK.Location = new System.Drawing.Point(380, 316);
            this.textBoxScaleFactorK.Name = "textBoxScaleFactorK";
            this.textBoxScaleFactorK.Size = new System.Drawing.Size(200, 35);
            this.textBoxScaleFactorK.TabIndex = 19;
            // 
            // textBoxDeformedVolume
            // 
            this.textBoxDeformedVolume.Location = new System.Drawing.Point(380, 94);
            this.textBoxDeformedVolume.Name = "textBoxDeformedVolume";
            this.textBoxDeformedVolume.Size = new System.Drawing.Size(200, 35);
            this.textBoxDeformedVolume.TabIndex = 15;
            // 
            // labelDeformedVolume
            // 
            this.labelDeformedVolume.AutoSize = true;
            this.labelDeformedVolume.Location = new System.Drawing.Point(30, 101);
            this.labelDeformedVolume.Name = "labelDeformedVolume";
            this.labelDeformedVolume.Size = new System.Drawing.Size(298, 28);
            this.labelDeformedVolume.TabIndex = 14;
            this.labelDeformedVolume.Text = "Deformed Volume [cm^3]";
            // 
            // textBoxScaleFactorZ
            // 
            this.textBoxScaleFactorZ.Location = new System.Drawing.Point(380, 262);
            this.textBoxScaleFactorZ.Name = "textBoxScaleFactorZ";
            this.textBoxScaleFactorZ.Size = new System.Drawing.Size(200, 35);
            this.textBoxScaleFactorZ.TabIndex = 3;
            // 
            // labelScaleFactorZ
            // 
            this.labelScaleFactorZ.AutoSize = true;
            this.labelScaleFactorZ.Location = new System.Drawing.Point(30, 269);
            this.labelScaleFactorZ.Name = "labelScaleFactorZ";
            this.labelScaleFactorZ.Size = new System.Drawing.Size(160, 28);
            this.labelScaleFactorZ.TabIndex = 6;
            this.labelScaleFactorZ.Text = "Scale Factor z";
            // 
            // textBoxScaleFactorY
            // 
            this.textBoxScaleFactorY.Location = new System.Drawing.Point(380, 207);
            this.textBoxScaleFactorY.Name = "textBoxScaleFactorY";
            this.textBoxScaleFactorY.Size = new System.Drawing.Size(200, 35);
            this.textBoxScaleFactorY.TabIndex = 2;
            // 
            // labelScaleFactorY
            // 
            this.labelScaleFactorY.AutoSize = true;
            this.labelScaleFactorY.Location = new System.Drawing.Point(30, 214);
            this.labelScaleFactorY.Name = "labelScaleFactorY";
            this.labelScaleFactorY.Size = new System.Drawing.Size(254, 28);
            this.labelScaleFactorY.TabIndex = 5;
            this.labelScaleFactorY.Text = "Scale Factor y (height)";
            // 
            // textBoxScaleFactorX
            // 
            this.textBoxScaleFactorX.Location = new System.Drawing.Point(380, 152);
            this.textBoxScaleFactorX.Name = "textBoxScaleFactorX";
            this.textBoxScaleFactorX.Size = new System.Drawing.Size(200, 35);
            this.textBoxScaleFactorX.TabIndex = 1;
            // 
            // labelScaleFactorX
            // 
            this.labelScaleFactorX.AutoSize = true;
            this.labelScaleFactorX.Location = new System.Drawing.Point(30, 159);
            this.labelScaleFactorX.Name = "labelScaleFactorX";
            this.labelScaleFactorX.Size = new System.Drawing.Size(160, 28);
            this.labelScaleFactorX.TabIndex = 4;
            this.labelScaleFactorX.Text = "Scale Factor x";
            // 
            // textBoxOriginalVolume
            // 
            this.textBoxOriginalVolume.Location = new System.Drawing.Point(380, 42);
            this.textBoxOriginalVolume.Name = "textBoxOriginalVolume";
            this.textBoxOriginalVolume.Size = new System.Drawing.Size(200, 35);
            this.textBoxOriginalVolume.TabIndex = 13;
            // 
            // labelOriginalVolume
            // 
            this.labelOriginalVolume.AutoSize = true;
            this.labelOriginalVolume.Location = new System.Drawing.Point(30, 49);
            this.labelOriginalVolume.Name = "labelOriginalVolume";
            this.labelOriginalVolume.Size = new System.Drawing.Size(280, 28);
            this.labelOriginalVolume.TabIndex = 12;
            this.labelOriginalVolume.Text = "Original Volume [cm^3]";
            // 
            // groupBoxMethod
            // 
            this.groupBoxMethod.Controls.Add(this.radioButtonAlongXYZ);
            this.groupBoxMethod.Controls.Add(this.radioButtonAlongVertexNormal);
            this.groupBoxMethod.Controls.Add(this.radioButtonAlongCentroidVertexVector);
            this.groupBoxMethod.Location = new System.Drawing.Point(361, 6);
            this.groupBoxMethod.Name = "groupBoxMethod";
            this.groupBoxMethod.Size = new System.Drawing.Size(380, 165);
            this.groupBoxMethod.TabIndex = 18;
            this.groupBoxMethod.TabStop = false;
            this.groupBoxMethod.Text = "Method";
            // 
            // radioButtonAlongXYZ
            // 
            this.radioButtonAlongXYZ.AutoSize = true;
            this.radioButtonAlongXYZ.Location = new System.Drawing.Point(14, 117);
            this.radioButtonAlongXYZ.Name = "radioButtonAlongXYZ";
            this.radioButtonAlongXYZ.Size = new System.Drawing.Size(97, 21);
            this.radioButtonAlongXYZ.TabIndex = 2;
            this.radioButtonAlongXYZ.TabStop = true;
            this.radioButtonAlongXYZ.Text = "Along x y z";
            this.radioButtonAlongXYZ.UseVisualStyleBackColor = true;
            this.radioButtonAlongXYZ.CheckedChanged += new System.EventHandler(this.radioButtonChooseTaskMethod_CheckedChanged);
            // 
            // radioButtonAlongVertexNormal
            // 
            this.radioButtonAlongVertexNormal.AutoSize = true;
            this.radioButtonAlongVertexNormal.Location = new System.Drawing.Point(14, 79);
            this.radioButtonAlongVertexNormal.Name = "radioButtonAlongVertexNormal";
            this.radioButtonAlongVertexNormal.Size = new System.Drawing.Size(158, 21);
            this.radioButtonAlongVertexNormal.TabIndex = 1;
            this.radioButtonAlongVertexNormal.TabStop = true;
            this.radioButtonAlongVertexNormal.Text = "Along Vertex Normal";
            this.radioButtonAlongVertexNormal.UseVisualStyleBackColor = true;
            this.radioButtonAlongVertexNormal.CheckedChanged += new System.EventHandler(this.radioButtonChooseTaskMethod_CheckedChanged);
            // 
            // radioButtonAlongCentroidVertexVector
            // 
            this.radioButtonAlongCentroidVertexVector.AutoSize = true;
            this.radioButtonAlongCentroidVertexVector.Location = new System.Drawing.Point(14, 41);
            this.radioButtonAlongCentroidVertexVector.Name = "radioButtonAlongCentroidVertexVector";
            this.radioButtonAlongCentroidVertexVector.Size = new System.Drawing.Size(211, 21);
            this.radioButtonAlongCentroidVertexVector.TabIndex = 0;
            this.radioButtonAlongCentroidVertexVector.TabStop = true;
            this.radioButtonAlongCentroidVertexVector.Text = "Along Centroid Vertex Vector";
            this.radioButtonAlongCentroidVertexVector.UseVisualStyleBackColor = true;
            this.radioButtonAlongCentroidVertexVector.CheckedChanged += new System.EventHandler(this.radioButtonChooseTaskMethod_CheckedChanged);
            // 
            // groupBoxTask
            // 
            this.groupBoxTask.Controls.Add(this.radioButtonSpecifyVolume);
            this.groupBoxTask.Controls.Add(this.radioButtonSpecifyScaleFactor);
            this.groupBoxTask.Location = new System.Drawing.Point(49, 6);
            this.groupBoxTask.Name = "groupBoxTask";
            this.groupBoxTask.Size = new System.Drawing.Size(293, 165);
            this.groupBoxTask.TabIndex = 17;
            this.groupBoxTask.TabStop = false;
            this.groupBoxTask.Text = "Task";
            // 
            // radioButtonSpecifyVolume
            // 
            this.radioButtonSpecifyVolume.AutoSize = true;
            this.radioButtonSpecifyVolume.Location = new System.Drawing.Point(14, 101);
            this.radioButtonSpecifyVolume.Name = "radioButtonSpecifyVolume";
            this.radioButtonSpecifyVolume.Size = new System.Drawing.Size(126, 21);
            this.radioButtonSpecifyVolume.TabIndex = 10;
            this.radioButtonSpecifyVolume.TabStop = true;
            this.radioButtonSpecifyVolume.Text = "Specify Volume";
            this.radioButtonSpecifyVolume.UseVisualStyleBackColor = true;
            this.radioButtonSpecifyVolume.CheckedChanged += new System.EventHandler(this.radioButtonChooseTaskMethod_CheckedChanged);
            // 
            // radioButtonSpecifyScaleFactor
            // 
            this.radioButtonSpecifyScaleFactor.AutoSize = true;
            this.radioButtonSpecifyScaleFactor.Location = new System.Drawing.Point(14, 53);
            this.radioButtonSpecifyScaleFactor.Name = "radioButtonSpecifyScaleFactor";
            this.radioButtonSpecifyScaleFactor.Size = new System.Drawing.Size(165, 21);
            this.radioButtonSpecifyScaleFactor.TabIndex = 9;
            this.radioButtonSpecifyScaleFactor.TabStop = true;
            this.radioButtonSpecifyScaleFactor.Text = "Specify Scale Factors";
            this.radioButtonSpecifyScaleFactor.UseVisualStyleBackColor = true;
            this.radioButtonSpecifyScaleFactor.CheckedChanged += new System.EventHandler(this.radioButtonChooseTaskMethod_CheckedChanged);
            // 
            // buttonReset2
            // 
            this.buttonReset2.Location = new System.Drawing.Point(762, 501);
            this.buttonReset2.Name = "buttonReset2";
            this.buttonReset2.Size = new System.Drawing.Size(233, 58);
            this.buttonReset2.TabIndex = 16;
            this.buttonReset2.Text = "Reset";
            this.buttonReset2.UseVisualStyleBackColor = true;
            this.buttonReset2.Click += new System.EventHandler(this.buttonReset2_Click);
            // 
            // buttonExportObj2
            // 
            this.buttonExportObj2.Location = new System.Drawing.Point(762, 431);
            this.buttonExportObj2.Name = "buttonExportObj2";
            this.buttonExportObj2.Size = new System.Drawing.Size(233, 58);
            this.buttonExportObj2.TabIndex = 8;
            this.buttonExportObj2.Text = "Export Obj";
            this.buttonExportObj2.UseVisualStyleBackColor = true;
            this.buttonExportObj2.Click += new System.EventHandler(this.buttonExportObj_Click);
            // 
            // buttonImportObj2
            // 
            this.buttonImportObj2.Location = new System.Drawing.Point(762, 274);
            this.buttonImportObj2.Name = "buttonImportObj2";
            this.buttonImportObj2.Size = new System.Drawing.Size(233, 58);
            this.buttonImportObj2.TabIndex = 7;
            this.buttonImportObj2.Text = "Import Obj";
            this.buttonImportObj2.UseVisualStyleBackColor = true;
            this.buttonImportObj2.Click += new System.EventHandler(this.buttonImportObj2_Click);
            // 
            // buttonDeform
            // 
            this.buttonDeform.Location = new System.Drawing.Point(762, 361);
            this.buttonDeform.Name = "buttonDeform";
            this.buttonDeform.Size = new System.Drawing.Size(233, 58);
            this.buttonDeform.TabIndex = 0;
            this.buttonDeform.Text = "Deform";
            this.buttonDeform.UseVisualStyleBackColor = true;
            this.buttonDeform.Click += new System.EventHandler(this.buttonDeform_Click);
            // 
            // tabPageStandardDeformer
            // 
            this.tabPageStandardDeformer.BackColor = System.Drawing.Color.Transparent;
            this.tabPageStandardDeformer.Controls.Add(this.panel2);
            this.tabPageStandardDeformer.Controls.Add(this.panel1);
            this.tabPageStandardDeformer.Controls.Add(this.buttonExportObj);
            this.tabPageStandardDeformer.Controls.Add(this.buttonExit);
            this.tabPageStandardDeformer.Controls.Add(this.buttonReset);
            this.tabPageStandardDeformer.Controls.Add(this.buttonGenerate);
            this.tabPageStandardDeformer.Location = new System.Drawing.Point(4, 37);
            this.tabPageStandardDeformer.Name = "tabPageStandardDeformer";
            this.tabPageStandardDeformer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStandardDeformer.Size = new System.Drawing.Size(1061, 600);
            this.tabPageStandardDeformer.TabIndex = 0;
            this.tabPageStandardDeformer.Text = "Standard Deformer";
            this.tabPageStandardDeformer.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelWaistHipRatio);
            this.panel2.Controls.Add(this.textBoxWaistHipRatio);
            this.panel2.Controls.Add(this.textBoxComputationTime);
            this.panel2.Controls.Add(this.labelComputationTime);
            this.panel2.Controls.Add(this.textBoxWaistCircumference);
            this.panel2.Controls.Add(this.labelWaistCircumference);
            this.panel2.Location = new System.Drawing.Point(41, 333);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(426, 240);
            this.panel2.TabIndex = 45;
            // 
            // labelWaistHipRatio
            // 
            this.labelWaistHipRatio.AutoSize = true;
            this.labelWaistHipRatio.Location = new System.Drawing.Point(14, 105);
            this.labelWaistHipRatio.Name = "labelWaistHipRatio";
            this.labelWaistHipRatio.Size = new System.Drawing.Size(188, 28);
            this.labelWaistHipRatio.TabIndex = 45;
            this.labelWaistHipRatio.Text = "Waist Hip Ratio";
            // 
            // textBoxWaistHipRatio
            // 
            this.textBoxWaistHipRatio.Location = new System.Drawing.Point(243, 105);
            this.textBoxWaistHipRatio.Name = "textBoxWaistHipRatio";
            this.textBoxWaistHipRatio.Size = new System.Drawing.Size(163, 35);
            this.textBoxWaistHipRatio.TabIndex = 44;
            this.textBoxWaistHipRatio.TextChanged += new System.EventHandler(this.textBoxAdjustNumber_TextChanged);
            // 
            // textBoxComputationTime
            // 
            this.textBoxComputationTime.Location = new System.Drawing.Point(243, 176);
            this.textBoxComputationTime.Name = "textBoxComputationTime";
            this.textBoxComputationTime.Size = new System.Drawing.Size(163, 35);
            this.textBoxComputationTime.TabIndex = 40;
            this.textBoxComputationTime.TextChanged += new System.EventHandler(this.textBoxAdjustNumber_TextChanged);
            this.textBoxComputationTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPreventEnteringCharacters_KeyPress);
            // 
            // labelComputationTime
            // 
            this.labelComputationTime.AutoSize = true;
            this.labelComputationTime.Location = new System.Drawing.Point(14, 160);
            this.labelComputationTime.Name = "labelComputationTime";
            this.labelComputationTime.Size = new System.Drawing.Size(157, 56);
            this.labelComputationTime.TabIndex = 41;
            this.labelComputationTime.Text = "Computation\r\nTime [s]";
            // 
            // textBoxWaistCircumference
            // 
            this.textBoxWaistCircumference.Location = new System.Drawing.Point(243, 29);
            this.textBoxWaistCircumference.Name = "textBoxWaistCircumference";
            this.textBoxWaistCircumference.Size = new System.Drawing.Size(163, 35);
            this.textBoxWaistCircumference.TabIndex = 42;
            this.textBoxWaistCircumference.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPreventEnteringCharacters_KeyPress);
            // 
            // labelWaistCircumference
            // 
            this.labelWaistCircumference.AutoSize = true;
            this.labelWaistCircumference.Location = new System.Drawing.Point(14, 20);
            this.labelWaistCircumference.Name = "labelWaistCircumference";
            this.labelWaistCircumference.Size = new System.Drawing.Size(227, 56);
            this.labelWaistCircumference.TabIndex = 43;
            this.labelWaistCircumference.Text = "Waist\r\nCircumference [cm]";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textBoxWeight);
            this.panel1.Controls.Add(this.labelWeight);
            this.panel1.Controls.Add(this.textBoxHeight);
            this.panel1.Controls.Add(this.labelMaxWeight);
            this.panel1.Controls.Add(this.labelHeight);
            this.panel1.Controls.Add(this.labelMinWeight);
            this.panel1.Controls.Add(this.labelMaxHeight);
            this.panel1.Controls.Add(this.labelMinHeight);
            this.panel1.Controls.Add(this.trackBarWeight);
            this.panel1.Controls.Add(this.labelMaxBMI);
            this.panel1.Controls.Add(this.trackBarHeight);
            this.panel1.Controls.Add(this.labelMinBMI);
            this.panel1.Controls.Add(this.labelBMI);
            this.panel1.Controls.Add(this.textBoxBMI);
            this.panel1.Controls.Add(this.trackBarBMI);
            this.panel1.Location = new System.Drawing.Point(41, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(995, 284);
            this.panel1.TabIndex = 44;
            // 
            // textBoxWeight
            // 
            this.textBoxWeight.Location = new System.Drawing.Point(196, 199);
            this.textBoxWeight.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxWeight.Name = "textBoxWeight";
            this.textBoxWeight.Size = new System.Drawing.Size(210, 35);
            this.textBoxWeight.TabIndex = 27;
            this.textBoxWeight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBoxWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPreventEnteringCharacters_KeyPress);
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(14, 199);
            this.labelWeight.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(142, 28);
            this.labelWeight.TabIndex = 26;
            this.labelWeight.Text = "Weight [kg]";
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(196, 112);
            this.textBoxHeight.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(210, 35);
            this.textBoxHeight.TabIndex = 24;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBoxHeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.textBoxHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // labelMaxWeight
            // 
            this.labelMaxWeight.AutoSize = true;
            this.labelMaxWeight.Location = new System.Drawing.Point(892, 227);
            this.labelMaxWeight.Name = "labelMaxWeight";
            this.labelMaxWeight.Size = new System.Drawing.Size(79, 28);
            this.labelMaxWeight.TabIndex = 34;
            this.labelMaxWeight.Text = "label6";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(14, 115);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(131, 28);
            this.labelHeight.TabIndex = 23;
            this.labelHeight.Text = "Height [m]";
            // 
            // labelMinWeight
            // 
            this.labelMinWeight.AutoSize = true;
            this.labelMinWeight.Location = new System.Drawing.Point(424, 227);
            this.labelMinWeight.Name = "labelMinWeight";
            this.labelMinWeight.Size = new System.Drawing.Size(79, 28);
            this.labelMinWeight.TabIndex = 33;
            this.labelMinWeight.Text = "label5";
            // 
            // labelMaxHeight
            // 
            this.labelMaxHeight.AutoSize = true;
            this.labelMaxHeight.Location = new System.Drawing.Point(892, 140);
            this.labelMaxHeight.Name = "labelMaxHeight";
            this.labelMaxHeight.Size = new System.Drawing.Size(79, 28);
            this.labelMaxHeight.TabIndex = 32;
            this.labelMaxHeight.Text = "label4";
            // 
            // labelMinHeight
            // 
            this.labelMinHeight.AutoSize = true;
            this.labelMinHeight.Location = new System.Drawing.Point(424, 140);
            this.labelMinHeight.Name = "labelMinHeight";
            this.labelMinHeight.Size = new System.Drawing.Size(79, 28);
            this.labelMinHeight.TabIndex = 31;
            this.labelMinHeight.Text = "label3";
            // 
            // trackBarWeight
            // 
            this.trackBarWeight.Location = new System.Drawing.Point(447, 199);
            this.trackBarWeight.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarWeight.Name = "trackBarWeight";
            this.trackBarWeight.Size = new System.Drawing.Size(500, 56);
            this.trackBarWeight.TabIndex = 28;
            this.trackBarWeight.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // labelMaxBMI
            // 
            this.labelMaxBMI.AutoSize = true;
            this.labelMaxBMI.Location = new System.Drawing.Point(892, 58);
            this.labelMaxBMI.Name = "labelMaxBMI";
            this.labelMaxBMI.Size = new System.Drawing.Size(79, 28);
            this.labelMaxBMI.TabIndex = 30;
            this.labelMaxBMI.Text = "label2";
            // 
            // trackBarHeight
            // 
            this.trackBarHeight.Location = new System.Drawing.Point(447, 112);
            this.trackBarHeight.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarHeight.Name = "trackBarHeight";
            this.trackBarHeight.Size = new System.Drawing.Size(500, 56);
            this.trackBarHeight.TabIndex = 25;
            this.trackBarHeight.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // labelMinBMI
            // 
            this.labelMinBMI.AutoSize = true;
            this.labelMinBMI.Location = new System.Drawing.Point(424, 58);
            this.labelMinBMI.Name = "labelMinBMI";
            this.labelMinBMI.Size = new System.Drawing.Size(79, 28);
            this.labelMinBMI.TabIndex = 29;
            this.labelMinBMI.Text = "label1";
            // 
            // labelBMI
            // 
            this.labelBMI.AutoSize = true;
            this.labelBMI.Location = new System.Drawing.Point(14, 30);
            this.labelBMI.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelBMI.Name = "labelBMI";
            this.labelBMI.Size = new System.Drawing.Size(166, 28);
            this.labelBMI.TabIndex = 20;
            this.labelBMI.Text = "BMI [kg/m^2]";
            // 
            // textBoxBMI
            // 
            this.textBoxBMI.Location = new System.Drawing.Point(196, 30);
            this.textBoxBMI.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxBMI.Name = "textBoxBMI";
            this.textBoxBMI.Size = new System.Drawing.Size(210, 35);
            this.textBoxBMI.TabIndex = 21;
            this.textBoxBMI.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBoxBMI.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.textBoxBMI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // trackBarBMI
            // 
            this.trackBarBMI.Location = new System.Drawing.Point(447, 30);
            this.trackBarBMI.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarBMI.Name = "trackBarBMI";
            this.trackBarBMI.Size = new System.Drawing.Size(489, 56);
            this.trackBarBMI.TabIndex = 22;
            this.trackBarBMI.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // buttonExportObj
            // 
            this.buttonExportObj.Location = new System.Drawing.Point(685, 391);
            this.buttonExportObj.Name = "buttonExportObj";
            this.buttonExportObj.Size = new System.Drawing.Size(200, 50);
            this.buttonExportObj.TabIndex = 39;
            this.buttonExportObj.Text = "Export Obj";
            this.buttonExportObj.UseVisualStyleBackColor = true;
            this.buttonExportObj.Click += new System.EventHandler(this.buttonExportObj_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(685, 509);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(5);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(200, 50);
            this.buttonExit.TabIndex = 37;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(685, 449);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(5);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(200, 50);
            this.buttonReset.TabIndex = 36;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(685, 333);
            this.buttonGenerate.Margin = new System.Windows.Forms.Padding(5);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(200, 50);
            this.buttonGenerate.TabIndex = 35;
            this.buttonGenerate.Text = "Generate Mesh";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // FormObesePhantomGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 641);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Book Antiqua", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormObesePhantomGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Obese Phantom Generator";
            this.Load += new System.EventHandler(this.FormObesePhantomGenerator_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageFreeDeformer.ResumeLayout(false);
            this.tabPageFreeDeformer.PerformLayout();
            this.groupBoxData2.ResumeLayout(false);
            this.groupBoxData2.PerformLayout();
            this.groupBoxMethod.ResumeLayout(false);
            this.groupBoxMethod.PerformLayout();
            this.groupBoxTask.ResumeLayout(false);
            this.groupBoxTask.PerformLayout();
            this.tabPageStandardDeformer.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBMI)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageStandardDeformer;
        private System.Windows.Forms.TabPage tabPageFreeDeformer;
        private System.Windows.Forms.Button buttonExportObj;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonDeform;
        private System.Windows.Forms.Label labelScaleFactorZ;
        private System.Windows.Forms.Label labelScaleFactorY;
        private System.Windows.Forms.Label labelScaleFactorX;
        private System.Windows.Forms.TextBox textBoxScaleFactorZ;
        private System.Windows.Forms.TextBox textBoxScaleFactorY;
        private System.Windows.Forms.TextBox textBoxScaleFactorX;
        private System.Windows.Forms.Button buttonExportObj2;
        private System.Windows.Forms.Button buttonImportObj2;
        private System.Windows.Forms.Label labelComputationTime;
        private System.Windows.Forms.TextBox textBoxComputationTime;
        private System.Windows.Forms.TextBox textBoxDeformedVolume;
        private System.Windows.Forms.Label labelDeformedVolume;
        private System.Windows.Forms.TextBox textBoxOriginalVolume;
        private System.Windows.Forms.Label labelOriginalVolume;
        private System.Windows.Forms.RadioButton radioButtonSpecifyVolume;
        private System.Windows.Forms.RadioButton radioButtonSpecifyScaleFactor;
        private System.Windows.Forms.Button buttonReset2;
        private System.Windows.Forms.GroupBox groupBoxMethod;
        private System.Windows.Forms.RadioButton radioButtonAlongVertexNormal;
        private System.Windows.Forms.RadioButton radioButtonAlongCentroidVertexVector;
        private System.Windows.Forms.GroupBox groupBoxTask;
        private System.Windows.Forms.Label labelScaleFactorK;
        private System.Windows.Forms.TextBox textBoxScaleFactorK;
        private System.Windows.Forms.TextBox textBoxComputationTime2;
        private System.Windows.Forms.Label labelComputationTime2;
        private System.Windows.Forms.RadioButton radioButtonAlongXYZ;
        private System.Windows.Forms.GroupBox groupBoxData2;
        private System.Windows.Forms.Button buttonTrial;
        private System.Windows.Forms.TextBox textBoxTrial;
        private System.Windows.Forms.Label labelWaistCircumference;
        private System.Windows.Forms.TextBox textBoxWaistCircumference;
        private System.Windows.Forms.Label labelMaxWeight;
        private System.Windows.Forms.Label labelMinWeight;
        private System.Windows.Forms.Label labelMaxHeight;
        private System.Windows.Forms.Label labelMinHeight;
        private System.Windows.Forms.Label labelMaxBMI;
        private System.Windows.Forms.Label labelMinBMI;
        private System.Windows.Forms.Label labelBMI;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.TextBox textBoxBMI;
        private System.Windows.Forms.TextBox textBoxWeight;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TrackBar trackBarBMI;
        private System.Windows.Forms.TrackBar trackBarWeight;
        private System.Windows.Forms.TrackBar trackBarHeight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelWaistHipRatio;
        private System.Windows.Forms.TextBox textBoxWaistHipRatio;
    }
}

