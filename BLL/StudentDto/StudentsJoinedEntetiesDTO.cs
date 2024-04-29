using DAL.Models;
namespace BLL.StudentDto;
public  class StudentsJoinedEntetiesDTO
{
    public int? StudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MiddleName { get; set; }

    public string TicketNumber { get; set; }

    public int BirthYear { get; set; }

    public string BirthPlace { get; set; }

    public string Address { get; set; }

    public string Gmail { get; set; }

    //public virtual ICollection<GroupEnrollment> GroupEnrollments { get; set; } = new List<GroupEnrollment>();

    public virtual List<StudentBookDTO>  Books { get; set; }
    public virtual List <StudentDebtDTO> StudentDebts{ get; set; }


    public List<GroupDTO> Groups { get; set; }
    public List<HobbieDTO> Hobbies { get; set; }
    //public virtual ICollection<StudentScholarshipAudit> StudentScholarshipAudits { get; set; } = new List<StudentScholarshipAudit>();

    //public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();

    //public virtual ICollection<StudentsDormitory> StudentsDormitories { get; set; } = new List<StudentsDormitory>();
}
