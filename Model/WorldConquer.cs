using ConsoleAppSquareMaster.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Model
{
    public class WorldConquer
    {
        private readonly bool[,] world;
        private readonly int[,] worldempires;

        public WorldConquer(bool[,] world)
        {
            this.world = world;
            int maxX = world.GetLength(0);
            int maxY = world.GetLength(1);
            worldempires = new int[maxX, maxY];

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    worldempires[x, y] = world[x, y] ? 0 : -1;
                }
            }
        }

        public async Task<int[,]> ConquerAsync(List<Empire> empires, int turns)
        {
            var tasks = new List<Task>();

            foreach (var empire in empires)
            {
                tasks.Add(Task.Run(() =>
                {
                    var strategy = ConquerStrategyFactory.GetStrategy(empire.Strategy);
                    strategy.Conquer(world, worldempires, empire.Id, empire.StartPosition, turns);
                }));
            }

            await Task.WhenAll(tasks);

            return worldempires;
        }
    }
}
