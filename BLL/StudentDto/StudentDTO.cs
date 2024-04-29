using System.ComponentModel.DataAnnotations;

namespace BLL.StudentDto;
public class StudentDTO
{
    public int? Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    [Required]
    [RegularExpression(@"^[A-Z]{2}\d{8}$",ErrorMessage = "Number must be in example format AA12345678")]
    public string TicketNumber { get; set; }

    [Required(ErrorMessage = "this field is required")]
    [RegularExpression(@"^[1-9]\d{3}$", ErrorMessage = "you must enter only year for example: '2003'")]
    public int BirthYear { get; set; }
    public string BirthPlace { get; set; }
    public string Address { get; set; }

    //[RegularExpression(@"^(male|female|not specified)$", ErrorMessage = "Incorrect input data")]

    public string Gender { get; set; }

    //[RegularExpression(@"^(Married|no data)$",ErrorMessage = "Incorrect input data")]
    public string MaritalStatus { get; set; }

    [Required(ErrorMessage ="this field is required")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov|ua)$", ErrorMessage = "Invalid .")]
    public string Gmail { get; set; }

}
