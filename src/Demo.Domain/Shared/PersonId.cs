namespace Demo.Domain.Shared
{
    public struct PersonId
    {
        public string Id { get; }

        public PersonId(string id)
        {
            Id = id;
        }
    }
}