using AutoMapper;
using BLL.StudentDto;
using DAL.BookDetailsDTO;
using DAL.Models;
using DAL.Repository.StudentSortingRepository;
namespace BLL.MappingProfiles;
public class StudentApiProfile:Profile
{
    public StudentApiProfile()
    {
        CreateMap<StudentDTO,Student>().ForMember(p=>p.StudentId,x=>x.MapFrom(src=>src.Id)).ReverseMap();
        CreateMap<GroupDTO, Group>().ForMember(p => p.GroupName,x=>x.MapFrom(src=>src.GroupName)).ReverseMap();
        CreateMap<SubjectDTO, Subject>().ReverseMap();
        CreateMap<StudentBookDTO, Book>().ReverseMap();
        CreateMap<StudentDebtDTO, StudentDebt>().ReverseMap();
        CreateMap<HobbieDTO, Hobbie>().ReverseMap();
        CreateMap<BllStudentBookDTO,DalBookDetailsDto>().ReverseMap();
        CreateMap<Student, StudentsJoinedEntetiesDTO>()
         .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.StudentGroups.Select(sg => new GroupDTO
         {
             GroupName = sg.Group.GroupName
         })))
         .ForMember(dest => dest.Hobbies, opt => opt.MapFrom(src => src.StudentHobbies.Select(sh => new HobbieDTO
         {
             HobbyName = sh.Hobby.HobbyName
         })))
         .ForMember(dest=>dest.StudentDebts,opt =>opt.MapFrom(src=>src.StudentDebt == null ? null:new StudentDebtDTO
         {
             DebtDate = src.StudentDebt.DebtDate,
             Amount = src.StudentDebt.Amount,
             Paid = src.StudentDebt.Paid,
             PaymentDate= src.StudentDebt.PaymentDate
         }))
         .ForMember(dest=>dest.Books,opt=>opt.MapFrom(src=>src.StudentBooks.Select(sh=>new StudentBookDTO
         {
           Author = sh.Book.Author,
           Genre = sh.Book.Genre,
           Price = sh.Book.Price,
           Title = sh.Book.Title,
         })));
        CreateMap<PagedEntityResults, StudentSortingDTO>()
           .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
           .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
        CreateMap<UserDTO,UserEntity>().ReverseMap();
    }
}
