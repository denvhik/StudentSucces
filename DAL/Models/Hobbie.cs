using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Hobbie
{
    public int HobbyId { get; set; }

    public string HobbyName { get; set; } 

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<StudentHobby> StudentHobbies { get; set; } = new List<StudentHobby>();
}
