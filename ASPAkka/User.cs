namespace ASPAkka
{
    public readonly record struct CreateUser(string Name);
    
    public readonly record struct GetUserById(Guid Id);

    public readonly record struct GetUsers();

    public record User(Guid Id, string Name);
}
