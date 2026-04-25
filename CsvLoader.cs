using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DecaySchemeApp
{
    internal class CsvLoader
    {
        private static readonly Regex GateRegex = new Regex(
            @"p-gate(\d+(\.\d+)?)\.csv$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        public static List<GateFile> LoadFolder(string folderPath)
        {
            var results = new List<GateFile>();

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");
            }

            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);

            foreach (string file in csvFiles)
            {
                double? gateEnergy = ExtractGateEnergy(Path.GetFileName(file));
                if (gateEnergy == null)
                {
                    continue;
                }

                var gateFile = new GateFile
                {
                    FilePath = file,
                    FileName = Path.GetFileName(file),
                    GateEnergy = gateEnergy.Value,
                    Peaks = LoadPeaksFromCsv(file)
                };

                results.Add(gateFile);
            }

            return results.OrderBy(g => g.GateEnergy).ToList();
        }

        private static double? ExtractGateEnergy(string fileName)
        {
            Match match = GateRegex.Match(fileName);
            if (!match.Success)
            {
                return null;
            }

            if (double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            return null;
        }

        private static List<Peak> LoadPeaksFromCsv(string filePath)
        {
            var peaks = new List<Peak>();
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length < 2)
            {
                return peaks;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(',');

                if (parts.Length < 8)
                {
                    continue;
                }

                try
                {
                    var peak = new Peak
                    {
                        RedChi = ParseDouble(parts[0]),
                        XMax = ParseDouble(parts[1]),
                        Energy = ParseDouble(parts[2]),
                        EnergyError = ParseDouble(parts[3]),
                        Area = ParseDouble(parts[4]),
                        AreaError = ParseDouble(parts[5]),
                        Intensity = ParseDouble(parts[6]),
                        IntensityError = ParseDouble(parts[7])
                    };

                    peaks.Add(peak);
                }
                catch
                {
                    Console.WriteLine($"Skipped malformed row in {Path.GetFileName(filePath)}: {line}");
                }
            }

            return peaks;
        }

        private static double ParseDouble(string text)
        {
            return double.Parse(text.Trim(), CultureInfo.InvariantCulture);
        }
    }
}