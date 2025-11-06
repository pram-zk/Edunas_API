using API_Edunas.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Edunas.Data
{
    public class AppDbContext : DbContext
    {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Users> Users { get; set; }
            public DbSet<Subjects> Subjects { get; set; }
            public DbSet<Quiz> Quizzes { get; set; }
            public DbSet<Quiz_question> Quiz_questions { get; set; }
            public DbSet<User_quiz_result> user_quiz_results { get; set; }
            public DbSet<Video> Video { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Quiz>()
                    .HasOne(q => q.Subjects)
                    .WithMany(s => s.Quiz)
                    .HasForeignKey(q => q.Subject_id);

                modelBuilder.Entity<Video>()
                    .HasOne(v => v.Subjects)
                    .WithMany(s => s.Video)
                    .HasForeignKey(v => v.Subject_id);

                modelBuilder.Entity<Quiz_question>()
                    .HasOne(qq => qq.Quiz)
                    .WithMany(q => q.Questions)
                    .HasForeignKey(qq => qq.quiz_id);

                modelBuilder.Entity<User_quiz_result>()
                    .HasOne(uqr => uqr.Quiz)
                    .WithMany(q => q.quiz_Results)
                    .HasForeignKey(uqr => uqr.quiz_id);
        }
    }
}
