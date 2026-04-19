using System.ComponentModel.DataAnnotations;

namespace Scheduler.API.DTOs.Validations;

public class StartsWithAttribute(string[] prefixes) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var str = value as string;

        if (string.IsNullOrWhiteSpace(str)) return ValidationResult.Success;

        bool valid = prefixes.Any(prefix => !string.IsNullOrWhiteSpace(prefix)
            && str.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
        );

        if (valid) return ValidationResult.Success;

        var allowed = string.Join(", ", prefixes);

        return new ValidationResult($"Must start with one of: ({allowed})");
    }
}