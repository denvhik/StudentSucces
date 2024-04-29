
namespace BLL.StudentDto;

public class StudentDebtDTO
{
    public decimal Amount { get; set; }

    public DateTime DebtDate { get; set; }

    public bool Paid { get; set; }

    public DateTime? PaymentDate { get; set; }
}
