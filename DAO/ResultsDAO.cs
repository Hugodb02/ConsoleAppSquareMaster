using ConsoleAppSquareMaster.Analysis;
using ConsoleAppSquareMaster.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ConsoleAppSquareMaster.DAO
{
    public class ResultsDAO
    {
        private readonly IMongoCollection<BsonDocument> _resultsCollection;

        public ResultsDAO(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("WorldConquestDB");
            _resultsCollection = database.GetCollection<BsonDocument>("SimulationResults");
        }

        public void SaveSimulationResult(WorldData worldData, List<EmpireStatistics> statistics, List<Empire> empires)
        {
            var document = new BsonDocument
            {
                { "WorldName", worldData.Name },
                { "AlgorithmType", worldData.AlgorithmType },
                { "MaxX", worldData.MaxX },
                { "MaxY", worldData.MaxY },
                { "Coverage", worldData.Coverage },
                { "Empires", new BsonArray(empires.ConvertAll(empire => new BsonDocument
                {
                    { "EmpireId", empire.Id },
                    { "StartPosition", new BsonDocument
                        {
                            { "X", empire.StartPosition.x },
                            { "Y", empire.StartPosition.y }
                        }
                    },
                    { "Strategy", empire.Strategy }
                })) },
                { "Results", new BsonArray(statistics.ConvertAll(stat => new BsonDocument
                {
                    { "EmpireId", stat.EmpireId },
                    { "CellsOccupied", stat.CellsOccupied },
                    { "PercentageOfWorld", stat.PercentageOfWorld }
                })) }
            };

            _resultsCollection.InsertOne(document);
        }
    }
}
