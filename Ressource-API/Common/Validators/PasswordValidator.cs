using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ressource_API.Common.Validators;

public class PasswordValidator : ValidationAttribute
{
    private static readonly PasswordConstraint[] Constraints = 
    {
        new(new Regex(@"[0-9]{0,}"), "Il manque des chiffres."),
        new(new Regex(@"[A-Z]{1,}"), "Il manque des lettres majuscules."),
        new(new Regex(@"[a-z]{1,}"), "Il manque des lettres minuscules."),
        new(new Regex(@".{5,20}"), "Le mot de passe doit contenir entre 5 et 20 caractères."),
        new(new Regex(@"[.+*?!:;,^@/$(){}|]{1,}"), "Il manque des symboles.")
    };

    public override bool IsValid(object? value)
    {
        var input = value?.ToString();

        if (string.IsNullOrEmpty(input))
        {
            ErrorMessage = "Le mot de passe doit être renseigné";
            return false;
        }

        var messages = new List<string>();

        foreach (var constraint in Constraints)
        {
            if (!constraint.RegularExpression.IsMatch(input))
            {
                messages.Add(constraint.ReturnedErrorMessage);
            }
        }

        ErrorMessage = string.Join("\n", messages);

        return messages.Count == 0;
    }
}

public class PasswordConstraint(Regex regularExpression, string returnedErrorMessage)
{
    public Regex RegularExpression { get; set; } = regularExpression;
    public string ReturnedErrorMessage { get; set; } = returnedErrorMessage;
}