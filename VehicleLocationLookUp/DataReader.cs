using System.Text;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Collections.Concurrent;

namespace VehicleLocationLookUp
{
    internal static class DataReader
    {
        public static ConcurrentBag<Record> ReadDataFromFile()
        {
            Stopwatch stopwatch = new ();
            stopwatch.Start();

            string filePath = "VehiclePositions.dat";
            ConcurrentBag<Record> records = new ();

            // Create a memory-mapped file from the .dat file
            using var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
            // Create a view accessor for the entire contents of the file
            using var accessor = mmf.CreateViewAccessor();
            unsafe
            {
                byte* pData = null;
                accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pData);
                int dataLength = (int)accessor.Capacity;
                byte[] data = new byte[dataLength];
                accessor.ReadArray(0, data, 0, data.Length);

                try
                {
                    int chunkSize = data.Length / Environment.ProcessorCount;
                    int chunkStart = 0;
                    int chunkEnd = chunkSize;

                    List<Task> tasks = new ();

                    while (chunkStart < data.Length)
                    {
                        int currentChunkSize = Math.Min(chunkSize, data.Length - chunkStart);
                        byte[] chunk = new byte[currentChunkSize];
                        Array.Copy(data, chunkStart, chunk, 0, currentChunkSize);

                        var task = Task.Run(() =>
                        {
                            // Create a BinaryReader from the memory-mapped view
                            using BinaryReader reader = new (new MemoryStream(chunk));
                            // Read the contents of the file
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                try
                                {
                                    records.Add(new Record()
                                    {
                                        PositionId = reader.ReadInt32(),
                                        VehicleRegistration = ReadStringUntilNull(reader),
                                        Latitude = reader.ReadSingle(),
                                        Longitude = reader.ReadSingle(),
                                        RecordedTimeUTC = reader.ReadUInt64()
                                    });
                                }
                                catch
                                {
                                }
                            }
                        });

                        tasks.Add(task);
                        chunkStart += chunkSize;
                        chunkEnd += chunkSize;
                    }

                    Task.WaitAll(tasks.ToArray());
                }
                finally
                {
                    accessor.SafeMemoryMappedViewHandle.ReleasePointer();
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Elapsed time for data read: {stopwatch.Elapsed}");

            return records;
        }

        private static string ReadStringUntilNull(BinaryReader reader)
        {
            StringBuilder stringBuilder = new ();
            byte currentByte = reader.ReadByte();
            while (currentByte != 0)
            {
                stringBuilder.Append((char)currentByte);
                currentByte = reader.ReadByte();
            }
            return stringBuilder.ToString();
        }
    }
}
