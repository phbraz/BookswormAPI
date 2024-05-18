namespace BookswormAPI.Models;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime ValidTo { get; set; }
}