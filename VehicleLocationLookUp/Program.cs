// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using VehicleLocationLookUp;

var stopwatch = new Stopwatch();

stopwatch.Start();

var vehicles = LocationLookUp.FindClosestLocation(DataOrganizer.DataToClasters(DataReader.ReadDataFromFile().ToList()));

stopwatch.Stop();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"Total elapsed time: {stopwatch.Elapsed}");
Console.ForegroundColor = ConsoleColor.White;
foreach (var record in vehicles)
{
    Console.WriteLine($"PositionId: {record.PositionId}, Distance: {record.Distance.ToString("0.00")}");
}

Console.ReadKey();

