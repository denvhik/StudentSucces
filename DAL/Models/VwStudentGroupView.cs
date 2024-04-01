using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class VwStudentGroupView
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } 

    public string LastName { get; set; } 

    public int GroupId { get; set; }

    public string GroupName { get; set; }
}
