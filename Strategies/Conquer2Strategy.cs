using System;
using System.Collections.Generic;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Conquer2Strategy : IConquerStrategy
    {
        public void Conquer(bool[,] world, int[,] worldempires, int empireId, (int x, int y) startPosition, int turns)
        {
            int maxx = world.GetLength(0);
            int maxy = world.GetLength(1);
            Random random = new Random();

            var empireCells = new List<(int, int)> { startPosition }; // Start met de startpositie
            worldempires[startPosition.x, startPosition.y] = empireId;

            // Iteratieve verovering
            for (int i = 0; i < turns; i++)
            {
                if (empireCells.Count > 0)
                {
                    int index = FindWithMostEmptyNeighbours(empireCells, worldempires, maxx, maxy);
                    var (x, y) = empireCells[index];
                    int direction = random.Next(4);

                    switch (direction)
                    {
                        case 0 when x < maxx - 1 && worldempires[x + 1, y] == 0:
                            worldempires[x + 1, y] = empireId;
                            empireCells.Add((x + 1, y));
                            break;
                        case 1 when x > 0 && worldempires[x - 1, y] == 0:
                            worldempires[x - 1, y] = empireId;
                            empireCells.Add((x - 1, y));
                            break;
                        case 2 when y < maxy - 1 && worldempires[x, y + 1] == 0:
                            worldempires[x, y + 1] = empireId;
                            empireCells.Add((x, y + 1));
                            break;
                        case 3 when y > 0 && worldempires[x, y - 1] == 0:
                            worldempires[x, y - 1] = empireId;
                            empireCells.Add((x, y - 1));
                            break;
                    }
                }
            }
        }

        private int FindWithMostEmptyNeighbours(List<(int, int)> empireCells, int[,] worldempires, int maxx, int maxy)
        {
            int maxScore = 0;
            List<int> indexes = new();

            for (int i = 0; i < empireCells.Count; i++)
            {
                var (x, y) = empireCells[i];
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
