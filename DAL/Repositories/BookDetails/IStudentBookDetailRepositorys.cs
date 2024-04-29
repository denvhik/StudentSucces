using DAL.BookDetailsDTO;

namespace DAL.Repository.BookDetails;
public interface IStudentBookDetailRepositorys
{
    Task<IEnumerable<DalBookDetailsDto>> BookStudentDetails();
}
