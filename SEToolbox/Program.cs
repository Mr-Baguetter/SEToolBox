using SEToolBox.Tabs;

namespace SEToolBox
{
    public partial class MainForm : Form
    {
        private TabControl mainTabControl;
        private GPSTriangulatorTab gpsTab;
        internal static Version version = new(0, 0, 3);

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Space Engineers Toolbox";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(700, 500);

            mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);

            TabPage gpsTabPage = new TabPage("GPS Triangulator");
            gpsTab = new GPSTriangulatorTab();
            gpsTab.Dock = DockStyle.Fill;
            gpsTabPage.Controls.Add(gpsTab);
            mainTabControl.TabPages.Add(gpsTabPage);

            TabPage calculatorTabPage = new TabPage("Calculators");
            CalculatorTab calculatorTab = new CalculatorTab();
            calculatorTab.Dock = DockStyle.Fill;
            calculatorTabPage.Controls.Add(calculatorTab);
            mainTabControl.TabPages.Add(calculatorTabPage);

            TabPage changeLogTabPage = new TabPage("Change Log");
            ChangeLogTab changelogTab = new ChangeLogTab();
            changelogTab.Dock = DockStyle.Fill;
            changeLogTabPage.Controls.Add(changelogTab);
            mainTabControl.TabPages.Add(changeLogTabPage);

            this.Controls.Add(mainTabControl);
            this.ResumeLayout(false);
            this.Size = new Size(1100, 1000);
            this.MinimumSize = new Size(800, 750);
            this.Icon = new Icon("SEToolBox.ico");
        }
    }

    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}