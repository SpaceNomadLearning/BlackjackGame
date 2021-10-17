using System;
using System.Collections.Generic;
using Xunit;

namespace MyApp.Tests
{
    public sealed class BlackJackGameTests
    {
        [Fact]
        public void BlackJackGame_Should_Ensure_At_Least_1_Player()
        {
            // Arrange
            var players = Array.Empty<Player>();
            const string expectedErrorMessage = "A game of blackjack should have at least one player.";

            // Act
            var exception = Assert.Throws<ApplicationException>(() => new BlackJackGame(players));

            // Asset
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void BlackJackGame_Should_Ensure_At_Most_4_Players()
        {
            // Arrange
            var players = new[]
            {
                new Player("Player-1", riskAversion: 0),
                new Player("Player-2", riskAversion: 0),
                new Player("Player-3", riskAversion: 0),
                new Player("Player-4", riskAversion: 0),
                new Player("Player-5", riskAversion: 0)
            };
            const string expectedErrorMessage = "A game of blackjack should have at most four players.";

            // Act
            var exception = Assert.Throws<ApplicationException>(() => new BlackJackGame(players));

            // Asset
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void BlackJackGame_Should_Ensure_That_Each_Player_Gets_2_Cards()
        {
            // Arrange
            const int defaultNrOfCardsThatEachPlayerShouldReceive = 2;
            var players = new[]
            {
                new Player("Player-1", riskAversion: 4),
                new Player("Player-2", riskAversion: 4),
                new Player("Player-3", riskAversion: 4),
                new Player("Player-4", riskAversion: 4)
            };

            // Act
            var game = new BlackJackGame(players);
            game.Start();

            // Assert
            foreach (var player in players)
            {
                Assert.True(player.Cards.Length >= defaultNrOfCardsThatEachPlayerShouldReceive);
            }
        }

        [Theory]
        [MemberData(nameof(GetPlayersSetsData))]
        public void BlackJackGame_NoInfiniteLoop(Player[] players)
        {
            // Arrange
            const int defaultNrOfCardsThatEachPlayerShouldReceive = 2;
            var game = new BlackJackGame(players);

            // Act
            game.Start();

            // Assert
            foreach (var player in players)
            {
                Assert.True(player.Cards.Length >= defaultNrOfCardsThatEachPlayerShouldReceive);
            }
        }

        public static IEnumerable<object[]> GetPlayersSetsData()
        {
            foreach (var players in GetPlayersSets())
                yield return new object[] { players };
        }

        private static IEnumerable<Player[]> GetPlayersSets()
        {
            var random = new Random();
            // TODO: Tweak the number of player sets that we should test.
            const int maxNrOfPlayersSets = 50;
            for (int i = 0; i <= maxNrOfPlayersSets; i++)
            {
                yield return new[] {
                    new Player("Player-1", riskAversion: random.Next(4)),
                    new Player("Player-2", riskAversion: random.Next(4)),
                    new Player("Player-3", riskAversion: random.Next(4)),
                    new Player("Player-4", riskAversion: random.Next(4)),
                };
            }
        }
    }
}
