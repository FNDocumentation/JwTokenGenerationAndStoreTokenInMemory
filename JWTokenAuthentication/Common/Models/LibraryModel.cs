using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class LibraryModel
    {
        public  string Token { get; set; }
        public List<BooksModel> Books { get; set; }
        
    }
}
