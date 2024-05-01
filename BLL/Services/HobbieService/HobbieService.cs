using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace BLL.Services.HobbieService;
public class HobbieService : IHobbieService
{
    private readonly ILogger<HobbieService> _logger;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Hobbie> _genericRepository;
    public HobbieService(IGenericRepository<Hobbie> genericRepository, IMapper mapper, ILogger<HobbieService> logger)
    {
        _mapper = mapper;
        _genericRepository = genericRepository;
        _logger = logger;
    }
    public async Task AddHobbieAsync(HobbieDTO hobbieDTO)
    {
        try
        {
            //_logger.LogInformation("start method AddStudentAsync");
            var model = _mapper.Map<Hobbie>(hobbieDTO);
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

    public async Task<bool> DeleteHobbieAsync(int id)
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

    public async Task<IEnumerable<HobbieDTO>> GetHobbieAsync()
    {
        try
        {
            _logger.LogInformation("Початок методу GetStudentAsync");
            var Hobbies = await _genericRepository.GetAllAsync();
            _logger.LogInformation($"Student: {Hobbies}");
            var HobieDTO = _mapper.Map<IEnumerable<HobbieDTO>>(Hobbies);
            _logger.LogInformation($"Student: {HobieDTO}");
            return HobieDTO;
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

    public  async Task UpgradeHobbieAsync(int id, HobbieDTO HobbieDTO)
    {
        try
        {
            _logger.LogInformation("start method UpgradeStudentAsync");
            var HobbieById = await _genericRepository.GetByIdAsync(id);
            _logger.LogInformation($"{HobbieById}");
            if (HobbieById == null) return;
            HobbieDTO.Id = HobbieById.HobbyId;
            HobbieById.HobbyName = HobbieDTO.HobbyName;
            await _genericRepository.UpdateAsync(HobbieById);
            _logger.LogInformation($"{HobbieById}");

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
