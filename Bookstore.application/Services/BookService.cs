using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bookstore.Application.Common;
using Bookstore.Application.DTOs;
using Bookstore.Application.Interfaces;
using Bookstore.Domain.Entities;
using Bookstore.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Application.Services
{
public class BookService : IBookRepository
    {
        private readonly BookstoreDbContext _context;
        private readonly IMapper _mapper;
        private const int MAX_BOOKS = 5;

        public BookService(BookstoreDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(CreateBookDto book)
        {
            var totalBooks = await _context.books.CountAsync();

            if (totalBooks >= MAX_BOOKS)
                return Result.Failure("No es posible registrar el libro, se alcanzó el máximo permitido.");

            var authorExists = await _context.authors
                .AnyAsync(a => a.Id == book.AuthorId);

            if (!authorExists)
                return Result.Failure("El autor no está registrado");

            var entity = _mapper.Map<Book>(book);
            await _context.books.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<GetBooksDto>> GetById(int id)
        {
            var book = await (
                from b in _context.books.AsNoTracking()
                join a in _context.authors.AsNoTracking()
                on b.AuthorId equals a.Id
                where b.Id == id
                select new GetBooksDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    Year = b.Year,
                    Pages = b.Pages,
                    AuthorName = a.FullName
                }
            ).FirstOrDefaultAsync();

            if (book == null)
                return Result<GetBooksDto>.Failure("Libro no encontrado");

            return Result<GetBooksDto>.Success(book);
        }

        public async Task<Result<PagedResult<GetBooksDto>>> GetAll(PaginationParams pagination)
        {
            var query = _context.books
                .Include(b => b.Author)
                .AsNoTracking()
                .AsQueryable();

            var total = await query.CountAsync();

            var books = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ProjectTo<GetBooksDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var result = new PagedResult<GetBooksDto>
            {
                Items = books,
                TotalRecords = total,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };

            return Result<PagedResult<GetBooksDto>>.Success(result);
        }
    }
}


