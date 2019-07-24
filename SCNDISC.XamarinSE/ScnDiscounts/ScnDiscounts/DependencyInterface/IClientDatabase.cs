using SQLite;

namespace ScnDiscounts.DependencyInterface
{
    public interface IClientDatabase
    {
        SQLiteConnection GetSQLiteConnection(string fileName);
    }
}