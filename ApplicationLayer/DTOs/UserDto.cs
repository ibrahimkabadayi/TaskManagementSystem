namespace Application.DTOs;

public class UserDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public List<int> projectUserIds { get; set; }
}