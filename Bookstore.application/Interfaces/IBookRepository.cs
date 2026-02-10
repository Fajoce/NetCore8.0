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
    public interface IBookRepository
    {
        Task<Result> AddAsync(CreateBookDto book);

        Task<Result<GetBooksDto>> GetById(int id);

        Task<Result<PagedResult<GetBooksDto>>> GetAll(PaginationParams pagination);
    }
}
