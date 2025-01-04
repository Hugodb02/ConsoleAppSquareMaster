using System;
using System.Collections.Generic;

namespace ConsoleAppSquareMaster.Analysis
{
    public class ResultAnalyzer
    {
        public List<EmpireStatistics> AnalyzeResults(int[,] conqueredWorld)
        {
            var empireStats = new Dictionary<int, EmpireStatistics>();
            int totalConquerableCells = 0; // Tel alleen de veroverbare cellen

            for (int x = 0; x < conqueredWorld.GetLength(0); x++)
            {
                for (int y = 0; y < conqueredWorld.GetLength(1); y++)
                {
                    int empireId = conqueredWorld[x, y];

                    // Obstakels (-1) worden niet meegeteld
                    if (empireId >= 0)
                    {
                        totalConquerableCells++; // Tel alleen cellen die veroverbaar zijn
                    }

                    if (empireId > 0) // Alleen empires tellen (geen leeg gebied of obstakels)
                    {
                        if (!empireStats.ContainsKey(empireId))
                        {
                            empireStats[empireId] = new EmpireStatistics { EmpireId = empireId };
                        }

                        empireStats[empireId].CellsOccupied++;
                    }
                }
            }

            // Bereken het percentage op basis van het totaal aantal veroverbare cellen
            foreach (var stats in empireStats.Values)
            {
                stats.PercentageOfWorld = (double)stats.CellsOccupied / totalConquerableCells * 100;
            }

            return new List<EmpireStatistics>(empireStats.Values);
        }
    }

    public class EmpireStatistics
    {
        public int EmpireId { get; set; }
        public int CellsOccupied { get; set; }
        public double PercentageOfWorld { get; set; }

        public override string ToString()
        {
            return $"Empire {EmpireId}: {CellsOccupied} vakjes ({PercentageOfWorld:F2}%)";
        }
    }
}
