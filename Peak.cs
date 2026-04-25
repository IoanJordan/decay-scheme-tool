using System;

namespace DecaySchemeTool
{
    public class Peak
    {
        public double RedChi { get; set; }
        public double XMax { get; set; }
        public double Energy { get; set; }
        public double EnergyError { get; set; }
        public double Area { get; set; }
        public double AreaError { get; set; }
        public double Intensity { get; set; }
        public double IntensityError { get; set; }

        public override string ToString()
        {
            return $"E={Energy:F3} keV ± {EnergyError:F3}, Int={Intensity:F3} ± {IntensityError:F3}, RedChi={RedChi:F3}";
        }
    }
}