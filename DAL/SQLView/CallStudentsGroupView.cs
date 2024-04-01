using Microsoft.EntityFrameworkCore;
using DAL.SQLViewDTO;
using DAL.Models;

namespace DAL.SQLView;

public class CallStudentsGroupView
{
    private readonly StudentSuccesContext _studentSuccesContext;
    public CallStudentsGroupView(StudentSuccesContext studentSuccesContext)
    {
        _studentSuccesContext = studentSuccesContext;
    }

    public async Task<List<StudentGroupViewDTO>> CallStudentGroupViewDataAsync()
    {
             return await _studentSuccesContext.Set<StudentGroupViewDTO>()
            .FromSqlRaw("SELECT * FROM MyView")
            .ToListAsync();
    }
}
