using System;
using examinationPlatform.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace examinationPlatform.Models
{
    public partial class exam_platformContext : DbContext, IExam_Platform_Context
    {
        public exam_platformContext()
        {
        }

        public exam_platformContext(DbContextOptions<exam_platformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<ExamContent> ExamContent { get; set; }
        public virtual DbSet<ExamRecord> ExamRecord { get; set; }
        public virtual DbSet<ExamStorage> ExamStorage { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<TestStorage> TestStorage { get; set; }
        public virtual DbSet<UserHistory> UserHistory { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=122.51.180.247;database=exam_platform;uid=sa;pwd=Aa553144121;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("collection");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.UsersId).HasColumnName("users_id");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Collection)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK__collectio__test___267ABA7A");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Collection)
                    .HasForeignKey(d => d.UsersId)
                    .HasConstraintName("FK__collectio__users__25869641");
            });

            modelBuilder.Entity<ExamContent>(entity =>
            {  

                entity.ToTable("exam_content");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamContent)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK__exam_cont__exam___182C9B23");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.ExamContent)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK__exam_cont__test___173876EA");
            });

            modelBuilder.Entity<ExamRecord>(entity =>
            {
                entity.ToTable("exam_record");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.PublishDate)
                    .HasColumnName("publish_date")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Score)
                    .HasColumnName("score")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsersId).HasColumnName("users_id");

                entity.Property(e => e.WrongCount).HasColumnName("wrong_count");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamRecord)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK__exam_reco__exam___1ED998B2");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.ExamRecord)
                    .HasForeignKey(d => d.UsersId)
                    .HasConstraintName("FK__exam_reco__users__1DE57479");
            });

            modelBuilder.Entity<ExamStorage>(entity =>
            {
                entity.ToTable("exam_storage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Grade)
                    .HasColumnName("grade")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Group)
                    .HasColumnName("group_")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                   .HasMaxLength(30)
                   .IsUnicode(false);
                  
                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PublishTime)
                    .HasColumnName("publish_time")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Publisher).HasColumnName("publisher");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.PublisherNavigation)
                    .WithMany(p => p.ExamStorage)
                    .HasForeignKey(d => d.Publisher)
                    .HasConstraintName("FK__exam_stor__publi__15502E78");
            });

            modelBuilder.Entity<Information>(entity =>
            {
                entity.ToTable("information");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PublishDate)
                    .HasColumnName("publish_date")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Publisher).HasColumnName("publisher");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.PublisherNavigation)
                    .WithMany(p => p.Information)
                    .HasForeignKey(d => d.Publisher)
                    .HasConstraintName("FK__informati__publi__1B0907CE");
            });

            modelBuilder.Entity<TestStorage>(entity =>
            {
                entity.ToTable("test_storage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Answer)
                    .HasColumnName("answer")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.difficulty)
                  .HasColumnName("difficulty")
                  .HasMaxLength(30)
                  .IsUnicode(false);

                entity.Property(e => e.Explain)
                    .HasColumnName("explain")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Grade)
                    .HasColumnName("grade")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PublishDate)
                    .HasColumnName("publish_date")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Publisher).HasColumnName("publisher");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.PublisherNavigation)
                    .WithMany(p => p.TestStorage)
                    .HasForeignKey(d => d.Publisher)
                    .HasConstraintName("FK__test_stor__publi__1273C1CD");
            });

            modelBuilder.Entity<UserHistory>(entity =>
            {
                entity.ToTable("user_history");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Answer)
                    .HasColumnName("answer")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.State).HasColumnName("state_");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UsersId).HasColumnName("users_id");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.UserHistory)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK__user_hist__test___22AA2996");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.UserHistory)
                    .HasForeignKey(d => d.UsersId)
                    .HasConstraintName("FK__user_hist__users__21B6055D");

                entity.HasOne(d => d.Exam)
               .WithMany(p => p.UserHistory)
               .HasForeignKey(d => d.ExamId)
               .HasConstraintName("FK_user_history_exam_storage");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.College)
                    .HasColumnName("college")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImgUrl)
                    .HasColumnName("img_url")
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.class_)
                    .HasColumnName("class_")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.major)
                    .HasColumnName("major")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nickname)
                    .HasColumnName("nickname")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserAccount)
                    .HasColumnName("user_account")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserGroup).HasColumnName("user_group");

                entity.Property(e => e.UserPassword)
                    .HasColumnName("user_password")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
