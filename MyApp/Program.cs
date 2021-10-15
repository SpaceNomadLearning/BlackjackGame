using System;

namespace MyApp
{
    internal static class Program
    {
        private static void Main()
        {
            var random = new Random();
            var players = new[]
            {
                new Player("Player-1", riskAversion: random.Next(4)),
                new Player("Player-2", riskAversion: random.Next(4)),
                new Player("Player-3", riskAversion: random.Next(4)),
                new Player("Player-4", riskAversion: random.Next(4))
            };
            var game = new BlackJackGame(players);
            game.Start();
            game.WriteResultTo(Console.Out);
        }
    }
}
