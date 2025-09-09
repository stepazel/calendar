namespace Calendar.Users;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string GetNameWithAbbreviatedLastName()
    {
        var parts = Name.Split(' ');
        var firstName = parts[0];
        var lastName = parts[1];
        return $"{firstName} {lastName[0]}.";
    }
}