using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data
{
    // Bridge between our C# models and the MySQL database
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        // Each DbSet = one table in the database
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Book)
                .WithMany(b => b.Issues)
                .HasForeignKey(i => i.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Member)
                .WithMany(m => m.Issues)
                .HasForeignKey(i => i.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.Issue)
                .WithOne(i => i.Fine)
                .HasForeignKey<Fine>(f => f.IssueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Issue>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Member>()
                .Property(m => m.MembershipType)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


        }
    }
}
