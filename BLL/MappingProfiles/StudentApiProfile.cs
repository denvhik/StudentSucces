using AutoMapper;
using BLL.StudentDto;
using DAL.Models;


namespace BLL.MappingProfiles
{
    public class StudentApiProfile:Profile
    {
        public StudentApiProfile()
        {
            CreateMap<StudentDTO,Student>().ReverseMap();
            CreateMap<GroupDTO, Group>().ReverseMap();
            CreateMap<SubjectDTO, Subject>().ReverseMap();
            CreateMap<HobbieDTO, Hobbie>().ReverseMap();
        }
    }
}
