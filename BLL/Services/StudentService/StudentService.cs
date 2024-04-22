using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;
using DAL.StoredProcedureDTO;
using DAL.StoredProcedures;
using Handling;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BLL.Services.StudentService;
public class StudentService : IStudentService
{
    private readonly ILogger<StudentService> _logger;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Student> _genericRepository;
    private readonly IGenericRepository<StudentBook> _studentBookRepository;
    private readonly ICallStoredProcedureRepository _callStoredProcedureRepository;
    private readonly UserFriendlyException _userFriendlyException;
    private readonly IMemoryCache _memoryCache;
    public StudentService(IGenericRepository<Student> genericRepository,IMapper mapper,
        ICallStoredProcedureRepository callStoredProcedureRepository, 
        ILogger<StudentService> logger,
        IGenericRepository<StudentBook> studentBookRepository, IMemoryCache memoryCache)
    {
        _callStoredProcedureRepository = callStoredProcedureRepository;
        _mapper = mapper;
        _genericRepository = genericRepository;
        _logger = logger;
        _studentBookRepository = studentBookRepository;
        _memoryCache = memoryCache;
    }

    public async Task AddStudentAsync(StudentDTO studentDto)
    {
        try
        {
            var model = _mapper.Map<Student>(studentDto);
            await _genericRepository.AddAsync(model);
            _logger.LogInformation($"Model:{model}");
            await _genericRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task <bool> DeleteStudentAsync(int id)
    {
        try
        {
            //_logger.LogInformation($"start method DeleteStudentAsync");
            await _genericRepository.DeleteByIdAsync(id);
            //_logger.LogInformation($"Deleted {id}");
            await _genericRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task<List<StudentDTO>> GetStudentAsync()
    {
        try
        {
            var cacheKey = "studentsCacheKey";
            if (!_memoryCache.TryGetValue(cacheKey, out List<StudentDTO> studentDTO))
            {
                _logger.LogInformation("Дані не знайдено в кеші, отримання з джерела даних");//
                var studentModel = await _genericRepository.GetAllAsync();
                studentDTO = _mapper.Map<List<StudentDTO>>(studentModel).ToList();
                _logger.LogInformation($"Student: {studentDTO}");
                
                _memoryCache.Set(cacheKey, studentDTO, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) 
                });
            }
            else
            {
                _logger.LogInformation("Дані знайдено в кеші");
            }

            return studentDTO;
        }
        catch (SystemExeptionHandle ex)
        {
            _logger.LogError(ex.Message, ex);
            throw new UserFriendlyException(ex.Message, ex);
        }
        
    }

    public async Task UpgradeStudentAsync( StudentDTO studentDTO)
    {
        try
        {
            _logger.LogInformation("start method UpgradeStudentAsync");
            var studentById = await _genericRepository.GetByIdAsync(studentDTO.Id);
            _logger.LogInformation($"{studentById}");
            if (studentById == null) return;

            studentById.TicketNumber = studentDTO.TicketNumber;
            studentById.FirstName = studentDTO.FirstName;
            studentById.LastName = studentDTO.LastName;
            studentById.MiddleName = studentDTO.MiddleName;
            studentById.MaritalStatus = studentDTO.MaritalStatus.ToString();
            studentById.BirthYear = studentDTO.BirthYear;
            studentById.BirthPlace = studentDTO.BirthPlace;
            studentById.Gender = studentDTO.Gender.ToString();
            studentById.Address = studentDTO.Address;
            studentById.Gmail = studentDTO.Gmail;
            await _genericRepository.UpdateAsync(studentById);
            _logger.LogInformation($"{studentById}");
      
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task CallCalculateScholarshipForAllStudentAsync(int month,int year)
    {
        try
        {
                _logger.LogInformation("start method CallCalculateScholarshipForAllStudentAsync");
                await _callStoredProcedureRepository.CallCalculateScholarshipForAllStudentAsync(month, year);
                _logger.LogInformation("end method CallCalculateScholarshipForAllStudentAsync");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }
    public  async Task <IEnumerable<TopScoreResultDTO>> CallGetTopScoresProcedureAsync(int score)
    {
        try
        {
            _logger.LogInformation("start method CallGetTopScoresProcedureAsync");
            var result = await _callStoredProcedureRepository.CallGetTopScoresProcedureAsync(score);
            _logger.LogInformation($"{score} ({result})");
            _logger.LogInformation($"end method CallGetTopScoresProcedureAsync");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task CallInsertStudentsDormitoryProcedureAsync()
    {
        try
        {
            _logger.LogInformation("start method CallInsertStudentsDormitoryProcedureAsync");
            await _callStoredProcedureRepository.CallInsertStudentsDormitoryProcedureAsync();
            _logger.LogInformation("end method  CallInsertStudentsDormitoryProcedureAsync");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task<IEnumerable<OverdueBookReportDTO>> CallOverdueBookReportAsync()
    {
        try
        {
            _logger.LogInformation("start method CallOverdueBookReportAsync");
            var result = await _callStoredProcedureRepository.CallOverdueBookReportAsync();
            _logger.LogInformation("end method CallOverdueBookReportAsync");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task <IEnumerable<StudentRatingResult>> CallSortStudentRatingAsync()
    {
        try
        {
            _logger.LogInformation("start method CallSortStudentRatingAsync");
            var result = await _callStoredProcedureRepository.CallSortStudentRatingAsync();
            _logger.LogInformation("start method CallSortStudentRatingAsync");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task <StudentDTO> GetByIdAsync(int id)
    {
        try
        {
            var student = await _genericRepository.GetByIdAsync(id);
            var result = _mapper.Map<StudentDTO>(student);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }
    public async Task<string> ReturningBook(int studentId,int bookId,DateTime EndTime)
    {
        try
        {
            var studentbook = await _callStoredProcedureRepository.CallReturnBookProcedureAsync(studentId, bookId, EndTime);
            return studentbook;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task<StudentDTO> GetStudentByIdAsync(int studentid)
    {
      var studentbyid = await _genericRepository.GetByIdAsync(studentid);
        var studentDTO = _mapper.Map<StudentDTO>(studentbyid);
        return studentDTO;
    }

    public  async Task<string> TakeBook(int studentId, int BookId)
    {
        try
        {
            var studentbook = await _callStoredProcedureRepository.CallTakeBookProcedureAsync(studentId, BookId);
            return studentbook;
        }
      
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task<List<StudentDTO>> GetStudentByParametrAsync(int pageindex, int pagesize)
    {
        try
        {
            var cacheKey = "studentsCacheKey";
            if (!_memoryCache.TryGetValue(cacheKey, out List<StudentDTO> studentDTO))
            {
                _logger.LogInformation("Дані не знайдено в кеші, отримання з джерела даних");
                var studentModel = await _genericRepository.GetPagingAsync(pageindex,pagesize);
                studentDTO = _mapper.Map<List<StudentDTO>>(studentModel).ToList();
                _logger.LogInformation($"Student: {studentDTO}");

                _memoryCache.Set(cacheKey, studentDTO, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }
            else
            {
                _logger.LogInformation("Дані знайдено в кеші");
            }

            return studentDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }
}
