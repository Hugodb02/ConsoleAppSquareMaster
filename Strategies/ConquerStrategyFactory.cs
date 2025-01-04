using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public static class ConquerStrategyFactory
    {
        public static IConquerStrategy GetStrategy(string strategyType)
        {
            //Conquer1: Breidt een empire uit door een lineaire of vaste patroonstructuur zonder rekening te houden met reeds bezette gebieden.
            //Conquer2: Gebruikt een random - walk - algoritme, waarbij de uitbreiding in willekeurige richtingen plaatsvindt, maar stopt mogelijk niet bij reeds ingenomen gebieden.
            //Conquer3: Past een flood - fill - algoritme toe, waarbij aangrenzende vakjes systematisch worden ingenomen totdat een grens wordt bereikt, zonder prioriteit voor reeds veroverde gebieden.
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
