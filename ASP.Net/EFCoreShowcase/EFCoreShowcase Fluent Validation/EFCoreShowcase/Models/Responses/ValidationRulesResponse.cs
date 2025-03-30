namespace EFCoreShowcase.Models.Responses;

public class ValidationRulesResponse
{
    public ValidationProperties Properties { get; set; } = new();
}

public class ValidationProperties
{
    public ValidationProperty Address { get; set; } = new();
    public ValidationProperty Subject { get; set; } = new();
    public ValidationProperty Message { get; set; } = new();
}

public class ValidationProperty
{
    public ValidationRuleDetail[] Validators { get; set; } = Array.Empty<ValidationRuleDetail>();
}

public class ValidationRuleDetail
{
    public string Name { get; set; } = string.Empty;
    public string? Expression { get; set; }
    public int? Max { get; set; }
}
