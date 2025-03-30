namespace EFCoreShowcase.DTOs;

public record ProductQuestionDto
{
    public long Id { get; init; }
    public string QuestionText { get; init; } = null!;
    public string CustomerName { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
    public ICollection<ProductAnswerDto> Answers { get; init; } = new List<ProductAnswerDto>();
}

public record ProductAnswerDto
{
    public long Id { get; init; }
    public string AnswerText { get; init; } = null!;
    public string CustomerName { get; init; } = null!;
    public bool IsVerified { get; init; }
    public DateTime CreatedAt { get; init; }
}
