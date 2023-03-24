using Microsoft.EntityFrameworkCore;

namespace ImageRecorder.Application;
public class ImageRecorderContext : DbContext
{
    /// <summary>
    /// Initialize the database with some values (holidays, ...).
    /// Unlike Seed, this method is also called in production.
    /// </summary>
    private void Initialize()
    {
        // Your code
    }
    /// <summary>
    /// Generates random values for testing the application. This method is only called in development mode.
    /// </summary>
    private void Seed()
    {
        // Your Code
    }

    /// <summary>
    /// Creates the database. Called once at application startup.
    /// </summary>
    public void CreateDatabase(bool isDevelopment)
    {
        if (isDevelopment) { Database.EnsureDeleted(); }
        // EnsureCreated only creates the model if the database does not exist or it has no
        // tables. Returns true if the schema was created.  Returns false if there are
        // existing tables in the database. This avoids initializing multiple times.
        if (Database.EnsureCreated()) { Initialize(); }
        if (isDevelopment) Seed();
    }
}