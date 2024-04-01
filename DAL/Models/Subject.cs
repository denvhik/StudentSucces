using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; }

    public int TeacherId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
