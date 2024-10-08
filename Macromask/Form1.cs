using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Tesseract;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Macromask2
{
    public partial class Form1 : Form
    {
        // P/Invoke to use the GetCursorPos,SetCursorPos function
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32.dll")] public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);
        [DllImport("user32.dll")] public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")] public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")] public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //RegisterHotKey(this.Handle, HOTKEY_ID, YOUR_HOTKEY_MODIFIERS, YOUR_HOTKEY_KEY);
        public const int WM_HOTKEY = 0x0312, MOD_ALT = 0x0001, MOD_CONTROL = 0x0002, MOD_SHIFT = 0x0004, MOD_WIN = 0x0008;
        private const int HOTKEY_ID = 1; // Unique identifier for your hotkey
        private const int YOUR_HOTKEY_MODIFIERS = 0; // Choose your modifiers
        private const int YOUR_HOTKEY_KEY = (int)Keys.F6; // Choose your key
        int skipOrderLineType = 0;

        private TooltipForm tooltipForm; // The custom tooltip form
        private bool getRectXY;
        private object getXYSender;

        // Constants for mouse actions
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

        private SaveFileDialog exportFileDialog1;
        private List<ControlGroup> actionLines = new List<ControlGroup>();
        struct ControlGroup
        {
            public Button removeLine;
            public TextBox checkX1;
            public TextBox checkY1;
            public TextBox checkX2;
            public TextBox checkY2;
            public Button getXY;
            public ComboBox checkOptions;
            public TextBox checkOCR;
            public TextBox actionX;
            public TextBox actionY;
            public ComboBox targetAction;
            public TextBox keyboardType;
        }
        public Form1()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, 1, 0, (int)Keys.F6);
            RegisterHotKey(this.Handle, 2, 0, (int)Keys.F7);
            this.exportFileDialog1 = new SaveFileDialog();
            buttonRun.Click += new EventHandler(buttonRun_Click);
            // Initialize the tooltip form
            tooltipForm = new TooltipForm();
            tooltipForm.Show();
            tooltipForm.SelectionCompleted += TooltipForm_SelectionCompleted;
            // Register the Tick event handler for the Timer
            timer1.Interval = 50;  // Timer interval in milliseconds
            timer1.Tick += new EventHandler(timer1_Tick);
            // Set up the button click event for area selection
            buttonGetBoxXY.Click += buttonGetBoxXY_Click;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            ControlGroup newGroup = new ControlGroup();
            newGroup.removeLine = buttonRemoveLine;
            newGroup.checkX1 = textBoxGetX1;
            newGroup.checkX2 = textBoxGetX2;
            newGroup.checkY1 = textBoxGetY1;
            newGroup.checkY2 = textBoxGetY2;
            newGroup.getXY = buttonGetBoxXY;
            newGroup.checkOptions = comboBoxChecks;
            newGroup.checkOCR = textBoxOCRTest;
            newGroup.actionX = textBoxActionX;
            newGroup.actionY = textBoxActionY;
            newGroup.targetAction = comboBoxActions;
            newGroup.keyboardType = textBoxTypeKey;
            actionLines.Add(newGroup);
        }
        // Event handler for the Timer tick event
        public class OCRProcessor
        {
            // Method to convert a Bitmap to Pix (manually)
            public Pix ConvertBitmapToPix(Bitmap bitmap)
            {
                // Save the bitmap to a memory stream in PNG format
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Position = 0; // Rewind the stream

                    // Load the stream into a Pix object
                    return Pix.LoadFromMemory(stream.ToArray());
                }
            }

            public string PerformOCR(Bitmap bitmap)
            {
                string ocrText = string.Empty;

                try
                {
                    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = ConvertBitmapToPix(bitmap))
                        {
                            using (var page = engine.Process(img))
                            {
                                ocrText = page.GetText();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during OCR: " + ex.Message);
                }

                return ocrText;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((getRectXY)) return;

            if (checkBoxDisplayXY.Checked)
            {
                // Get the global mouse position
                System.Drawing.Point cursorPos;
                GetCursorPos(out cursorPos);

                // Update the custom tooltip with the coordinates
                tooltipForm.UpdateCoordinates(cursorPos.X, cursorPos.Y);

                // Move the tooltip form near the mouse cursor
                tooltipForm.Location = new System.Drawing.Point(cursorPos.X + 10, cursorPos.Y + 10);

                // Make sure the tooltip form is visible
                tooltipForm.Visible = true;
            }
            else
            {
                // Hide the tooltip if checkbox is unchecked
                tooltipForm.Visible = false;
            }
        }
        // Event handler for buttonRun click
        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (ValidateActionLines())
            {
                buttonRun.Enabled = false;
                foreach (var actionLine in actionLines)
                {
                    Thread.Sleep(40);
                    bool checkConfirmed = false;
                    bool skipLine = false;
                    actionLine.checkOptions.BackColor = Color.Red;
                    while (!checkConfirmed)
                    {
                        if (skipOrderLineType != 0)
                        {
                            var result = MessageBox.Show("Hotkey pressed! " + skipOrderLineType + "\nDo you want to Skip the task line?\nNo will only skip OCR check on this task", "Skip Task", MessageBoxButtons.YesNoCancel);
                            skipOrderLineType = 0;
                            if (result == DialogResult.Yes)
                            {
                                checkConfirmed = true;
                                skipLine = true;
                                continue;
                            }
                            if (result == DialogResult.No)
                            {
                                checkConfirmed = true;
                            }
                        }
                        switch (actionLine.checkOptions.SelectedItem.ToString())
                        {
                            case "N/A":
                                checkConfirmed = true;
                                break;
                            case "OCR":
                                // Capture the screen area
                                Bitmap capturedImage = CaptureScreenArea(
                                    int.Parse(actionLine.checkX1.Text),
                                    int.Parse(actionLine.checkY1.Text),
                                    int.Parse(actionLine.checkX2.Text),
                                    int.Parse(actionLine.checkY2.Text));

                                // Perform OCR on the captured image
                                var ocrProcessor = new OCRProcessor();
                                string extractedText = ocrProcessor.PerformOCR(capturedImage).TrimEnd();

                                // Display the result
                                textBoxOCR.Text = extractedText;
                                if (extractedText == actionLine.checkOCR.Text) checkConfirmed = true;
                                break;
                            default:
                                MessageBox.Show("Please select a valid action.");
                                checkConfirmed = true;
                                break;
                        }
                        Application.DoEvents();
                    }
                    actionLine.checkOptions.BackColor = Color.White;
                    if (skipLine)
                    {
                        skipLine = false;
                    }
                    else
                    {
                        actionLine.targetAction.BackColor = Color.Red;
                        // Move the mouse to the specified coordinates
                        SetCursorPos(int.Parse(actionLine.actionX.Text), int.Parse(actionLine.actionY.Text));
                        switch (actionLine.targetAction.SelectedItem.ToString())
                        {
                            case "Mouse Move":
                                break;

                            case "Left Click":
                                performMouse_Click(MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_LEFTUP);
                                break;

                            case "Right Click":
                                performMouse_Click(MOUSEEVENTF_RIGHTDOWN, MOUSEEVENTF_RIGHTUP);
                                break;

                            case "Double Click":
                                performMouse_Click(MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_LEFTUP);
                                performMouse_Click(MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_LEFTUP);
                                break;

                            case "Hold Left Click":
                                // Simulate holding the left button down
                                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                break;

                            case "Release Left Click":
                                // Simulate releasing the left button
                                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                break;

                            case "Type":
                                // Get the text from the textbox and send the keys
                                string keysToType = actionLine.keyboardType.Text;
                                SendKeysToTextBox(keysToType);
                                break;

                            default:
                                MessageBox.Show("Please select a valid action.");
                                break;
                        }
                        actionLine.targetAction.BackColor = Color.White;
                    }
                }
                buttonRun.Enabled = true;
            }
        }
        private void performMouse_Click(uint downEvent, uint upEvent)
        {
            // Get the current mouse position
            GetCursorPos(out System.Drawing.Point cursorPos);

            // Simulate the mouse down and up events
            mouse_event(downEvent, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
            mouse_event(upEvent, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
        }
        private void buttonGetBoxXY_Click(object sender, EventArgs e)
        {
            getXYSender = sender;
            getRectXY = true;
            tooltipForm.WindowState = FormWindowState.Maximized;
            // Show the tooltip form for selecting the area
            tooltipForm.Show();
        }
        private bool ValidateActionLines()
        {
            var invalidMessage = "";
            var lineCount = 1;
            foreach (var line in actionLines)
            {
                if (line.checkOptions.SelectedItem != "N/A" &&
                    int.TryParse(line.checkX1.Text, out int x1) &&
                    int.TryParse(line.checkY1.Text, out int y1) &&
                    int.TryParse(line.checkX2.Text, out int x2) &&
                    int.TryParse(line.checkY2.Text, out int y2))
                {
                    if (x1 == x2 || y1 == y2) invalidMessage += "Line " + lineCount + " Check co-ordinates incorrect\n";
                }
                else
                {
                    if (line.checkOptions.SelectedItem != "N/A") invalidMessage += "Line " + lineCount + " Check co-ordinates must have numerals\n";
                }
                if (!line.checkOptions.Items.Contains(line.checkOptions.Text)) invalidMessage += "Line " + lineCount + " Check Type incorrect\n";
                if (!int.TryParse(line.actionX.Text, out x1) || !int.TryParse(line.actionY.Text, out y1)) invalidMessage += "Line " + lineCount + " Action Pos must have numerals\n";
                if (!line.targetAction.Items.Contains(line.targetAction.Text)) invalidMessage += "Line " + lineCount + " Action Type incorrect\n";
                lineCount++;
            }
            if (invalidMessage != "")
            {
                MessageBox.Show(invalidMessage);
                return false;
            }
            return true;
        }
        // Event handler when the selection is completed
        private void TooltipForm_SelectionCompleted(System.Drawing.Point start, System.Drawing.Point end)
        {
            foreach (var line in actionLines)
            {
                if (line.getXY == getXYSender)
                {
                    // Populate the textboxes with the start and end coordinates
                    line.checkX1.Text = start.X.ToString();
                    line.checkY1.Text = start.Y.ToString();
                    line.checkX2.Text = end.X.ToString();
                    line.checkY2.Text = end.Y.ToString();
                    break;
                }
            }
            getRectXY = false;
            tooltipForm.WindowState = FormWindowState.Normal;
        }
        private void SendKeysToTextBox(string input)
        {
            try
            {
                // Send the typed keys using SendKeys
                SendKeys.Send(input);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while typing: " + ex.Message);
            }
        }
        private void buttonOCR_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxGetX1.Text, out int x1) &&
                int.TryParse(textBoxGetY1.Text, out int y1) &&
                int.TryParse(textBoxGetX2.Text, out int x2) &&
                int.TryParse(textBoxGetY2.Text, out int y2))
            {
                if (x1 == x2 || y1 == y2)
                {
                    MessageBox.Show("Selection too small.");
                    return;
                }
                else
                {
                    // Capture the screen area
                    Bitmap capturedImage = CaptureScreenArea(x1, y1, x2, y2);

                    // Perform OCR on the captured image
                    var ocrProcessor = new OCRProcessor();
                    string extractedText = ocrProcessor.PerformOCR(capturedImage);

                    // Display the result
                    textBoxOCR.Text = extractedText;
                }
            }
            else
            {
                MessageBox.Show("Invalid coordinates entered.");
            }
        }
        private Bitmap CaptureScreenArea(int x1, int y1, int x2, int y2)
        {
            // Calculate width and height of the capture area
            int width = Math.Abs(x2 - x1);
            int height = Math.Abs(y2 - y1);
            Rectangle rect = new Rectangle(Math.Min(x1, x2), Math.Min(y1, y2), width, height);

            // Create a bitmap of the selected area
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);

            // Capture the screen area
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }
            return bmp;
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                // Handle the hotkey here
                skipOrderLineType = m.WParam.ToInt32();
                textBoxOCR.Text = skipOrderLineType.ToString();
                // Be sure to call the base WndProc to ensure other messages are processed
                base.WndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        private void buttonAddLine_Click(object sender, EventArgs e)
        {
            int line = actionLines.Count + 1;
            int newLineHeight = actionLines[actionLines.Count - 1].removeLine.Location.Y + buttonRemoveLine.Height + 6;
            ControlGroup newGroup = new ControlGroup();

            newGroup.removeLine = new Button();
            newGroup.removeLine.Location = new System.Drawing.Point(buttonRemoveLine.Location.X, newLineHeight);
            newGroup.removeLine.Size = buttonRemoveLine.Size;
            newGroup.removeLine.Text = buttonRemoveLine.Text;
            newGroup.removeLine.TabIndex = buttonRemoveLine.TabIndex + 12 * line;
            newGroup.removeLine.Click += buttonRemoveLine_Click;

            newGroup.checkX1 = new TextBox();
            newGroup.checkX1.Location = new System.Drawing.Point(textBoxGetX1.Location.X, newLineHeight);
            newGroup.checkX1.Size = textBoxGetX1.Size;
            newGroup.checkX1.Text = "0";
            newGroup.checkX1.TabIndex = textBoxGetX1.TabIndex + 12 * line;
            newGroup.checkX1.KeyPress += textBoxNumeric_KeyPress;
            newGroup.checkX1.MaxLength = 4;

            newGroup.checkX2 = new TextBox();
            newGroup.checkX2.Location = new System.Drawing.Point(textBoxGetX2.Location.X, newLineHeight);
            newGroup.checkX2.Size = textBoxGetX2.Size;
            newGroup.checkX2.Text = "0";
            newGroup.checkX2.TabIndex = textBoxGetX2.TabIndex + 12 * line;
            newGroup.checkX2.KeyPress += textBoxNumeric_KeyPress;
            newGroup.checkX2.MaxLength = 4;

            newGroup.checkY1 = new TextBox();
            newGroup.checkY1.Location = new System.Drawing.Point(textBoxGetY1.Location.X, newLineHeight);
            newGroup.checkY1.Size = textBoxGetY1.Size;
            newGroup.checkY1.Text = "0";
            newGroup.checkY1.TabIndex = textBoxGetY1.TabIndex + 12 * line;
            newGroup.checkY1.KeyPress += textBoxNumeric_KeyPress;
            newGroup.checkY1.MaxLength = 4;

            newGroup.checkY2 = new TextBox();
            newGroup.checkY2.Location = new System.Drawing.Point(textBoxGetY2.Location.X, newLineHeight);
            newGroup.checkY2.Size = textBoxGetY2.Size;
            newGroup.checkY2.Text = "0";
            newGroup.checkY2.TabIndex = textBoxGetY2.TabIndex + 12 * line;
            newGroup.checkY2.KeyPress += textBoxNumeric_KeyPress;
            newGroup.checkY2.MaxLength = 4;

            newGroup.getXY = new Button();
            newGroup.getXY.Location = new System.Drawing.Point(buttonGetBoxXY.Location.X, newLineHeight);
            newGroup.getXY.Size = buttonGetBoxXY.Size;
            newGroup.getXY.Text = buttonGetBoxXY.Text;
            newGroup.getXY.TabIndex = buttonGetBoxXY.TabIndex + 12 * line;
            newGroup.getXY.Click += buttonGetBoxXY_Click;

            newGroup.checkOptions = new ComboBox();
            newGroup.checkOptions.Location = new System.Drawing.Point(comboBoxChecks.Location.X, newLineHeight);
            newGroup.checkOptions.Items.AddRange(comboBoxChecks.Items.Cast<Object>().ToArray());
            newGroup.checkOptions.Size = comboBoxChecks.Size;
            newGroup.checkOptions.Text = actionLines[actionLines.Count - 1].checkOptions.Text;
            newGroup.checkOptions.TabIndex = comboBoxChecks.TabIndex + 12 * line;

            newGroup.checkOCR = new TextBox();
            newGroup.checkOCR.Location = new System.Drawing.Point(textBoxOCRTest.Location.X, newLineHeight);
            newGroup.checkOCR.Size = textBoxOCRTest.Size;
            newGroup.checkOCR.TabIndex = textBoxOCRTest.TabIndex + 12 * line;

            newGroup.actionX = new TextBox();
            newGroup.actionX.Location = new System.Drawing.Point(textBoxActionX.Location.X, newLineHeight);
            newGroup.actionX.Size = textBoxActionX.Size;
            newGroup.actionX.Text = "0";
            newGroup.actionX.TabIndex = textBoxActionX.TabIndex + 12 * line;
            newGroup.actionX.KeyPress += textBoxNumeric_KeyPress;
            newGroup.actionX.MaxLength = 4;

            newGroup.actionY = new TextBox();
            newGroup.actionY.Location = new System.Drawing.Point(textBoxActionY.Location.X, newLineHeight);
            newGroup.actionY.Size = textBoxActionY.Size;
            newGroup.actionY.Text = "0";
            newGroup.actionY.TabIndex = textBoxActionY.TabIndex + 12 * line;
            newGroup.actionY.KeyPress += textBoxNumeric_KeyPress;
            newGroup.actionY.MaxLength = 4;

            newGroup.targetAction = new ComboBox();
            newGroup.targetAction.Location = new System.Drawing.Point(comboBoxActions.Location.X, newLineHeight);
            newGroup.targetAction.Items.AddRange(comboBoxActions.Items.Cast<Object>().ToArray());
            newGroup.targetAction.Size = comboBoxActions.Size;
            newGroup.targetAction.Text = actionLines[actionLines.Count - 1].targetAction.Text;
            newGroup.targetAction.TabIndex = comboBoxActions.TabIndex + 12 * line;

            newGroup.keyboardType = new TextBox();
            newGroup.keyboardType.Location = new System.Drawing.Point(textBoxTypeKey.Location.X, newLineHeight);
            newGroup.keyboardType.Size = textBoxTypeKey.Size;
            newGroup.keyboardType.Text = textBoxTypeKey.Text;
            newGroup.keyboardType.TabIndex = textBoxTypeKey.TabIndex + 12 * line;

            panelActions.Controls.Add(newGroup.removeLine);
            panelActions.Controls.Add(newGroup.checkX1);
            panelActions.Controls.Add(newGroup.checkY1);
            panelActions.Controls.Add(newGroup.checkX2);
            panelActions.Controls.Add(newGroup.checkY2);
            panelActions.Controls.Add(newGroup.getXY);
            panelActions.Controls.Add(newGroup.checkOptions);
            panelActions.Controls.Add(newGroup.checkOCR);
            panelActions.Controls.Add(newGroup.actionX);
            panelActions.Controls.Add(newGroup.actionY);
            panelActions.Controls.Add(newGroup.targetAction);
            panelActions.Controls.Add(newGroup.keyboardType);
            actionLines.Add(newGroup);
            buttonAddLine.Location = new System.Drawing.Point(buttonRemoveLine.Location.X, newLineHeight + buttonRemoveLine.Height + 6);
            labelDescriptions.Location = new System.Drawing.Point(labelDescriptions.Location.X, buttonAddLine.Location.Y);
        }
        private void buttonRemoveLine_Click(object sender, EventArgs e)
        {
            foreach (var line in actionLines)
            {
                if (line.removeLine == sender)
                {
                    panelActions.Controls.Remove(line.removeLine);
                    panelActions.Controls.Remove(line.getXY);
                    panelActions.Controls.Remove(line.keyboardType);
                    panelActions.Controls.Remove(line.checkOCR);
                    panelActions.Controls.Remove(line.actionY);
                    panelActions.Controls.Remove(line.actionX);
                    panelActions.Controls.Remove(line.targetAction);
                    panelActions.Controls.Remove(line.checkY2);
                    panelActions.Controls.Remove(line.checkX2);
                    panelActions.Controls.Remove(line.checkY1);
                    panelActions.Controls.Remove(line.checkX1);
                    panelActions.Controls.Remove(line.checkOptions);

                    actionLines.Remove(line);
                    break;
                }
            }
            OrderGUILines();
        }
        private void OrderGUILines()
        {
            var scrollPosition = panelActions.VerticalScroll.Value;
            panelActions.AutoScroll = false;
            for (int i = 1; i < actionLines.Count; i++)
            {
                int linePosY = actionLines[i - 1].removeLine.Location.Y + buttonRemoveLine.Height + 6 /*+ scrollPosition*/;
                actionLines[i].checkOptions.Location = new System.Drawing.Point(actionLines[i].checkOptions.Location.X, linePosY);
                actionLines[i].checkX1.Location = new System.Drawing.Point(actionLines[i].checkX1.Location.X, linePosY);
                actionLines[i].checkY1.Location = new System.Drawing.Point(actionLines[i].checkY1.Location.X, linePosY);
                actionLines[i].checkX2.Location = new System.Drawing.Point(actionLines[i].checkX2.Location.X, linePosY);
                actionLines[i].checkY2.Location = new System.Drawing.Point(actionLines[i].checkY2.Location.X, linePosY);
                actionLines[i].targetAction.Location = new System.Drawing.Point(actionLines[i].targetAction.Location.X, linePosY);
                actionLines[i].actionX.Location = new System.Drawing.Point(actionLines[i].actionX.Location.X, linePosY);
                actionLines[i].actionY.Location = new System.Drawing.Point(actionLines[i].actionY.Location.X, linePosY);
                actionLines[i].removeLine.Location = new System.Drawing.Point(actionLines[i].removeLine.Location.X, linePosY);
                actionLines[i].keyboardType.Location = new System.Drawing.Point(actionLines[i].keyboardType.Location.X, linePosY);
                actionLines[i].getXY.Location = new System.Drawing.Point(actionLines[i].getXY.Location.X, linePosY);
                actionLines[i].checkOCR.Location = new System.Drawing.Point(actionLines[i].checkOCR.Location.X, linePosY);
            }
            buttonAddLine.Location = new System.Drawing.Point(buttonRemoveLine.Location.X, actionLines[actionLines.Count - 1].removeLine.Location.Y + buttonRemoveLine.Height + 6);
            labelDescriptions.Location = new System.Drawing.Point(labelDescriptions.Location.X, buttonAddLine.Location.Y);
            panelActions.AutoScroll = true;
            panelActions.VerticalScroll.Value = scrollPosition;
        }
        private void textBoxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control keys (like backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Prevent the character from being entered
            }
        }
        private void buttonExport_Click(object sender, EventArgs e)
        {
            exportFileDialog1.DefaultExt = "txt";
            exportFileDialog1.Filter = "txt file (*.txt)|*.txt";
            exportFileDialog1.Title = "Save Tasks to...";
            exportFileDialog1.FileName = "Export-" + DateTime.Now.ToShortDateString().Replace(@"/", "-") + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".txt";
            exportFileDialog1.ShowDialog();
            string exportOrders = "";
            foreach (var action in actionLines)
            {
                exportOrders += action.checkX1.Text + "|";
                exportOrders += action.checkY1.Text + "|";
                exportOrders += action.checkX2.Text + "|";
                exportOrders += action.checkY2.Text + "|";
                exportOrders += action.checkOptions.Text + "|";
                exportOrders += action.checkOCR.Text + "|";
                exportOrders += action.actionX.Text + "|";
                exportOrders += action.actionY.Text + "|";
                exportOrders += action.targetAction.Text + "|";
                exportOrders += action.keyboardType.Text + "\n";
            }

            File.WriteAllText(exportFileDialog1.FileName, exportOrders);
        }
        private void BtnSearchImage_Click(object sender, EventArgs e)
        {
            // Step 1: Open File Dialog to select PNG image
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG Images|*.png",
                Title = "Select a PNG Image"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;

                // Step 2: Capture the screen and match the selected image
                using (Bitmap screenBmp = CaptureScreen())
                using (Mat screenMat = BitmapConverter.ToMat(screenBmp))
                using (Mat templateMat = Cv2.ImRead(selectedImagePath, ImreadModes.Color))
                {
                    // Perform template matching
                    Mat result = new Mat();
                    Cv2.MatchTemplate(screenMat, templateMat, result, TemplateMatchModes.CCoeffNormed);
                    Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                    // Step 3: Check if match is found and move the mouse
                    if (maxVal > 0.8) // Confidence threshold
                    {
                        // Move the mouse to the location where the image is found
                        SetCursorPos(maxLoc.X + templateMat.Width / 2, maxLoc.Y + templateMat.Height / 2);
                        MessageBox.Show("Image found and mouse moved.");
                    }
                    else
                    {
                        MessageBox.Show("Image not found on the screen.");
                    }
                }
            }
        }

        // Method to capture the entire screen
        private Bitmap CaptureScreen()
        {
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            Bitmap screenBitmap = new Bitmap(screenSize.Width, screenSize.Height);
            using (Graphics g = Graphics.FromImage(screenBitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, screenSize.Size);
            }
            return screenBitmap;
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The software is provided \"as is\", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, "+
                "fitness for a particular purpose, and non-infringement. In no event shall the author be liable for any claim, damages, or other liability, whether in an action of contract, "+
                "tort, or otherwise, arising from, out of, or in connection with the software or the use or other dealings in the software.\r\n\r\n" +
                "This product includes software developed by the\r\nTesseract OCR Project (https://github.com/tesseract-ocr) and \r\n"+
                "OpenCV team (http://opencv.org)\r\ndistributed under the Apache License 2.0.");
        }
    }
}
