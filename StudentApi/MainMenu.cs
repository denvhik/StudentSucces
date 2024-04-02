using BLL.Services.StudentService;
using BLL.StudentDto;
using ADO_NET.ViewService;


namespace StudentApi;

public class MainMenu
{
    private readonly IStudentService _studentService;
    private readonly CallView _callView;
    public MainMenu(IStudentService studentService, CallView callView)
    {
        _studentService = studentService;
        _callView = callView;
    }

    public async Task ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Delete Student");
            Console.WriteLine("3. Get Students");
            Console.WriteLine("4. Upgrade Student");
            Console.WriteLine("5. Calculate Scholarship For All Students");
            Console.WriteLine("6. Get Top Scores Procedure");
            Console.WriteLine("7. Insert Students Dormitory Procedure");
            Console.WriteLine("8. Overdue Book Report");
            Console.WriteLine("9. Sort Student Rating");
            Console.WriteLine("10. Get Student by group");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddStudent();
                    break;
                case "2":
                    await DeleteStudent();
                    break;
                case "3":
                    await ShowStudents();
                    break;
                case "4":
                   await UpgradeStudent();
                    break;
                case "5":
                    await CalculateScholarship();
                    break;
                case "6":
                    await GetTopScores();
                    break;
                case "7":
                    await _studentService.CallInsertStudentsDormitoryProcedureAsync();
                    break;
                case "8":
                    await ShowOverdueBookReport();
                    break;
                case "9":
                    await ShowStudentRatings();
                    break;
                case "10":
                    await CallView();
                    break;
                case "0":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task ShowStudents()
    {
        var students = await _studentService.GetStudentAsync();
        Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10} {7,-10} {8,-10}", "FirstName", "LastName", "MiddleName", "TicketNumber", "BirthYear", "BirthPlace", "Address", "Gender", "MaritalStatus");
        foreach (var student in students)
        {
            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10} {7,-10} {8,-10}", student.FirstName, student.LastName, student.MiddleName, student.TicketNumber, student.BirthYear, student.BirthPlace, student.Address, student.Gender, student.MaritalStatus);
        }
    }

    private async Task CalculateScholarship()
    {
        Console.Write("Enter month: ");
        var month = int.Parse(Console.ReadLine());
        Console.Write("Enter year: ");
        var year = int.Parse(Console.ReadLine());
        if (month > 0 || month <= 12)
        {
            await _studentService.CallCalculateScholarshipForAllStudentAsync(month, year);
        }
        else
        {
            Console.WriteLine("enter month between 1-12 ");
            return;
        }
    }

    private async Task GetTopScores()
    {
        Console.Write("Enter score: ");
        var score = int.Parse(Console.ReadLine());
        var result = await _studentService.CallGetTopScoresProcedureAsync(score);
        Console.WriteLine("{0,-10} {1,-10} {2,-15}", "Score", "FirstName", "LastName");
        foreach (var student in result)
        {
            Console.WriteLine("{0,-10} {1,-10} {2,-15}", student.Score, student.FirstName, student.LastName);
        }
    }

    private async Task ShowOverdueBookReport()
    {
        var overduebookTask = await _studentService.CallOverdueBookReportAsync();
        Console.WriteLine("{0,-10} {1,-10} {2,-15} {3,-15} {4,-15}", "FirstName", "LastName", "NumberOfBooks", "DaysOverdue", "TotalDebt");
        foreach (var report in overduebookTask)
        {
            Console.WriteLine("{0,-10} {1,-10} {2,-15} {3,-15} {4,-15}", report.FirstName, report.LastName, report.NumberOfBooks, report.DaysOverdue, report.TotalDebt);
        }
    }

    private async Task ShowStudentRatings()
    {
        var studentRatings = await _studentService.CallSortStudentRatingAsync();
        Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10}", "FirstName", "LastName", "Score", "SubjectName");
        foreach (var rating in studentRatings)
        {
            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10}", rating.FirstName, rating.LastName, rating.Score, rating.SubjectName);
        }
    }

    private async Task AddStudent()
    {
        var studentDto = new StudentDTO();

        Console.Write("Enter First Name: ");
        studentDto.FirstName = Console.ReadLine();

        Console.Write("Enter Last Name: ");
        studentDto.LastName = Console.ReadLine();

        Console.Write("Enter Middle Name: ");
        studentDto.MiddleName = Console.ReadLine();

        Console.Write("Enter Ticket Number: ");
        studentDto.TicketNumber = Console.ReadLine();

        Console.Write("Enter Birth Year: ");
        studentDto.BirthYear = int.Parse(Console.ReadLine());

        Console.Write("Enter Birth Place: ");
        studentDto.BirthPlace = Console.ReadLine();

        Console.Write("Enter Address: ");
        studentDto.Address = Console.ReadLine();

        Console.Write("Enter Gender: ");
        studentDto.Gender = Console.ReadLine();

        Console.Write("Enter Marital Status: ");
        studentDto.MaritalStatus = Console.ReadLine();

        await _studentService.AddStudentAsync(studentDto);
    }

    private async Task DeleteStudent()
    {
        Console.Write("Enter student ID to delete: ");
        var idToDelete = int.Parse(Console.ReadLine());

    
        bool isDeleted = await _studentService.DeleteStudentAsync(idToDelete);

      
        if (isDeleted)
        {
            Console.WriteLine("Student deleted successfully.");
        }
        else
        {
            Console.WriteLine("Failed to delete student.");
        }
    }

    private async Task UpgradeStudent()
    {
        Console.Write("Enter student ID to upgrade: ");
        var idToUpgrade = int.Parse(Console.ReadLine());
        var student = await _studentService.GetByIdAsync(idToUpgrade);

        if (student != null)
        {
            var studentDto = new StudentDTO();

            Console.Write("Enter First Name: ");
            studentDto.FirstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            studentDto.LastName = Console.ReadLine();

            Console.Write("Enter Middle Name: ");
            studentDto.MiddleName = Console.ReadLine();

            Console.Write("Enter Ticket Number: ");
            studentDto.TicketNumber = Console.ReadLine();

            Console.Write("Enter Birth Year: ");
            studentDto.BirthYear = int.Parse(Console.ReadLine());

            Console.Write("Enter Birth Place: ");
            studentDto.BirthPlace = Console.ReadLine();

            Console.Write("Enter Address: ");
            studentDto.Address = Console.ReadLine();

            Console.Write("Enter Gender: ");
            studentDto.Gender = Console.ReadLine();

            Console.Write("Enter Marital Status: ");
            studentDto.MaritalStatus = Console.ReadLine();

            await _studentService.UpgradeStudentAsync(idToUpgrade, studentDto);
        }
        else
        {
            Console.WriteLine("Student with the provided ID not found.");
        }
    }
    private  async Task  CallView() 
    {
       await  _callView.ReadDataFromView();
    }
}
