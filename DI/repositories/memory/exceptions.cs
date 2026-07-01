namespace someCrud.DI.repositories.memory;

class DuplicatedEntry : Exception
{
    public DuplicatedEntry(string type) : base("Duplicated entry on store: " + type) {}
}

class NotFoundEntry : Exception
{
    public NotFoundEntry(string type) : base ("Not found entry on store: " +  type) {}
}