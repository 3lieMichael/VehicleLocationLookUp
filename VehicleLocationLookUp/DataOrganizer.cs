using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace VehicleLocationLookUp
{
    internal static class DataOrganizer
    {
        public static Dictionary<string, List<Record>>? DataToClasters(List<Record>? vehicles)
        {
            if (vehicles == null)
            {
                return null;
            }

            Stopwatch stopwatch = new ();

            stopwatch.Start();

            ConcurrentDictionary<string, List<Record>> dict = new ();

            Parallel.ForEach(vehicles, vehicle =>
            {
                StringBuilder keyBuilder = new ();
                keyBuilder.Append(vehicle.Latitude.ToString("0.0"));
                keyBuilder.Append(",");
                keyBuilder.Append(vehicle.Longitude.ToString("0.0"));

                dict.AddOrUpdate(keyBuilder.ToString(),
                    addValue: new List<Record> { vehicle },
                    updateValueFactory: (existingKey, existingValue) =>
                    {
                        existingValue.Add(vehicle);
                        return existingValue;
                    });
            });

            stopwatch.Stop();

            Console.WriteLine($"Elapsed time for data claster: {stopwatch.Elapsed}");

            return new Dictionary<string, List<Record>>(dict);
        }
    }
}
