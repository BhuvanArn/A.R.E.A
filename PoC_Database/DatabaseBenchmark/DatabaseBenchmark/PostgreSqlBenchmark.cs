using Npgsql;
using System.Diagnostics;

namespace DatabaseBenchmark;

public class PostgreSqlBenchmark : IDatabaseBenchmark
{
    private readonly string _connectionString = "Host=localhost;Username=postgres;Password=password;Database=postgres";
    private readonly string _tableName = "benchmark_table";

    public void Setup()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand($"DROP TABLE IF EXISTS {_tableName};", conn);
        cmd.ExecuteNonQuery();

        cmd.CommandText = $"CREATE TABLE {_tableName} (id SERIAL PRIMARY KEY, data TEXT);";
        cmd.ExecuteNonQuery();
    }

    public void Cleanup()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand($"DROP TABLE IF EXISTS {_tableName};", conn);
        cmd.ExecuteNonQuery();
    }

    public void InsertSingle()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        var data = SampleData.GetSampleText();

        using var cmd = new NpgsqlCommand($"INSERT INTO {_tableName} (data) VALUES (@data);", conn);
        cmd.Parameters.AddWithValue("data", data);

        var stopwatch = Stopwatch.StartNew();
        cmd.ExecuteNonQuery();
        stopwatch.Stop();

        LogResult("InsertSingle", stopwatch.ElapsedMilliseconds);
    }

    public void InsertBulk(int count)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        var data = SampleData.GetSampleText();

        var stopwatch = Stopwatch.StartNew();
        using var transaction = conn.BeginTransaction();
        using var cmd = new NpgsqlCommand($"INSERT INTO {_tableName} (data) VALUES (@data);", conn);
        cmd.Parameters.AddWithValue("data", data);

        for (int i = 0; i < count; i++)
        {
            cmd.ExecuteNonQuery();
        }

        transaction.Commit();
        stopwatch.Stop();

        LogResult("InsertBulk", stopwatch.ElapsedMilliseconds);
    }

    public void ReadSingle()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand($"SELECT data FROM {_tableName} WHERE id = 1;", conn);

        var stopwatch = Stopwatch.StartNew();
        var result = cmd.ExecuteScalar();
        stopwatch.Stop();

        LogResult("ReadSingle", stopwatch.ElapsedMilliseconds);
    }

    public void ReadBulk(int count)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand($"SELECT data FROM {_tableName} LIMIT @count;", conn);
        cmd.Parameters.AddWithValue("count", count);

        var stopwatch = Stopwatch.StartNew();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var data = reader.GetString(0);
        }

        stopwatch.Stop();

        LogResult("ReadBulk", stopwatch.ElapsedMilliseconds);
    }

    public void UpdateSingle()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        var newData = SampleData.GetSampleText();

        using var cmd = new NpgsqlCommand($"UPDATE {_tableName} SET data = @data WHERE id = 1;", conn);
        cmd.Parameters.AddWithValue("data", newData);

        var stopwatch = Stopwatch.StartNew();
        cmd.ExecuteNonQuery();
        stopwatch.Stop();

        LogResult("UpdateSingle", stopwatch.ElapsedMilliseconds);
    }

    public void UpdateBulk(int count)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        var newData = SampleData.GetSampleText();

        var stopwatch = Stopwatch.StartNew();
        using var transaction = conn.BeginTransaction();
        using var cmd = new NpgsqlCommand($"UPDATE {_tableName} SET data = @data WHERE id = @id;", conn);
        cmd.Parameters.AddWithValue("data", newData);
        cmd.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);

        for (int i = 1; i <= count; i++)
        {
            cmd.Parameters["id"].Value = i;
            cmd.ExecuteNonQuery();
        }

        transaction.Commit();
        stopwatch.Stop();

        LogResult("UpdateBulk", stopwatch.ElapsedMilliseconds);
    }

    public void DeleteSingle()
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand($"DELETE FROM {_tableName} WHERE id = 1;", conn);

        var stopwatch = Stopwatch.StartNew();
        cmd.ExecuteNonQuery();
        stopwatch.Stop();

        LogResult("DeleteSingle", stopwatch.ElapsedMilliseconds);
    }

    public void DeleteBulk(int count)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        var stopwatch = Stopwatch.StartNew();
        using var cmd = new NpgsqlCommand($"DELETE FROM {_tableName};", conn);
        cmd.ExecuteNonQuery();
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