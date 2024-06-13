using Microsoft.EntityFrameworkCore;

namespace Medoxity.Models
{
    public class MedicalPlatformContext : DbContext
{
    public MedicalPlatformContext(DbContextOptions<MedicalPlatformContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Drug> Drugs { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<User>().HasKey(a => a.Username);
            modelBuilder.Entity<Drug>().HasKey(a => a.DrugID);
            modelBuilder.Entity<Comment>().HasKey(a => a.CommentID);
            modelBuilder.Entity<Like>().HasKey(a => a.LikeID);
            modelBuilder.Entity<Rating>().HasKey(a => a.RatingID);
            modelBuilder.Entity<Document>().HasKey(a => a.DocumentID);
            modelBuilder.Entity<Message>().HasKey(a => a.MessageID);

            base.OnModelCreating(modelBuilder);
        
    }
}
}
