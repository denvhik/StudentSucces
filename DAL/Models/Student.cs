using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; } 

    public string MiddleName { get; set; }

    public string TicketNumber { get; set; } 

    public int BirthYear { get; set; }

    public string BirthPlace { get; set; } 

    public string Address { get; set; } 

    public string Gender { get; set; } 

    public string MaritalStatus { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    [Required(ErrorMessage = "field Gmail  is required")]
    [EmailAddress]
    public string Gmail { get; set; } 

    public virtual ICollection<GroupEnrollment> GroupEnrollments { get; set; } = new List<GroupEnrollment>();

    public virtual ICollection<StudentBook> StudentBooks { get; set; } = new List<StudentBook>();
    public virtual StudentDebt StudentDebt { get; set; }

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();

    public virtual ICollection<StudentHobby> StudentHobbies { get; set; } = new List<StudentHobby>();

    public virtual ICollection<StudentScholarshipAudit> StudentScholarshipAudits { get; set; } = new List<StudentScholarshipAudit>();

    public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();

    public virtual ICollection<StudentsDormitory> StudentsDormitories { get; set; } = new List<StudentsDormitory>();
    public override string ToString()
    {
        return base.ToString();
    }
}
