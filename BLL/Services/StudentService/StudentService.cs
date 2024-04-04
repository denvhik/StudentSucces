using AutoMapper;
using BLL.StudentDto;
using DAL.Models;
using DAL.Repository.Implementation;
using DAL.StoredProcedureDTO;
using DAL.StoredProcedures;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;


namespace BLL.Services.StudentService;
public class StudentService : IStudentService
{
    private readonly ILogger<StudentService> _logger;
    private readonly IMapper _mapper;
    private readonly GenericRepository<Student> _genericRepository;
    private readonly GenericRepository<StudentBook> _studentBookRepository;
    private readonly ICallStoredProcedureRepository _callStoredProcedureRepository;
    public StudentService(GenericRepository<Student> genericRepository,IMapper mapper,ICallStoredProcedureRepository callStoredProcedureRepository, ILogger<StudentService> logger,GenericRepository<StudentBook> studentBookRepository)
    {
        _callStoredProcedureRepository = callStoredProcedureRepository;
        _mapper = mapper;
        _genericRepository = genericRepository;
        _logger = logger;
        _studentBookRepository = studentBookRepository;
    }

    public async Task AddStudentAsync(StudentDTO studentDto)
    {
        try
        {
            //_logger.LogInformation("start method AddStudentAsync");
            var model = _mapper.Map<Student>(studentDto);
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
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
        }
    }

    public async Task<IEnumerable<StudentDTO>> GetStudentAsync()
    {
        try
        {
            _logger.LogInformation("Початок методу GetStudentAsync");
            var studentModel = await _genericRepository.GetAllAsync();
            _logger.LogInformation($"Student: {studentModel}");
            var studentDTO = _mapper.Map<IEnumerable<StudentDTO>>(studentModel);
            _logger.LogInformation($"Student: {studentDTO}");
            return studentDTO;
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

    public async Task UpgradeStudentAsync(int id, StudentDTO studentDTO)
    {
        try
        {
            _logger.LogInformation("start method UpgradeStudentAsync");
            var studentById = await _genericRepository.GetByIdAsync(id);
            _logger.LogInformation($"{studentById}");
            if (studentById == null) return;
            studentDTO.Id = studentById.StudentId;
            studentById.TicketNumber = studentDTO.TicketNumber;
            studentById.FirstName = studentDTO.FirstName;
            studentById.LastName = studentDTO.LastName;
            studentById.MiddleName = studentDTO.MiddleName;
            studentById.MaritalStatus = studentDTO.MaritalStatus;
            studentById.BirthYear = studentDTO.BirthYear;
            studentById.BirthPlace = studentDTO.BirthPlace;
            studentById.Gender = studentDTO.Gender;
            studentById.Address = studentDTO.Address;
            await _genericRepository.UpdateAsync(studentById);
            _logger.LogInformation($"{studentById}");
      
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

    public async Task CallCalculateScholarshipForAllStudentAsync(int month,int year)
    {
        try
        {
            if (month > 0 || month <= 12)
            {
                _logger.LogInformation("start method CallCalculateScholarshipForAllStudentAsync");
                await _callStoredProcedureRepository.CallCalculateScholarshipForAllStudentAsync(month, year);
                _logger.LogInformation("end method CallCalculateScholarshipForAllStudentAsync");
            }
            else 
            {
                return;
            }
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
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
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
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
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
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
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
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
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
        catch (SqlException ex)
        {
            throw UserFriendlyException.FromSqlException(ex);
        }
        catch (Exception ex)
        {
            throw UserFriendlyException.FromException(ex);
        }
    }
    public async Task<bool> ReturningBook(int studentId,int bookId,DateTime EndTime)
    {
        try
        {
            var studentbook = await _studentBookRepository.GetByIdAsync(studentId);
            studentbook.BookId = bookId;
            studentbook.StudentId = studentId;
            studentbook.CheckEndDate = EndTime;
            await _studentBookRepository.UpdateAsync(studentbook);
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
}
