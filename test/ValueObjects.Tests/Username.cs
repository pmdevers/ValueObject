using ValueObjects;

namespace ValueObject.Tests;

public partial record struct Username : IValueObject<Username, string>
{
    public static bool IsValid(string value)
    {
        string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.";
        if (string.IsNullOrEmpty(value) || value.Length < 3 || value.Length > 20)
        {
            return false;
        }
        return value.All(c => allowedChars.Contains(c));
    }
}
