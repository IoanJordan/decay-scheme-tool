using System.Collections.Generic;

namespace DecaySchemeApp
{
    public class GateFile
    {
        public string FilePath { get; set; } = "";
        public string FileName { get; set; } = "";
        public double GateEnergy { get; set; }
        public List<Peak> Peaks { get; set; } = new List<Peak>();
    }
}