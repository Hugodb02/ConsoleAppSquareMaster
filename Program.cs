using ConsoleAppSquareMaster.Analysis;
using ConsoleAppSquareMaster.DAO;
using ConsoleAppSquareMaster.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "mongodb://localhost:27017";
            var worldDAO = new WorldDAO(connectionString);

            var worldsInDb = worldDAO.GetAllWorlds();
            if (worldsInDb.Count == 0)
            {
                Console.WriteLine("Geen werelden gevonden in de database. Nieuwe werelden worden gegenereerd...");
                GenerateAndSaveWorldsToMongoDB(worldDAO);
                worldsInDb = worldDAO.GetAllWorlds();
            }

            var selectedWorldData = worldsInDb[2]; // Selecteer een wereld
            Console.WriteLine($"Wereld geselecteerd: {selectedWorldData.Name}");

            var world = new World();
            bool[,] generatedWorld = selectedWorldData.AlgorithmType == "Column-based"
                ? world.BuildWorld1(selectedWorldData.MaxY, selectedWorldData.MaxX)
                : world.BuildWorld2(selectedWorldData.MaxY, selectedWorldData.MaxX, selectedWorldData.Coverage);

            var empires = GenerateEmpires(generatedWorld, 4, new Dictionary<int, string>
            {
                { 1, "Conquer1" },
                { 2, "Conquer2" },
                { 3, "Conquer3" },
                { 4, "Conquer2" }
            });

            var worldConquer = new WorldConquer(generatedWorld);

            Console.WriteLine("Wereld veroveren gestart...");
            var conqueredWorld = await worldConquer.ConquerAsync(empires, 25000);
            Console.WriteLine("Wereld veroverd.");

            // Statistieken berekenen
            int totalCells = generatedWorld.GetLength(0) * generatedWorld.GetLength(1);
            var analyzer = new ResultAnalyzer();
            var statistics = analyzer.AnalyzeResults(conqueredWorld);

            // Statistieken opslaan in de database
            var resultsDAO = new ResultsDAO(connectionString);
            resultsDAO.SaveSimulationResult(selectedWorldData, statistics, empires);


            // Toon statistieken in de console
            Console.WriteLine("Statistieken per empire:");
            foreach (var stats in statistics)
            {
                Console.WriteLine(stats);
            }

            // Gebruik BitmapWriter om de wereld te tekenen en op te slaan als afbeelding
            var bitmapWriter = new BitmapWriter();
            bitmapWriter.DrawWorld(conqueredWorld);

            Console.WriteLine("Veroverde wereld opgeslagen als afbeelding.");
        }

        private static void GenerateAndSaveWorldsToMongoDB(WorldDAO worldDAO)
        {
            var world = new World();
            var worlds = new List<WorldData>();

            for (int i = 0; i < 5; i++)
            {
                bool[,] generatedWorld1 = world.BuildWorld1(100, 100);
                worlds.Add(new WorldData
                {
                    Name = $"ColumnWorld{i + 1}",
                    AlgorithmType = "Column-based",
                    MaxX = 100,
                    MaxY = 100,
                    Coverage = World.CalculateCoverage(generatedWorld1)
                });

                double coverageInput = 0.8;
                bool[,] generatedWorld2 = world.BuildWorld2(150, 150, coverageInput);
                worlds.Add(new WorldData
                {
                    Name = $"SeedWorld{i + 1}",
                    AlgorithmType = "Seed-based",
                    MaxX = 150,
                    MaxY = 150,
                    Coverage = coverageInput
                });
            }

            worldDAO.SaveWorlds(worlds);
            Console.WriteLine("Werelden succesvol opgeslagen in de database.");
        }

        private static List<Empire> GenerateEmpires(bool[,] world, int numberOfEmpires, Dictionary<int, string> strategies)
        {
            var empires = new List<Empire>();
            var random = new Random();
            int maxX = world.GetLength(0);
            int maxY = world.GetLength(1);

            for (int i = 1; i <= numberOfEmpires; i++)
            {
                int x, y;
                bool isValid;

                do
                {
                    x = random.Next(maxX);
                    y = random.Next(maxY);

                    isValid = world[x, y] && !empires.Exists(e => e.StartPosition == (x, y));
                } while (!isValid);

                empires.Add(new Empire(i, (x, y), strategies[i]));
                Console.WriteLine($"Geldige startpositie voor Empire {i}: ({x}, {y})");
            }

            return empires;
        }
    }
}
