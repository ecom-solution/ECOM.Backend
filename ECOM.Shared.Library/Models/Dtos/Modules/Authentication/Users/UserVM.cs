namespace ECOM.Shared.Library.Models.Dtos.Modules.Authentication.Users
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string SecurityStamp { get; set; } = string.Empty;
        public string ConcurrencyStamp { get; set; } = string.Empty;
        public string TimeZoneId { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
