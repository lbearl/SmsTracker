using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmsTracker.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfFalseAttribute : ValidationAttribute, IClientModelValidator
{
    private string PropertyName { get; set; }

    public RequiredIfFalseAttribute(string propertyName)
    {
        PropertyName = propertyName;
        ErrorMessage = "The {0} field is required";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();

        bool.TryParse(type.GetProperty(PropertyName).GetValue(instance)?.ToString(), out var propertyValue);

        if (!propertyValue && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
        MergeAttribute(context.Attributes, "data-val-requirediffalse", errorMessage);
    }

    private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key)) return false;
        
        attributes.Add(key, value);
        return true;
    }
}