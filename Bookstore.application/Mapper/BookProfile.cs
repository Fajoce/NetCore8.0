using AutoMapper;
using Bookstore.Application.DTOs;
using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.Mapper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, GetBooksDto>()
                .ForMember(dest => dest.AuthorName,
                           opt => opt.MapFrom(src => src.Author.FullName));

            CreateMap<CreateBookDto, Book>();
        }
    }
}
