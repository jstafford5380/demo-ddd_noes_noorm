namespace Demo.Domain.Shared
{
    public struct SpeciesId
    {
        public int Id { get; }

        public SpeciesId(int id)
        {
            Id = id;
        }

        public override string ToString() => Id.ToString();
    }
}