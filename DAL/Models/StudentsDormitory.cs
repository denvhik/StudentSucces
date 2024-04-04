namespace DAL.Models;
public partial class StudentsDormitory
{
    public int StudentDormitoryId { get; set; }

    public int StudentId { get; set; }

    public int DormitoryId { get; set; }

    public DateTime? CheckStartDate { get; set; }

    public DateTime? CheckEndDate { get; set; }

    public virtual Dormitory Dormitory { get; set; } 

    public virtual Student Student { get; set; } 
}
