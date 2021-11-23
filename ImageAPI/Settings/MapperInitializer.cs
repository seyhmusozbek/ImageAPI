using AutoMapper;
using ImageAPI.Entities;
using ImageAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAPI.Settings
{
    public class MapperInitializer:Profile
    {
        public MapperInitializer()
        {
            CreateMap<ImageDetail, ImageDetailDTO>().ReverseMap();
        }
    }
}
