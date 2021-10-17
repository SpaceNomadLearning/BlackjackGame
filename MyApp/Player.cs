using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MyApp
{
    public class Player
    {
        private readonly List<Card> _cards = new();

        public Player(string name, int riskAversion)
        {
            if (riskAversion < 0 || riskAversion > 4)
                throw new IndexOutOfRangeException("Player risk aversion should have a value between 0 and 4.");

            Name = name;
            RiskAversion = riskAversion;
        }

        public string Name { get; }
        public int RiskAversion { get; }
        public virtual ImmutableArray<Card> Cards => _cards.ToImmutableArray();
        public virtual int CardsValue => _cards.Sum(c => c.Value);

        public virtual bool TakeNextCard
        {
            get
            {
                // Each player:
                //     - has a certain risk aversion (0...4); a player will stop drawing cards if he/she reaches
                //       the 21-riskAversion value for his/her hand. Eg: risk aversion of 4 (will always stop at 17)
                var is21RiskAversion = CardsValue >= (21 - RiskAversion);
                return !is21RiskAversion;
            }
        }

        public virtual void ReceiveCard(Card card)
        {
            if (!TakeNextCard)
                throw new ApplicationException($"The player '{Name}' does not need another card.");
            else
                _cards.Add(card);
        }
    }
}
