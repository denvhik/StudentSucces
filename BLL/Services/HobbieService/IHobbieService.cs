using BLL.StudentDto;
namespace BLL.Services.HobbieService;
public interface IHobbieService
{
    Task<IEnumerable<HobbieDTO>> GetHobbieAsync();
    Task AddHobbieAsync(HobbieDTO student);
    Task UpgradeHobbieAsync(int id, HobbieDTO studentDTO);
    Task<bool> DeleteHobbieAsync(int id);
}
