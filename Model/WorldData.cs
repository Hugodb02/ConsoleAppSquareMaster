using MongoDB.Bson;

public class WorldData
{

    public ObjectId Id { get; set; } // MongoDB ObjectId
    public string Name { get; set; }
    public string AlgorithmType { get; set; }
    public int MaxX { get; set; }
    public int MaxY { get; set; }
    public double Coverage { get; set; }
}
