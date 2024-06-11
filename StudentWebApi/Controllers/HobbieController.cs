using BLL.Services.HobbieService;
using BLL.StudentDto;
using Handling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HobbieController : ControllerBase
    {
        private readonly IHobbieService _hobbieService;

        public HobbieController( IHobbieService hobbieService)
        {
            _hobbieService = hobbieService;
        }

        [HttpGet("GetTeachers")]
        [Authorize(Roles = "Admin,Member,Menager")]
        public async Task<ActionResult<List<HobbyDTO>>> GetTeacherList()
        {
            List<HobbyDTO> hobbiesDTO = new();
            try
            {
                hobbiesDTO = await _hobbieService.GetHobbieAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return hobbiesDTO;
        }
        [HttpDelete("DeleteTeacher/{id:int}")]
        [Authorize(Roles = "Admin,Menager")]
        public async Task<ActionResult<bool>> DeleteTeacher(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException();
            }
            var student = await _hobbieService.DeleteHobbieAsync(id);
            if (student == false)
            {
                throw new KeyNotFoundException("teacher with this id was not found");
            }
            return student;
        }
        [HttpPost("Create")]
        [Authorize(Roles = "Admin,Menager")]
        public async Task<ActionResult> CreateNewTeacher([FromBody] HobbyDTO hobbiesDTO)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _hobbieService.AddHobbieAsync(hobbiesDTO);

            }
            catch (Exception)
            {
                throw new UserFriendlyException("somethinf went wrong");
            }
            return Ok("your created techer succsesfuly");
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdatedTeacher([FromBody] HobbyDTO hobbiesDTO)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _hobbieService.UpgradeHobbieAsync(hobbiesDTO);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException();
            }
            catch (Exception)
            {
                throw new SystemExeptionHandle("Something went wrong");
            }
            return Ok("Your Updated information about tacher succsesfully");
        }
    }
}
