using DroneDelivery.Domain.Interfaces;

namespace DroneDelivery.WebApi.Services
{
	public class DeliveryService : IDeliveryService
	{
		private readonly IGraphService _graphService;

		public DeliveryService(IGraphService graphService)
		{
			_graphService = graphService;
		}

		public async Task<string> DeliveryPackage(IList<string> entries)
		{
			var graph = await _graphService.GetGraphAsync();

			var shortestPath1 = FindShortestPath(graph, entries.ElementAt(0), entries.ElementAt(1));
			var shortestPath2 = FindShortestPath(graph, entries.ElementAt(1), entries.ElementAt(2));

			double timeFirstPath = CalculateTotalTime(graph, shortestPath1);
			double timeSecondPath = CalculateTotalTime(graph, shortestPath2);

			shortestPath2.RemoveAt(0);

			return $"{string.Join(" -> ", shortestPath1)} -> {string.Join(" -> ", shortestPath2)}, and will take {Math.Round(timeFirstPath + timeSecondPath, 2)} seconds to be delivered as fast as possible.";
		}

		private List<string> FindShortestPath(Dictionary<string, Dictionary<string, double>> graph, string start, string stop)
		{
			var visited = new HashSet<string>();
			var distances = new Dictionary<string, double>();
			var predecessors = new Dictionary<string, string>();
			var queue = new List<string>();

			InitializeDistances(graph.Keys, distances, predecessors, start);

			queue.Add(start);

			while (queue.Count > 0)
			{
				var currentVertex = FindClosestUnvisitedVertex(queue, distances);
				queue.Remove(currentVertex);

				if (currentVertex == stop)
					break;

				visited.Add(currentVertex);

				UpdateDistances(graph[currentVertex], distances, predecessors, currentVertex, queue);
			}

			return BuildShortestPath(predecessors, start, stop);
		}

		private double CalculateTotalTime(Dictionary<string, Dictionary<string, double>> graph, List<string> path)
		{
			double totalTime = 0;
			for (int i = 0; i < path.Count - 1; i++)
			{
				string current = path[i];
				string next = path[i + 1];
				totalTime += graph[current][next];
			}
			return totalTime;
		}

		private void InitializeDistances(IEnumerable<string> vertices, Dictionary<string, double> distances,
			Dictionary<string, string> predecessors, string start)
		{
			foreach (var vertex in vertices)
			{
				distances[vertex] = double.MaxValue;
				predecessors[vertex] = null;
			}

			distances[start] = 0;
		}

		private string FindClosestUnvisitedVertex(List<string> queue, Dictionary<string, double> distances)
		{
			return queue.OrderBy(v => distances[v]).First();
		}

		private void UpdateDistances(Dictionary<string, double> neighbors, Dictionary<string, double> distances,
			Dictionary<string, string> predecessors, string currentVertex, List<string> queue)
		{
			foreach (var neighbor in neighbors)
			{
				var alternativeRoute = distances[currentVertex] + neighbor.Value;
				if (alternativeRoute < distances[neighbor.Key])
				{
					distances[neighbor.Key] = alternativeRoute;
					predecessors[neighbor.Key] = currentVertex;
					queue.Add(neighbor.Key);
				}
			}
		}

		private List<string> BuildShortestPath(Dictionary<string, string> predecessors, string start, string stop)
		{
			var shortestPath = new List<string>();
			var current = stop;
			while (current != null)
			{
				shortestPath.Add(current);
				current = predecessors[current];
			}

			shortestPath.Reverse();
			return shortestPath;
		}
	}
}
