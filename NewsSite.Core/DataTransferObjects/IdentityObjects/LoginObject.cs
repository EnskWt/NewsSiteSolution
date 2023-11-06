using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.DataTransferObjects.IdentityObjects
{
    public class LoginObject
    {
        [Required(ErrorMessage = "Username can't be blank.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password can't be blank.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
