using System.Collections.Generic;

namespace DecaySchemeTool
{
    public class GateFile
    {
        public string FilePath { get; set; } = "";
        public string FileName { get; set; } = "";
        public double GateEnergy { get; set; }
        public List<Peak> Peaks { get; set; } = new List<Peak>();
    }
}