namespace DatabaseBenchmark;

class BenchmarkRunner
{
    static void Main(string[] args)
    {
        var benchmarks = new List<IDatabaseBenchmark>
        {
            new PostgreSqlBenchmark(),
            new MySqlBenchmark(),
            new MariaDbBenchmark(),
            new MongoDbBenchmark()
        };

        foreach (var benchmark in benchmarks)
        {
            Console.WriteLine($"Starting benchmarks for {benchmark.GetType().Name}");
            benchmark.Setup();

            benchmark.InsertSingle();
            benchmark.InsertBulk(1000);

            benchmark.ReadSingle();
            benchmark.ReadBulk(1000);

            benchmark.UpdateSingle();
            benchmark.UpdateBulk(1000);

            benchmark.DeleteSingle();
            benchmark.DeleteBulk(1000);

            benchmark.Cleanup();
            Console.WriteLine($"Completed benchmarks for {benchmark.GetType().Name}\n");
        }
    }
}