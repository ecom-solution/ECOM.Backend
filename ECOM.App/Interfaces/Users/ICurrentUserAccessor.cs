namespace ECOM.App.Interfaces.Users
{
    public interface ICurrentUserAccessor
    {
        Guid Id { get; }
        string UserName { get; }
        string Email { get; }
    }
}
