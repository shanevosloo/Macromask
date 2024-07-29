using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tesseract;
//https://www.codeproject.com/Articles/21913/TeboScreen-Basic-C-Screen-Capture-Application
//https://www.codeproject.com/Articles/5264831/How-to-Send-Inputs-using-Csharp
//https://stackoverflow.com/questions/15428566/capturing-a-key-without-focusing-the-window
//https://ironsoftware.com/csharp/ocr/tutorials/how-to-read-text-from-an-image-in-csharp-net/
namespace MacroMask
{
    public partial class MainMaskForm : Form
    {
        private System.Windows.Forms.SaveFileDialog exportFileDialog1;
        private ControlGroup controlLine;
        private List<ControlGroup> orderLines = new List<ControlGroup>();
        private object lastGetXYSender;
        private int skipOrderLineType = 0;
        private string toolMessageAdd = "";
        private string resultOCR = "";
        struct ControlGroup
        {
            public ComboBox checkOptions;
            public NumericUpDown checkX1;
            public NumericUpDown checkY1;
            public NumericUpDown checkX2;
            public NumericUpDown checkY2;
            public ComboBox targetOrder;
            public NumericUpDown targetX;
            public NumericUpDown targetY;
            public TextBox checkOCR;
            public TextBox keyboardType;
            public Button getXY;
            public Button removeLineButton;
        }
        public MainMaskForm()
        {
            InitializeComponent();
            this.exportFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            HotKeyer.RegisterHotKey(this.Handle, 1, 0, (int)Keys.F6);
            HotKeyer.RegisterHotKey(this.Handle, 2, 0, (int)Keys.F7);
            SetupControls();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == HotKeyer.WM_HOTKEY)
            {
                // Handle the hotkey here
                //MessageBox.Show("Hotkey pressed! " + m.WParam.ToInt32()+"\nDo you want to Skip the task line?/nNo will only skip OCR check on this task","Skip Task",MessageBoxButtons.YesNoCancel);
                skipOrderLineType = m.WParam.ToInt32();

                // Be sure to call the base WndProc to ensure other messages are processed
                base.WndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        private void SetupControls()
        {
            controlLine.checkOCR = checkOCR;
            controlLine.checkOptions = checkOptions;
            controlLine.removeLineButton = removeLineButton;
            controlLine.getXY = getXY;
            controlLine.targetOrder = targetOrder;
            controlLine.targetX = targetX;
            controlLine.targetY = targetY;
            controlLine.checkX1 = checkX1;
            controlLine.checkY1 = checkY1;
            controlLine.checkX2 = checkX2;
            controlLine.checkY2 = checkY2;
            controlLine.keyboardType = keyboardType;
            orderLines.Add(controlLine);
        }
        public static string GetText()
        {
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile("OCRScreenshot.bmp"))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                        Console.WriteLine("Text (GetText): \r\n{0}", text);
                        Console.WriteLine("Text (iterator):");
                        return text;
                    }
                }
            }
        }
        private void RunButton_Click(object sender, EventArgs e)
        {
            skipOrderLineType = 0;
            runButton.Enabled = false;
            for (int i = 0; i < orderLines.Count; i++)
            {
                if (orderLines[i].checkOptions.Text == "OCR Check")
                {
                    Point curPos = new Point(Cursor.Position.X - (int)orderLines[i].checkX1.Value, Cursor.Position.Y - (int)orderLines[i].checkY1.Value);
                    Size curSize = new Size();
                    if (Cursor.Current == null) { curSize.Height = 0; curSize.Width = 0; }
                    else
                    {
                        curSize.Height = Cursor.Current.Size.Height;
                        curSize.Width = Cursor.Current.Size.Width;
                    }
                    var startPoint = new Point((int)orderLines[i].checkX1.Value, (int)orderLines[i].checkY1.Value);
                    var areaOCR = new Rectangle(startPoint.X, startPoint.Y, (int)orderLines[i].checkX2.Value - (int)orderLines[i].checkX1.Value, (int)orderLines[i].checkY2.Value - (int)orderLines[i].checkY1.Value);
                    if (areaOCR.Width > 0 || areaOCR.Height > 0)
                    {
                        ScreenShot.CaptureImage(false, curSize, curPos, startPoint, Point.Empty, areaOCR, "OCRScreenshot.bmp", ".bmp");
                        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                        {
                            using (var img = Pix.LoadFromFile("OCRScreenshot.bmp"))
                            {
                                using (var page = engine.Process(img))
                                {
                                    resultOCR = page.GetText().TrimEnd();
                                    selectionTextBox.Text = resultOCR;
                                    toolMessageAdd = "Found with " + Math.Round(page.GetMeanConfidence() * 100, 1) + "% Confidence: " + resultOCR.Substring(0, Math.Clamp(resultOCR.Length, 0, 15));
                                    toolMessageAdd = toolMessageAdd.Replace("\n", " ");
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(20);
                    if (skipOrderLineType == 2)
                    {
                        var result = MessageBox.Show("Hotkey pressed! " + skipOrderLineType + "\nDo you want to Skip the task line?\nNo will only skip OCR check on this task", "Skip Task", MessageBoxButtons.YesNoCancel);
                        skipOrderLineType = 0;
                        if (result == DialogResult.Yes)
                        {
                            continue;
                        }
                        if (result == DialogResult.No)
                        {
                            if (orderLines.Count - 1 == i)
                            {
                                continue;
                            }
                            else
                            {
                                i++;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(20);
                if (!resultOCR.Equals(orderLines[i].checkOCR.Text) && orderLines[i].checkOptions.Text == "OCR Check")
                {
                    Application.DoEvents();
                    i--;
                    continue;
                }
                if (orderLines[i].targetOrder.Text == "Type")
                {
                    SendKeys.Send(orderLines[i].keyboardType.Text);
                }
                else
                {
                    InputSender.MouseActioner((int)orderLines[i].targetX.Value, (int)orderLines[i].targetY.Value, orderLines[i].targetOrder.Text);
                }
                toolMessageAdd = "";
            }
            runButton.Enabled = true;
            /*            foreach (var order in orderLines)
                        {
                            if (order.checkOptions.Text == "OCR Check")
                            {
                                Point curPos = new Point(Cursor.Position.X - (int)order.checkX1.Value, Cursor.Position.Y - (int)order.checkY1.Value);
                                Size curSize = new Size();
                                curSize.Height = Cursor.Current.Size.Height;
                                curSize.Width = Cursor.Current.Size.Width;
                                var startPoint = new Point((int)order.checkX1.Value, (int)order.checkY1.Value);
                                var areaOCR = new Rectangle(startPoint.X, startPoint.Y, (int)order.checkX2.Value - (int)order.checkX1.Value, (int)order.checkY2.Value - (int)order.checkY1.Value);
                                string textFromOCR = "";
                                ScreenShot.CaptureImage(false, curSize, curPos, startPoint, Point.Empty, areaOCR, "OCRScreenshot.bmp", ".bmp");
                                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                                {
                                    using (var img = Pix.LoadFromFile("OCRScreenshot.bmp"))
                                    {
                                        using (var page = engine.Process(img))
                                        {
                                            textFromOCR = page.GetText();
                                            selectionTextBox.Text = textFromOCR;
                                            toolMessageAdd = "Found with " + Math.Round(page.GetMeanConfidence() * 100, 1) + "% Confidence: " + textFromOCR.Substring(0, Math.Clamp(textFromOCR.Length, 0, 15));
                                            toolMessageAdd = toolMessageAdd.Replace("\n", " ");
                                        }
                                    }
                                }
                                System.Threading.Thread.Sleep(20);
                                if (skipOrderLineType == 2)
                                {
                                    var result = MessageBox.Show("Hotkey pressed! " + skipOrderLineType + "\nDo you want to Skip the task line?/nNo will only skip OCR check on this task", "Skip Task", MessageBoxButtons.YesNoCancel);
                                    skipOrderLineType = 0;
                                    if (result == DialogResult.Yes)
                                    {

                                    }
                                    if (result == DialogResult.No)
                                    {

                                    }
                                }
                            }
                            if (order.targetOrder.Text == "Type")
                            {
                                SendKeys.Send(order.keyboardType.Text);
                            }
                            else
                            {
                                InputSender.MouseActioner((int)order.targetX.Value, (int)order.targetY.Value, order.targetOrder.Text);
                            }
                            System.Threading.Thread.Sleep(20);
                        }*/
        }
        private void DoOCRWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
        }
        private void UpdateSelectionAfterAreaCapture(object sender, EventArgs e)
        {
            selectionTextBox.Text = ScreenShot.selectionRect.ToString();
            if (lastGetXYSender == Screenshot)
            {

            }
            else
            {
                foreach (var line in orderLines)
                {
                    if (line.getXY == lastGetXYSender)
                    {
                        line.checkY1.Value = ScreenShot.selectionRect.Top;
                        line.checkX1.Value = ScreenShot.selectionRect.Left;
                        line.checkY2.Value = ScreenShot.selectionRect.Bottom;
                        line.checkX2.Value = ScreenShot.selectionRect.Right;
                        break;
                    }
                }
            }
            OrderGUIAfterRemoveLine();
        }
        private void RemoveLineButton_Click(object sender, EventArgs e)
        {
            foreach (var line in orderLines)
            {
                if (line.removeLineButton == sender)
                {
                    Controls.Remove(line.removeLineButton);
                    Controls.Remove(line.getXY);
                    Controls.Remove(line.keyboardType);
                    Controls.Remove(line.checkOCR);
                    Controls.Remove(line.targetY);
                    Controls.Remove(line.targetX);
                    Controls.Remove(line.targetOrder);
                    Controls.Remove(line.checkY2);
                    Controls.Remove(line.checkX2);
                    Controls.Remove(line.checkY1);
                    Controls.Remove(line.checkX1);
                    Controls.Remove(line.checkOptions);

                    orderLines.Remove(line);
                    break;
                }
            }
            OrderGUIAfterRemoveLine();
        }
        private void GetXYButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            AreaCapture form1 = new AreaCapture();
            form1.InstanceRef = this;
            form1.Show();
            ScreenShot.selectionOnly = true;
            lastGetXYSender = sender;
        }
        private void AddLineButton_Click(object sender, EventArgs e)
        {
            string newLineText = @"N/A|0|0|0|0|Move|0|0||{ENTER}";
            AddLines(newLineText);
        }
        private void OrderGUIAfterRemoveLine()
        {
            for (int i = 0; i < orderLines.Count; i++)
            {
                int linePosY = 37 + 30 * i;
                orderLines[i].checkOptions.Location = new Point(orderLines[i].checkOptions.Location.X, linePosY);
                orderLines[i].checkX1.Location = new Point(orderLines[i].checkX1.Location.X, linePosY);
                orderLines[i].checkY1.Location = new Point(orderLines[i].checkY1.Location.X, linePosY);
                orderLines[i].checkX2.Location = new Point(orderLines[i].checkX2.Location.X, linePosY);
                orderLines[i].checkY2.Location = new Point(orderLines[i].checkY2.Location.X, linePosY);
                orderLines[i].targetOrder.Location = new Point(orderLines[i].targetOrder.Location.X, linePosY);
                orderLines[i].targetX.Location = new Point(orderLines[i].targetX.Location.X, linePosY);
                orderLines[i].targetY.Location = new Point(orderLines[i].targetY.Location.X, linePosY);
                orderLines[i].removeLineButton.Location = new Point(orderLines[i].removeLineButton.Location.X, linePosY);
                orderLines[i].keyboardType.Location = new Point(orderLines[i].keyboardType.Location.X, linePosY);
                orderLines[i].getXY.Location = new Point(orderLines[i].getXY.Location.X, linePosY);
                orderLines[i].checkOCR.Location = new Point(orderLines[i].checkOCR.Location.X, linePosY);
            }
        }
        private void MouseXYToolStatusUpdate(object sender, EventArgs e)
        {
            toolStrip.Text = System.Windows.Forms.Control.MousePosition.X.ToString() + "," + System.Windows.Forms.Control.MousePosition.Y.ToString() + " " + toolMessageAdd;
        }
        private void Screenshot_Click(object sender, EventArgs e)
        {
            this.Hide();
            AreaCapture form1 = new AreaCapture();
            form1.InstanceRef = this;
            form1.Show();
            ScreenShot.selectionOnly = false;
            lastGetXYSender = sender;
        }
        private void OnFocusSelectAllNumericUpDown(object sender, EventArgs e)
        {
            NumericUpDown curBox = sender as NumericUpDown;
            curBox.Select();
            curBox.Select(0, curBox.Text.Length);
        }
        private void AddLines(string import)
        {
            string[] importArray = import.Split('|');
            int orderLineCount = orderLines.Count;
            int linePosY = 30 + orderLines.Last().removeLineButton.Location.Y;
            ControlGroup tempOrder = new ControlGroup();
            tempOrder.checkOptions = new ComboBox();
            tempOrder.checkX1 = new NumericUpDown();
            tempOrder.checkY1 = new NumericUpDown();
            tempOrder.checkX2 = new NumericUpDown();
            tempOrder.checkY2 = new NumericUpDown();
            tempOrder.targetOrder = new ComboBox();
            tempOrder.targetX = new NumericUpDown();
            tempOrder.targetY = new NumericUpDown();
            tempOrder.checkOCR = new TextBox();
            tempOrder.keyboardType = new TextBox();
            tempOrder.getXY = new Button();
            tempOrder.removeLineButton = new Button();

            // 
            // removeLineButton
            // 
            tempOrder.removeLineButton.Location = new Point(6, linePosY);
            tempOrder.removeLineButton.Name = "removeLineButton" + "-" + orderLineCount;
            tempOrder.removeLineButton.Size = new Size(23, 23);
            tempOrder.removeLineButton.TabIndex = 18 + orderLineCount * 12;
            tempOrder.removeLineButton.Text = "-";
            tempOrder.removeLineButton.UseVisualStyleBackColor = true;
            tempOrder.removeLineButton.Click += RemoveLineButton_Click;
            //
            // checkOptions
            //
            tempOrder.checkOptions.FormattingEnabled = true;
            tempOrder.checkOptions.Items.AddRange(new object[] { "N/A", "Image search", "OCR Check", "Image action" });
            tempOrder.checkOptions.Location = new Point(29, linePosY);
            tempOrder.checkOptions.Name = "checkOptions" + "-" + orderLineCount;
            tempOrder.checkOptions.Size = new Size(95, 23);
            tempOrder.checkOptions.TabIndex = 19 + orderLineCount * 12;
            tempOrder.checkOptions.Text = importArray[0];
            // 
            // checkX1
            // 
            tempOrder.checkX1.AllowDrop = true;
            tempOrder.checkX1.Location = new Point(130, linePosY);
            tempOrder.checkX1.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            tempOrder.checkX1.Name = "checkX1" + "-" + orderLineCount;
            tempOrder.checkX1.Size = new Size(45, 23);
            tempOrder.checkX1.TabIndex = 20 + orderLineCount * 12;
            tempOrder.checkX1.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkY1
            // 
            tempOrder.checkY1.Location = new Point(181, linePosY);
            tempOrder.checkY1.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            tempOrder.checkY1.Name = "checkY1" + "-" + orderLineCount;
            tempOrder.checkY1.Size = new Size(45, 23);
            tempOrder.checkY1.TabIndex = 21 + orderLineCount * 12;
            tempOrder.checkY1.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkX2
            // 
            tempOrder.checkX2.Location = new Point(232, linePosY);
            tempOrder.checkX2.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            tempOrder.checkX2.Name = "checkX2" + "-" + orderLineCount;
            tempOrder.checkX2.Size = new Size(45, 23);
            tempOrder.checkX2.TabIndex = 22 + orderLineCount * 12;
            tempOrder.checkX2.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkY2
            // 
            tempOrder.checkY2.Location = new Point(283, linePosY);
            tempOrder.checkY2.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            tempOrder.checkY2.Name = "checkY2" + "-" + orderLineCount;
            tempOrder.checkY2.Size = new Size(45, 23);
            tempOrder.checkY2.TabIndex = 23 + orderLineCount * 12;
            tempOrder.checkY2.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // getXY
            // 
            tempOrder.getXY.Location = new Point(330, linePosY);
            tempOrder.getXY.Name = "getXY" + "-" + orderLineCount;
            tempOrder.getXY.Size = new Size(54, 23);
            tempOrder.getXY.TabIndex = 24 + orderLineCount * 12;
            tempOrder.getXY.Text = "Get X,Y";
            tempOrder.getXY.UseVisualStyleBackColor = true;
            tempOrder.getXY.Click += GetXYButton_Click;
            // 
            // targetOrder
            // 
            tempOrder.targetOrder.FormattingEnabled = true;
            tempOrder.targetOrder.Items.AddRange(new object[] { "Move", "LeftClick", "LeftHold", "LeftRelease", "RightClick", "Type" });
            tempOrder.targetOrder.Location = new Point(387, linePosY);
            tempOrder.targetOrder.Name = "targetOrder" + "-" + orderLineCount;
            tempOrder.targetOrder.Size = new Size(85, 23);
            tempOrder.targetOrder.TabIndex = 25 + orderLineCount * 12;
            tempOrder.targetOrder.Text = importArray[5];
            // 
            // targetX
            // 
            tempOrder.targetX.Location = new Point(478, linePosY);
            tempOrder.targetX.Name = "targetX" + "-" + orderLineCount;
            tempOrder.targetX.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            tempOrder.targetX.Size = new Size(45, 23);
            tempOrder.targetX.TabIndex = 26 + orderLineCount * 12;
            tempOrder.targetX.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // targetY
            // 
            tempOrder.targetY.Location = new Point(529, linePosY);
            tempOrder.targetY.Name = "targetY";
            tempOrder.targetY.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
            tempOrder.targetY.Size = new Size(45, 23);
            tempOrder.targetY.TabIndex = 27 + orderLineCount * 12;
            tempOrder.targetY.Enter += OnFocusSelectAllNumericUpDown;
            // 
            // checkOCR
            // 
            tempOrder.checkOCR.Location = new Point(580, linePosY);
            tempOrder.checkOCR.Name = "checkOCR" + "-" + orderLineCount;
            tempOrder.checkOCR.Size = new Size(100, 23);
            tempOrder.checkOCR.TabIndex = 28 + orderLineCount * 12;
            tempOrder.checkOCR.PlaceholderText = "OCR Word";
            // 
            // keyboardType
            // 
            tempOrder.keyboardType.Location = new Point(686, linePosY);
            tempOrder.keyboardType.Name = "keyboardType" + "-" + orderLineCount;
            tempOrder.keyboardType.Text = importArray[9];
            tempOrder.keyboardType.ScrollBars = ScrollBars.Both;
            tempOrder.keyboardType.Size = new Size(100, 23);
            tempOrder.keyboardType.TabIndex = 29 + orderLineCount * 12;
            Controls.Add(tempOrder.removeLineButton);
            Controls.Add(tempOrder.getXY);
            Controls.Add(tempOrder.keyboardType);
            Controls.Add(tempOrder.checkOCR);
            Controls.Add(tempOrder.targetY);
            Controls.Add(tempOrder.targetX);
            Controls.Add(tempOrder.targetOrder);
            Controls.Add(tempOrder.checkY2);
            Controls.Add(tempOrder.checkX2);
            Controls.Add(tempOrder.checkY1);
            Controls.Add(tempOrder.checkX1);
            Controls.Add(tempOrder.checkOptions);

            orderLines.Add(tempOrder);
            //Button button2 = removeLineButton.Clone();
            //button2.Location = new Point(removeLineButton.Location.X, removeLineButton.Location.Y + 90);
        }
        private void ExportButton_Click(object sender, EventArgs e)
        {
            exportFileDialog1.DefaultExt = "txt";
            exportFileDialog1.Filter = "txt file (*.txt)|*.txt";
            exportFileDialog1.Title = "Save Tasks to...";
            exportFileDialog1.FileName = "Export-" + DateTime.Now.ToShortDateString().Replace(@"/", "-") + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".txt";
            exportFileDialog1.ShowDialog();
            string exportOrders = "";
            foreach (var order in orderLines)
            {
                exportOrders += order.checkOptions.Text + "|";
                exportOrders += order.checkX1.Text + "|";
                exportOrders += order.checkY1.Text + "|";
                exportOrders += order.checkX2.Text + "|";
                exportOrders += order.checkY2.Text + "|";
                exportOrders += order.targetOrder.Text + "|";
                exportOrders += order.targetX.Text + "|";
                exportOrders += order.targetY.Text + "|";
                exportOrders += order.checkOCR.Text + "|";
                exportOrders += order.keyboardType.Text + "\n";
            }

            File.WriteAllText(exportFileDialog1.FileName, exportOrders);
        }
    }

    class ScreenShot
    {
        public static bool saveToClipboard = false;
        public static bool selectionOnly = true;
        public static Rectangle selectionRect = new Rectangle();

        public static void CaptureImage(bool showCursor, Size curSize, Point curPos, Point SourcePoint, Point DestinationPoint, Rectangle SelectionRectangle, string FilePath, string extension)
        {
            using (Bitmap bitmap = new Bitmap(SelectionRectangle.Width, SelectionRectangle.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(SourcePoint, DestinationPoint, SelectionRectangle.Size);

                    if (showCursor)
                    {
                        Rectangle cursorBounds = new Rectangle(curPos, curSize);
                        Cursors.Default.Draw(g, cursorBounds);
                    }
                }

                if (saveToClipboard)
                {
                    Image img = (Image)bitmap;
                    Clipboard.SetImage(img);
                }
                else
                {
                    switch (extension)
                    {
                        case ".bmp":
                            bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case ".jpg":
                            bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case ".gif":
                            bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case ".tiff":
                            bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Tiff);
                            break;
                        case ".png":
                            bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        default:
                            bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                    }
                }
            }
        }
    }
    class HotKeyer
    {
        [DllImport("user32.dll")] public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")] public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        //RegisterHotKey(this.Handle, HOTKEY_ID, YOUR_HOTKEY_MODIFIERS, YOUR_HOTKEY_KEY);
        public const int WM_HOTKEY = 0x0312, MOD_ALT = 0x0001, MOD_CONTROL = 0x0002, MOD_SHIFT = 0x0004, MOD_WIN = 0x0008;

        private const int HOTKEY_ID = 1; // Unique identifier for your hotkey
        private const int YOUR_HOTKEY_MODIFIERS = 0; // Choose your modifiers
        private const int YOUR_HOTKEY_KEY = (int)Keys.F6; // Choose your key


    }
    class InputSender
    {
        #region Imports/Structs/Enums
        /*        [StructLayout(LayoutKind.Sequential)]
                public struct KeyboardInput
                {
                    public ushort wVk;
                    public ushort wScan;
                    public uint dwFlags;
                    public uint time;
                    public IntPtr dwExtraInfo;
                }
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            //            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public HardwareInput hi;
        }

        public struct Input
        {
            public int type;
            public InputUnion u;
        }

        [Flags]
        public enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        /*        [Flags]
                public enum KeyEventF
                {
                    KeyDown = 0x0000,
                    ExtendedKey = 0x0001,
                    KeyUp = 0x0002,
                    Unicode = 0x0004,
                    Scancode = 0x0008
                }
        */
        [Flags]
        public enum MouseEventF
        {
            Absolute = 0x8000,
            HWheel = 0x01000,
            Move = 0x0001,
            MoveNoCoalesce = 0x2000,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            XDown = 0x0080,
            XUp = 0x0100
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);
        #endregion

        #region Wrapper Methods
        public static POINT GetCursorPosition()
        {
            GetCursorPos(out POINT point);
            return point;
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }
        /*
                public static void SendKeyboardInput(KeyboardInput[] kbInputs)
                {
                    Input[] inputs = new Input[kbInputs.Length];

                    for (int i = 0; i < kbInputs.Length; i++)
                    {
                        inputs[i] = new Input
                        {
                            type = (int)InputType.Keyboard,
                            u = new InputUnion
                            {
                                ki = kbInputs[i]
                            }
                        };
                    }

                    SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
                }

                public static void ClickKey(ushort scanCode)
                {
                    var inputs = new KeyboardInput[]
                    {
                        new KeyboardInput
                        {
                            wScan = scanCode,
                            dwFlags = (uint)(KeyEventF.KeyDown | KeyEventF.Scancode),
                            dwExtraInfo = GetMessageExtraInfo()
                        },
                        new KeyboardInput
                        {
                            wScan = scanCode,
                            dwFlags = (uint)(KeyEventF.KeyUp | KeyEventF.Scancode),
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    };
                    SendKeyboardInput(inputs);
                }
        */
        public static void MouseActioner(int x, int y, string action)
        {
            System.Threading.Thread.Sleep(20);

            InputSender.SetCursorPosition(x, y);
            System.Threading.Thread.Sleep(20);
            if (action == "LeftClick")
            {
                InputSender.SendMouseInput(new InputSender.MouseInput[]
                    {
                new InputSender.MouseInput
                {
                    dwFlags = (uint)InputSender.MouseEventF.LeftDown
                },
                new InputSender.MouseInput
                {
                    dwFlags = (uint)InputSender.MouseEventF.LeftUp
                }
                    });
            }
            if (action == "RightClick")
            {
                InputSender.SendMouseInput(new InputSender.MouseInput[]
                    {
                new InputSender.MouseInput
                {
                    dwFlags = (uint)InputSender.MouseEventF.RightDown
                },
                new InputSender.MouseInput
                {
                    dwFlags = (uint)InputSender.MouseEventF.RightUp
                }
                    });
            }
            if (action == "LeftHold")
            {
                InputSender.SendMouseInput(new InputSender.MouseInput[]
                    {
                new InputSender.MouseInput
                {
                    dwFlags = (uint)InputSender.MouseEventF.LeftDown
                }
                    });
            }
            if (action == "LeftRelease")
            {
                InputSender.SendMouseInput(new InputSender.MouseInput[]
                    {
                new InputSender.MouseInput
                {
                    dwFlags = (uint)InputSender.MouseEventF.LeftUp
                }
                    });
            }
            System.Threading.Thread.Sleep(20);
        }

        public static void SendMouseInput(MouseInput[] mInputs)
        {
            Input[] inputs = new Input[mInputs.Length];

            for (int i = 0; i < mInputs.Length; i++)
            {
                inputs[i] = new Input
                {
                    type = (int)InputType.Mouse,
                    u = new InputUnion
                    {
                        mi = mInputs[i]
                    }
                };
            }

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }
        #endregion
    }
}