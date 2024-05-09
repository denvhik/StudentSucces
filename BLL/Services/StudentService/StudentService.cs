using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;
using DAL.Repository.StudentSortingRepository;
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
    private readonly ICallStoredProcedureRepository _callStoredProcedureRepository;
    private readonly IStudentSortingRepository _studentSortingRepository;
    private readonly IMemoryCache _memoryCache;
    public StudentService(IGenericRepository<Student> genericRepository, IMapper mapper,
        ICallStoredProcedureRepository callStoredProcedureRepository,
        ILogger<StudentService> logger,
        IGenericRepository<StudentBook> studentBookRepository, IMemoryCache memoryCache, IStudentSortingRepository studentSortingRepository)
    {
        _callStoredProcedureRepository = callStoredProcedureRepository;
        _mapper = mapper;
        _genericRepository = genericRepository;
        _logger = logger;
        _memoryCache = memoryCache;
        _studentSortingRepository = studentSortingRepository;
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
    /// <summary>
    /// Retrieves a list of students from the cache, or from the data source if not present in the cache.
    /// </summary>
    /// <returns>A list of StudentDTO objects.</returns>
    /// <exception cref="UserFriendlyException">An exception that is thrown if there is an error during the database query or data mapping.</exception>
    /// <remarks>
    /// This method first attempts to retrieve the data from the cache. If the data is not available in the cache,
    /// it then fetches the data from the database. The fetched data is then mapped to a list of StudentDTO objects.
    /// If successful, this list is returned; otherwise, a UserFriendlyException is thrown detailing the error.
    /// The cache expiration is set to 10 minutes after which it will need to refresh from the data source.
    /// This method includes logging both when data is fetched from the cache and when it is retrieved from the data source.
    /// </remarks>
    public async Task<List<StudentDTO>> GetStudentAsync()
    {
        try
        {
            var cacheKey = "studentsCacheKey";
            if (!_memoryCache.TryGetValue(cacheKey, out List<StudentDTO> studentDTO))
            {
                _logger.LogInformation("Data not found in cache, fetch from data source");//
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
                _logger.LogInformation("Data found in cache");
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
    /// <summary>
    /// Retrieves the top score results based on the given score, either from the cache or by calling a stored procedure if not available in cache.
    /// </summary>
    /// <param name="score">The score threshold used to fetch top scores.</param>
    /// <returns>An IEnumerable of TopScoreResultDTO representing the top scores above the specified threshold.</returns>
    /// <exception cref="UserFriendlyException">An exception that is thrown if there is an error during the execution of the stored procedure or while accessing the cache.</exception>
    /// <remarks>
    /// This method checks if the top scores for a given score threshold are already available in the cache.
    /// If the data is not cached, it executes a stored procedure via the repository to retrieve these scores and then caches the result with a sliding expiration of 5 minutes.
    /// This means that the cache entry will expire if it is not accessed within 5 minutes, helping to ensure that the data is not stale.
    /// Logs are generated at the start of the method, when the data is fetched from the database, and when the method execution ends.
    /// </remarks>
    public async Task <IEnumerable<TopScoreResultDTO>> CallGetTopScoresProcedureAsync(int score)
    {
        try
        {
            _logger.LogInformation("start method CallGetTopScoresProcedureAsync");
            var cacheKey = $"TopScores-{score}";
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<TopScoreResultDTO> result))
            {
                result = await _callStoredProcedureRepository.CallGetTopScoresProcedureAsync(score);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Кешування на 5 хвилин
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            }
            _logger.LogInformation($"{score} ({result})");
            _logger.LogInformation("end method CallGetTopScoresProcedureAsync");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task<string> CallInsertStudentsDormitoryProcedureAsync(List<int>studentId, int? dormitoryId)
    {
        try
        {
            _logger.LogInformation("start method CallInsertStudentsDormitoryProcedureAsync");
           var result =   await _callStoredProcedureRepository.CallInsertStudentsDormitoryProcedureAsync(studentId, dormitoryId);
            _logger.LogInformation("end method  CallInsertStudentsDormitoryProcedureAsync");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new UserFriendlyException(ex.Message, ex);
        }
        
    }
    /// <summary>
    /// Asynchronously retrieves a list of overdue books by calling a stored procedure.
    /// </summary>
    /// <returns>An IEnumerable of OverdueBookReportDTO representing the list of overdue books.</returns>
    /// <exception cref="UserFriendlyException">An exception that is thrown if there is an error during the execution of the stored procedure or any other part of the data retrieval process.</exception>
    /// <remarks>
    /// This method executes a stored procedure to fetch details about books that are overdue. It is designed to handle potentially complex queries that are abstracted away by the stored procedure.
    /// The method starts by logging the initiation of the process, then executes the stored procedure, and finally logs the completion before returning the results.
    /// If an exception occurs during the execution, it logs the error and throws a UserFriendlyException to ensure that the caller can understand and handle the error appropriately.
    /// This method provides a critical functionality for library management systems, especially in identifying books that need to be returned or tracked due to overdue status.
    /// </remarks>
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
    public async Task<StudentDTO> GetStudentByIdAsync(int studentid)
    {
        var studentbyid = await _genericRepository.GetByIdAsync(studentid);
        var studentDTO = _mapper.Map<StudentDTO>(studentbyid);
        return studentDTO;
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
    /// <summary>
    /// Retrieves a paginated list of students either from the cache or from the data source if not available in cache.
    /// </summary>
    /// <param name="pageIndex">The index of the page of data to retrieve.</param>
    /// <param name="pageSize">The number of records to retrieve per page.</param>
    /// <returns>A List of StudentDTO objects that represents a paginated list of students.</returns>
    /// <exception cref="UserFriendlyException">An exception that is thrown if there is an error during the data fetching process from the database or during the mapping process.</exception>
    /// <remarks>
    /// This method first checks if the data for the specified page index and page size is available in the cache.
    /// If the data is not found in the cache, it fetches the data from the repository using the specified parameters
    /// for pagination and then maps the result to a list of StudentDTO objects. The fetched data is then cached
    /// with an absolute expiration time set to 10 minutes.
    /// If the data is found in the cache, it is returned directly from the cache.
    /// Logs are generated to indicate whether data was fetched from the cache or the data source.
    /// This method helps in reducing database load by caching paginated results and improves response time for fetching data that is accessed frequently.
    /// </remarks>
    public async Task<List<StudentDTO>> GetStudentByParametrAsync(int pageindex, int pagesize)
    {
        try
        {
            var cacheKey = "studentsCacheKey";
            if (!_memoryCache.TryGetValue(cacheKey, out List<StudentDTO> studentDTO))
            {
                _logger.LogInformation("Data not found in cache, fetch from data source");
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
                _logger.LogInformation("Data  found in cache");
            }

            return studentDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }

    public async Task<StudentSortingDTO> GetSortingEntity(string term, string sort, int page, int limit)
    {
        try
        {
            var studentmodel = await _studentSortingRepository.GetSortingEntity(term, sort, page, limit);
            var mapedmodel = _mapper.Map<StudentSortingDTO>(studentmodel);
            return mapedmodel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw new UserFriendlyException(ex.Message, ex);
        }
    }
}
