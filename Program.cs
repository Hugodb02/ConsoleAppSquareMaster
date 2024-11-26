using ConsoleAppSquareMaster.Model;
using ConsoleAppSquareMaster.Strategies;
using System;

namespace ConsoleAppSquareMaster
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //GenerateAndSaveWorldsToMongoDB();



            //var w = world.BuildWorld2(100, 100, 0.60);
            //var w = world.BuildWorld1(100, 100);
            //DisplayBuildedWorld(w);
            //WorldConquer wq = new WorldConquer(w);
            //var ww = wq.Conquer3(5, 25000);
            //for (int i = 0; i < ww.GetLength(1); i++)
            //{
            //    for (int j = 0; j < ww.GetLength(0); j++)
            //    {
            //        string ch;
            //        switch (ww[j, i])
            //        {
            //            case -1: ch = " "; break;
            //            case 0: ch = "."; break;
            //            default: ch = ww[j, i].ToString(); break;
            //        }
            //        Console.Write(ch);
            //    }
            //    Console.WriteLine();
            //}
            //BitmapWriter bmw = new BitmapWriter();
            //bmw.DrawWorld(ww);

            // Genereer een wereld en voer strategieën uit
            var world = new World();
            bool[,] generatedWorld = world.BuildWorld2(100, 100, 0.6);

            // Voer strategieën uit en verkrijg het eindresultaat
            var conquerResult = ExecuteStrategies(generatedWorld);

            // Visualiseer de veroverde wereld
            var bmw = new BitmapWriter();
            bmw.DrawWorld(conquerResult);
        }

        private static int[,] ExecuteStrategies(bool[,] world)
        {
            // Instantieer WorldConquer met de gegenereerde wereld
            var worldConquer = new WorldConquer(world);

            // Pas strategieën toe
            Console.WriteLine("Toepassen van Conquer1Strategy...");
            var conquer1Result = worldConquer.Conquer1(5, 25000);
            DisplayConqueredWorld(conquer1Result);

            Console.WriteLine("Toepassen van Conquer2Strategy...");
            var conquer2Result = worldConquer.Conquer2(5, 25000);
            DisplayConqueredWorld(conquer2Result);

            Console.WriteLine("Toepassen van Conquer3Strategy...");
            var conquer3Result = worldConquer.Conquer3(5, 25000);
            DisplayConqueredWorld(conquer3Result);

            // Retourneer het eindresultaat (laatste strategie)
            return conquer3Result;
        }

        private static void GenerateAndSaveWorldsToMongoDB()
        {
            var connectionString = "mongodb://localhost:27017";
            var worldDAO = new WorldDAO(connectionString);
            var worlds = new List<WorldData>();

            var world = new World();

            for (int i = 0; i < 5; i++) //Genereer 10 werelden, 5 column-based en 5 seed-based
            {
                // Column-based wereld
                bool[,] generatedWorld1 = world.BuildWorld1(100, 100);
                worlds.Add(new WorldData
                {
                    Name = $"ColumnWorld{i + 1}",
                    AlgorithmType = "Column-based",
                    MaxX = 100,
                    MaxY = 100,
                    Coverage = World.CalculateCoverage(generatedWorld1)
                });

                // Seed-based wereld
                double coverageInput = 0.8; // 80% dekkingsgraad
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

        private static void DisplayBuildedWorld(bool[,] w)
        {
            for (int i = 0; i < w.GetLength(1); i++)
            {
                for (int j = 0; j < w.GetLength(0); j++)
                {
                    char ch;
                    if (w[j, i]) ch = '*'; else ch = ' ';
                    Console.Write(ch);
                }
                Console.WriteLine();
            }
        }

        private static void DisplayConqueredWorld(int[,] worldEmpires)
        {
            for (int i = 0; i < worldEmpires.GetLength(1); i++)
            {
                for (int j = 0; j < worldEmpires.GetLength(0); j++)
                {
                    string ch = worldEmpires[j, i] switch
                    {
                        -1 => " ",  // Niet deel van de wereld
                        0 => ".",   // Onveroverd
                        _ => worldEmpires[j, i].ToString() // Empire ID
                    };
                    Console.Write(ch);
                }
                Console.WriteLine();
            }
        }
    }
}
