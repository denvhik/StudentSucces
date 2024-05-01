
namespace DAL.StoredProcedureDTO;
public class OverdueBookReportDTO
{
    public int StudentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int NumberOfBooks { get; set; }
    public int DaysOverdue { get; set; }
    public decimal TotalDebt { get; set; }
}
