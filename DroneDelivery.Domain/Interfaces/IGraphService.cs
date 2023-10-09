namespace DroneDelivery.Domain.Interfaces
{
	public interface IGraphService
	{
		Task<Dictionary<string, Dictionary<string, double>>> GetGraphAsync();
	}
}
