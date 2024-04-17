using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.StudentService;
using BLL.StudentDto;
using Newtonsoft.Json;
using System.Collections;

namespace StudentMVC.Controllers;

[Route("[controller]/[action]")]
public class StudentController : Controller
{
    private IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async   Task<IActionResult> Index()
    {
        //const int pageSize = 5;
        //if (pg < 1) pg = 1;
        //int recsCount = students.Count();
        //Pager pager = new Pager(recsCount, pg, pageSize);
        //int reskip = (pg - 1) * pageSize;
        //var data = students.Skip(reskip).Take(pager.PageSize).ToList();
        //ViewBag.Pager = pager;
        
            return View();
 
       
    }
    [HttpGet]

    public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
    {
        var students = await _studentService.GetStudentAsync();
        return Json(DataSourceLoader.Load(students, loadOptions));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Post(string values)
    {
        try
        {
            var valuesDict = JsonConvert.DeserializeObject<StudentDTO>(values);
            if (!TryValidateModel(valuesDict))
                return BadRequest(ModelState);
            if (ModelState.IsValid)
            {
                await _studentService.AddStudentAsync(valuesDict);
            }
            return Json(new { success = true, message = $"Student Added successfully" });
        }
        catch (UserFriendlyException ex) 
        {
            return Json(new { success = false,message = $"Failed  add student. Error: {ex.Message}" });
        }
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Put(int key,string values)
    {
        try
        {
            var model = await _studentService.GetByIdAsync(key);
            if (model == null)
                return StatusCode(409, "Object not found");
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            if (ModelState.IsValid)
            {
                PopulateModel(model, valuesDict);
                await _studentService.UpgradeStudentAsync(model);
            }
            return Json(new {success=true, message = $"student updated succes" });
        } 
        catch (UserFriendlyException ex) 
        {
            return Json(new { success = false, message = $"Failed to update student. Error: {ex.Message}" });
        }
        catch 
        {
            return BadRequest(ModelState);
        }   
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int key)
    {
        try 
        { 
        
                var deleted = await _studentService.DeleteStudentAsync(key);
                return Json(new { success = true, message = "Student delete successfully." });
        }
        catch (UserFriendlyException ex)
        {
            return Json(new { success = false, message = $"Failed to delete student. Error: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Failed to delete student. Error: {ex.Message}" });
        }


    }


    

    //    [HttpDelete]
    //    public async Task Delete(int key)
    //    {
    //        var model = await _context.Students.FirstOrDefaultAsync(item => item.StudentId == key);

    //        _context.Students.Remove(model);
    //        await _context.SaveChangesAsync();
    //    }


    private void PopulateModel(StudentDTO model, IDictionary values)
    {
        string STUDENT_ID = nameof(StudentDTO.Id);
        string FIRST_NAME = nameof(StudentDTO.FirstName);
        string LAST_NAME = nameof(StudentDTO.LastName);
        string MIDDLE_NAME = nameof(StudentDTO.MiddleName);
        string TICKET_NUMBER = nameof(StudentDTO.TicketNumber);
        string BIRTH_YEAR = nameof(StudentDTO.BirthYear);
        string BIRTH_PLACE = nameof(StudentDTO.BirthPlace);
        string ADDRESS = nameof(StudentDTO.Address);
        string GENDER = nameof(StudentDTO.Gender);
        string MARITAL_STATUS = nameof(StudentDTO.MaritalStatus);
        string GMAIL = nameof(StudentDTO.Gmail);

        if (values.Contains(STUDENT_ID))
        {
            model.Id = Convert.ToInt32(values[STUDENT_ID]);
        }

        if (values.Contains(FIRST_NAME))
        {
            model.FirstName = Convert.ToString(values[FIRST_NAME]);
        }

        if (values.Contains(LAST_NAME))
        {
            model.LastName = Convert.ToString(values[LAST_NAME]);
        }

        if (values.Contains(MIDDLE_NAME))
        {
            model.MiddleName = Convert.ToString(values[MIDDLE_NAME]);
        }

        if (values.Contains(TICKET_NUMBER))
        {
            model.TicketNumber = Convert.ToString(values[TICKET_NUMBER]);
        }

        if (values.Contains(BIRTH_YEAR))
        {
            model.BirthYear = Convert.ToInt32(values[BIRTH_YEAR]);
        }

        if (values.Contains(BIRTH_PLACE))
        {
            model.BirthPlace = Convert.ToString(values[BIRTH_PLACE]);
        }

        if (values.Contains(ADDRESS))
        {
            model.Address = Convert.ToString(values[ADDRESS]);
        }

        if (values.Contains(GENDER))
        {
            model.Gender = Convert.ToString(values[GENDER]);
        }

        if (values.Contains(MARITAL_STATUS))
        {
            model.MaritalStatus = Convert.ToString(values[MARITAL_STATUS]);
        }
        if (values.Contains(GMAIL))
        {
            model.Gmail = Convert.ToString(values[GMAIL]);
        }
    }

    //    private T ConvertTo<T>(object value)
    //    {
    //        var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
    //        if (converter != null)
    //        {
    //            return (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
    //        }
    //        else
    //        {
    //            If necessary, implement a type conversion here
    //            throw new NotImplementedException();
    //        }
    //    }

    //    private string GetFullErrorMessage(ModelStateDictionary modelState)
    //    {
    //        var messages = new List<string>();

    //        foreach (var entry in modelState)
    //        {
    //            foreach (var error in entry.Value.Errors)
    //                messages.Add(error.ErrorMessage);
    //        }

    //        return String.Join(" ", messages);
    //    }
    //}
    [HttpGet]
    public async Task<IActionResult> AddressesLookup(DataSourceLoadOptions loadOptions)
    {
        var lookup = (await _studentService.GetStudentAsync()).Select(i => new
        {
            Value = i.Id,
            Text = i.Gender + ", " + i.MaritalStatus
        }).OrderBy(i => i.Text).AsQueryable();
        return Json(DataSourceLoader.Load(lookup, loadOptions));
    }

    [HttpGet]
    public IActionResult GenderLookup(DataSourceLoadOptions loadOptions)
    {//todochange in database with field collection 
        var genders = new List<object>
        {
        new { Value = "male", Text = "Male" },
        new { Value = "female", Text = "Female" },
        new { Value = "not specified", Text = "Not Specified" }
         };
        return Json(genders);
    }
    [HttpGet]
    public IActionResult Maritalstatus(DataSourceLoadOptions loadOptions)
    {
        //todochange in database with field collection 
        var marital = new List<object>
    {
        new { Value = "married", Text = "Married" },
        new { Value = "No data", Text = "No data" },
       
    };
        return Json(marital);
    }
}
