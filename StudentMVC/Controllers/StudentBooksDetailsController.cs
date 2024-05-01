using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;

using BLL.Services.StudentBookService;

namespace StudentMVC.Controllers;

[Route("[controller]/[action]")]
public class StudentBooksDetailsController : Controller
{
   private readonly IStudentBookDetails _studentBookDetails;
    public StudentBooksDetailsController(IStudentBookDetails studentBookDetails) {
       _studentBookDetails = studentBookDetails;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View("~/Views/Student/IndexStudentBookDetails.cshtml");
    }
    [HttpGet]
    public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _studentBookDetails.StudentBookDetails();
            return Json(DataSourceLoader.Load(result, loadOptions));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }

    //    [HttpPost]
    //    public async Task<IActionResult> Post(string values) {
    //        var model = new Book();
    //        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
    //        PopulateModel(model, valuesDict);

    //        if(!TryValidateModel(model))
    //            return BadRequest(GetFullErrorMessage(ModelState));

    //        var result = _context.Books.AddAsync(model);
    //        await _context.SaveChangesAsync();

    //        return Json(new { result.Entity.BookId });
    //    }

    //    [HttpPut]
    //    public async Task<IActionResult> Put(int key, string values) {
    //        var model = await _context.Books.FirstOrDefaultAsync(item => item.BookId == key);
    //        if(model == null)
    //            return StatusCode(409, "Object not found");

    //        var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
    //        PopulateModel(model, valuesDict);

    //        if(!TryValidateModel(model))
    //            return BadRequest(GetFullErrorMessage(ModelState));

    //        await _context.SaveChangesAsync();
    //        return Ok();
    //    }

    //    [HttpDelete]
    //    public async Task Delete(int key) {
    //        var model = await _context.Books.FirstOrDefaultAsync(item => item.BookId == key);

    //        _context.Books.Remove(model);
    //        await _context.SaveChangesAsync();
    //    }


    //    private void PopulateModel(Book model, IDictionary values) {
    //        string BOOK_ID = nameof(Book.BookId);
    //        string AUTHOR = nameof(Book.Author);
    //        string TITLE = nameof(Book.Title);
    //        string GENRE = nameof(Book.Genre);
    //        string PRICE = nameof(Book.Price);
    //        string CREATED_DATE_TIME = nameof(Book.CreatedDateTime);
    //        string CREATED_BY = nameof(Book.CreatedBy);
    //        string MODIFIED_DATE_TIME = nameof(Book.ModifiedDateTime);
    //        string MODIFIED_BY = nameof(Book.ModifiedBy);

    //        if(values.Contains(BOOK_ID)) {
    //            model.BookId = Convert.ToInt32(values[BOOK_ID]);
    //        }

    //        if(values.Contains(AUTHOR)) {
    //            model.Author = Convert.ToString(values[AUTHOR]);
    //        }

    //        if(values.Contains(TITLE)) {
    //            model.Title = Convert.ToString(values[TITLE]);
    //        }

    //        if(values.Contains(GENRE)) {
    //            model.Genre = Convert.ToString(values[GENRE]);
    //        }

    //        if(values.Contains(PRICE)) {
    //            model.Price = values[PRICE] != null ? Convert.ToDecimal(values[PRICE], CultureInfo.InvariantCulture) : (decimal?)null;
    //        }

    //        if(values.Contains(CREATED_DATE_TIME)) {
    //            model.CreatedDateTime = values[CREATED_DATE_TIME] != null ? Convert.ToDateTime(values[CREATED_DATE_TIME]) : (DateTime?)null;
    //        }

    //        if(values.Contains(CREATED_BY)) {
    //            model.CreatedBy = values[CREATED_BY] != null ? ConvertTo<System.Guid>(values[CREATED_BY]) : (Guid?)null;
    //        }

    //        if(values.Contains(MODIFIED_DATE_TIME)) {
    //            model.ModifiedDateTime = values[MODIFIED_DATE_TIME] != null ? Convert.ToDateTime(values[MODIFIED_DATE_TIME]) : (DateTime?)null;
    //        }

    //        if(values.Contains(MODIFIED_BY)) {
    //            model.ModifiedBy = values[MODIFIED_BY] != null ? ConvertTo<System.Guid>(values[MODIFIED_BY]) : (Guid?)null;
    //        }
    //    }

    //    private T ConvertTo<T>(object value) {
    //        var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
    //        if(converter != null) {
    //            return (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
    //        } else {
    //            // If necessary, implement a type conversion here
    //            throw new NotImplementedException();
    //        }
    //    }

    //    private string GetFullErrorMessage(ModelStateDictionary modelState) {
    //        var messages = new List<string>();

    //        foreach(var entry in modelState) {
    //            foreach(var error in entry.Value.Errors)
    //                messages.AddAsync(error.ErrorMessage);
    //        }

    //        return String.Join(" ", messages);
    //    }
}