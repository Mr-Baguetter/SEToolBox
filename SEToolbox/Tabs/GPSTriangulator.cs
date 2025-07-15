using SEToolBox.API.Data;
using SEToolBox.API.Struts;

namespace SEToolBox.Tabs
{
    public class GPSTriangulatorTab : UserControl
    {
        private List<GPSPoint> gpsPoints;
        private DataGridView gpsDataGrid;
        private RichTextBox resultTextBox;
        private Button addButton;
        private Button calculateButton;
        private Button clearButton;
        private Button importButton;
        private TextBox importTextBox;

        public GPSTriangulatorTab()
        {
            InitializeComponent();
            gpsPoints = new List<GPSPoint>();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Space Engineers Toolbox - GPS Triangulator";

            gpsDataGrid = new DataGridView();
            gpsDataGrid.Location = new Point(12, 12);
            gpsDataGrid.Size = new Size(760, 200);
            gpsDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gpsDataGrid.AllowUserToAddRows = false;
            gpsDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gpsDataGrid.Columns.Add("Name", "Name");
            gpsDataGrid.Columns.Add("X", "X");
            gpsDataGrid.Columns.Add("Y", "Y");
            gpsDataGrid.Columns.Add("Z", "Z");

            gpsDataGrid.Columns["Name"].Width = 200;
            gpsDataGrid.Columns["X"].Width = 150;
            gpsDataGrid.Columns["Y"].Width = 150;
            gpsDataGrid.Columns["Z"].Width = 150;

            this.Controls.Add(gpsDataGrid);

            Label importLabel = new Label();
            importLabel.Text = "Import GPS String:";
            importLabel.Location = new Point(12, 225);
            importLabel.Size = new Size(120, 20);
            this.Controls.Add(importLabel);

            importTextBox = new TextBox();
            importTextBox.Location = new Point(12, 248);
            importTextBox.Size = new Size(400, 20);
            importTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            importTextBox.PlaceholderText = "GPS:Point Name:12345.67:8910.11:12345.67:";
            this.Controls.Add(importTextBox);

            Label version = new Label();
            version.Text = $"Alpha {MainForm.version}";
            version.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            version.Location = new Point(40, 130);
            version.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            version.Size = new Size(300, 25);
            this.Controls.Add(version);

            importButton = new Button();
            importButton.Text = "Import";
            importButton.Location = new Point(420, 246);
            importButton.Size = new Size(80, 25);
            importButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            importButton.Click += ImportButton_Click;
            this.Controls.Add(importButton);

            Label manualLabel = new Label();
            manualLabel.Text = "Manual Entry:";
            manualLabel.Location = new Point(12, 285);
            manualLabel.Size = new Size(100, 20);
            this.Controls.Add(manualLabel);

            Label nameLabel = new Label();
            nameLabel.Text = "Name:";
            nameLabel.Location = new Point(12, 310);
            nameLabel.Size = new Size(50, 20);
            this.Controls.Add(nameLabel);

            TextBox nameTextBox = new TextBox();
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Location = new Point(65, 308);
            nameTextBox.Size = new Size(100, 20);
            this.Controls.Add(nameTextBox);

            Label xLabel = new Label();
            xLabel.Text = "X:";
            xLabel.Location = new Point(175, 310);
            xLabel.Size = new Size(20, 20);
            this.Controls.Add(xLabel);

            TextBox xTextBox = new TextBox();
            xTextBox.Name = "xTextBox";
            xTextBox.Location = new Point(195, 308);
            xTextBox.Size = new Size(80, 20);
            this.Controls.Add(xTextBox);

            Label yLabel = new Label();
            yLabel.Text = "Y:";
            yLabel.Location = new Point(285, 310);
            yLabel.Size = new Size(20, 20);
            this.Controls.Add(yLabel);

            TextBox yTextBox = new TextBox();
            yTextBox.Name = "yTextBox";
            yTextBox.Location = new Point(305, 308);
            yTextBox.Size = new Size(80, 20);
            this.Controls.Add(yTextBox);

            Label zLabel = new Label();
            zLabel.Text = "Z:";
            zLabel.Location = new Point(395, 310);
            zLabel.Size = new Size(20, 20);
            this.Controls.Add(zLabel);

            TextBox zTextBox = new TextBox();
            zTextBox.Name = "zTextBox";
            zTextBox.Location = new Point(415, 308);
            zTextBox.Size = new Size(80, 20);
            this.Controls.Add(zTextBox);

            addButton = new Button();
            addButton.Text = "Add Point";
            addButton.Location = new Point(12, 340);
            addButton.Size = new Size(80, 30);
            addButton.Click += AddButton_Click;
            this.Controls.Add(addButton);

            calculateButton = new Button();
            calculateButton.Text = "Calculate";
            calculateButton.Location = new Point(100, 340);
            calculateButton.Size = new Size(80, 30);
            calculateButton.Click += CalculateButton_Click;
            this.Controls.Add(calculateButton);

            clearButton = new Button();
            clearButton.Text = "Clear All";
            clearButton.Location = new Point(188, 340);
            clearButton.Size = new Size(80, 30);
            clearButton.Click += ClearButton_Click;
            this.Controls.Add(clearButton);

            Button deleteButton = new Button();
            deleteButton.Text = "Delete Selected";
            deleteButton.Location = new Point(276, 340);
            deleteButton.Size = new Size(100, 30);
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            Label resultLabel = new Label();
            resultLabel.Text = "Triangulation Results:";
            resultLabel.Location = new Point(12, 385);
            resultLabel.Size = new Size(150, 20);
            this.Controls.Add(resultLabel);

            resultTextBox = new RichTextBox();
            resultTextBox.Location = new Point(12, 408);
            resultTextBox.Size = new Size(760, 240);
            resultTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            resultTextBox.ReadOnly = true;
            resultTextBox.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            this.Controls.Add(resultTextBox);

            this.ResumeLayout(false);
        }
        private void ImportButton_Click(object sender, EventArgs e)
        {
            try
            {
                string gpsString = importTextBox.Text.Trim();
                if (string.IsNullOrEmpty(gpsString))
                    return;

                GPSPoint point = ParseGPSString(gpsString);
                if (point != null)
                {
                    gpsPoints.Add(point);
                    RefreshGrid();
                    importTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Invalid GPS format. Expected format: GPS:Name:X:Y:Z:");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing GPS: {ex.Message}");
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox nameBox = this.Controls.Find("nameTextBox", true)[0] as TextBox;
                TextBox xBox = this.Controls.Find("xTextBox", true)[0] as TextBox;
                TextBox yBox = this.Controls.Find("yTextBox", true)[0] as TextBox;
                TextBox zBox = this.Controls.Find("zTextBox", true)[0] as TextBox;

                if (string.IsNullOrEmpty(nameBox.Text) ||
                    !double.TryParse(xBox.Text, out double x) ||
                    !double.TryParse(yBox.Text, out double y) ||
                    !double.TryParse(zBox.Text, out double z))
                {
                    MessageBox.Show("Please fill in all fields with valid numbers.");
                    return;
                }

                GPSPoint point = new GPSPoint
                {
                    Name = nameBox.Text,
                    X = x,
                    Y = y,
                    Z = z
                };

                gpsPoints.Add(point);
                RefreshGrid();

                nameBox.Clear();
                xBox.Clear();
                yBox.Clear();
                zBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding point: {ex.Message}");
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (gpsPoints.Count < 2)
            {
                MessageBox.Show("Need at least 2 GPS points to triangulate.");
                return;
            }

            try
            {
                Vector3D centerPoint = CalculateCenterPoint(gpsPoints);

                string resultText = $"Triangulation of {gpsPoints.Count} GPS coordinates:\n\n";
                resultText += $"X: {centerPoint.X:F2}\n";
                resultText += $"Y: {centerPoint.Y:F2}\n";
                resultText += $"Z: {centerPoint.Z:F2}\n\n";

                resultText += $"GPS String: GPS:Triangulation:{centerPoint.X:F2}:{centerPoint.Y:F2}:{centerPoint.Z:F2}:\n\n";

                resultText += "Distances from Triangulation to each GPS coordinate:\n";
                foreach (var point in gpsPoints)
                {
                    double distance = Vector3D.Distance(centerPoint, new Vector3D(point.X, point.Y, point.Z));
                    resultText += $"{point.Name}: {distance:F2} units\n";
                }

                double avgDistance = gpsPoints.Average(p =>
                    Vector3D.Distance(centerPoint, new Vector3D(p.X, p.Y, p.Z)));
                resultText += $"\nAverage distance from center: {avgDistance:F2} units";

                resultTextBox.Text = resultText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating Triangulation: {ex.Message}");
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            gpsPoints.Clear();
            RefreshGrid();
            resultTextBox.Clear();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (gpsDataGrid.SelectedRows.Count > 0)
            {
                int index = gpsDataGrid.SelectedRows[0].Index;
                if (index >= 0 && index < gpsPoints.Count)
                {
                    gpsPoints.RemoveAt(index);
                    RefreshGrid();
                }
            }
        }

        private void RefreshGrid()
        {
            gpsDataGrid.Rows.Clear();
            foreach (var point in gpsPoints)
            {
                gpsDataGrid.Rows.Add(point.Name, point.X, point.Y, point.Z);
            }
        }

        private GPSPoint ParseGPSString(string gpsString)
        {
            if (!gpsString.StartsWith("GPS:"))
                return null;

            string[] parts = gpsString.Split(':');
            if (parts.Length < 5)
                return null;

            if (!double.TryParse(parts[2], out double x) ||
                !double.TryParse(parts[3], out double y) ||
                !double.TryParse(parts[4], out double z))
                return null;

            return new GPSPoint
            {
                Name = parts[1],
                X = x,
                Y = y,
                Z = z
            };
        }

        private Vector3D CalculateCenterPoint(List<GPSPoint> points)
        {
            if (points.Count == 0)
                throw new ArgumentException("No points provided");

            double avgX = points.Average(p => p.X);
            double avgY = points.Average(p => p.Y);
            double avgZ = points.Average(p => p.Z);

            return new Vector3D(avgX, avgY, avgZ);
        }
    }
}
