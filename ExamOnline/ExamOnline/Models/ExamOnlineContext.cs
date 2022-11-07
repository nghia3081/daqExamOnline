using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamOnline.Models
{
    public partial class ExamOnlineContext : DbContext
    {
        public ExamOnlineContext()
        {
        }

        public ExamOnlineContext(DbContextOptions<ExamOnlineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Score> Scores { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbconnection = new AppSetting().GetDbConnection();
                optionsBuilder.UseSqlServer(dbconnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__account__AB6E6165FEA32A0D");

                entity.ToTable("account");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .HasColumnName("email");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__account__role_id__38996AB5");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("answer");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__answer__question__46E78A0C");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("exam");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("question");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK__question__exam_i__440B1D61");

                entity.HasMany(d => d.AnswersNavigation)
                    .WithMany(p => p.Questions)
                    .UsingEntity<Dictionary<string, object>>(
                        "QuestionAnswer",
                        l => l.HasOne<Answer>().WithMany().HasForeignKey("AnswerId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__question___answe__4AB81AF0"),
                        r => r.HasOne<Question>().WithMany().HasForeignKey("QuestionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__question___quest__49C3F6B7"),
                        j =>
                        {
                            j.HasKey("QuestionId", "AnswerId").HasName("PK__question__BDF531780F817AF6");

                            j.ToTable("question_answer");

                            j.IndexerProperty<Guid>("QuestionId").HasColumnName("question_id");

                            j.IndexerProperty<Guid>("AnswerId").HasColumnName("answer_id");
                        });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.HasKey(e => new { e.Student, e.ExamId })
                    .HasName("PK__score__63D82DC650419A15");

                entity.ToTable("score");

                entity.Property(e => e.Student)
                    .HasMaxLength(250)
                    .HasColumnName("student");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.CorrectTotal).HasColumnName("correct_total");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__score__exam_id__4E88ABD4");

                entity.HasOne(d => d.StudentNavigation)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.Student)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__score__student__4D94879B");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("subject");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.TeacherEmail)
                    .HasMaxLength(250)
                    .HasColumnName("teacher");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.TeacherEmail)
                    .HasConstraintName("FK__subject__teacher__3B75D760");

                entity.HasMany(d => d.Exams)
                    .WithMany(p => p.Subjects)
                    .UsingEntity<Dictionary<string, object>>(
                        "SubjectExam",
                        l => l.HasOne<Exam>().WithMany().HasForeignKey("ExamId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__subject_e__exam___412EB0B6"),
                        r => r.HasOne<Subject>().WithMany().HasForeignKey("SubjectId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__subject_e__subje__403A8C7D"),
                        j =>
                        {
                            j.HasKey("SubjectId", "ExamId").HasName("PK__subject___D9CC31DEBEF8D522");

                            j.ToTable("subject_exam");

                            j.IndexerProperty<string>("SubjectId").HasMaxLength(10).HasColumnName("subject_id");

                            j.IndexerProperty<Guid>("ExamId").HasColumnName("exam_id");
                        });

                entity.HasMany(d => d.Students)
                    .WithMany(p => p.SubjectsOfStudent)
                    .UsingEntity<Dictionary<string, object>>(
                        "SubjectStudent",
                        l => l.HasOne<Account>().WithMany().HasForeignKey("Student").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__subject_s__stude__628FA481"),
                        r => r.HasOne<Subject>().WithMany().HasForeignKey("SubjectId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__subject_s__subje__619B8048"),
                        j =>
                        {
                            j.HasKey("SubjectId", "Student").HasName("PK__subject___CEA5F8C7011E5500");

                            j.ToTable("subject_student");

                            j.IndexerProperty<string>("SubjectId").HasMaxLength(10).HasColumnName("subject_id");

                            j.IndexerProperty<string>("Student").HasMaxLength(250).HasColumnName("student");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
