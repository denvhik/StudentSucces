using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;

namespace BLL.Services.StudentsDetailsService;
public class StudentsDetailsService:IStudentsDetailsService
{
    private readonly IGenericRepository<Student> _genericRepository;
    private readonly IMapper _mapper;
    public StudentsDetailsService(IGenericRepository<Student> genericRepository, IMapper mapper)
    {
        _genericRepository = genericRepository;
        _mapper = mapper;
    }
    public async Task<StudentsJoinedEntetiesDTO> GetStudentEntetyByIdAsync(int id, params string[] includes)
    {
        try 
        {
            var query =( await _genericRepository.GetEntityById(x => x.StudentId == id, includes)).FirstOrDefault();
            var result = _mapper.Map<StudentsJoinedEntetiesDTO>(query);
            return result;
        }
        catch(Exception ex) 
        {
            throw new UserFriendlyException(ex);
        }
    }
}
