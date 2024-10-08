namespace Macromask2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            buttonRun = new Button();
            checkBoxDisplayXY = new CheckBox();
            timer1 = new System.Windows.Forms.Timer(components);
            textBoxActionX = new TextBox();
            textBoxActionY = new TextBox();
            comboBoxActions = new ComboBox();
            buttonGetBoxXY = new Button();
            textBoxTypeKey = new TextBox();
            textBoxOCR = new TextBox();
            buttonOCR = new Button();
            comboBoxChecks = new ComboBox();
            textBoxOCRTest = new TextBox();
            buttonRemoveLine = new Button();
            buttonAddLine = new Button();
            panelActions = new Panel();
            labelDescriptions = new Label();
            textBoxGetX1 = new TextBox();
            textBoxGetY1 = new TextBox();
            textBoxGetX2 = new TextBox();
            textBoxGetY2 = new TextBox();
            buttonExport = new Button();
            buttonAbout = new Button();
            buttonImport = new Button();
            panelActions.SuspendLayout();
            SuspendLayout();
            // 
            // buttonRun
            // 
            buttonRun.Location = new Point(886, 12);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new Size(75, 23);
            buttonRun.TabIndex = 0;
            buttonRun.Text = "Run";
            buttonRun.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayXY
            // 
            checkBoxDisplayXY.AutoSize = true;
            checkBoxDisplayXY.Location = new Point(796, 12);
            checkBoxDisplayXY.Name = "checkBoxDisplayXY";
            checkBoxDisplayXY.Size = new Size(84, 19);
            checkBoxDisplayXY.TabIndex = 1;
            checkBoxDisplayXY.Text = "Display X,Y";
            checkBoxDisplayXY.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxDisplayXY.UseVisualStyleBackColor = true;
            // 
            // textBoxActionX
            // 
            textBoxActionX.Location = new Point(507, 3);
            textBoxActionX.MaxLength = 4;
            textBoxActionX.Name = "textBoxActionX";
            textBoxActionX.Size = new Size(36, 23);
            textBoxActionX.TabIndex = 109;
            textBoxActionX.Text = "0";
            textBoxActionX.KeyPress += textBoxNumeric_KeyPress;
            // 
            // textBoxActionY
            // 
            textBoxActionY.Location = new Point(549, 3);
            textBoxActionY.MaxLength = 4;
            textBoxActionY.Name = "textBoxActionY";
            textBoxActionY.Size = new Size(36, 23);
            textBoxActionY.TabIndex = 110;
            textBoxActionY.Text = "0";
            textBoxActionY.KeyPress += textBoxNumeric_KeyPress;
            // 
            // comboBoxActions
            // 
            comboBoxActions.FormattingEnabled = true;
            comboBoxActions.Items.AddRange(new object[] { "Mouse Move", "Right Click", "Left Click", "Double Click", "Hold Left Click", "Release Left Click", "Mouse to Image", "Type" });
            comboBoxActions.Location = new Point(591, 3);
            comboBoxActions.Name = "comboBoxActions";
            comboBoxActions.Size = new Size(121, 23);
            comboBoxActions.TabIndex = 111;
            comboBoxActions.Text = "Mouse Move";
            // 
            // buttonGetBoxXY
            // 
            buttonGetBoxXY.Location = new Point(192, 3);
            buttonGetBoxXY.Name = "buttonGetBoxXY";
            buttonGetBoxXY.Size = new Size(16, 23);
            buttonGetBoxXY.TabIndex = 106;
            buttonGetBoxXY.Text = "<";
            buttonGetBoxXY.UseVisualStyleBackColor = true;
            // 
            // textBoxTypeKey
            // 
            textBoxTypeKey.Location = new Point(718, 3);
            textBoxTypeKey.Name = "textBoxTypeKey";
            textBoxTypeKey.Size = new Size(213, 23);
            textBoxTypeKey.TabIndex = 112;
            // 
            // textBoxOCR
            // 
            textBoxOCR.Location = new Point(615, 12);
            textBoxOCR.Name = "textBoxOCR";
            textBoxOCR.Size = new Size(172, 23);
            textBoxOCR.TabIndex = 11;
            textBoxOCR.Text = "OCR Result";
            // 
            // buttonOCR
            // 
            buttonOCR.Location = new Point(534, 12);
            buttonOCR.Name = "buttonOCR";
            buttonOCR.Size = new Size(75, 23);
            buttonOCR.TabIndex = 12;
            buttonOCR.Text = "OCR Test";
            buttonOCR.UseVisualStyleBackColor = true;
            buttonOCR.Click += buttonOCR_Click;
            // 
            // comboBoxChecks
            // 
            comboBoxChecks.FormattingEnabled = true;
            comboBoxChecks.Items.AddRange(new object[] { "N/A", "OCR", "Image Search" });
            comboBoxChecks.Location = new Point(214, 3);
            comboBoxChecks.Name = "comboBoxChecks";
            comboBoxChecks.Size = new Size(90, 23);
            comboBoxChecks.TabIndex = 107;
            comboBoxChecks.Text = "N/A";
            // 
            // textBoxOCRTest
            // 
            textBoxOCRTest.Location = new Point(310, 3);
            textBoxOCRTest.Name = "textBoxOCRTest";
            textBoxOCRTest.Size = new Size(188, 23);
            textBoxOCRTest.TabIndex = 108;
            // 
            // buttonRemoveLine
            // 
            buttonRemoveLine.Enabled = false;
            buttonRemoveLine.Location = new Point(4, 3);
            buttonRemoveLine.Name = "buttonRemoveLine";
            buttonRemoveLine.Size = new Size(16, 23);
            buttonRemoveLine.TabIndex = 101;
            buttonRemoveLine.Text = "-";
            buttonRemoveLine.UseVisualStyleBackColor = true;
            buttonRemoveLine.Visible = false;
            buttonRemoveLine.Click += buttonRemoveLine_Click;
            // 
            // buttonAddLine
            // 
            buttonAddLine.Location = new Point(4, 32);
            buttonAddLine.Name = "buttonAddLine";
            buttonAddLine.Size = new Size(16, 23);
            buttonAddLine.TabIndex = 100;
            buttonAddLine.Text = "+";
            buttonAddLine.UseVisualStyleBackColor = true;
            buttonAddLine.Click += buttonAddLine_Click;
            // 
            // panelActions
            // 
            panelActions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelActions.AutoScroll = true;
            panelActions.Controls.Add(labelDescriptions);
            panelActions.Controls.Add(textBoxOCRTest);
            panelActions.Controls.Add(buttonAddLine);
            panelActions.Controls.Add(textBoxActionX);
            panelActions.Controls.Add(buttonRemoveLine);
            panelActions.Controls.Add(textBoxActionY);
            panelActions.Controls.Add(comboBoxActions);
            panelActions.Controls.Add(comboBoxChecks);
            panelActions.Controls.Add(textBoxGetX1);
            panelActions.Controls.Add(textBoxGetY1);
            panelActions.Controls.Add(textBoxGetX2);
            panelActions.Controls.Add(textBoxTypeKey);
            panelActions.Controls.Add(textBoxGetY2);
            panelActions.Controls.Add(buttonGetBoxXY);
            panelActions.Location = new Point(0, 62);
            panelActions.Name = "panelActions";
            panelActions.Size = new Size(961, 376);
            panelActions.TabIndex = 17;
            // 
            // labelDescriptions
            // 
            labelDescriptions.AutoSize = true;
            labelDescriptions.Location = new Point(24, 32);
            labelDescriptions.Margin = new Padding(3);
            labelDescriptions.Name = "labelDescriptions";
            labelDescriptions.Padding = new Padding(0, 5, 0, 0);
            labelDescriptions.Size = new Size(773, 20);
            labelDescriptions.TabIndex = 19;
            labelDescriptions.Text = "Check co-ordinates                             Check Type            Check Details                                          Action Pos         Action Type                      Action Values";
            labelDescriptions.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxGetX1
            // 
            textBoxGetX1.Location = new Point(24, 3);
            textBoxGetX1.MaxLength = 4;
            textBoxGetX1.Name = "textBoxGetX1";
            textBoxGetX1.Size = new Size(36, 23);
            textBoxGetX1.TabIndex = 102;
            textBoxGetX1.Text = "0";
            textBoxGetX1.KeyPress += textBoxNumeric_KeyPress;
            // 
            // textBoxGetY1
            // 
            textBoxGetY1.Location = new Point(66, 3);
            textBoxGetY1.MaxLength = 4;
            textBoxGetY1.Name = "textBoxGetY1";
            textBoxGetY1.Size = new Size(36, 23);
            textBoxGetY1.TabIndex = 103;
            textBoxGetY1.Text = "0";
            textBoxGetY1.KeyPress += textBoxNumeric_KeyPress;
            // 
            // textBoxGetX2
            // 
            textBoxGetX2.Location = new Point(108, 3);
            textBoxGetX2.MaxLength = 4;
            textBoxGetX2.Name = "textBoxGetX2";
            textBoxGetX2.Size = new Size(36, 23);
            textBoxGetX2.TabIndex = 104;
            textBoxGetX2.Text = "0";
            textBoxGetX2.KeyPress += textBoxNumeric_KeyPress;
            // 
            // textBoxGetY2
            // 
            textBoxGetY2.Location = new Point(150, 3);
            textBoxGetY2.MaxLength = 4;
            textBoxGetY2.Name = "textBoxGetY2";
            textBoxGetY2.Size = new Size(36, 23);
            textBoxGetY2.TabIndex = 105;
            textBoxGetY2.Text = "0";
            textBoxGetY2.KeyPress += textBoxNumeric_KeyPress;
            // 
            // buttonExport
            // 
            buttonExport.Location = new Point(453, 12);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new Size(75, 23);
            buttonExport.TabIndex = 18;
            buttonExport.Text = "Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // buttonAbout
            // 
            buttonAbout.Location = new Point(12, 12);
            buttonAbout.Name = "buttonAbout";
            buttonAbout.Size = new Size(48, 23);
            buttonAbout.TabIndex = 19;
            buttonAbout.Text = "About";
            buttonAbout.UseVisualStyleBackColor = true;
            buttonAbout.Click += buttonAbout_Click;
            // 
            // buttonImport
            // 
            buttonImport.Location = new Point(372, 12);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new Size(75, 23);
            buttonImport.TabIndex = 113;
            buttonImport.Text = "Import";
            buttonImport.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(973, 450);
            Controls.Add(buttonImport);
            Controls.Add(buttonAbout);
            Controls.Add(buttonExport);
            Controls.Add(panelActions);
            Controls.Add(buttonOCR);
            Controls.Add(textBoxOCR);
            Controls.Add(checkBoxDisplayXY);
            Controls.Add(buttonRun);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "MacroMask";
            Load += Form1_Load;
            panelActions.ResumeLayout(false);
            panelActions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonRun;
        private CheckBox checkBoxDisplayXY;
        private System.Windows.Forms.Timer timer1;
        private TextBox textBoxActionX;
        private TextBox textBoxActionY;
        private ComboBox comboBoxActions;
        private Button buttonGetBoxXY;
        private TextBox textBoxTypeKey;
        private TextBox textBoxOCR;
        private Button buttonOCR;
        private ComboBox comboBoxChecks;
        private TextBox textBoxOCRTest;
        private Button buttonRemoveLine;
        private Button buttonAddLine;
        private Panel panelActions;
        private TextBox textBoxGetX1;
        private TextBox textBoxGetY1;
        private TextBox textBoxGetX2;
        private TextBox textBoxGetY2;
        private Button buttonExport;
        private Label labelDescriptions;
        private Button buttonAbout;
        private Button buttonImport;
    }
}
