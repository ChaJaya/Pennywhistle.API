namespace Pennywhistle.Application.Common.Contracts
{
    /// <summary>
    /// User service
    /// </summary>
    public interface ICurrentUserService
    {
        string UserId { get; }
    }
}
