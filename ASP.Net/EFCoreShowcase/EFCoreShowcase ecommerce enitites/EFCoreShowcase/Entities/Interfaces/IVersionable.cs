namespace EFCoreShowcase.Entities.Interfaces;

public interface IVersionable
{
    byte[] RowVersion { get; set; }
}
