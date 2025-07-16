using SEToolBox.API;

namespace SEToolBox.Tabs
{
    public class HelpTab : UserControl
    {
        private Button updateButton;
        private Button aboutButton;
        private Label versionLabel;

        public HelpTab()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.BackColor = Color.White;
            this.Padding = new Padding(20);

            Label titleLabel = new Label();
            titleLabel.Text = "Space Engineers Toolbox";
            titleLabel.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);
            this.Controls.Add(titleLabel);

            versionLabel = new Label();
            versionLabel.Text = $"Version {MainForm.version}";
            versionLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
            versionLabel.Location = new Point(20, 60);
            versionLabel.Size = new Size(200, 20);
            this.Controls.Add(versionLabel);

            Label developerLabel = new Label();
            developerLabel.Text = "Developed by Mr-Baguetter";
            developerLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
            developerLabel.Location = new Point(20, 85);
            developerLabel.Size = new Size(200, 20);
            this.Controls.Add(developerLabel);

            updateButton = new Button();
            updateButton.Text = "Check for Updates";
            updateButton.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
            updateButton.Location = new Point(20, 130);
            updateButton.Size = new Size(150, 35);
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += UpdateButton_Click;
            this.Controls.Add(updateButton);

            aboutButton = new Button();
            aboutButton.Text = "About";
            aboutButton.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
            aboutButton.Location = new Point(180, 130);
            aboutButton.Size = new Size(100, 35);
            aboutButton.UseVisualStyleBackColor = true;
            aboutButton.Click += AboutButton_Click;
            this.Controls.Add(aboutButton);

            Label descriptionLabel = new Label();
            descriptionLabel.Text = "A collection of useful tools for Space Engineers players including GPS triangulation, calculators, and more.";
            descriptionLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);
            descriptionLabel.Location = new Point(20, 190);
            descriptionLabel.Size = new Size(500, 40);
            descriptionLabel.AutoSize = false;
            this.Controls.Add(descriptionLabel);

            this.ResumeLayout(false);
        }

        private async void UpdateButton_Click(object sender, EventArgs e) => await CheckAndInstallUpdates(true);

        private void AboutButton_Click(object sender, EventArgs e) => MessageBox.Show($"Space Engineers Toolbox\nVersion {MainForm.version}\n\nDeveloped by Mr. Baguetter\n\nA collection of useful tools for Space Engineers players.", "About SEToolBox", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public void SetUpdateButtonState(bool enabled, string text)
        {
            if (updateButton.InvokeRequired)
            {
                updateButton.Invoke(new Action(() =>
                {
                    updateButton.Enabled = enabled;
                    updateButton.Text = text;
                }));
            }
            else
            {
                updateButton.Enabled = enabled;
                updateButton.Text = text;
            }
        }

        public async Task CheckAndInstallUpdates(bool showNoUpdateMessage)
        {
            try
            {
                SetUpdateButtonState(false, "Checking for updates...");

                var hasUpdates = await AutoUpdater.CheckForUpdatesAsync();

                if (hasUpdates)
                {
                    var result = MessageBox.Show("A new version is available. The application will restart after the update. Continue?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SetUpdateButtonState(false, "Downloading update...");

                        var success = await AutoUpdater.DownloadAndInstallUpdateAsync();
                        if (success)
                            Application.Exit();
                        else
                            MessageBox.Show("Update failed. Please try again later.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (showNoUpdateMessage)
                    MessageBox.Show("You are running the latest version.", "No Updates Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUpdateButtonState(true, "Check for Updates");
            }
        }
    }
}
