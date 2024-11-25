using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace DatabaseBenchmark;

public class MongoDbBenchmark : IDatabaseBenchmark
{
    private readonly IMongoDatabase _database;
    private readonly string _collectionName = "benchmark_collection";

    public MongoDbBenchmark()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase("benchmarkdb");
    }

    public void Setup()
    {
        _database.DropCollection(_collectionName);
        _database.CreateCollection(_collectionName);
    }

    public void Cleanup()
    {
        _database.DropCollection(_collectionName);
    }

    public void InsertSingle()
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);
        var document = new BsonDocument { { "data", SampleData.GetSampleText() } };

        var stopwatch = Stopwatch.StartNew();
        collection.InsertOne(document);
        stopwatch.Stop();

        LogResult("InsertSingle", stopwatch.ElapsedMilliseconds);
    }

    public void InsertBulk(int count)
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);
        var documents = new BsonDocument[count];
        var data = SampleData.GetSampleText();

        for (int i = 0; i < count; i++)
        {
            documents[i] = new BsonDocument { { "data", data } };
        }

        var stopwatch = Stopwatch.StartNew();
        collection.InsertMany(documents);
        stopwatch.Stop();

        LogResult("InsertBulk", stopwatch.ElapsedMilliseconds);
    }

    public void ReadSingle()
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);

        var stopwatch = Stopwatch.StartNew();
        var result = collection.Find(new BsonDocument()).FirstOrDefault();
        stopwatch.Stop();

        LogResult("ReadSingle", stopwatch.ElapsedMilliseconds);
    }

    public void ReadBulk(int count)
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);

        var stopwatch = Stopwatch.StartNew();
        var results = collection.Find(new BsonDocument()).Limit(count).ToList();
        stopwatch.Stop();

        LogResult("ReadBulk", stopwatch.ElapsedMilliseconds);
    }

    public void UpdateSingle()
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);
        var filter = Builders<BsonDocument>.Filter.Empty;
        var update = Builders<BsonDocument>.Update.Set("data", SampleData.GetSampleText());

        var stopwatch = Stopwatch.StartNew();
        collection.UpdateOne(filter, update);
        stopwatch.Stop();

        LogResult("UpdateSingle", stopwatch.ElapsedMilliseconds);
    }

    public void UpdateBulk(int count)
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);
        var filter = Builders<BsonDocument>.Filter.Empty;
        var update = Builders<BsonDocument>.Update.Set("data", SampleData.GetSampleText());

        var stopwatch = Stopwatch.StartNew();
        collection.UpdateMany(filter, update);
        stopwatch.Stop();

        LogResult("UpdateBulk", stopwatch.ElapsedMilliseconds);
    }

    public void DeleteSingle()
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);
        var filter = Builders<BsonDocument>.Filter.Empty;

        var stopwatch = Stopwatch.StartNew();
        collection.DeleteOne(filter);
        stopwatch.Stop();

        LogResult("DeleteSingle", stopwatch.ElapsedMilliseconds);
    }

    public void DeleteBulk(int count)
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);

        var stopwatch = Stopwatch.StartNew();
        collection.DeleteMany(new BsonDocument());
        stopwatch.Stop();

        LogResult("DeleteBulk", stopwatch.ElapsedMilliseconds);
    }

    private void LogResult(string operation, long milliseconds)
    {
        string log = $"{DateTime.Now},{GetType().Name},{operation},{milliseconds}";
        Console.WriteLine($"{GetType().Name} {operation}: {milliseconds} ms");
        File.AppendAllText("benchmark_results.csv", log + Environment.NewLine);
    }
}
