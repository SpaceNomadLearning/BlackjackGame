using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyApp
{
    public sealed class BlackJackGame
    {
        private readonly Dealer _dealer;
        public BlackJackGame(Player[] players)
        {
            if (players == null || players.Length == 0)
                throw new ApplicationException("A game of blackjack should have at least one player.");

            if (players.Length > 4)
                throw new ApplicationException("A game of blackjack should have at most four players.");

            Players = players;
            _dealer = new Dealer("Dealer", new CardDeck());
        }

        public Player[] Players { get; }

        public void Start()
        {
            var activeGamePlayers = new List<Player> { _dealer };
            activeGamePlayers.AddRange(Players);

            // Playing the game:
            //
            // - Initial phase
            //     - The deck gets shuffled
            _dealer.Shuffle();

            //     - Each player gets two cards
            foreach (var player in activeGamePlayers)
            {
                _dealer.DealCard(player);
                _dealer.DealCard(player);
            }

            if (InstantWinConditionsAreMet(activeGamePlayers))
                return;

            // - After the initial phase, the players take turns by receiving a card
            var playersWhoShouldReceiveCard = activeGamePlayers.ToArray();
            while (_dealer.AnyCardsLeft)
            {
                // The game ends:
                //     - When a player instant-wins
                //     - When all the players stop or lose by going over 21
                playersWhoShouldReceiveCard = playersWhoShouldReceiveCard.Where(p => p.CardsValue < 21 && p.TakeNextCard)
                                                                         .ToArray();
                if (playersWhoShouldReceiveCard.Length == 0)
                    return;

                foreach (var player in playersWhoShouldReceiveCard)
                {
                    if (player.TakeNextCard)
                        _dealer.DealCard(player);
                }

                if (InstantWinConditionsAreMet(playersWhoShouldReceiveCard))
                {
                    return;
                }
            }
        }

        public void WriteResultTo(TextWriter writer)
        {
            var allPlayers = new List<Player> { _dealer };
            allPlayers.AddRange(Players);

            writer.Write("Player".PadRight(10));
            writer.Write("| Aversion".PadRight(11));
            writer.Write("| Points".PadRight(10));
            writer.Write("| Hand");
            writer.WriteLine();
            foreach (var player in allPlayers)
            {
                writer.Write(player.Name.PadRight(10));
                writer.Write($"| {player.RiskAversion}".PadRight(11));
                writer.Write($"| {player.CardsValue}".PadRight(10));

                writer.Write("| ");
                foreach (var card in player.Cards)
                    writer.Write($"{card}".PadRight(4));

                writer.WriteLine();
            }

            // Instant-Win conditions:
            //     - "blackjack" hand - A player's first two cards are an ACE and a car with a 10 value.
            //     - A player reaches exactely 21
            var instantWiners = allPlayers.Where(p => p.CardsValue == 21);
            if (instantWiners.Any())
            {
                var winners = string.Join(", ", instantWiners.Select(p => p.Name));
                writer.WriteLine($"Instant-Win: {winners}");
            }
            else
            {
                // End-game winner (non-instant-win)
                //     - The players/dealer that haven’t lost and are the closest to 21
                var endGameWinner = Players.Where(p => p.CardsValue < 21)
                                        .OrderByDescending(p => p.CardsValue)
                                        .FirstOrDefault();

                // The dealer:
                //     - Doesn’t lose if he/she goes over 21: in the end-game winner scenario, a
                //       dealer with a hand of 22 will win against a player with a hand of 18.
                if (_dealer.CardsValue > (endGameWinner?.CardsValue ?? 0))
                    endGameWinner = _dealer;

                writer.WriteLine($"End-game-Win: {endGameWinner.Name}");
            }
        }

        private static bool InstantWinConditionsAreMet(IEnumerable<Player> players)
        {
            // Instant-Win conditions:
            //     - "blackjack" hand - A player's first two cards are an ACE and a car with a 10 value.
            //     - A player reaches exactely 21
            return players.Any(p => p.CardsValue == 21);
        }

        internal sealed class Dealer : Player
        {
            private readonly CardDeck _cardDeck;

            public Dealer(string name, CardDeck cardDeck)
                : base(name, riskAversion: 4)
            {
                // The dealer:
                //     - Acts exactly like any other player with the following exceptions
                //          - Has a fixed risk aversion of 4 (will always stop at 17)

                _cardDeck = cardDeck ?? throw new ArgumentNullException(nameof(cardDeck));
            }

            public bool AnyCardsLeft => !_cardDeck.IsDeckEmpty;

            public void DealCard(Player player)
            {
                var card = _cardDeck.TakeCard();
                player.ReceiveCard(card);
            }

            public void Shuffle()
            {
                var rnd = new Random();
                var maxShuffleTimes = rnd.Next(26, 100);
                var maxNrOfCards = _cardDeck.Cards.Length;

                for (int i = 0; i < maxShuffleTimes; i++)
                {
                    var firstCardIndex = rnd.Next(maxNrOfCards);
                    var secondCardIndex = rnd.Next(maxNrOfCards);
                    _cardDeck.SwapCards(firstCardIndex, secondCardIndex);
                }
            }
        }
    }
}
