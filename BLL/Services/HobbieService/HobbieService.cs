using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;
using DAL.StoredProcedures;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace BLL.Services.HobbieService;
public class HobbieService : IHobbieService
{
    private readonly ILogger<HobbieService> _logger;
    private readonly IMapper _mapper;
    private readonly GenericRepository<Hobbie> _genericRepository;
    private readonly ICallStoredProcedureRepository _callStoredProcedureRepository;
    public HobbieService(GenericRepository<Hobbie> genericRepository, IMapper mapper, ICallStoredProcedureRepository callStoredProcedureRepository, ILogger<HobbieService> logger)
    {
        _callStoredProcedureRepository = callStoredProcedureRepository;
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

    public Task<bool> DeleteHobbieAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<HobbieDTO>> GetHobbieAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpgradeHobbieAsync(int id, HobbieDTO studentDTO)
    {
        throw new NotImplementedException();
    }
}
