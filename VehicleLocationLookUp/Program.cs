// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using VehicleLocationLookUp;

Stopwatch stopwatch = new ();

stopwatch.Start();

var vehicles = LocationLookUp.FindClosestLocation(DataOrganizer.DataToClusters(DataReader.ReadDataFromFile()?.ToList()));

stopwatch.Stop();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"Total elapsed time: {stopwatch.Elapsed}");
Console.ForegroundColor = ConsoleColor.White;

foreach (var record in vehicles ?? new List<VehicleDistance>())
{
    Console.WriteLine($"PositionId: {record.PositionId}, Distance: {record.Distance.ToString("0.00")}");
}

Console.WriteLine($"Press any key to continue . . .");
Console.ReadKey();

