using System;
using System.Linq;
using System.Threading.Tasks;
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
        public void CardDeck_TakeCard_Should_Throw_Exception_For_Empty_Deck()
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

        [Fact]
        public async Task CardDeck_TakeCard_Ensure_ThreadSafe()
        {
            const int maxNrOfRuns = 100;
            for (int runs = 0; runs < maxNrOfRuns; runs++)
            {
                // Arrange
                const int maxNrOfTasks = 100;
                var tasks = new Task[maxNrOfTasks];
                var cardDeck = new CardDeck();

                try
                {
                    // Act
                    for (int i = 0; i < tasks.Length; i++)
                    {
                        tasks[i] = Task.Run(() =>
                        {
                            try
                            {
                                cardDeck.TakeCard();
                            }
                            catch (ApplicationException)
                            {
                                // We want to ignore ApplicationException(s) since we are interested
                                // to see which other exceptions may occur, like `ArgumentOutOfRangeException`, etc...
                            }
                        });
                    }
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    // Assert
                    Assert.Null(e);
                }
            }
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(-1, -1)]
        [InlineData(1, -1)]
        [InlineData(1, 53)]
        public void CardDeck_SwapCards_Should_Throw_Exception_For_OutOfRange_Indexes(int firstCardIndex, int secondCardIndex)
        {
            // Arrange
            var cardDeck = new CardDeck();

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => cardDeck.SwapCards(firstCardIndex, secondCardIndex));
        }

        [Fact]
        public async Task CardDeck_SwapCards_Ensure_ThreadSafe()
        {
            // Arrange
            const int expectedNrOfSuits = 4;
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

            var rnd = new Random();
            const int maxNrOfTasks = 100;
            var tasks = new Task[maxNrOfTasks];
            var cardDeck = new CardDeck();
            var maxNrOfCards = cardDeck.Cards.Length;

            const int maxNrOfRuns = 100;
            for (int runs = 0; runs < maxNrOfRuns; runs++)
            {
                // Act
                for (int i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = Task.Run(() =>
                    {
                        var maxShuffleTimes = rnd.Next(26, 100);
                        for (int index = 0; index < maxShuffleTimes; index++)
                        {
                            var firstCardIndex = rnd.Next(maxNrOfCards);
                            var secondCardIndex = rnd.Next(maxNrOfCards);
                            cardDeck.SwapCards(firstCardIndex, secondCardIndex);
                        }
                    });
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);

                // Assert
                foreach (var card in defaultSuitOfCards)
                {
                    var actualNrOfSuits = cardDeck.Cards.Count(c => c.Name == card.Name && c.Value == card.Value);
                    if (actualNrOfSuits != expectedNrOfSuits)
                    {
                        var message = $"Card '{card}' expected {expectedNrOfSuits} times, and found {actualNrOfSuits} times.";
                        Assert.Empty(message);
                    }
                }
            }
        }
    }
}
