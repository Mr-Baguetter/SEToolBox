using SEToolBox.API.Enums;

namespace SEToolBox.API.Data
{
    public class ThrusterData
    {
        public string Name { get; set; }
        public double MaxThrust { get; set; }
        public double PowerConsumption { get; set; }
        public double EffectivenessOnPlanet { get; set; }
        public double EffectivenessInSpace { get; set; }
        public bool IsLarge { get; set; }
        public ThrusterType Type { get; set; }
    }
}
