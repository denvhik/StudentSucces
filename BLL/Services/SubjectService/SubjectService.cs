using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
namespace BLL.Services.SubjectService;

public class SubjectService
{
    private readonly ILogger<SubjectService> _logger;
    private readonly IMapper _mapper;
    private readonly GenericRepository<Subject> _genericRepository;

    public SubjectService(GenericRepository<Subject> genericRepository, IMapper mapper,  ILogger<SubjectService> logger)
    {
        _mapper = mapper;
        _genericRepository = genericRepository;
        _logger = logger;
    }
    public async Task AddSubjectAsync(Subject subjectDto)
    {
        try
        {
            //_logger.LogInformation("start method AddStudentAsync");
            var model = _mapper.Map<Subject>(subjectDto);
            //_logger.LogInformation($"Student: {model}");
            await _genericRepository.AddAsync(model);
            //_logger.LogInformation($"Model:{model}");
            await _genericRepository.SaveChangesAsync();
        }
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
        }
    }

    public async Task<bool> DeleteSubjectAsync(int id)
    {
        try
        {
            //_logger.LogInformation($"start method DeleteHobbieAsync");
            await _genericRepository.DeleteByIdAsync(id);
            //_logger.LogInformation($"Deleted {id}");
            await _genericRepository.SaveChangesAsync();
            return true;
        }
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
        }
    }

    public async Task<IEnumerable<SubjectDTO>> GetSubjectAsync()
    {
        try
        {
            _logger.LogInformation("Початок методу GetStudentAsync");
            var Subject = await _genericRepository.GetAllAsync();
            _logger.LogInformation($"Student: {Subject}");
            var SubjectDTO = _mapper.Map<IEnumerable<SubjectDTO>>(Subject);
            _logger.LogInformation($"Student: {SubjectDTO}");
            return SubjectDTO;
        }
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
        }
    }

    public async Task UpgradeSubjectAsync(int id, SubjectDTO subjectDTO)
    {
        try
        {
            _logger.LogInformation("start method UpgradeStudentAsync");
            var SubjectById = await _genericRepository.GetByIdAsync(id);
            _logger.LogInformation($"{SubjectById}");
            if (SubjectById == null) return;
            subjectDTO.Id = SubjectById.SubjectId;
            SubjectById.SubjectName= subjectDTO.SubjectName;
            await _genericRepository.UpdateAsync(SubjectById);
            _logger.LogInformation($"{SubjectById}");

        }
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
        }
    }
}
