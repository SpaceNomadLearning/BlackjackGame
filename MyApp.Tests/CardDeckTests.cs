using System;
using System.Linq;
using Xunit;

namespace MyApp.Tests
{
    public sealed class CardDeckTests
    {
        [Fact]
        public void CardDeck_Should_Have_52_Cards()
        {
            const int defaultNrOfCardsInDeck = 52;
            var cardDeck = new CardDeck();
            Assert.Equal(defaultNrOfCardsInDeck, cardDeck.Cards.Length);
        }

        [Fact]
        public void CardDeck_Should_Contain_4_Suits_With_13_Cards_Each()
        {
            // Arrange
            var defaultSuitOfCards = new[]
            {
                new Card("A", 11),

                new Card("J", 10),
                new Card("K", 10),
                new Card("Q", 10),
                new Card("10", 10),

                new Card("2", 2),
                new Card("3", 3),
                new Card("4", 4),
                new Card("5", 5),
                new Card("6", 6),
                new Card("7", 7),
                new Card("8", 8),
                new Card("9", 9)
            };

            // Act
            var cardDeck = new CardDeck();

            // Assert
            foreach (var card in defaultSuitOfCards)
            {
                var nrOfSuits = cardDeck.Cards.Count(c => c.Name == card.Name && c.Value == card.Value);
                Assert.Equal(4, nrOfSuits);
            }
        }

        [Fact]
        public void CardDeck_Should_Throw_Exception_For_Empty_Deck()
        {
            // Arrange
            const string expectedErrorMessage = "The deck is empty.";
            const int defaultNrOfCardsInDeck = 52;
            var cardDeck = new CardDeck();
            for (int i = 0; i < defaultNrOfCardsInDeck; i++)
                cardDeck.TakeCard();

            // Act
            var exception = Assert.Throws<ApplicationException>(() => cardDeck.TakeCard());

            // Assert
            Assert.Equal(expectedErrorMessage, exception.Message);
        }
    }
}
