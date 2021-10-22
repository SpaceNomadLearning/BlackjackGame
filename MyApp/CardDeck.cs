using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MyApp
{
    public sealed class CardDeck
    {
        private readonly List<Card> _cards;

        public CardDeck()
        {
            _cards = new List<Card>();
            for (int i = 0; i < 4; i++)
            {
                _cards.AddRange(new[]
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
                });
            }
        }

        public bool IsDeckEmpty => _cards.Count == 0;
        public ImmutableArray<Card> Cards => _cards.ToImmutableArray();

        public Card TakeCard()
        {
            if (IsDeckEmpty)
                throw new ApplicationException("The deck is empty.");

            const int index = 0;
            var card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }

        public void SwapCards(int firstCardIndex, int secondCardIndex)
        {
            const int minIndex = 0;
            var maxIndex = _cards.Count;

            if (firstCardIndex < minIndex || firstCardIndex > maxIndex ||
                secondCardIndex < minIndex || secondCardIndex > maxIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (firstCardIndex == secondCardIndex)
                return;

            var item = _cards[firstCardIndex];
            _cards[firstCardIndex] = _cards[secondCardIndex];
            _cards[secondCardIndex] = item;
        }
    }
}
