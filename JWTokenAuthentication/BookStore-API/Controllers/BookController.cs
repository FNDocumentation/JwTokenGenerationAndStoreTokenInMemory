using BookStore_API.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private IConfiguration _config;
        private ITokenService _iTokenService;

        public BookController(IConfiguration config,
                              ITokenService iTokenService)
        {
            _config = config;
            _iTokenService = iTokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            LibraryModel library = new LibraryModel();
            List<BooksModel> books = new List<BooksModel>();
            
            //var user = AuthenticateUser(login);
            LoginModel user = new LoginModel { Username = "Faizan.Jalil", Password = "password" };
            string token = await _iTokenService.FetchTokenAsync(user);
                       
            //if (login.Username == "Faizan.Jalil")
            {
                books.Add(new BooksModel { Id = 1, Name = "book1" });
                books.Add(new BooksModel { Id = 2, Name = "book2" });
                books.Add(new BooksModel { Id = 3, Name = "book3" });
            }

            library.Token = token;
            library.Books = books;

            return Ok(library);
        }

    }
}
