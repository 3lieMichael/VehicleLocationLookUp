using System.Diagnostics;

namespace VehicleLocationLookUp
{
    internal class LocationLookUp
    {
        public static List<VehicleDistance> FindClosestLocation(Dictionary<string, List<Record>> vehicleGroups)
        {
            Stopwatch stopwatch = new ();

            stopwatch.Start();

            List<Position> positions = new ()
            {
                new (34.544909, -102.100843),
                new (32.345544, -99.123124),
                new (33.234235, -100.214124),
                new (35.195739, -95.348899),
                new (31.895839, -97.789573),
                new (32.895839, -101.789573),
                new (34.115839, -100.225732),
                new (32.335839, -99.992232),
                new (33.535339, -94.792232),
                new (32.234235, -100.222222)
            };
            List<VehicleDistance> distanceList = new ();

            foreach (var position in positions)
            {
                var vehicles = vehicleGroups[$"{position.Latitude.ToString("0.0")},{position.Longitude.ToString("0.0")}"];
                var closestVehicle = vehicles[0];
                var vehicleDistance = 0.0;

                for (int i = 0; i < vehicles.Count; i++)
                {
                    var distance = CalculateDistance(position, new (vehicles[i].Latitude, vehicles[i].Longitude));

                    if (i == 0)
                    {
                        vehicleDistance = distance;
                    }

                    if (distance < vehicleDistance)
                    {
                        closestVehicle = vehicles[i];
                        vehicleDistance = distance;
                    }
                }
                distanceList.Add(new VehicleDistance
                {
                    PositionId = closestVehicle.PositionId,
                    Distance = vehicleDistance
                });
            }

            stopwatch.Stop();

            Console.WriteLine($"Elapsed time for location look-up: {stopwatch.Elapsed}");

            return distanceList;
        }

        private static double CalculateDistance(Position x, Position y)
        {
            double radius = 6371; // Earth's radius in kilometers
            double latX = DegreesToRadians(x.Latitude);
            double longX = DegreesToRadians(x.Longitude);
            double latY = DegreesToRadians(y.Latitude);
            double longY = DegreesToRadians(y.Longitude);

            double deltaLatitude = latY - latX;
            double deltaLongitude = longY - longX;

            double a = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) +
                       Math.Cos(latX) * Math.Cos(latY) *
                       Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return radius * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
    }
}
