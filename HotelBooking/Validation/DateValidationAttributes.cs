using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Validation;

/// <summary>
/// Validates that a date is not in the past (must be today or future)
/// </summary>
public class FutureDateAttribute : ValidationAttribute
{
    public FutureDateAttribute() : base("The date cannot be in the past.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is DateTime dateValue)
        {
            if (dateValue.Date < DateTime.Today)
            {
                return new ValidationResult(ErrorMessage ?? "The date cannot be in the past.");
            }
        }

        return ValidationResult.Success;
    }
}

/// <summary>
/// Validates that a date is greater than another date property
/// </summary>
public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty)
        : base("End date must be after start date.")
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var currentValue = (DateTime)value;

        var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (comparisonProperty == null)
        {
            return new ValidationResult($"Property {_comparisonProperty} not found.");
        }

        var comparisonValue = (DateTime?)comparisonProperty.GetValue(validationContext.ObjectInstance);
        if (comparisonValue == null)
        {
            return ValidationResult.Success;
        }

        if (currentValue <= comparisonValue)
        {
            return new ValidationResult(ErrorMessage ?? $"Must be after {_comparisonProperty}.");
        }

        return ValidationResult.Success;
    }
}
