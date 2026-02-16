using AutoMapper;
using Bookstore.Application.Common;
using Bookstore.Application.DTOs;
using Bookstore.Application.Services;
using Bookstore.Domain.Entities;
using Bookstore.Infraestructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bookstore.Tests.Services
{
    public class BookServiceTests
    {
        // ===============================
        // CREATE CONTEXT IN MEMORY
        // ===============================
        private BookstoreDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<BookstoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookstoreDbContext(options);
        }

        // ===============================
        // CREATE AUTOMAPPER
        // ===============================
        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateBookDto, Book>();

                cfg.CreateMap<Book, GetBooksDto>()
                    .ForMember(d => d.AuthorName,
                        o => o.MapFrom(s => s.Author.FullName));
            });

            return config.CreateMapper();
        }

        // ===============================
        // TEST ADD SUCCESS
        // ===============================
        [Fact]
        public async Task AddAsync_ShouldCreateBook_WhenValid()
        {
            var context = CreateContext();
            var mapper = CreateMapper();

            var author = new Author
            {
                Id = 1,
                FullName = "Autor Test",
                BirthDate = DateTime.Now,
                City = "Bogotá",
                Email = "autor@test.com"
            };

            context.authors.Add(author);
            await context.SaveChangesAsync();

            var service = new BookService(context, mapper);

            var dto = new CreateBookDto
            {
                Title = "Libro Test",
                Genre = "Drama",
                Pages = 100,
                Year = 2020,
                AuthorId = 1
            };

            var result = await service.AddAsync(dto);

            result.IsSuccess.Should().BeTrue();
            context.books.Count().Should().Be(1);
        }

        // ===============================
        // TEST ADD MAX BOOKS
        // ===============================
     
        [Fact]
        public async Task AddAsync_ShouldFail_WhenMaxBooksReached()
        {
            var context = CreateContext();
            var mapper = CreateMapper();

            // Crear Author completo
            var author = new Author
            {
                Id = 1,
                FullName = "Autor Test",
                BirthDate = DateTime.Now,
                City = "Bogotá",
                Email = "autor@test.com"
            };

            context.authors.Add(author);

            // Insertar 5 libros (límite)
            for (int i = 1; i <= 5; i++)
            {
                context.books.Add(new Book
                {
                    Title = $"Libro {i}",
                    Genre = "Drama",
                    Pages = 100,
                    Year = 2000,
                    AuthorId = 1
                });
            }

            await context.SaveChangesAsync();

            var service = new BookService(context, mapper);

            var dto = new CreateBookDto
            {
                Title = "Libro Extra",
                Genre = "Drama",
                Pages = 120,
                Year = 2022,
                AuthorId = 1
            };

            var result = await service.AddAsync(dto);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("No es posible registrar el libro, se alcanzó el máximo permitido.");
        }


        // ===============================
        // TEST ADD AUTHOR NOT EXISTS
        // ===============================
        [Fact]
        public async Task AddAsync_ShouldFail_WhenAuthorDoesNotExist()
        {
            var context = CreateContext();
            var mapper = CreateMapper();

            var service = new BookService(context, mapper);

            var dto = new CreateBookDto
            {
                Title = "Libro Test",
                Genre = "Drama",
                Pages = 100,
                Year = 2020,
                AuthorId = 999
            };

            var result = await service.AddAsync(dto);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("autor");
        }

        // ===============================
        // TEST GET BY ID SUCCESS
        // ===============================
        [Fact]
        public async Task GetById_ShouldReturnBook_WhenExists()
        {
            var context = CreateContext();
            var mapper = CreateMapper();

            var author = new Author
            {
                Id = 1,
                FullName = "Autor Test",
                City = "Bogotá",
                Email = "autor@test.com"
            };

            context.authors.Add(author);

            context.books.Add(new Book
            {
                Id = 1,
                Title = "Libro Test",
                Genre = "Drama",
                Pages = 100,
                Year = 2020,
                AuthorId = 1,
                Author = author
            });

            await context.SaveChangesAsync();

            var service = new BookService(context, mapper);

            var result = await service.GetById(1);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value!.AuthorName.Should().Be("Autor Test");
        }

        // ===============================
        // TEST GET BY ID NOT FOUND
        // ===============================
        [Fact]
        public async Task GetById_ShouldFail_WhenBookDoesNotExist()
        {
            var context = CreateContext();
            var mapper = CreateMapper();

            var service = new BookService(context, mapper);

            var result = await service.GetById(99);

            result.IsSuccess.Should().BeFalse();
        }

        // ===============================
        // TEST GET ALL PAGINATION
        // ===============================
        [Fact]
        public async Task GetAll_ShouldReturnPagedBooks()
        {
            var context = CreateContext();
            var mapper = CreateMapper();

            var author = new Author
            {
                Id = 1,
                FullName = "Autor Test",
                Email = "gabo@test.com",
                City = "Aracataca"
            };

            context.authors.Add(author);

            for (int i = 1; i <= 3; i++)
            {
                context.books.Add(new Book
                {
                    Id = i,
                    Title = $"Libro {i}",
                    Genre = "Drama",
                    Pages = 100,
                    Year = 2020,
                    AuthorId = 1,
                    Author = author
                });
            }

            await context.SaveChangesAsync();

            var service = new BookService(context, mapper);

            var pagination = new PaginationParams
            {
                Page = 1,
                PageSize = 2
            };

            var result = await service.GetAll(pagination);

            result.IsSuccess.Should().BeTrue();
            //result.Value!.Items.Count.Should().Be(2);
            result.Value.TotalRecords.Should().Be(3);
        }
    }
}
