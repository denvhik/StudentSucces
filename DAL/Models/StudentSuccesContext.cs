using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Models;

public partial class StudentSuccesContext : DbContext
{
    private readonly IConfiguration _configuration;


    public StudentSuccesContext(DbContextOptions<StudentSuccesContext> options,IConfiguration configuration )
        : base(options)
    {

        _configuration = configuration;
    }

    public virtual DbSet<BookCopy> BookCopies { get; set; }
    public virtual DbSet<StudentDebt> StudentDebts { get; set; }
    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Dormitory> Dormitories { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupEnrollment> GroupEnrollments { get; set; }

    public virtual DbSet<Hobbie> Hobbies { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentBook> StudentBooks { get; set; }

    public virtual DbSet<StudentGroup> StudentGroups { get; set; }

    public virtual DbSet<StudentHobby> StudentHobbies { get; set; }

    public virtual DbSet<StudentScholarshipAudit> StudentScholarshipAudits { get; set; }

    public virtual DbSet<StudentSubject> StudentSubjects { get; set; }

    public virtual DbSet<StudentsDormitory> StudentsDormitories { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<VwStudentGroupView> VwStudentGroupViews { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:StudentConnections"]);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK_studentSucces_BookID");

            entity
                .ToTable("Book")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("BooksHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK_BookCopies_BookID");

            entity.Property(e => e.BookId)
                .ValueGeneratedNever()
                .HasColumnName("BookID");
            entity.Property(e => e.NumberOfCopies).HasDefaultValue(20);

            entity.HasOne(d => d.Book).WithOne(p => p.BookCopy)
                .HasForeignKey<BookCopy>(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookCopies_BookID");
        });
        modelBuilder.Entity<StudentDebt>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK_StudentDebt_StudentID");

            entity
                .ToTable("StudentDebt")
                .ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.UseHistoryTable("StudentDebtHistory", "dbo");
                    ttb
                        .HasPeriodStart("SysStartTime")
                        .HasColumnName("SysStartTime");
                    ttb
                        .HasPeriodEnd("SysEndTime")
                        .HasColumnName("SysEndTime");
                }));

            entity.Property(e => e.StudentId)
                .ValueGeneratedNever()
                .HasColumnName("StudentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DebtDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithOne(p => p.StudentDebt)
                .HasForeignKey<StudentDebt>(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentDebt_StudentID");
        });

        modelBuilder.Entity<Dormitory>(entity =>
        {
            entity.HasKey(e => e.DormitoryId).HasName("PK_studentSucces_DormitoryID");

            entity
                .ToTable("Dormitory")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("DormitoryHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.DormitoryId).HasColumnName("DormitoryID");
            entity.Property(e => e.Capacity).HasDefaultValue(3);
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DormitoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK_studentSucces_GroupID");

            entity.ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("GroupsHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<GroupEnrollment>(entity =>
        {
            entity.HasKey(e => e.GroupEnrollmentId).HasName("PK_StudentSucces_GroupEnrolmentID");

            entity
                .ToTable("GroupEnrollment")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("GroupEnrollmentHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.HasIndex(e => new { e.EnrollmentStartDate, e.EnrollmentEndDate }, "IX_EnrollmentStartDate_EnrollmentEndDate");

            entity.Property(e => e.GroupEnrollmentId).HasColumnName("GroupEnrollmentID");
            entity.Property(e => e.EnrollmentEndDate).HasColumnType("datetime");
            entity.Property(e => e.EnrollmentStartDate).HasColumnType("datetime");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupEnrollments)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupEnrollment_GroupID");

            entity.HasOne(d => d.Student).WithMany(p => p.GroupEnrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupEnrollment_StudentID");
        });

        modelBuilder.Entity<Hobbie>(entity =>
        {
            entity.HasKey(e => e.HobbyId).HasName("PK_studentSucces_HobbyID");

            entity
                .ToTable("Hobbie")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("HobbiesHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.HobbyId).HasColumnName("HobbyID");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HobbyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK_StudentSucces_StudentsID");

            entity
                .ToTable("Student", tb => tb.HasTrigger("tr_Student_StudentAuditTime"))
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StudentsHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BirthPlace)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StudentBook>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.BookId }).HasName("PK_StudentSucces_StudentBookID");

            entity
                .ToTable("StudentBook", tb => tb.HasTrigger("CheckBorrowedBooksAmount"))
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StudentBookHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.HasIndex(e => new { e.StudentId, e.BookId }, "IX_StudentBook_StudentID_BookID");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.CheckEndDate).HasColumnType("datetime");
            entity.Property(e => e.CheckStartDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.StudentBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_BookID");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentBooks)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_StudentBookID");
        });

        modelBuilder.Entity<StudentGroup>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.GroupId }).HasName("PK_StudentSucces_StudentGroupid");

            entity
                .ToTable("StudentGroup", tb => tb.HasTrigger("tr_Student_StudentGroupAudit"))
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StudentGroupHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_GroupId");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_StudentID");
        });

        modelBuilder.Entity<StudentHobby>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.HobbyId }).HasName("PK_StudentSucces_StudentHobbyID");

            entity
                .ToTable("StudentHobby")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StudentHobbyHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.HobbyId).HasColumnName("HobbyID");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Hobby).WithMany(p => p.StudentHobbies)
                .HasForeignKey(d => d.HobbyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_HobbyID");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentHobbies)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_StudentHobbyID");
        });

        modelBuilder.Entity<StudentScholarshipAudit>(entity =>
        {
            entity.HasKey(e => e.StudentAuditId);

            entity
                .ToTable("StudentScholarshipAudit")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("ScholarshipAuditHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.StudentAuditId).HasColumnName("StudentAuditID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AuditDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentScholarshipAudits)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_StudentID");
        });

        modelBuilder.Entity<StudentSubject>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.SubjectId }).HasName("PK_StudentSucces_StudentSubjectID");

            entity
                .ToTable("StudentSubject")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StudentSubjectHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentSubjects)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_StudenSubjecttID");

            entity.HasOne(d => d.Subject).WithMany(p => p.StudentSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSucces_SubjectID");
        });

        modelBuilder.Entity<StudentsDormitory>(entity =>
        {
            entity.HasKey(e => e.StudentDormitoryId).HasName("PC_StudentSucces_StudentDormitoryID");

            entity
                .ToTable("StudentsDormitory", tb => tb.HasTrigger("CheckRoomCapacity"))
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StudentsDormitoryHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.StudentDormitoryId).HasColumnName("StudentDormitoryID");
            entity.Property(e => e.CheckEndDate).HasColumnType("datetime");
            entity.Property(e => e.CheckStartDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DormitoryId).HasColumnName("DormitoryID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Dormitory).WithMany(p => p.StudentsDormitories)
                .HasForeignKey(d => d.DormitoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentDormitory_DormitoryID");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentsDormitories)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentDormitory_StudentID");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK_studentSucces_SubjectID");

            entity
                .ToTable("Subject")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("SubjectsHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK_studentSucces_TeachersID");

            entity
                .ToTable("Teacher")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("TeachersHistory", "dbo");
                        ttb
                            .HasPeriodStart("SysStartTime")
                            .HasColumnName("SysStartTime");
                        ttb
                            .HasPeriodEnd("SysEndTime")
                            .HasColumnName("SysEndTime");
                    }));

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.CreatedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasDefaultValueSql("(0x00)");
            entity.Property(e => e.ModifiedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TeacherName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.Subjects).WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "TeacherSubject",
                    r => r.HasOne<Subject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TeacherSubject_SubjectID"),
                    l => l.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TeacherSubject_TeacherID"),
                    j =>
                    {
                        j.HasKey("TeacherId", "SubjectId");
                        j
                            .ToTable("TeacherSubject")
                            .ToTable(tb => tb.IsTemporal(ttb =>
                                {
                                    ttb.UseHistoryTable("TeacherSubjectHistory", "dbo");
                                    ttb
                                        .HasPeriodStart("SysStartTime")
                                        .HasColumnName("SysStartTime");
                                    ttb
                                        .HasPeriodEnd("SysEndTime")
                                        .HasColumnName("SysEndTime");
                                }));
                        j.IndexerProperty<int>("TeacherId").HasColumnName("TeacherID");
                        j.IndexerProperty<int>("SubjectId").HasColumnName("SubjectID");
                    });
        });

        modelBuilder.Entity<VwStudentGroupView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_StudentGroupView");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
