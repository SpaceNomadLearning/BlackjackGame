namespace MyApp
{
    public sealed record Card(string Name, int Value)
    {
        public override string ToString() => Name;
    }
}
