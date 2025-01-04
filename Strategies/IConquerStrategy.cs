using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public interface IConquerStrategy
    {
        void Conquer(bool[,] world, int[,] worldempires, int empireId, (int x, int y) startPosition, int turns);
    }

}
