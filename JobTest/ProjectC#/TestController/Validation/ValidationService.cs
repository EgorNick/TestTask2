using System.Text.RegularExpressions;

namespace TestController.Validation;

public static class ValidationService
{
    private static readonly Regex PhoneRegex = new Regex(@"^\+?\d{1,3}\s?\(?\d{2,3}\)?\s?\d{3}[-\s]?\d{2,4}$");
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static bool IsValidPhone(string? phone) =>
        !string.IsNullOrWhiteSpace(phone) && PhoneRegex.IsMatch(phone) && phone.Count(char.IsDigit) >= 11;

    public static bool IsValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
}