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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageRegular = new System.Windows.Forms.TabPage();
            this.buttonExportObj = new System.Windows.Forms.Button();
            this.buttonImportObj = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.labelMaxWeight = new System.Windows.Forms.Label();
            this.labelMinWeight = new System.Windows.Forms.Label();
            this.labelMaxHeight = new System.Windows.Forms.Label();
            this.labelMinHeight = new System.Windows.Forms.Label();
            this.labelMaxBMI = new System.Windows.Forms.Label();
            this.labelMinBMI = new System.Windows.Forms.Label();
            this.labelBMI = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.textBoxBMI = new System.Windows.Forms.TextBox();
            this.textBoxWeight = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.trackBarBMI = new System.Windows.Forms.TrackBar();
            this.trackBarWeight = new System.Windows.Forms.TrackBar();
            this.trackBarHeight = new System.Windows.Forms.TrackBar();
            this.tabPageFree = new System.Windows.Forms.TabPage();
            this.buttonExportObj2 = new System.Windows.Forms.Button();
            this.buttonImportObj2 = new System.Windows.Forms.Button();
            this.labelZScaleFactor = new System.Windows.Forms.Label();
            this.labelYScaleFactor = new System.Windows.Forms.Label();
            this.labelXScaleFactor = new System.Windows.Forms.Label();
            this.textBoxZScaleFactor = new System.Windows.Forms.TextBox();
            this.textBoxYScaleFactor = new System.Windows.Forms.TextBox();
            this.textBoxXScaleFactor = new System.Windows.Forms.TextBox();
            this.buttonFreelyScale = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPageRegular.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBMI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).BeginInit();
            this.tabPageFree.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageRegular);
            this.tabControl.Controls.Add(this.tabPageFree);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1027, 616);
            this.tabControl.TabIndex = 21;
            // 
            // tabPageRegular
            // 
            this.tabPageRegular.BackColor = System.Drawing.Color.Transparent;
            this.tabPageRegular.Controls.Add(this.buttonExportObj);
            this.tabPageRegular.Controls.Add(this.buttonImportObj);
            this.tabPageRegular.Controls.Add(this.buttonExit);
            this.tabPageRegular.Controls.Add(this.buttonReset);
            this.tabPageRegular.Controls.Add(this.buttonGenerate);
            this.tabPageRegular.Controls.Add(this.labelMaxWeight);
            this.tabPageRegular.Controls.Add(this.labelMinWeight);
            this.tabPageRegular.Controls.Add(this.labelMaxHeight);
            this.tabPageRegular.Controls.Add(this.labelMinHeight);
            this.tabPageRegular.Controls.Add(this.labelMaxBMI);
            this.tabPageRegular.Controls.Add(this.labelMinBMI);
            this.tabPageRegular.Controls.Add(this.labelBMI);
            this.tabPageRegular.Controls.Add(this.labelWeight);
            this.tabPageRegular.Controls.Add(this.labelHeight);
            this.tabPageRegular.Controls.Add(this.textBoxBMI);
            this.tabPageRegular.Controls.Add(this.textBoxWeight);
            this.tabPageRegular.Controls.Add(this.textBoxHeight);
            this.tabPageRegular.Controls.Add(this.trackBarBMI);
            this.tabPageRegular.Controls.Add(this.trackBarWeight);
            this.tabPageRegular.Controls.Add(this.trackBarHeight);
            this.tabPageRegular.Location = new System.Drawing.Point(4, 33);
            this.tabPageRegular.Name = "tabPageRegular";
            this.tabPageRegular.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegular.Size = new System.Drawing.Size(1019, 579);
            this.tabPageRegular.TabIndex = 0;
            this.tabPageRegular.Text = "Regular";
            this.tabPageRegular.UseVisualStyleBackColor = true;
            // 
            // buttonExportObj
            // 
            this.buttonExportObj.Location = new System.Drawing.Point(695, 351);
            this.buttonExportObj.Name = "buttonExportObj";
            this.buttonExportObj.Size = new System.Drawing.Size(200, 50);
            this.buttonExportObj.TabIndex = 39;
            this.buttonExportObj.Text = "Export Obj";
            this.buttonExportObj.UseVisualStyleBackColor = true;
            this.buttonExportObj.Click += new System.EventHandler(this.buttonExportObj_Click);
            // 
            // buttonImportObj
            // 
            this.buttonImportObj.Location = new System.Drawing.Point(118, 351);
            this.buttonImportObj.Name = "buttonImportObj";
            this.buttonImportObj.Size = new System.Drawing.Size(200, 50);
            this.buttonImportObj.TabIndex = 38;
            this.buttonImportObj.Text = "Import Obj";
            this.buttonImportObj.UseVisualStyleBackColor = true;
            this.buttonImportObj.Click += new System.EventHandler(this.buttonImportObj_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(408, 462);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(200, 50);
            this.buttonExit.TabIndex = 37;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(118, 462);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(200, 50);
            this.buttonReset.TabIndex = 36;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(408, 351);
            this.buttonGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(200, 50);
            this.buttonGenerate.TabIndex = 35;
            this.buttonGenerate.Text = "Generate Mesh";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // labelMaxWeight
            // 
            this.labelMaxWeight.AutoSize = true;
            this.labelMaxWeight.Location = new System.Drawing.Point(926, 253);
            this.labelMaxWeight.Name = "labelMaxWeight";
            this.labelMaxWeight.Size = new System.Drawing.Size(63, 24);
            this.labelMaxWeight.TabIndex = 34;
            this.labelMaxWeight.Text = "label6";
            // 
            // labelMinWeight
            // 
            this.labelMinWeight.AutoSize = true;
            this.labelMinWeight.Location = new System.Drawing.Point(396, 253);
            this.labelMinWeight.Name = "labelMinWeight";
            this.labelMinWeight.Size = new System.Drawing.Size(63, 24);
            this.labelMinWeight.TabIndex = 33;
            this.labelMinWeight.Text = "label5";
            // 
            // labelMaxHeight
            // 
            this.labelMaxHeight.AutoSize = true;
            this.labelMaxHeight.Location = new System.Drawing.Point(926, 160);
            this.labelMaxHeight.Name = "labelMaxHeight";
            this.labelMaxHeight.Size = new System.Drawing.Size(63, 24);
            this.labelMaxHeight.TabIndex = 32;
            this.labelMaxHeight.Text = "label4";
            // 
            // labelMinHeight
            // 
            this.labelMinHeight.AutoSize = true;
            this.labelMinHeight.Location = new System.Drawing.Point(396, 160);
            this.labelMinHeight.Name = "labelMinHeight";
            this.labelMinHeight.Size = new System.Drawing.Size(63, 24);
            this.labelMinHeight.TabIndex = 31;
            this.labelMinHeight.Text = "label3";
            // 
            // labelMaxBMI
            // 
            this.labelMaxBMI.AutoSize = true;
            this.labelMaxBMI.Location = new System.Drawing.Point(926, 51);
            this.labelMaxBMI.Name = "labelMaxBMI";
            this.labelMaxBMI.Size = new System.Drawing.Size(63, 24);
            this.labelMaxBMI.TabIndex = 30;
            this.labelMaxBMI.Text = "label2";
            // 
            // labelMinBMI
            // 
            this.labelMinBMI.AutoSize = true;
            this.labelMinBMI.Location = new System.Drawing.Point(396, 51);
            this.labelMinBMI.Name = "labelMinBMI";
            this.labelMinBMI.Size = new System.Drawing.Size(63, 24);
            this.labelMinBMI.TabIndex = 29;
            this.labelMinBMI.Text = "label1";
            // 
            // labelBMI
            // 
            this.labelBMI.AutoSize = true;
            this.labelBMI.Location = new System.Drawing.Point(21, 43);
            this.labelBMI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBMI.Name = "labelBMI";
            this.labelBMI.Size = new System.Drawing.Size(141, 24);
            this.labelBMI.TabIndex = 20;
            this.labelBMI.Text = "BMI [kg/m^2]";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(21, 250);
            this.labelWeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(117, 24);
            this.labelWeight.TabIndex = 26;
            this.labelWeight.Text = "Weight [kg]";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(21, 152);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(110, 24);
            this.labelHeight.TabIndex = 23;
            this.labelHeight.Text = "Height [m]";
            // 
            // textBoxBMI
            // 
            this.textBoxBMI.Location = new System.Drawing.Point(188, 43);
            this.textBoxBMI.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxBMI.Name = "textBoxBMI";
            this.textBoxBMI.Size = new System.Drawing.Size(181, 32);
            this.textBoxBMI.TabIndex = 21;
            this.textBoxBMI.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxWeight
            // 
            this.textBoxWeight.Location = new System.Drawing.Point(188, 250);
            this.textBoxWeight.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxWeight.Name = "textBoxWeight";
            this.textBoxWeight.Size = new System.Drawing.Size(181, 32);
            this.textBoxWeight.TabIndex = 27;
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(188, 152);
            this.textBoxHeight.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(181, 32);
            this.textBoxHeight.TabIndex = 24;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // trackBarBMI
            // 
            this.trackBarBMI.Location = new System.Drawing.Point(451, 51);
            this.trackBarBMI.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarBMI.Name = "trackBarBMI";
            this.trackBarBMI.Size = new System.Drawing.Size(482, 56);
            this.trackBarBMI.TabIndex = 22;
            this.trackBarBMI.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // trackBarWeight
            // 
            this.trackBarWeight.Location = new System.Drawing.Point(451, 253);
            this.trackBarWeight.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarWeight.Name = "trackBarWeight";
            this.trackBarWeight.Size = new System.Drawing.Size(482, 56);
            this.trackBarWeight.TabIndex = 28;
            this.trackBarWeight.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // trackBarHeight
            // 
            this.trackBarHeight.Location = new System.Drawing.Point(451, 161);
            this.trackBarHeight.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarHeight.Name = "trackBarHeight";
            this.trackBarHeight.Size = new System.Drawing.Size(482, 56);
            this.trackBarHeight.TabIndex = 25;
            this.trackBarHeight.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // tabPageFree
            // 
            this.tabPageFree.BackColor = System.Drawing.Color.Transparent;
            this.tabPageFree.Controls.Add(this.buttonExportObj2);
            this.tabPageFree.Controls.Add(this.buttonImportObj2);
            this.tabPageFree.Controls.Add(this.labelZScaleFactor);
            this.tabPageFree.Controls.Add(this.labelYScaleFactor);
            this.tabPageFree.Controls.Add(this.labelXScaleFactor);
            this.tabPageFree.Controls.Add(this.textBoxZScaleFactor);
            this.tabPageFree.Controls.Add(this.textBoxYScaleFactor);
            this.tabPageFree.Controls.Add(this.textBoxXScaleFactor);
            this.tabPageFree.Controls.Add(this.buttonFreelyScale);
            this.tabPageFree.Location = new System.Drawing.Point(4, 33);
            this.tabPageFree.Name = "tabPageFree";
            this.tabPageFree.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFree.Size = new System.Drawing.Size(1019, 579);
            this.tabPageFree.TabIndex = 1;
            this.tabPageFree.Text = "Free";
            this.tabPageFree.UseVisualStyleBackColor = true;
            // 
            // buttonExportObj2
            // 
            this.buttonExportObj2.Location = new System.Drawing.Point(679, 344);
            this.buttonExportObj2.Name = "buttonExportObj2";
            this.buttonExportObj2.Size = new System.Drawing.Size(200, 50);
            this.buttonExportObj2.TabIndex = 8;
            this.buttonExportObj2.Text = "Export Obj";
            this.buttonExportObj2.UseVisualStyleBackColor = true;
            this.buttonExportObj2.Click += new System.EventHandler(this.buttonExportObj_Click);
            // 
            // buttonImportObj2
            // 
            this.buttonImportObj2.Location = new System.Drawing.Point(107, 344);
            this.buttonImportObj2.Name = "buttonImportObj2";
            this.buttonImportObj2.Size = new System.Drawing.Size(200, 50);
            this.buttonImportObj2.TabIndex = 7;
            this.buttonImportObj2.Text = "Import Obj";
            this.buttonImportObj2.UseVisualStyleBackColor = true;
            this.buttonImportObj2.Click += new System.EventHandler(this.buttonImportObj2_Click);
            // 
            // labelZScaleFactor
            // 
            this.labelZScaleFactor.AutoSize = true;
            this.labelZScaleFactor.Location = new System.Drawing.Point(117, 247);
            this.labelZScaleFactor.Name = "labelZScaleFactor";
            this.labelZScaleFactor.Size = new System.Drawing.Size(20, 24);
            this.labelZScaleFactor.TabIndex = 6;
            this.labelZScaleFactor.Text = "z";
            // 
            // labelYScaleFactor
            // 
            this.labelYScaleFactor.AutoSize = true;
            this.labelYScaleFactor.Location = new System.Drawing.Point(117, 183);
            this.labelYScaleFactor.Name = "labelYScaleFactor";
            this.labelYScaleFactor.Size = new System.Drawing.Size(21, 24);
            this.labelYScaleFactor.TabIndex = 5;
            this.labelYScaleFactor.Text = "y";
            // 
            // labelXScaleFactor
            // 
            this.labelXScaleFactor.AutoSize = true;
            this.labelXScaleFactor.Location = new System.Drawing.Point(117, 115);
            this.labelXScaleFactor.Name = "labelXScaleFactor";
            this.labelXScaleFactor.Size = new System.Drawing.Size(20, 24);
            this.labelXScaleFactor.TabIndex = 4;
            this.labelXScaleFactor.Text = "x";
            // 
            // textBoxZScaleFactor
            // 
            this.textBoxZScaleFactor.Location = new System.Drawing.Point(215, 239);
            this.textBoxZScaleFactor.Name = "textBoxZScaleFactor";
            this.textBoxZScaleFactor.Size = new System.Drawing.Size(181, 32);
            this.textBoxZScaleFactor.TabIndex = 3;
            // 
            // textBoxYScaleFactor
            // 
            this.textBoxYScaleFactor.Location = new System.Drawing.Point(215, 175);
            this.textBoxYScaleFactor.Name = "textBoxYScaleFactor";
            this.textBoxYScaleFactor.Size = new System.Drawing.Size(181, 32);
            this.textBoxYScaleFactor.TabIndex = 2;
            // 
            // textBoxXScaleFactor
            // 
            this.textBoxXScaleFactor.Location = new System.Drawing.Point(215, 107);
            this.textBoxXScaleFactor.Name = "textBoxXScaleFactor";
            this.textBoxXScaleFactor.Size = new System.Drawing.Size(181, 32);
            this.textBoxXScaleFactor.TabIndex = 1;
            // 
            // buttonFreelyScale
            // 
            this.buttonFreelyScale.Location = new System.Drawing.Point(402, 344);
            this.buttonFreelyScale.Name = "buttonFreelyScale";
            this.buttonFreelyScale.Size = new System.Drawing.Size(200, 50);
            this.buttonFreelyScale.TabIndex = 0;
            this.buttonFreelyScale.Text = "Scale";
            this.buttonFreelyScale.UseVisualStyleBackColor = true;
            this.buttonFreelyScale.Click += new System.EventHandler(this.buttonFreelyScale_Click);
            // 
            // FormObesePhantomGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 616);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormObesePhantomGenerator";
            this.Text = "Obese Phantom Generator";
            this.tabControl.ResumeLayout(false);
            this.tabPageRegular.ResumeLayout(false);
            this.tabPageRegular.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBMI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).EndInit();
            this.tabPageFree.ResumeLayout(false);
            this.tabPageFree.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageRegular;
        private System.Windows.Forms.TabPage tabPageFree;
        private System.Windows.Forms.Button buttonExportObj;
        private System.Windows.Forms.Button buttonImportObj;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonGenerate;
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
        private System.Windows.Forms.Button buttonFreelyScale;
        private System.Windows.Forms.Label labelZScaleFactor;
        private System.Windows.Forms.Label labelYScaleFactor;
        private System.Windows.Forms.Label labelXScaleFactor;
        private System.Windows.Forms.TextBox textBoxZScaleFactor;
        private System.Windows.Forms.TextBox textBoxYScaleFactor;
        private System.Windows.Forms.TextBox textBoxXScaleFactor;
        private System.Windows.Forms.Button buttonExportObj2;
        private System.Windows.Forms.Button buttonImportObj2;
    }
}

