using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BLL.CustomAtributes;
public class FullTeachersNameAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var fullName = value as string;
        if (string.IsNullOrWhiteSpace(fullName))
        {
            ErrorMessage = "Name cannot be empty.";
            return false;
        }

        // Regex for testing format: "Name1 Name2 Name3"
        // \b - end of word
        // \s - whitespace
        // [A-ZА-Я] - first Uppercase letter (Ukrainian also included)
        // [a-zа-я]+ - one or more lowecase letter (Ukrainian also included)
        var regex = new Regex(@"^\b[A-ZА-Я][a-zа-я]+\s[A-ZА-Я][a-zа-я]+\s[A-ZА-Я][a-zа-я]+\b$");

        if (!regex.IsMatch(fullName))
        {
            ErrorMessage = "Name must be in format: 'Firstname Middlename Lastname', each starting with a capital letter.";
            return false;
        }

        return true;
    }
}
