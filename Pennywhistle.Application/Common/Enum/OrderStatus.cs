namespace Pennywhistle.Application.Common.Enum
{
    /// <summary>
    /// Order status 
    /// </summary>
    enum OrderStatus
    {
        PendingOrder = 0,
        CancelledOrder = 1,
        PrepareOrder = 2,
        ReadyToPickUp = 3,
        OrderDelivered = 4,
        OrderComplete = 5
    }

}
