using System.Diagnostics;
using MySqlConnector;

namespace DatabaseBenchmark;

public class MariaDbBenchmark : IDatabaseBenchmark
{
    private readonly string _connectionString =
        "Server=localhost;Port=3307;User ID=root;Password=password;Database=benchmarkdb";

    private readonly string _tableName = "benchmark_table";

    public void Setup()
    {
        try
        {
            var createDbConnectionString = "Server=localhost;Port=3307;User ID=root;Password=password;";
            using var conn = new MySqlConnection(createDbConnectionString);
            conn.Open();

            using var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS benchmarkdb;", conn);
            cmd.ExecuteNonQuery();

            using var benchmarkConn = new MySqlConnection(_connectionString);
            benchmarkConn.Open();

            using var tableCmd = new MySqlCommand($"DROP TABLE IF EXISTS {_tableName};", benchmarkConn);
            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText = $"CREATE TABLE {_tableName} (id INT AUTO_INCREMENT PRIMARY KEY, data TEXT);";
            tableCmd.ExecuteNonQuery();

            Console.WriteLine("MariaDbBenchmark Setup: 'benchmarkdb' and 'benchmark_table' are ready.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark Setup Error: {ex.Message}");
            throw;
        }
    }

    public void Cleanup()
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand($"DROP TABLE IF EXISTS {_tableName};", conn);
            cmd.ExecuteNonQuery();

            Console.WriteLine("MariaDbBenchmark Cleanup: 'benchmark_table' has been dropped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark Cleanup Error: {ex.Message}");
            throw;
        }
    }

    public void InsertSingle()
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var data = SampleData.GetSampleText();

            using var cmd = new MySqlCommand($"INSERT INTO {_tableName} (data) VALUES (@data);", conn);
            cmd.Parameters.AddWithValue("@data", data);

            var stopwatch = Stopwatch.StartNew();
            cmd.ExecuteNonQuery();
            stopwatch.Stop();

            LogResult("InsertSingle", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark InsertSingle Error: {ex.Message}");
        }
    }

    public void InsertBulk(int count)
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var data = SampleData.GetSampleText();

            var stopwatch = Stopwatch.StartNew();
            using var transaction = conn.BeginTransaction();
            using var cmd = new MySqlCommand($"INSERT INTO {_tableName} (data) VALUES (@data);", conn, transaction);
            cmd.Parameters.AddWithValue("@data", data);

            for (int i = 0; i < count; i++)
            {
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
            stopwatch.Stop();

            LogResult("InsertBulk", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark InsertBulk Error: {ex.Message}");
        }
    }

    public void ReadSingle()
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand($"SELECT data FROM {_tableName} WHERE id = 1;", conn);

            var stopwatch = Stopwatch.StartNew();
            var result = cmd.ExecuteScalar();
            stopwatch.Stop();

            LogResult("ReadSingle", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark ReadSingle Error: {ex.Message}");
        }
    }

    public void ReadBulk(int count)
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand($"SELECT data FROM {_tableName} LIMIT @count;", conn);
            cmd.Parameters.AddWithValue("@count", count);

            var stopwatch = Stopwatch.StartNew();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var data = reader.GetString(0);
                // Optionally process data
            }

            stopwatch.Stop();

            LogResult("ReadBulk", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark ReadBulk Error: {ex.Message}");
        }
    }

    public void UpdateSingle()
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var newData = SampleData.GetSampleText();

            using var cmd = new MySqlCommand($"UPDATE {_tableName} SET data = @data WHERE id = 1;", conn);
            cmd.Parameters.AddWithValue("@data", newData);

            var stopwatch = Stopwatch.StartNew();
            cmd.ExecuteNonQuery();
            stopwatch.Stop();

            LogResult("UpdateSingle", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark UpdateSingle Error: {ex.Message}");
        }
    }

    public void UpdateBulk(int count)
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var newData = SampleData.GetSampleText();

            var stopwatch = Stopwatch.StartNew();
            using var transaction = conn.BeginTransaction();
            using var cmd =
                new MySqlCommand($"UPDATE {_tableName} SET data = @data WHERE id = @id;", conn, transaction);
            cmd.Parameters.AddWithValue("@data", newData);
            cmd.Parameters.Add("@id", MySqlDbType.Int32);

            for (int i = 1; i <= count; i++)
            {
                cmd.Parameters["@id"].Value = i;
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
            stopwatch.Stop();

            LogResult("UpdateBulk", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark UpdateBulk Error: {ex.Message}");
        }
    }

    public void DeleteSingle()
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand($"DELETE FROM {_tableName} WHERE id = 1;", conn);

            var stopwatch = Stopwatch.StartNew();
            cmd.ExecuteNonQuery();
            stopwatch.Stop();

            LogResult("DeleteSingle", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark DeleteSingle Error: {ex.Message}");
        }
    }

    public void DeleteBulk(int count)
    {
        try
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var stopwatch = Stopwatch.StartNew();
            using var cmd = new MySqlCommand($"DELETE FROM {_tableName};", conn);
            cmd.ExecuteNonQuery();
            stopwatch.Stop();

            LogResult("DeleteBulk", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MariaDbBenchmark DeleteBulk Error: {ex.Message}");
        }
    }

    private void LogResult(string operation, long milliseconds)
    {
        string log = $"{DateTime.Now},{GetType().Name},{operation},{milliseconds}";
        Console.WriteLine($"{GetType().Name} {operation}: {milliseconds} ms");
        File.AppendAllText("benchmark_results.csv", log + Environment.NewLine);
    }
}
