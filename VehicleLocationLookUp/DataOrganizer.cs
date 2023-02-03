using System.Collections.Concurrent;
using System.Diagnostics;

namespace VehicleLocationLookUp
{
    internal static class DataOrganizer
    {
        public static Dictionary<string, List<Record>>? DataToClusters(List<Record>? vehicles)
        {
            if (vehicles == null)
            {
                return null;
            }

            Stopwatch stopwatch = new ();

            stopwatch.Start();

            ConcurrentDictionary<string, List<Record>> dict = new ();

            Parallel.For(0, vehicles.Count, i =>
            {
                dict.AddOrUpdate($"{vehicles[i].Latitude:0.0},{vehicles[i].Longitude:0.0}",
                    addValue: new List<Record> { vehicles[i] },
                    updateValueFactory: (existingKey, existingValue) =>
                    {
                        existingValue.Add(vehicles[i]);
                        return existingValue;
                    });
            });

            stopwatch.Stop();

            Console.WriteLine($"Elapsed time for data cluster: {stopwatch.Elapsed}");

            return new Dictionary<string, List<Record>>(dict);
        }
    }
}
