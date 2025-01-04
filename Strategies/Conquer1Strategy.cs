using System;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Conquer1Strategy : IConquerStrategy
    {
        public void Conquer(bool[,] world, int[,] worldempires, int empireId, (int x, int y) startPosition, int turns)
        {
            var (startX, startY) = startPosition;

            for (int t = 0; t < turns; t++)
            {
                // Start de uitbreiding vanaf de startpositie
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    for (int y = 0; y < world.GetLength(1); y++)
                    {
                        if (worldempires[x, y] == empireId)
                        {
                            TryExpand(world, worldempires, empireId, x, y);
                        }
                    }
                }

                // Zorg ervoor dat de startpositie blijft bezet door het empire
                worldempires[startX, startY] = empireId;
            }
        }

        private void TryExpand(bool[,] world, int[,] worldempires, int empireId, int x, int y)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int newX = x + dx[i];
                int newY = y + dy[i];

                if (newX >= 0 && newX < world.GetLength(0) &&
                    newY >= 0 && newY < world.GetLength(1) &&
                    world[newX, newY] &&
                    worldempires[newX, newY] == 0)
                {
                    worldempires[newX, newY] = empireId;
                }
            }
        }
    }
}
