using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public static class ConquerStrategyFactory
    {
        public static IConquerStrategy GetStrategy(string strategyType)
        {
            return strategyType switch
            {
                "Conquer1" => new Conquer1Strategy(),
                "Conquer2" => new Conquer2Strategy(),
                "Conquer3" => new Conquer3Strategy(),
                _ => throw new ArgumentException($"Invalid strategy type: {strategyType}")
            };
        }
    }

}
