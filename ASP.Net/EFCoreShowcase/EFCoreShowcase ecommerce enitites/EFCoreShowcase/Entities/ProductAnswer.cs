public class ProductAnswer : AuditableEntity
{
    public required string AnswerText { get; set; }
    public bool IsVerified { get; set; }

    public long QuestionId { get; set; }
    public required ProductQuestion Question { get; set; }

    public long CustomerId { get; set; }
    public required Customer Customer { get; set; }
}
