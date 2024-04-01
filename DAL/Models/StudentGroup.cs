using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StudentGroup
{
    public int StudentId { get; set; }

    public int GroupId { get; set; }

    public bool? Boss { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual Group Group { get; set; }

    public virtual Student Student { get; set; } 
}
