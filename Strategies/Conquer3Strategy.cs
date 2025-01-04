using System;
using System.Collections.Generic;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Conquer3Strategy : IConquerStrategy
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
                    int index = random.Next(empireCells.Count);
                    PickEmpty(world, worldempires, empireCells, index, empireId, maxx, maxy, random);
                }
            }
        }

        private void PickEmpty(
            bool[,] world,
            int[,] worldempires,
            List<(int, int)> empireCells,
            int index,
            int empireId,
            int maxx,
            int maxy,
            Random random)
        {
            var (x, y) = empireCells[index];
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
                worldempires[newX, newY] = empireId;
                empireCells.Add((newX, newY));
            }
        }
    }
}
