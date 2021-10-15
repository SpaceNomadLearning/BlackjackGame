namespace MyApp
{
    public sealed class Card
    {
        public Card(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public int Value { get; }

        public override string ToString() => Name;
    }
}
