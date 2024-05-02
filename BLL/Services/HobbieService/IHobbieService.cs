using BLL.StudentDto;
namespace BLL.Services.HobbieService;
public interface IHobbieService
{
    Task<IEnumerable<HobbieDTO>> GetHobbieAsync();
    Task AddHobbieAsync(HobbieDTO hobbie);
    Task UpgradeHobbieAsync(int id, HobbieDTO hobbieDTO);
    Task<bool> DeleteHobbieAsync(int id);
}
