using Sieve.Attributes;

namespace StudentWebApi.Models;
public class StudentApiDto
{
    public int? Id { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string FirstName { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string LastName { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string MiddleName { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string TicketNumber { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public int BirthYear { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string BirthPlace { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string Address { get; set; }

    //[RegularExpression(@"^(male|female|not specified)$", ErrorMessage = "Incorrect input data")]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Gender { get; set; }

    //[RegularExpression(@"^(Married|no data)$",ErrorMessage = "Incorrect input data")]
    [Sieve(CanFilter = true, CanSort = true)]
    public string MaritalStatus { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Gmail { get; set; }
}
