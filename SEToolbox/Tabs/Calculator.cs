using SEToolBox.Tabs.Calculator_Tabs;

namespace SEToolBox.Tabs
{
    public class CalculatorTab : UserControl
    {
        private TabControl calculatorTabControl;
        private ThrustCalculatorTab thrustCalcTab;

        public CalculatorTab()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            calculatorTabControl = new TabControl();
            calculatorTabControl.Dock = DockStyle.Fill;
            calculatorTabControl.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);

            TabPage thrustTabPage = new TabPage("Thrust Calculator");
            thrustCalcTab = new ThrustCalculatorTab();
            thrustCalcTab.Dock = DockStyle.Fill;
            thrustTabPage.Controls.Add(thrustCalcTab);
            calculatorTabControl.TabPages.Add(thrustTabPage);

            this.Controls.Add(calculatorTabControl);
            this.ResumeLayout(false);
        }
    }
}