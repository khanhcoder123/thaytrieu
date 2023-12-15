using Microsoft.EntityFrameworkCore;

namespace Tranning.DataDBContext
{
    public class TranningDBContext : DbContext
    {
        public TranningDBContext(DbContextOptions<TranningDBContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<TraineeCourse> TraineeCourses {  get; set; }

        public DbSet<TrainerTopic> TrainerTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TraineeCourse>()
                .HasKey(tc => new { tc.trainee_id, tc.course_id });

            modelBuilder.Entity<TraineeCourse>()
                .HasOne(tc => tc.User)
                .WithMany(t => t.TraineeCourses)
                .HasForeignKey(tc => tc.trainee_id);

            modelBuilder.Entity<TraineeCourse>()
                .HasOne(tc => tc.Course)
                .WithMany(c => c.TraineeCourses)
                .HasForeignKey(tc => tc.course_id);

            modelBuilder.Entity<TrainerTopic>()
               .HasKey(tc => new { tc.trainer_id, tc.topic_id });

            modelBuilder.Entity<TrainerTopic>()
                .HasOne(tc => tc.User)
                .WithMany(t => t.TrainerTopics)
                .HasForeignKey(tc => tc.trainer_id);

            modelBuilder.Entity<TrainerTopic>()
                .HasOne(tc => tc.Topic)
                .WithMany(c => c.TrainerTopics)
                .HasForeignKey(tc => tc.topic_id);

            base.OnModelCreating(modelBuilder);
        }





    }
}
