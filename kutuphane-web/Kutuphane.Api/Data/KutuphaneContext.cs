using Kutuphane.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Data;

/// <summary>
/// EF Core database context (Code First). It maps our entities to
/// PostgreSQL tables and defines the relationships between them.
/// </summary>
public class KutuphaneContext : DbContext
{
    public KutuphaneContext(DbContextOptions<KutuphaneContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Relationships -------------------------------------------------

        // A Book belongs to one Category; a Category has many Books.
        // Restrict: a category cannot be deleted while it still has books.
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // A Loan references one Book; keep loan history if a book is removed.
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // A Loan references one Member; deleting a member removes their loans.
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Member)
            .WithMany(m => m.Loans)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- Seed data -----------------------------------------------------
        // Fixed UTC dates so the migration is deterministic.
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Roman" },
            new Category { Id = 2, Name = "Bilim" },
            new Category { Id = 3, Name = "Tarih" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Suç ve Ceza", Author = "Fyodor Dostoyevski", Isbn = "978-975-1", PublishedYear = 1866, TotalCopies = 3, CategoryId = 1 },
            new Book { Id = 2, Title = "1984", Author = "George Orwell", Isbn = "978-975-2", PublishedYear = 1949, TotalCopies = 2, CategoryId = 1 },
            new Book { Id = 3, Title = "Kozmos", Author = "Carl Sagan", Isbn = "978-975-3", PublishedYear = 1980, TotalCopies = 4, CategoryId = 2 },
            new Book { Id = 4, Title = "Nutuk", Author = "Mustafa Kemal Atatürk", Isbn = "978-975-4", PublishedYear = 1927, TotalCopies = 5, CategoryId = 3 }
        );

        modelBuilder.Entity<Member>().HasData(
            new Member { Id = 1, FullName = "Ahmet Yılmaz", Email = "ahmet@example.com", Phone = "0532 000 0001", RegisteredDate = seedDate },
            new Member { Id = 2, FullName = "Elif Kaya", Email = "elif@example.com", Phone = "0532 000 0002", RegisteredDate = seedDate }
        );

        modelBuilder.Entity<Loan>().HasData(
            new Loan
            {
                Id = 1,
                BookId = 1,
                MemberId = 1,
                LoanDate = seedDate,
                DueDate = seedDate.AddDays(14),
                ReturnDate = null
            }
        );
    }
}
