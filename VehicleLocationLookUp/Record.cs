// See https://aka.ms/new-console-template for more information
struct Record
{
    public int PositionId { get; set; }
    public string VehicleRegistration { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public ulong RecordedTimeUTC { get; set; }
}
