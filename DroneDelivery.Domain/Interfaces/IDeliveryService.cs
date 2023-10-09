namespace DroneDelivery.Domain.Interfaces
{
	public interface IDeliveryService
	{
		Task<string> DeliveryPackage(IList<string> entries);
	}
}
