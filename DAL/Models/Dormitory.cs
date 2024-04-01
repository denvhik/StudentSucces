using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Dormitory
{
    public int DormitoryId { get; set; }

    public string DormitoryName { get; set; }

    public int RoomNumber { get; set; }

    public int? Capacity { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<StudentsDormitory> StudentsDormitories { get; set; } = new List<StudentsDormitory>();
}
