namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? PhoneNum { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? PasswordHash { get; set; }
    public string Role { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? GoogleId { get; set; }
    public string? UserImg { get; set; }
}