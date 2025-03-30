namespace EFCoreShowcase.Constants;

public static class ValidationMessages
{
    public static class Product
    {
        public const string NameRequired = "Product name is required";
        public const string NameLength = "Product name cannot exceed {MaxLength} characters";
        public const string PricePositive = "Price must be greater than zero";
        public const string DescriptionLength = "Description cannot exceed {MaxLength} characters";
        public const string CategoryRequired = "A valid category must be selected";
    }

    public static class Category
    {
        public const string NameRequired = "Category name is required";
        public const string NameLength = "Category name cannot exceed {MaxLength} characters";
    }

    public static class Common
    {
        public const string InvalidPageNumber = "Page number must be greater than 0";
        public const string InvalidPageSize = "Page size must be between {MinPageSize} and {MaxPageSize}";
        public const string InvalidPriceRange = "Maximum price must be greater than or equal to minimum price";
        public const string SearchTermTooLong = "Search term cannot exceed {MaxLength} characters";
    }
}
