using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data
{
    // This is the bridge between our C# application and MySQL database.
    // Equivalent to Mongoose schema compilation and connection handling.
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        // DbSet represents a table in the database.
        // EF Core will create a table named "Books" based on the Book class.
        public DbSet<Book> Books { get; set; }
    }
}
