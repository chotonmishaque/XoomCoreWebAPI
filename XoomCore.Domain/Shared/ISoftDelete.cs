namespace XoomCore.Domain.Shared
{
    public interface ISoftDelete
    {
        long? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
