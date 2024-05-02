using BLL.CustomAtributes;

namespace BLL.StudentDto;
public class TeacherDTO
{
   
    public int Id { get; set; }
    [FullTeachersName(ErrorMessage = "Please enter a valid name.")]
    public string TeacherName { get; set; }
}
