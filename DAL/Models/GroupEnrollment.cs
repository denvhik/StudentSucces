using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class GroupEnrollment
{
    public int GroupEnrollmentId { get; set; }

    public int GroupId { get; set; }

    public int StudentId { get; set; }

    public DateTime EnrollmentStartDate { get; set; }

    public DateTime? EnrollmentEndDate { get; set; }

    public virtual Group Group { get; set; }

    public virtual Student Student { get; set; }
}
