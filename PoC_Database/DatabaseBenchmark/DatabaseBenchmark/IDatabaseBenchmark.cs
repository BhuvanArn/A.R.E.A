namespace DatabaseBenchmark;

public interface IDatabaseBenchmark
{
    void Setup();
    void Cleanup();
    void InsertSingle();
    void InsertBulk(int count);
    void ReadSingle();
    void ReadBulk(int count);
    void UpdateSingle();
    void UpdateBulk(int count);
    void DeleteSingle();
    void DeleteBulk(int count);
}