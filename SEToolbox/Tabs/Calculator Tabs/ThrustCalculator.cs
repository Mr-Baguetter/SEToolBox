using SEToolBox.API.Configs;
using SEToolBox.API.Data;
using SEToolBox.API.Enums;
using SEToolBox;

namespace SEToolBox.Tabs.Calculator_Tabs
{
    public class ThrustCalculatorTab : UserControl
    {
        private ComboBox planetComboBox;
        private TextBox massTextBox;
        private ListBox thrusterListBox;
        private TextBox thrusterCountTextBox;
        private Button addThrusterButton;
        private Button removeThrusterButton;
        private Button calculateButton;
        private Button clearButton;
        private Button loadBlueprintButton;
        private RichTextBox resultsTextBox;
        private List<ThrusterConfig> configuredThrusters;
        private Dictionary<string, PlanetData> planetData;
        private Dictionary<string, ThrusterData> thrusterData;

        public ThrustCalculatorTab()
        {
            configuredThrusters = new List<ThrusterConfig>();
            InitializePlanetData();
            InitializeThrusterData();
            InitializeComponent();
        }

        private void InitializePlanetData()
        {
            planetData = new Dictionary<string, PlanetData>
            {
                { "Earth-like", new PlanetData { Name = "Earth-like", GravityMultiplier = 1.0, AtmosphereDensity = 1.0, HasAtmosphere = true } },
                { "Mars", new PlanetData { Name = "Mars", GravityMultiplier = 0.9, AtmosphereDensity = 1.0, HasAtmosphere = true } },
                { "Alien Planet", new PlanetData { Name = "Alien Planet", GravityMultiplier = 1.1, AtmosphereDensity = 1.2, HasAtmosphere = true } },
                { "Moon", new PlanetData { Name = "Moon", GravityMultiplier = 0.25, AtmosphereDensity = 0.0, HasAtmosphere = false } },
                { "Europa", new PlanetData { Name = "Europa", GravityMultiplier = 0.25, AtmosphereDensity = 1.0, HasAtmosphere = true } },
                { "Titan", new PlanetData { Name = "Titan", GravityMultiplier = 0.25, AtmosphereDensity = 1.0, HasAtmosphere = true } },
                { "Triton", new PlanetData { Name = "Triton", GravityMultiplier = 0.25, AtmosphereDensity = 1.0, HasAtmosphere = true } },
                { "Pertam", new PlanetData { Name = "Pertam", GravityMultiplier = 1.2, AtmosphereDensity = 1.0, HasAtmosphere = true } },
                { "Space", new PlanetData { Name = "Space", GravityMultiplier = 0.0, AtmosphereDensity = 0.0, HasAtmosphere = false } }
            };
        }

        private void InitializeThrusterData()
        {
            thrusterData = new Dictionary<string, ThrusterData>
            {
                // Small-grid thrusters
                { "Small Atmospheric (SG)", new ThrusterData { Name = "Small Atmospheric (SG)", MaxThrust = 96, PowerConsumption = 0.6, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = false, Type = ThrusterType.Atmospheric } },
                { "Small Flat Atmospheric (SG)", new ThrusterData { Name = "Small Flat Atmospheric (SG)", MaxThrust = 32, PowerConsumption = 0.2, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = false, Type = ThrusterType.Atmospheric } },
                { "Small Ion (SG)", new ThrusterData { Name = "Small Ion (SG)", MaxThrust = 14.4, PowerConsumption = 0.2, EffectivenessOnPlanet = 0.2, EffectivenessInSpace = 1.0, IsLarge = false, Type = ThrusterType.Ion } },
                { "Small Hydrogen (SG)", new ThrusterData { Name = "Small Hydrogen (SG)", MaxThrust = 98.4, PowerConsumption = 0.080, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 1.0, IsLarge = false, Type = ThrusterType.Hydrogen } },
                { "Large Atmospheric (SG)", new ThrusterData { Name = "Large Atmospheric (SG)", MaxThrust = 576, PowerConsumption = 2.4, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = false, Type = ThrusterType.Atmospheric } },
                { "Large Flat Atmospheric (SG)", new ThrusterData { Name = "Large Flat Atmospheric (SG)", MaxThrust = 230, PowerConsumption = 1, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = false, Type = ThrusterType.Atmospheric } },
                { "Large Ion (SG)", new ThrusterData { Name = "Large Ion (SG)", MaxThrust = 172.8, PowerConsumption = 2.4, EffectivenessOnPlanet = 0.2, EffectivenessInSpace = 1.0, IsLarge = false, Type = ThrusterType.Ion } },
                { "Large Hydrogen (SG)", new ThrusterData { Name = "Large Hydrogen (SG)", MaxThrust = 480, PowerConsumption = 0.3856, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 1.0, IsLarge = false, Type = ThrusterType.Hydrogen } },
                { "Prototech Thruster (SG)", new ThrusterData { Name = "Prototech Thruster (SG)", MaxThrust = 561.6, PowerConsumption = 4.8, EffectivenessOnPlanet = 0.3, EffectivenessInSpace = 1.0, IsLarge = false, Type = ThrusterType.Ion } },

                // Large-grid thrusters
                { "Small Atmospheric (LG)", new ThrusterData { Name = "Large Atmospheric (LG)", MaxThrust = 648, PowerConsumption = 2.4, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = true, Type = ThrusterType.Atmospheric } },
                { "Small Flat Atmospheric (LG)", new ThrusterData { Name = "Large Flat Atmospheric (LG)", MaxThrust = 200, PowerConsumption = 0.8, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = true, Type = ThrusterType.Atmospheric } },
                { "Small Ion (LG)", new ThrusterData { Name = "Large Ion (LG)", MaxThrust = 345.6, PowerConsumption = 3.36, EffectivenessOnPlanet = 0.2, EffectivenessInSpace = 1.0, IsLarge = true, Type = ThrusterType.Ion } },
                { "Small Hydrogen (LG)", new ThrusterData { Name = "Large Hydrogen (LG)", MaxThrust = 1080, PowerConsumption = 0.80334, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 1.0, IsLarge = true, Type = ThrusterType.Hydrogen } },
                { "Large Atmospheric (LG)", new ThrusterData { Name = "Small Atmospheric (LG)", MaxThrust = 6480, PowerConsumption = 16.8, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = true, Type = ThrusterType.Atmospheric } },
                { "Large Flat Atmospheric (LG)", new ThrusterData { Name = "Small Flat Atmospheric (LG)", MaxThrust = 2600, PowerConsumption = 6.7, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 0.0, IsLarge = true, Type = ThrusterType.Atmospheric } },
                { "Large Ion (LG)", new ThrusterData { Name = "Small Ion (LG)", MaxThrust = 4320, PowerConsumption = 33.6, EffectivenessOnPlanet = 0.2, EffectivenessInSpace = 1.0, IsLarge = true, Type = ThrusterType.Ion } },
                { "Large Hydrogen (LG)", new ThrusterData { Name = "Large Hydrogen (LG)", MaxThrust = 7200, PowerConsumption = 4.82005, EffectivenessOnPlanet = 1.0, EffectivenessInSpace = 1.0, IsLarge = true, Type = ThrusterType.Hydrogen } },
                { "Prototech Thruster (LG)", new ThrusterData { Name = "Prototech Thruster (LG)", MaxThrust = 14040, PowerConsumption = 67.2, EffectivenessOnPlanet = 0.3, EffectivenessInSpace = 1.0, IsLarge = true, Type = ThrusterType.Ion } },
            };
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Space Engineers Toolbox - Thrust Calculator";

            Label titleLabel = new Label();
            titleLabel.Text = "Thrust Calculator";
            titleLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 25);
            this.Controls.Add(titleLabel);

            Label version = new Label();
            version.Text = $"Alpha {MainForm.version}";
            version.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            version.Location = new Point(40, 130);
            version.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            version.Size = new Size(300, 25);
            this.Controls.Add(version);

            Label planetLabel = new Label();
            planetLabel.Text = "Planet/Environment:";
            planetLabel.Location = new Point(20, 60);
            planetLabel.Size = new Size(120, 20);
            this.Controls.Add(planetLabel);

            planetComboBox = new ComboBox();
            planetComboBox.Location = new Point(145, 58);
            planetComboBox.Size = new Size(150, 21);
            planetComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            planetComboBox.Items.AddRange(planetData.Keys.ToArray());
            planetComboBox.SelectedIndex = 0;
            this.Controls.Add(planetComboBox);

            Label massLabel = new Label();
            massLabel.Text = "Ship Mass (kg):";
            massLabel.Location = new Point(320, 60);
            massLabel.Size = new Size(100, 20);
            this.Controls.Add(massLabel);

            massTextBox = new TextBox();
            massTextBox.Location = new Point(425, 58);
            massTextBox.Size = new Size(100, 20);
            massTextBox.Text = "100000";
            this.Controls.Add(massTextBox);

            Label thrusterLabel = new Label();
            thrusterLabel.Text = "Thruster Configuration:";
            thrusterLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            thrusterLabel.Location = new Point(20, 100);
            thrusterLabel.Size = new Size(200, 20);
            this.Controls.Add(thrusterLabel);

            Label selectThrusterLabel = new Label();
            selectThrusterLabel.Text = "Select Thruster:";
            selectThrusterLabel.Location = new Point(20, 130);
            selectThrusterLabel.Size = new Size(100, 20);
            this.Controls.Add(selectThrusterLabel);

            thrusterListBox = new ListBox();
            thrusterListBox.Location = new Point(20, 155);
            thrusterListBox.Size = new Size(200, 100);
            thrusterListBox.Items.AddRange(thrusterData.Keys.ToArray());
            this.Controls.Add(thrusterListBox);

            Label countLabel = new Label();
            countLabel.Text = "Count:";
            countLabel.Location = new Point(240, 155);
            countLabel.Size = new Size(50, 20);
            this.Controls.Add(countLabel);

            thrusterCountTextBox = new TextBox();
            thrusterCountTextBox.Location = new Point(290, 153);
            thrusterCountTextBox.Size = new Size(60, 20);
            thrusterCountTextBox.Text = "1";
            this.Controls.Add(thrusterCountTextBox);

            Label gridSizeLabel = new Label();
            gridSizeLabel.Text = "Grid Size:";
            gridSizeLabel.Location = new Point(540, 60);
            gridSizeLabel.Size = new Size(70, 20);
            this.Controls.Add(gridSizeLabel);

            ComboBox gridSizeComboBox = new ComboBox();
            gridSizeComboBox.Name = "gridSizeComboBox";
            gridSizeComboBox.Location = new Point(610, 58);
            gridSizeComboBox.Size = new Size(100, 21);
            gridSizeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            gridSizeComboBox.Items.AddRange(new[] { "Small", "Large" });
            gridSizeComboBox.SelectedIndexChanged += GridSizeComboBox_SelectedIndexChanged;
            gridSizeComboBox.SelectedIndex = 0;
            this.Controls.Add(gridSizeComboBox);

            addThrusterButton = new Button();
            addThrusterButton.Text = "Add Thruster";
            addThrusterButton.Location = new Point(360, 151);
            addThrusterButton.Size = new Size(90, 25);
            addThrusterButton.Click += AddThrusterButton_Click;
            this.Controls.Add(addThrusterButton);

            Label configLabel = new Label();
            configLabel.Text = "Current Configuration:";
            configLabel.Location = new Point(20, 270);
            configLabel.Size = new Size(150, 20);
            this.Controls.Add(configLabel);

            ListBox configListBox = new ListBox();
            configListBox.Name = "configListBox";
            configListBox.Location = new Point(20, 295);
            configListBox.Size = new Size(400, 80);
            this.Controls.Add(configListBox);

            removeThrusterButton = new Button();
            removeThrusterButton.Text = "Remove Selected";
            removeThrusterButton.Location = new Point(430, 295);
            removeThrusterButton.Size = new Size(100, 25);
            removeThrusterButton.Click += RemoveThrusterButton_Click;
            this.Controls.Add(removeThrusterButton);

            calculateButton = new Button();
            calculateButton.Text = "Calculate";
            calculateButton.Location = new Point(20, 385);
            calculateButton.Size = new Size(80, 30);
            calculateButton.Click += CalculateButton_Click;
            this.Controls.Add(calculateButton);

            clearButton = new Button();
            clearButton.Text = "Clear";
            clearButton.Location = new Point(110, 385);
            clearButton.Size = new Size(80, 30);
            clearButton.Click += ClearButton_Click;
            this.Controls.Add(clearButton);

            Label resultsLabel = new Label();
            resultsLabel.Text = "Results:";
            resultsLabel.Location = new Point(20, 425);
            resultsLabel.Size = new Size(100, 20);
            this.Controls.Add(resultsLabel);

            resultsTextBox = new RichTextBox();
            resultsTextBox.Location = new Point(20, 450);
            resultsTextBox.Size = new Size(600, 300);
            resultsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            resultsTextBox.ReadOnly = true;
            resultsTextBox.Font = new Font("Consolas", 9F, FontStyle.Regular);
            resultsTextBox.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            this.Controls.Add(resultsTextBox);

            this.ResumeLayout(false);
            GridSizeComboBox_SelectedIndexChanged(this.Controls.Find("gridSizeComboBox", true)[0], EventArgs.Empty);
        }

        private void AddThrusterButton_Click(object sender, EventArgs e)
        {
            if (thrusterListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a thruster type.");
                return;
            }

            if (!int.TryParse(thrusterCountTextBox.Text, out int count) || count <= 0)
            {
                MessageBox.Show("Please enter a valid count.");
                return;
            }

            string selectedThruster = thrusterListBox.SelectedItem.ToString();
            ThrusterData thrusterInfo = thrusterData[selectedThruster];

            configuredThrusters.Add(new ThrusterConfig
            {
                ThrusterType = selectedThruster,
                Count = count,
                Data = thrusterInfo
            });

            UpdateConfigurationDisplay();
            thrusterCountTextBox.Text = "1";
        }

        private void RemoveThrusterButton_Click(object sender, EventArgs e)
        {
            ListBox configListBox = this.Controls.Find("configListBox", true)[0] as ListBox;

            if (configListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a thruster configuration to remove.");
                return;
            }

            int selectedIndex = configListBox.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < configuredThrusters.Count)
            {
                configuredThrusters.RemoveAt(selectedIndex);
                UpdateConfigurationDisplay();
            }
        }

        private void GridSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox gridSizeComboBox = sender as ComboBox;
            string selectedSize = gridSizeComboBox.SelectedItem.ToString();
            bool isLargeGrid = selectedSize == "Large";

            var filteredThrusters = thrusterData.Values
                .Where(t => t.IsLarge == isLargeGrid)
                .Select(t => t.Name)
                .ToArray();

            thrusterListBox.Items.Clear();
            thrusterListBox.Items.AddRange(filteredThrusters);
        }

        private void UpdateConfigurationDisplay()
        {
            ListBox configListBox = this.Controls.Find("configListBox", true)[0] as ListBox;
            configListBox.Items.Clear();

            foreach (var config in configuredThrusters)
            {
                configListBox.Items.Add($"{config.Count}x {config.ThrusterType}");
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(massTextBox.Text, out double shipMass) || shipMass <= 0)
            {
                MessageBox.Show("Please enter a valid ship mass.");
                return;
            }

            if (configuredThrusters.Count == 0)
            {
                MessageBox.Show("Please add at least one thruster configuration.");
                return;
            }

            string selectedPlanet = planetComboBox.SelectedItem.ToString();
            PlanetData planet = planetData[selectedPlanet];

            CalculateThrustRequirements(shipMass, planet);
        }

        private void CalculateThrustRequirements(double shipMass, PlanetData planet)
        {
            double gravityForce = shipMass * planet.GravityMultiplier * 9.81;
            double totalThrust = 0;
            double totalPowerConsumption = 0;
            double totalHydrogenConsumption = 0;

            var results = new System.Text.StringBuilder();
            results.AppendLine($"=== THRUST CALCULATION RESULTS ===");
            results.AppendLine($"Planet: {planet.Name}");
            results.AppendLine($"Gravity: {planet.GravityMultiplier:F1}g");
            results.AppendLine($"Ship Mass: {shipMass:N0} kg");
            results.AppendLine($"Gravity Force to Counter: {gravityForce:N0} N");
            results.AppendLine();

            results.AppendLine("THRUSTER CONFIGURATION:");
            foreach (var config in configuredThrusters)
            {
                double effectiveness = planet.Name == "Space" ? config.Data.EffectivenessInSpace : config.Data.EffectivenessOnPlanet;

                if (config.Data.Type == ThrusterType.Atmospheric && planet.HasAtmosphere)
                {
                    effectiveness *= planet.AtmosphereDensity;
                }

                double thrusterForce = config.Data.MaxThrust * 1000 * effectiveness * config.Count;
                double powerConsumption = config.Data.PowerConsumption * config.Count;

                totalThrust += thrusterForce;
                if (config.Data.Type == ThrusterType.Hydrogen)
                {
                    totalHydrogenConsumption += powerConsumption;
                }
                else
                {
                    totalPowerConsumption += powerConsumption;
                }

                results.AppendLine($"{config.Count}x {config.ThrusterType}:");
                results.AppendLine($"  Effectiveness: {effectiveness:F1}");
                results.AppendLine($"  Total Force: {thrusterForce:N0} N");
                results.AppendLine($"  Power/Fuel: {powerConsumption:F2} {(config.Data.Type == ThrusterType.Hydrogen ? "L/s" : "MW")}");
                results.AppendLine();
            }

            results.AppendLine("SUMMARY:");
            results.AppendLine($"Total Available Thrust: {totalThrust:N0} N");
            results.AppendLine($"Required for Hover: {gravityForce:N0} N");

            if (planet.Name == "Space")
            {
                results.AppendLine($"Acceleration Available: {totalThrust / shipMass:F2} m/s²");
            }
            else
            {
                double excessThrust = totalThrust - gravityForce;
                results.AppendLine($"Excess Thrust: {excessThrust:N0} N");

                if (excessThrust > 0)
                {
                    double acceleration = excessThrust / shipMass;
                    results.AppendLine($"Upward Acceleration: {acceleration:F2} m/s²");
                    results.AppendLine($"Thrust-to-Weight Ratio: {totalThrust / gravityForce:F2}");

                    if (totalThrust / gravityForce >= 1.0)
                    {
                        results.AppendLine("✓ SHIP CAN HOVER AND LIFT OFF");
                    }
                    else
                    {
                        results.AppendLine("✗ INSUFFICIENT THRUST TO HOVER");
                    }
                }
                else
                {
                    results.AppendLine("✗ INSUFFICIENT THRUST TO COUNTER GRAVITY");
                    results.AppendLine($"Additional Thrust Needed: {Math.Abs(excessThrust):N0} N");
                }
            }

            results.AppendLine();
            results.AppendLine("POWER CONSUMPTION:");
            if (totalPowerConsumption > 0)
            {
                results.AppendLine($"Electrical Power: {totalPowerConsumption:F2} MW");
            }
            if (totalHydrogenConsumption > 0)
            {
                results.AppendLine($"Hydrogen: {totalHydrogenConsumption:F2} L/s");
            }

            resultsTextBox.Text = results.ToString();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            configuredThrusters.Clear();
            UpdateConfigurationDisplay();
            resultsTextBox.Clear();
        }
    }
}
