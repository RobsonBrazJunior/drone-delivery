using DroneDelivery.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DroneDelivery.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DeliveryController : ControllerBase
	{
		private readonly IDeliveryService _deliveryService;

		public DeliveryController(IDeliveryService deliveryService)
		{
			_deliveryService = deliveryService ?? throw new ArgumentNullException(nameof(deliveryService));
		}

		[HttpPost("delivery-package")]
		public async Task<IActionResult> DeliveryPackage(IList<string> entries)
		{
			var result = await _deliveryService.DeliveryPackage(entries);
			return Ok(result);
		}
	}
}
