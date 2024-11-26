using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Conquer2Strategy : IConquerStrategy
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
            int direction;
            for (int i = 0; i < turns; i++)
            {
                for (int e = 1; e <= empireId; e++)
                {
                    if (empires[e].Count > 0)
                    {
                        index = FindWithMostEmptyNeighbours(e, empires[e], worldempires, maxx, maxy);
                        direction = random.Next(4);
                        x = empires[e][index].Item1;
                        y = empires[e][index].Item2;

                        switch (direction)
                        {
                            case 0:
                                if (x < maxx - 1 && worldempires[x + 1, y] == 0)
                                {
                                    worldempires[x + 1, y] = e;
                                    empires[e].Add((x + 1, y));
                                }
                                break;
                            case 1:
                                if (x > 0 && worldempires[x - 1, y] == 0)
                                {
                                    worldempires[x - 1, y] = e;
                                    empires[e].Add((x - 1, y));
                                }
                                break;
                            case 2:
                                if (y < maxy - 1 && worldempires[x, y + 1] == 0)
                                {
                                    worldempires[x, y + 1] = e;
                                    empires[e].Add((x, y + 1));
                                }
                                break;
                            case 3:
                                if (y > 0 && worldempires[x, y - 1] == 0)
                                {
                                    worldempires[x, y - 1] = e;
                                    empires[e].Add((x, y - 1));
                                }
                                break;
                        }
                    }
                }
            }

            return worldempires;
        }

        private int FindWithMostEmptyNeighbours(int e, List<(int, int)> empire, int[,] worldempires, int maxx, int maxy)
        {
            List<int> indexes = new();
            int maxScore = 0;

            for (int i = 0; i < empire.Count; i++)
            {
                int x = empire[i].Item1;
                int y = empire[i].Item2;
                int score = CountEmptyNeighbours(x, y, worldempires, maxx, maxy);

                if (score > maxScore)
                {
                    indexes.Clear();
                    maxScore = score;
                    indexes.Add(i);
                }
                else if (score == maxScore)
                {
                    indexes.Add(i);
                }
            }

            Random random = new Random();
            return indexes[random.Next(indexes.Count)];
        }

        private int CountEmptyNeighbours(int x, int y, int[,] worldempires, int maxx, int maxy)
        {
            int count = 0;
            if (x > 0 && worldempires[x - 1, y] == 0) count++;
            if (x < maxx - 1 && worldempires[x + 1, y] == 0) count++;
            if (y > 0 && worldempires[x, y - 1] == 0) count++;
            if (y < maxy - 1 && worldempires[x, y + 1] == 0) count++;
            return count;
        }
    }
}
