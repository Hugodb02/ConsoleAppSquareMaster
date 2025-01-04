using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Model
{
    public class Empire
    {
        public int Id { get; set; }
        public (int x, int y) StartPosition { get; set; }
        public string Strategy { get; set; }

        public Empire(int id, (int x, int y) startPosition, string strategy)
        {
            Id = id;
            StartPosition = startPosition;
            Strategy = strategy;
        }
    }
}
