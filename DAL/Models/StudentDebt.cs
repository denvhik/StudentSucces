namespace DAL.Models;

public partial class StudentDebt
{
    public int StudentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime DebtDate { get; set; }

    public bool Paid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual Student Student { get; set; } = null!;
}
