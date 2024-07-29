namespace MacroMask
{
    partial class MainMaskForm
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
            runButton = new Button();
            selectionTextBox = new TextBox();
            removeLineButton = new Button();
            getXY = new Button();
            keyboardType = new TextBox();
            checkOCR = new TextBox();
            targetY = new NumericUpDown();
            targetX = new NumericUpDown();
            targetOrder = new ComboBox();
            checkY2 = new NumericUpDown();
            checkX2 = new NumericUpDown();
            checkY1 = new NumericUpDown();
            checkX1 = new NumericUpDown();
            checkOptions = new ComboBox();
            Screenshot = new Button();
            addLineButton = new Button();
            repeatCount = new NumericUpDown();
            label1 = new Label();
            statusStrip1 = new StatusStrip();
            toolStrip = new ToolStripStatusLabel();
            timerMouseXY = new System.Windows.Forms.Timer(components);
            ImportButton = new Button();
            ExportButton = new Button();
            label2 = new Label();
            backgroundWorkerOCR = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)targetY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)targetX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)checkY2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)checkX2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)checkY1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)checkX1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repeatCount).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // runButton
            // 
            runButton.Location = new Point(6, 5);
            runButton.Name = "runButton";
            runButton.Size = new Size(55, 29);
            runButton.TabIndex = 0;
            runButton.Text = "Run";
            runButton.UseVisualStyleBackColor = true;
            runButton.Click += RunButton_Click;
            // 
            // selectionTextBox
            // 
            selectionTextBox.Location = new Point(64, 10);
            selectionTextBox.Name = "selectionTextBox";
            selectionTextBox.Size = new Size(175, 23);
            selectionTextBox.TabIndex = 1;
            // 
            // removeLineButton
            // 
            removeLineButton.Location = new Point(7, 40);
            removeLineButton.Name = "removeLineButton";
            removeLineButton.Size = new Size(22, 23);
            removeLineButton.TabIndex = 6;
            removeLineButton.Text = "-";
            removeLineButton.UseVisualStyleBackColor = true;
            removeLineButton.Visible = false;
            removeLineButton.Click += RemoveLineButton_Click;
            // 
            // getXY
            // 
            getXY.Location = new Point(330, 40);
            getXY.Name = "getXY";
            getXY.Size = new Size(55, 23);
            getXY.TabIndex = 12;
            getXY.Text = "Get X,Y";
            getXY.UseVisualStyleBackColor = true;
            getXY.Click += GetXYButton_Click;
            // 
            // keyboardType
            // 
            keyboardType.Location = new Point(687, 38);
            keyboardType.Name = "keyboardType";
            keyboardType.ScrollBars = ScrollBars.Both;
            keyboardType.Size = new Size(101, 23);
            keyboardType.TabIndex = 17;
            keyboardType.Text = "{ENTER}";
            // 
            // checkOCR
            // 
            checkOCR.Location = new Point(581, 38);
            checkOCR.Name = "checkOCR";
            checkOCR.PlaceholderText = "OCR Word";
            checkOCR.Size = new Size(101, 23);
            checkOCR.TabIndex = 16;
            // 
            // targetY
            // 
            targetY.Location = new Point(531, 40);
            targetY.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            targetY.Name = "targetY";
            targetY.Size = new Size(45, 23);
            targetY.TabIndex = 15;
            targetY.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // targetX
            // 
            targetX.Location = new Point(479, 40);
            targetX.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            targetX.Name = "targetX";
            targetX.Size = new Size(45, 23);
            targetX.TabIndex = 14;
            targetX.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // targetOrder
            // 
            targetOrder.FormattingEnabled = true;
            targetOrder.Items.AddRange(new object[] { "Move", "LeftClick", "LeftHold", "LeftRelease", "RightClick", "Type" });
            targetOrder.Location = new Point(388, 40);
            targetOrder.Name = "targetOrder";
            targetOrder.Size = new Size(85, 23);
            targetOrder.TabIndex = 13;
            targetOrder.Text = "Move";
            // 
            // checkY2
            // 
            checkY2.Location = new Point(284, 40);
            checkY2.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            checkY2.Name = "checkY2";
            checkY2.Size = new Size(45, 23);
            checkY2.TabIndex = 11;
            checkY2.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkX2
            // 
            checkX2.Location = new Point(232, 40);
            checkX2.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            checkX2.Name = "checkX2";
            checkX2.Size = new Size(45, 23);
            checkX2.TabIndex = 10;
            checkX2.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkY1
            // 
            checkY1.Location = new Point(182, 40);
            checkY1.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            checkY1.Name = "checkY1";
            checkY1.Size = new Size(45, 23);
            checkY1.TabIndex = 9;
            checkY1.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkX1
            // 
            checkX1.AllowDrop = true;
            checkX1.Location = new Point(132, 40);
            checkX1.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            checkX1.Name = "checkX1";
            checkX1.Size = new Size(45, 23);
            checkX1.TabIndex = 8;
            checkX1.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkOptions
            // 
            checkOptions.FormattingEnabled = true;
            checkOptions.Items.AddRange(new object[] { "N/A", "Image search", "OCR Check", "Image action" });
            checkOptions.Location = new Point(29, 40);
            checkOptions.Name = "checkOptions";
            checkOptions.Size = new Size(95, 23);
            checkOptions.TabIndex = 7;
            checkOptions.Text = "N/A";
            // 
            // Screenshot
            // 
            Screenshot.Location = new Point(694, 8);
            Screenshot.Name = "Screenshot";
            Screenshot.Size = new Size(92, 23);
            Screenshot.TabIndex = 5;
            Screenshot.Text = "Capture img";
            Screenshot.UseVisualStyleBackColor = true;
            Screenshot.Click += Screenshot_Click;
            // 
            // addLineButton
            // 
            addLineButton.Location = new Point(626, 8);
            addLineButton.Name = "addLineButton";
            addLineButton.Size = new Size(62, 23);
            addLineButton.TabIndex = 4;
            addLineButton.Text = "Add Line";
            addLineButton.UseVisualStyleBackColor = true;
            addLineButton.Click += AddLineButton_Click;
            // 
            // repeatCount
            // 
            repeatCount.Location = new Point(575, 8);
            repeatCount.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            repeatCount.Name = "repeatCount";
            repeatCount.Size = new Size(43, 23);
            repeatCount.TabIndex = 3;
            repeatCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(526, 12);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 30;
            label1.Text = "Repeat";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStrip });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(799, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip
            // 
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(26, 17);
            toolStrip.Text = "Pos";
            // 
            // timerMouseXY
            // 
            timerMouseXY.Enabled = true;
            timerMouseXY.Tick += MouseXYToolStatusUpdate;
            // 
            // ImportButton
            // 
            ImportButton.Location = new Point(245, 8);
            ImportButton.Name = "ImportButton";
            ImportButton.Size = new Size(76, 23);
            ImportButton.TabIndex = 1;
            ImportButton.Text = "Import";
            ImportButton.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            ExportButton.Location = new Point(320, 8);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(76, 23);
            ExportButton.TabIndex = 2;
            ExportButton.Text = "Export";
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += ExportButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(395, 0);
            label2.Name = "label2";
            label2.Size = new Size(125, 36);
            label2.TabIndex = 31;
            label2.Text = "F6 - End Reapeat\r\nF7 - Pause/Skip Order/OCR\r\nF8 - Kill the App";
            // 
            // backgroundWorkerOCR
            // 
            backgroundWorkerOCR.WorkerSupportsCancellation = true;
            backgroundWorkerOCR.DoWork += DoOCRWork;
            // 
            // MainMaskForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(799, 450);
            Controls.Add(label2);
            Controls.Add(ExportButton);
            Controls.Add(ImportButton);
            Controls.Add(statusStrip1);
            Controls.Add(Screenshot);
            Controls.Add(addLineButton);
            Controls.Add(repeatCount);
            Controls.Add(label1);
            Controls.Add(removeLineButton);
            Controls.Add(getXY);
            Controls.Add(keyboardType);
            Controls.Add(checkOCR);
            Controls.Add(targetY);
            Controls.Add(targetX);
            Controls.Add(targetOrder);
            Controls.Add(checkY2);
            Controls.Add(checkX2);
            Controls.Add(checkY1);
            Controls.Add(checkX1);
            Controls.Add(checkOptions);
            Controls.Add(selectionTextBox);
            Controls.Add(runButton);
            Name = "MainMaskForm";
            Text = "MacroMask";
            Activated += UpdateSelectionAfterAreaCapture;
            ((System.ComponentModel.ISupportInitialize)targetY).EndInit();
            ((System.ComponentModel.ISupportInitialize)targetX).EndInit();
            ((System.ComponentModel.ISupportInitialize)checkY2).EndInit();
            ((System.ComponentModel.ISupportInitialize)checkX2).EndInit();
            ((System.ComponentModel.ISupportInitialize)checkY1).EndInit();
            ((System.ComponentModel.ISupportInitialize)checkX1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repeatCount).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button runButton;
        private TextBox selectionTextBox;
        private Button removeLineButton;
        private Button getXY;
        private TextBox keyboardType;
        private TextBox checkOCR;
        private NumericUpDown targetY;
        private NumericUpDown targetX;
        private ComboBox targetOrder;
        private NumericUpDown checkY2;
        private NumericUpDown checkX2;
        private NumericUpDown checkY1;
        private NumericUpDown checkX1;
        private ComboBox checkOptions;
        private Button Screenshot;
        private Button addLineButton;
        private NumericUpDown repeatCount;
        private Label label1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStrip;
        private System.Windows.Forms.Timer timerMouseXY;
        private Button ImportButton;
        private Button ExportButton;
        private Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerOCR;
    }
}