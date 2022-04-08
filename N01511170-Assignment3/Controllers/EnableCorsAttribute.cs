using System;

namespace N01511170_Assignment3.Controllers
{
    internal class EnableCorsAttribute : Attribute
    {
        public EnableCorsAttribute(string origins, string methods, string headers)
        {
        }
    }
}