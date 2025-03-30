namespace EFCoreShowcase.DTOs;

public record ReviewHelpfulnessDto
{
    public long Id { get; init; }
    public bool IsHelpful { get; init; }
    public long ReviewId { get; init; }
    public long CustomerId { get; init; }
}

public record CreateReviewHelpfulnessDto
{
    public bool IsHelpful { get; init; }
    public long ReviewId { get; init; }
}
