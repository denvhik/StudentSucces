using BLL.StudentDto;


namespace BLL.Services.StudentBookService
{
    public interface IStudentBookDetails
    {
        Task<List<BllStudentBookDTO>> StudentBookDetails();
    }
}
