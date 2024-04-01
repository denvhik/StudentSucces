using DAL.SQLViewDTO;
namespace DAL.SQLView;

public interface ICallStudentGroupView
{
    Task<List<StudentGroupViewDTO>> CallStudentGroupViewDataAsync();
}
