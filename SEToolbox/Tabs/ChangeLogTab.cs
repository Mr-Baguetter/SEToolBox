using System;
using System.Drawing;
using System.Windows.Forms;

namespace SEToolBox.Tabs
{
    public partial class ChangeLogTab : UserControl
    {
        private RichTextBox changeLogTextBox;
        private Label titleLabel;

        public ChangeLogTab()
        {
            InitializeComponent();
            LoadChangeLog();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            titleLabel = new Label();
            titleLabel.Text = "Change Log";
            titleLabel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(10, 10);
            titleLabel.ForeColor = Color.FromArgb(64, 64, 64);

            changeLogTextBox = new RichTextBox();
            changeLogTextBox.Location = new Point(10, 45);
            changeLogTextBox.Size = new Size(1000, 900);
            changeLogTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            changeLogTextBox.ReadOnly = true;
            changeLogTextBox.BackColor = Color.White;
            changeLogTextBox.Font = new Font("Consolas", 10F, FontStyle.Regular);
            changeLogTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            changeLogTextBox.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(titleLabel);
            this.Controls.Add(changeLogTextBox);

            this.ResumeLayout(false);
        }

        private void LoadChangeLog()
        {
            changeLogTextBox.Clear();

            AddChangeLogEntry("v0.0.3", "2025-7-15", new string[]
            {
                "• Added Change Logs.",
                "• Added an application icon.",
                "• Minor bug fixes."
            });

            changeLogTextBox.SelectionStart = 0;
            changeLogTextBox.ScrollToCaret();
        }

        private void AddChangeLogEntry(string version, string date, string[] changes)
        {
            changeLogTextBox.SelectionFont = new Font("Consolas", 12F, FontStyle.Bold);
            changeLogTextBox.SelectionColor = Color.FromArgb(0, 100, 0);
            changeLogTextBox.AppendText($"{version} - {date}\n");

            changeLogTextBox.SelectionFont = new Font("Consolas", 10F, FontStyle.Regular);
            changeLogTextBox.SelectionColor = Color.Gray;
            changeLogTextBox.AppendText(new string('-', 30) + "\n");

            changeLogTextBox.SelectionFont = new Font("Consolas", 10F, FontStyle.Regular);
            changeLogTextBox.SelectionColor = Color.Black;
            foreach (string change in changes)
            {
                changeLogTextBox.AppendText($"{change}\n");
            }

            changeLogTextBox.AppendText("\n");
        }

        public void AddNewEntry(string version, string date, string[] changes)
        {
            changeLogTextBox.SelectionStart = 0;

            string newEntry = "";
            newEntry += $"{version} - {date}\n";
            newEntry += new string('-', 30) + "\n";
            foreach (string change in changes)
            {
                newEntry += $"{change}\n";
            }
            newEntry += "\n";

            changeLogTextBox.SelectedText = newEntry;

            changeLogTextBox.SelectionStart = 0;
            changeLogTextBox.SelectionLength = version.Length + date.Length + 3;
            changeLogTextBox.SelectionFont = new Font("Consolas", 12F, FontStyle.Bold);
            changeLogTextBox.SelectionColor = Color.FromArgb(0, 100, 0);

            changeLogTextBox.SelectionStart = 0;
            changeLogTextBox.SelectionLength = 0;
            changeLogTextBox.ScrollToCaret();
        }
    }
}