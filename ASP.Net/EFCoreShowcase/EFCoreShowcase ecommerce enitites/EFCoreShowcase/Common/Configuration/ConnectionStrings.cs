public static class ConnectionStrings
{
    public const string DefaultConnection = "DefaultConnection";
    public const string ReadOnlyConnection = "ReadOnlyConnection";

    public static string GetConnectionString(IConfiguration configuration, string name = DefaultConnection)
    {
        return configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Connection string '{name}' not found.");
    }
}
