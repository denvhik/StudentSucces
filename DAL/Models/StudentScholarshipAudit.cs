using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StudentScholarshipAudit
{
    public int? StudentId { get; set; }

    public int StudentAuditId { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? AuditDate { get; set; }

    public virtual Student Student { get; set; }
}
