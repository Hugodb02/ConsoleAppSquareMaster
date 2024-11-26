using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
        public class Conquer3Strategy : IConquerStrategy
        {
            public int[,] Conquer(bool[,] world, int[,] worldempires, int empireId, int turns)
            {
                int maxx = world.GetLength(0);
                int maxy = world.GetLength(1);
                Random random = new Random();

                Dictionary<int, List<(int, int)>> empires = new(); // Empire ID -> List of owned cells
                int x, y;

                // Initialisatie: zoek startposities
                for (int i = 0; i < empireId; i++)
                {
                    bool ok = false;
                    while (!ok)
                    {
                        x = random.Next(maxx);
                        y = random.Next(maxy);
                        if (world[x, y] && worldempires[x, y] == 0)
                        {
                            ok = true;
                            worldempires[x, y] = i + 1;
                            empires.Add(i + 1, new List<(int, int)> { (x, y) });
                        }
                    }
                }

                // Iteratieve verovering
                int index;
                for (int i = 0; i < turns; i++)
                {
                    for (int e = 1; e <= empireId; e++)
                    {
                        if (empires[e].Count > 0)
                        {
                            index = random.Next(empires[e].Count);
                            PickEmpty(world, worldempires, empires[e], index, e, maxx, maxy, random);
                        }
                    }
                }

                return worldempires;
            }

            private void PickEmpty(
                bool[,] world,
                int[,] worldempires,
                List<(int, int)> empire,
                int index,
                int e,
                int maxx,
                int maxy,
                Random random)
            {
                var (x, y) = empire[index];
                var neighbors = new List<(int, int)>();

                // Zoek aangrenzende vrije locaties
                if (x > 0 && world[x - 1, y] && worldempires[x - 1, y] == 0) neighbors.Add((x - 1, y));
                if (x < maxx - 1 && world[x + 1, y] && worldempires[x + 1, y] == 0) neighbors.Add((x + 1, y));
                if (y > 0 && world[x, y - 1] && worldempires[x, y - 1] == 0) neighbors.Add((x, y - 1));
                if (y < maxy - 1 && world[x, y + 1] && worldempires[x, y + 1] == 0) neighbors.Add((x, y + 1));

                // Verover een willekeurige vrije locatie
                if (neighbors.Count > 0)
                {
                    var (newX, newY) = neighbors[random.Next(neighbors.Count)];
                    worldempires[newX, newY] = e;
                    empire.Add((newX, newY));
                }
            }
        }
    }
