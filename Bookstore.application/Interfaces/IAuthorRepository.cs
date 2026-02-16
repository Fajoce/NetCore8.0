using Bookstore.Api.DTOs;
using Bookstore.Application.Common;
using Bookstore.Application.DTOs;
using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Result<PagedResult<GetAuthorsDto>>> GetAll(PaginationParams pagination);


        Task<Result<GetAuthorsDto?>> GetByIdAsync(int id);

        Task<Result> AddAsync(CreateAuthorDto author);
    }
}
