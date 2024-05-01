using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace BLL.Services.TeacherService;

public class TeacherService:ITeacherService
{
    private readonly ILogger<TeacherService> _logger;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Teacher> _genericRepository;
    public TeacherService(IGenericRepository<Teacher> genericRepository, IMapper mapper, ILogger<TeacherService> logger)
    {
        _mapper = mapper;
        _genericRepository = genericRepository;
        _logger = logger;
    }
    public async Task AddTeacherAsync(TeacherDTO teacherDTO)
    {
        try
        {
            //_logger.LogInformation("start method AddTeacherAsync");
            var model = _mapper.Map<Teacher>(teacherDTO);
            //_logger.LogInformation($"Teacher: {model}");
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

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        try
        {
            //_logger.LogInformation($"start method DeleteTeacherAsync");
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

    public async Task<List<TeacherDTO>> GetTeacherAsync()
    {
        try
        {
            _logger.LogInformation("Початок методу GetTeacherAsync");
            var Teachers = await _genericRepository.GetAllAsync();
            _logger.LogInformation($"Teachers: {Teachers}");
            var TeacherDTO = _mapper.Map<List<TeacherDTO>>(Teachers);
            _logger.LogInformation($"Teachers: {TeacherDTO}");
            return TeacherDTO;
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

    public async Task UpgradeTeacherAsync(TeacherDTO teacherDTO)
    {
        try
        {
            _logger.LogInformation("start method UpgradeStudentAsync");
            var TeacherById = await _genericRepository.GetByIdAsync(teacherDTO.Id);
            _logger.LogInformation($"{TeacherById}");
            if (TeacherById == null) return;
            TeacherById.TeacherName = teacherDTO.TeacherName;
            await _genericRepository.UpdateAsync(TeacherById);
            _logger.LogInformation($"{TeacherById}");

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
