using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class TokenModel
    {
        public string Value { get; set; }
        public int ExpiresIn { get; set; }
    }
}
