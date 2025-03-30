namespace EFCoreShowcase.Constants;

public static class ValidationConstants
{
    public static class Product
    {
        public const int MaxNameLength = 200;
        public const int MaxDescriptionLength = 1000;
        public const int MinPrice = 0;
    }

    public static class Category
    {
        public const int MaxNameLength = 100;
    }

    public static class Pagination
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
        public const int MinPageSize = 1;
    }
}
