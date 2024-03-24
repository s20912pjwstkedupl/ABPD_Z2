using System;

namespace LegacyApp;

public class UserValidationService
{
    public bool AssertFirstName(string firstName)
    {
        return !string.IsNullOrEmpty(firstName);
    }

    public bool AssertLastName(string lastName)
    {
        return !string.IsNullOrEmpty(lastName);
    }

    public bool AssertEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public bool AssertDateOfBirth(DateTime dateOfBirth)
    {
        return GetAgeFromDateOfBirth(dateOfBirth) >= 21;
    }

    public bool AssertUserCreditLimit(bool hasCreditLimit, int creditValue)
    {
        return !hasCreditLimit || creditValue >= 500;
    }
    
    private int GetAgeFromDateOfBirth(DateTime dateOfBirth)
    {
        var now = DateTime.Now;
        int age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
        {
            age--;
        }

        return age;
    }
}