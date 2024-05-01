
namespace DAL.Repository.StudentSortingRepository;
public interface IStudentSortingRepository
{
    public Task<PagedEntityResults> GetSortingEntity(string term, string sort, int page, int limit);
}
