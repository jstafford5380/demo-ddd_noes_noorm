namespace Domain.Shared
{
    public struct PetId
    {
        public string Id { get; }

        public PetId(string id)
        {
            Id = id;
        }
    }
}