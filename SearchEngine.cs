using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DecaySchemeTool
{
    internal class SearchEngine
    {
        public static GateFile? FindGateFile(
            List<GateFile> gateFiles,
            double energy,
            double tolerance = 1.0)
        {
            return gateFiles
                .Where(g => Math.Abs(g.GateEnergy - energy) <= tolerance)
                .OrderBy(g => Math.Abs(g.GateEnergy - energy))
                .FirstOrDefault();
        }
        public static List<List<int>> BuildCoincidenceCountMatrix(
            List<GateFile> gateFiles,
            double tolerance)
        {
            var energies = gateFiles
                .OrderBy(g => g.GateEnergy)
                .Select(g => g.GateEnergy)
                .ToList();

            var matrix = new List<List<int>>();

            foreach (double rowEnergy in energies)
            {
                var row = new List<int>();

                foreach (double colEnergy in energies)
                {
                    int count;

                    if (Math.Abs(rowEnergy - colEnergy) <= tolerance)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = gateFiles.Count(g =>
                            g.Peaks.Any(p =>
                                Math.Abs(p.Energy - rowEnergy) <= tolerance)
                            &&
                            g.Peaks.Any(p =>
                                Math.Abs(p.Energy - colEnergy) <= tolerance));
                    }
                    row.Add(count);
                }

                matrix.Add(row);
            }

            return matrix;
        }

        public static List<(GateFile GateFile, Peak Peak)> FindMatchingPeaks(
            List<GateFile> gateFiles,
            double energy,
            double tolerance = 1.0)
        {
            var matches = new List<(GateFile, Peak)>();

            foreach (var gateFile in gateFiles)
            {
                foreach (var peak in gateFile.Peaks)
                {
                    if (Math.Abs(peak.Energy - energy) <= tolerance)
                    {
                        matches.Add((gateFile, peak));
                    }
                }
            }

            return matches
                .OrderBy(m => Math.Abs(m.Item2.Energy - energy))
                .ThenBy(m => m.Item1.GateEnergy)
                .ToList();
        }
        
		public static List<CoincidenceSummaryRow> BuildCoincidenceSummary(
			List<GateFile> gateFiles,
			double tolerance = 1.0)
		{
			var rows = new List<CoincidenceSummaryRow>();

			foreach (var sourceGate in gateFiles.OrderBy(g => g.GateEnergy))
			{
				int coincidenceCount = gateFiles.Count(otherGate =>
					!ReferenceEquals(otherGate, sourceGate) &&
					otherGate.Peaks.Any(peak => Math.Abs(peak.Energy - sourceGate.GateEnergy) <= tolerance));

				rows.Add(new CoincidenceSummaryRow
				{
					Eγ = sourceGate.GateEnergy,
					Coincidences = coincidenceCount
				});
			}

        return rows
            .OrderByDescending(r => r.Coincidences)
            .ThenBy(r => r.Eγ)
            .ToList();
		}

        public static List<(GateFile, Peak)> FindCascadePartners(
            List<GateFile> gateFiles,
            double energy,
            double tolerance)
        {
            var partners =
                new List<(GateFile, Peak)>();

            foreach (var gate in gateFiles)
            {
                bool containsEnergy =
                    gate.Peaks.Any(p =>
                        Math.Abs(p.Energy - energy)
                        <= tolerance);

                if (!containsEnergy)
                    continue;

                foreach (var peak in gate.Peaks)
                {
                    if (Math.Abs(
                        peak.Energy - energy)
                        <= tolerance)
                        continue;

                    partners.Add((gate, peak));
                }
            }

            return partners;
        }

        public static List<CascadeGroup> BuildCascadeGroups(
            List<GateFile> gateFiles,
            double tolerance,
            int minSharedCount)
        {
            var energies = gateFiles
                .OrderBy(g => g.GateEnergy)
                .Select(g => g.GateEnergy)
                .ToList();

            var matrix = BuildCoincidenceCountMatrix(gateFiles, tolerance);

            int n = energies.Count;
            bool[] visited = new bool[n];
            var groups = new List<CascadeGroup>();

            int threshold = Math.Max(1, minSharedCount);
            int nextGroupId = 1;

            for (int i = 0; i < n; i++)
            {
                if (visited[i])
                    continue;

                var queue = new Queue<int>();
                var component = new List<int>();

                visited[i] = true;
                queue.Enqueue(i);

                while (queue.Count > 0)
                {
                    int current = queue.Dequeue();
                    component.Add(current);

                    for (int j = 0; j < n; j++)
                    {
                        if (i == j || visited[j])
                            continue;

                        if (matrix[current][j] >= threshold)
                        {
                            visited[j] = true;
                            queue.Enqueue(j);
                        }
                    }
                }

                if (component.Count <= 1)
                    continue;

                int strength = 0;

                for (int a = 0; a < component.Count; a++)
                {
                    for (int b = a + 1; b < component.Count; b++)
                    {
                        strength += matrix[component[a]][component[b]];
                    }
                }

                groups.Add(new CascadeGroup
                {
                    GroupId = nextGroupId++,
                    Members = component.Select(index => energies[index]).OrderBy(e => e).ToList(),
                    GroupStrength = strength
                });
            }

            return groups
                .OrderByDescending(g => g.GroupStrength)
                .ThenByDescending(g => g.GroupSize)
                .ToList();
        }

        public static List<(GateFile GateFile, Peak Peak)> FindWildcardMatches(
            List<GateFile> gateFiles,
            double energy,
            double tolerance)
        {
            var matches = new List<(GateFile, Peak)>();

            foreach (var gateFile in gateFiles)
            {
                foreach (var peak in gateFile.Peaks)
                {
                    if (Math.Abs(peak.Energy - energy) <= tolerance)
                    {
                        matches.Add((gateFile, peak));
                    }
                }
            }

            return matches
                .OrderBy(m => m.Item1.GateEnergy)
                .ThenBy(m => Math.Abs(m.Item2.Energy - energy))
                .ToList();
        }
    }
}