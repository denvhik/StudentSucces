namespace BLL.StudentDto;

public  class BllStudentBookDTO
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public DateTime CheckStartDate { get; set; }
    public DateTime CheckEndDate { get; set; }
}
