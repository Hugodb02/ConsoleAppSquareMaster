using ConsoleAppSquareMaster.Strategies;
using System;
using System.Collections.Generic;

namespace ConsoleAppSquareMaster.Model
{
    public class WorldConquer
    {
        /* world indicates whether the grid cell on coordinate x,y is part of the world or not*/
        private bool[,] world;
        /* the values in worldempires are -1 if not part of the world, 0 if part of the world but not conquered by any empire, any other positive value indicates the empire (id) the grid cell belongs to
         */
        private int[,] worldempires;
        private int maxx, maxy;
        private Random random = new Random(1);

        public WorldConquer(bool[,] world)
        {
            this.world = world;
            maxx = world.GetLength(0);
            maxy = world.GetLength(1);
            worldempires = new int[maxx, maxy];
            for (int i = 0; i < world.GetLength(0); i++)
                for (int j = 0; j < world.GetLength(1); j++)
                    if (world[i, j])
                        worldempires[i, j] = 0;
                    else
                        worldempires[i, j] = -1;
        }

        public int[,] Conquer1(int nEmpires, int turns)
        {
            IConquerStrategy strategy = new Conquer1Strategy();
            return strategy.Conquer(world, worldempires, nEmpires, turns);
        }

        public int[,] Conquer2(int nEmpires, int turns)
        {
            IConquerStrategy strategy = new Conquer2Strategy();
            return strategy.Conquer(world, worldempires, nEmpires, turns);
        }

        public int[,] Conquer3(int nEmpires, int turns)
        {
            IConquerStrategy strategy = new Conquer3Strategy();
            return strategy.Conquer(world, worldempires, nEmpires, turns);
        }

    }
}
