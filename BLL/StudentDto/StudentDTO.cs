
namespace BLL.StudentDto;

public class StudentDTO
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string TicketNumber { get; set; }
    public int BirthYear { get; set; }
    public string BirthPlace { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
    public string MaritalStatus { get; set; }
   
    //public override string ToString()
    //{
    //    return $"{FirstName,-10} {LastName,-10} {MiddleName,-10} {TicketNumber,-10} {BirthYear,-10} {BirthPlace,-10} {Address,-10} {Gender,-10} {MaritalStatus,-10}";
    //}
}
