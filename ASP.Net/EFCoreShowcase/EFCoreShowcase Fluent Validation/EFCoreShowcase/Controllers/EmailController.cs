using EFCoreShowcase.Core.Validation.Email;
using EFCoreShowcase.Models;
using EFCoreShowcase.Models.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreShowcase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IValidator<Email> _validator;

    public EmailController(IValidator<Email> validator)
    {
        _validator = validator;
    }

    [HttpGet("validation-rules")]
    public IActionResult GetValidationRules()
    {
        var validator = _validator as EmailValidator;
        if (validator == null)
            return NotFound();

        var rules = new ValidationRulesResponse
        {
            Properties = new()
            {
                Address = new()
                {
                    Validators = new[]
                    {
                        new ValidationRuleDetail
                        {
                            Name = "RegularExpressionValidator",
                            Expression = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                        }
                    }
                },
                Subject = new()
                {
                    Validators = new[]
                    {
                        new ValidationRuleDetail { Name = "MaximumLengthValidator", Max = 100 }
                    }
                },
                Message = new()
                {
                    Validators = new[]
                    {
                        new ValidationRuleDetail { Name = "MaximumLengthValidator", Max = 1000 }
                    }
                }
            }
        };

        return Ok(rules);
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] Email email)
    {
        var result = await _validator.ValidateAsync(email);
        if (!result.IsValid)
        {
            return BadRequest(new
            {
                Errors = result.Errors.Select(e => new { Property = e.PropertyName, Error = e.ErrorMessage })
            });
        }

        // Email sending logic would go here
        return Ok(new { message = "Email sent successfully" });
    }
}
