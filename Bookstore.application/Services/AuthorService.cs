using Bookstore.Api.DTOs;
using Bookstore.Application.Common;
using Bookstore.Application.DTOs;
using Bookstore.Application.Interfaces;
using Bookstore.Domain.Entities;
using Bookstore.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Application.Services
{
    public class AuthorService : IAuthorRepository
    {
        private readonly BookstoreDbContext _context;

        public AuthorService(BookstoreDbContext context)
        {
            _context = context;
        }

        public async Task<Result> AddAsync(CreateAuthorDto author)
        {
            var entity = new Author
            {
                FullName = author.FullName,
                BirthDate = author.BirthDate,
                City = author.City,
                Email = author.Email,

            };
            await _context.authors.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Result.Success();

        }

        public async Task<Result<PagedResult<GetAuthorsDto>>> GetAll(PaginationParams pagination)
        {
            var query = _context.authors
                .AsNoTracking()
                .Select(a => new GetAuthorsDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    BirthDate = a.BirthDate,
                    City = a.City,
                    Email = a.Email
                })
                .AsQueryable();

            var total = await query.CountAsync();

            var authors = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var result = new PagedResult<GetAuthorsDto>
            {
                Items = authors,
                TotalRecords = total,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };

            return Result<PagedResult<GetAuthorsDto>>.Success(result);
        }




        public async Task<Result<GetAuthorsDto?>> GetByIdAsync(int id)
        {
            var author = await(from a in _context.authors.AsNoTracking()
                             where a.Id == id
                             select new GetAuthorsDto
                             {
                                 FullName = a.FullName,
                                 BirthDate = a.BirthDate,
                                 City = a.City,
                                 Email = a.Email,
                             }).FirstOrDefaultAsync();
            if (author == null)
                return Result<GetAuthorsDto?>.Failure("Autor no encontrado");

            return Result<GetAuthorsDto?>.Success(author);
        }
    }
}
