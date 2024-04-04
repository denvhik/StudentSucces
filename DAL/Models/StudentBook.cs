namespace DAL.Models;
public partial class StudentBook
{
    public int StudentId { get; set; }

    public int BookId { get; set; }

    public decimal PriceCheck { get; set; }

    public DateTime? CheckStartDate { get; set; }

    public DateTime? CheckEndDate { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual Book Book { get; set; }

    public virtual Student Student { get; set; } 
}