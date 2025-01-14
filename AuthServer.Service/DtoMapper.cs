﻿using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<AppUser, UserAppDto>().ReverseMap();

        }
    }
}
