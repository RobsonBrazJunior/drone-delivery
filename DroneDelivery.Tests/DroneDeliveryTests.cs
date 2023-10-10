using DroneDelivery.Domain.Interfaces;
using DroneDelivery.WebApi.Services;
using FluentAssertions;
using Moq;

namespace DroneDelivery.Tests
{
	public class DroneDeliveryTests
	{
		[Fact]
		public async Task DeliveryPackage_ShouldReturnFasterRoute()
		{
			// Arrange
			var graphServiceMock = new Mock<IGraphService>();
			graphServiceMock.Setup(service => service.GetGraphAsync())
				.ReturnsAsync(GetGraphDictionary());

			var deliveryService = new DeliveryService(graphServiceMock.Object);

			var entries = new List<string> { "A1", "B2", "C1" };

			// Act
			var result = await deliveryService.DeliveryPackage(entries);

			// Assert
			result.Should().NotBeNull();
			result.Should().Contain("A1 -> B1 -> B2 -> B1 -> C1");

			graphServiceMock.Verify(service => service.GetGraphAsync(), Times.Once);
		}

		[Fact]
		public async Task DeliveryPackage_InvalidEntries_ShouldThrowArgumentException()
		{
			// Arrange
			var graphServiceMock = new Mock<IGraphService>();
			var deliveryService = new DeliveryService(graphServiceMock.Object);

			var graph = new Dictionary<string, Dictionary<string, double>>
			{
				{ "A1", new Dictionary<string, double> { { "A2", 10.0 } } },
				{ "A2", new Dictionary<string, double> { { "A1", 10.0 } } },
			};

			graphServiceMock.Setup(service => service.GetGraphAsync()).ReturnsAsync(graph);

			var invalidEntries = new List<string> { "A1", "B1" };

			// Act
			Func<Task> act = async () => await deliveryService.DeliveryPackage(invalidEntries);

			// Assert
			await act.Should().ThrowAsync<ArgumentException>()
				.WithMessage("A entrada 'B1' não existe no grafo.");
		}

		private Dictionary<string, Dictionary<string, double>> GetGraphDictionary()
		{
			return new Dictionary<string, Dictionary<string, double>>
			{
				{ "A1", new Dictionary<string, double>
					{
						{ "A2", 11.88 },
						{ "B1", 5.50 }
					}
				},
				{ "A2", new Dictionary<string, double>
					{
						{ "A3", 22 },
						{ "B2", 24.6 },
						{ "A1", 21.77 }
					}
				},
				{ "A3", new Dictionary<string, double>
					{
						{ "A2", 22.5 }
					}
				},
				{ "B1", new Dictionary<string, double>
					{
						{ "A1", 23.01 },
						{ "B2", 5.00 },
						{ "C1", 10.00 }
					}
				},
				{ "B2", new Dictionary<string, double>
					{
						{ "A2", 26.7 },
						{ "B1", 5.00 }
					}
				},
				{ "C1", new Dictionary<string, double>
					{
						{ "B1", 11.16 },
						{ "C2", 18.99 },
					}
				}
			};
		}
	}
}