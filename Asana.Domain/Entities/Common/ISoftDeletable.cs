namespace Asana.Domain.Entities.Common
{
    public interface ISoftDeletable
    {
        bool IsDelete { get; set; }
    }
}
