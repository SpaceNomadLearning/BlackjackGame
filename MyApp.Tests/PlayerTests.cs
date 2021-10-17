using System;
using System.Collections.Generic;
using Xunit;

namespace MyApp.Tests
{
    public sealed class PlayerTests
    {
        [Theory]
        [InlineData(-44)]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(44)]
        public void Player_Should_Throw_Exception_For_OutOfBounce_RiskAversion(int wrongRiskAversion)
        {
            const string expectedErrorMessage = "Player risk aversion should have a value between 0 and 4.";
            var exception = Assert.Throws<ApplicationException>(() => new Player(name: string.Empty, wrongRiskAversion));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void Player_Should_Throw_Exception_When_Receiving_ToManyCards()
        {
            // Arrange
            const string expectedErrorMessage = "The player 'Smith' does not need another card.";
            var player = new Player("Smith", riskAversion: 4);
            player.ReceiveCard(new Card("K", 10));
            player.ReceiveCard(new Card("7", 7));

            // Act
            var exception = Assert.Throws<ApplicationException>(() => player.ReceiveCard(new Card("1", 1)));

            // Assert
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(GetPlayersWhichShouldStopTakingCardsData))]
        public void Player_Should_Stop_Taking_Cards_At_Risk_Aversion_Trashold(int riskAversion, Card[] hand)
        {
            var player = new Player(name: string.Empty, riskAversion);
            foreach (var card in hand)
                player.ReceiveCard(card);

            Assert.False(player.TakeNextCard);
        }

        public static IEnumerable<object[]> GetPlayersWhichShouldStopTakingCardsData()
        {
            foreach (var (riskAversion, hand) in GetPlayersWhichShouldStopTakingCards())
                yield return new object[] { riskAversion, hand };
        }

        private static IEnumerable<(int riskAversion, Card[] hand)> GetPlayersWhichShouldStopTakingCards()
        {
            yield return (riskAversion: 4, hand: new[] { new Card("K", 10), new Card("7", 7) });
            yield return (riskAversion: 4, hand: new[] { new Card("K", 10), new Card("8", 8) });
            yield return (riskAversion: 4, hand: new[] { new Card("K", 10), new Card("9", 9) });
            yield return (riskAversion: 4, hand: new[] { new Card("K", 10), new Card("K", 10) });
            yield return (riskAversion: 4, hand: new[] { new Card("K", 10), new Card("A", 11) });

            yield return (riskAversion: 3, hand: new[] { new Card("K", 10), new Card("8", 8) });
            yield return (riskAversion: 3, hand: new[] { new Card("K", 10), new Card("9", 9) });
            yield return (riskAversion: 3, hand: new[] { new Card("K", 10), new Card("K", 10) });
            yield return (riskAversion: 3, hand: new[] { new Card("K", 10), new Card("A", 11) });

            yield return (riskAversion: 2, hand: new[] { new Card("K", 10), new Card("9", 9) });
            yield return (riskAversion: 2, hand: new[] { new Card("K", 10), new Card("K", 10) });
            yield return (riskAversion: 2, hand: new[] { new Card("K", 10), new Card("A", 11) });

            yield return (riskAversion: 1, hand: new[] { new Card("K", 10), new Card("K", 10) });
            yield return (riskAversion: 1, hand: new[] { new Card("K", 10), new Card("A", 11) });

            yield return (riskAversion: 0, hand: new[] { new Card("K", 10), new Card("A", 11) });
        }
    }
}
