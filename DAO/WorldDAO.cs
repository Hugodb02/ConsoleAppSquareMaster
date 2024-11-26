using MongoDB.Driver;
using System.Collections.Generic;

public class WorldDAO
{
    private readonly IMongoCollection<WorldData> worldsCollection;

    public WorldDAO(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("WorldConquestDB"); // Creëert de database als deze niet bestaat
        worldsCollection = database.GetCollection<WorldData>("Worlds"); // Creëert de collectie als deze niet bestaat
    }

    public void SaveWorld(WorldData world)
    {
        worldsCollection.InsertOne(world);
    }

    public void SaveWorlds(List<WorldData> worlds)
    {
        worldsCollection.InsertMany(worlds);
    }

    public List<WorldData> GetAllWorlds()
    {
        return worldsCollection.Find(_ => true).ToList();
    }

    public WorldData GetWorldByName(string name)
    {
        return worldsCollection.Find(w => w.Name == name).FirstOrDefault();
    }
}
