﻿using AutoMapper;
using BLL.StudentDto;
using StudentWebApi.Models;

namespace StudentWebApi.Autommaper;
public class MapperProvider:Profile
{
    public MapperProvider()
    {
        CreateMap<StudentApiDto, StudentDTO>().ForMember(p => p.Id, x => x.MapFrom(src => src.Id)).ReverseMap();
    }
}
