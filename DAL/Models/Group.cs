namespace DAL.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } 

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<GroupEnrollment> GroupEnrollments { get; set; } = new List<GroupEnrollment>();

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}
