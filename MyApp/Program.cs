using System;

namespace MyApp
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                var game = PlayNewGame();
                game.WriteResultTo(Console.Out);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine($"Program crashed with next message '{e.Message}'.");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private static BlackJackGame PlayNewGame()
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
            return game;
        }
    }
}
