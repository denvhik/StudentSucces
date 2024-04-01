using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StudentSubject
{
    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public int Score { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual Student Student { get; set; }

    public virtual Subject Subject { get; set; }
}
