using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Application.DTOs;
using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.Mapper
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, GetAuthorsDto>();    

            CreateMap<GetAuthorsDto, Author>();
            CreateMap<CreateAuthorDto, Author>();
            CreateMap<Author, CreateAuthorDto>();
        }
    }
}
