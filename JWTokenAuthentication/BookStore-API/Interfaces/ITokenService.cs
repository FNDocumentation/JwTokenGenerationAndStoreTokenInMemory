using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Interfaces
{
    public interface ITokenService
    {
        Task<string> FetchTokenAsync(LoginModel user);
    }
}
