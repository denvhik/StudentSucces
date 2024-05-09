using BLL.StudentDto;
namespace BLL.Services.HobbieService;
public interface IHobbieService
{
    Task<List<HobbyDTO>> GetHobbieAsync();
    Task AddHobbieAsync(HobbyDTO hobbie);
    Task UpgradeHobbieAsync( HobbyDTO hobbieDTO);
    Task<bool> DeleteHobbieAsync(int id);
}
