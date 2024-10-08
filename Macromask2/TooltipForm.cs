namespace Macromask2
{
    public partial class TooltipForm : Form
    {
        private Label labelXY;
        private Point startPoint; // Store the starting point
        private Rectangle selectionRectangle; // Store the selection rectangle
        private bool isSelecting = false; // Track if the user is selecting an area
        public event Action<Point, Point> SelectionCompleted; // Event to notify when selection is done
        public TooltipForm()
        {
            InitializeComponent();
            // Configure form appearance (transparent, no borders)
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.LightYellow;  
            this.TopMost = true;  
            this.ShowInTaskbar = false;
            this.Opacity = 0.5;  

            // Add the label to show coordinates
            labelXY = new Label();
            labelXY.AutoSize = true;
            labelXY.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Controls.Add(labelXY);


            // Handle mouse events for drawing the selection area
            this.MouseDown += TooltipForm_MouseDown;
            this.MouseMove += TooltipForm_MouseMove;
            this.MouseUp += TooltipForm_MouseUp;
        }
        // Method to update coordinates displayed on the tooltip
        public void UpdateCoordinates(int x, int y)
        {
            labelXY.Text = $"X: {x}, Y: {y}";
            this.Size = labelXY.Size; // Adjust the size of the form to match the label
        }
        private void TooltipForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Start selection
                startPoint = e.Location;
                isSelecting = true;
            }
        }
        private void TooltipForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                // Update the selection rectangle as the user drags the mouse
                Point endPoint = e.Location;
                selectionRectangle = new Rectangle(
                    Math.Min(startPoint.X, endPoint.X),
                    Math.Min(startPoint.Y, endPoint.Y),
                    Math.Abs(startPoint.X - endPoint.X),
                    Math.Abs(startPoint.Y - endPoint.Y)
                );

                // Redraw the form to show the updated selection rectangle
                this.Invalidate(); // Trigger Paint event to draw the selection
            }
        }
        private void TooltipForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (isSelecting && e.Button == MouseButtons.Left)
            {
                // End selection
                Point endPoint = e.Location;
                isSelecting = false;

                // Trigger the selection completed event and pass the start and end points
                SelectionCompleted?.Invoke(startPoint, endPoint);

                // Hide the form after selection
                this.Hide();
            }
        }
        // Draw the selection rectangle on the form
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (isSelecting)
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, selectionRectangle);
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                isSelecting = false;  // Cancel the selection
                this.Hide();           // Hide the form
                return true;           // Indicate that we've handled the key press
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
