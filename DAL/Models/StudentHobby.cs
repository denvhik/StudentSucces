using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StudentHobby
{
    public int StudentId { get; set; }

    public int HobbyId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual Hobbie Hobby { get; set; }

    public virtual Student Student { get; set; }
}
