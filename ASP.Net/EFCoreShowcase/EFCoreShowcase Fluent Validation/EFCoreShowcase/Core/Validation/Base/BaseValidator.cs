using FluentValidation;

namespace EFCoreShowcase.Core.Validation.Base;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected void ApplyRuleSet(string ruleSet, Action rules)
    {
        RuleSet(ruleSet, rules);
        rules(); // Apply rules to default ruleset as well
    }
}
