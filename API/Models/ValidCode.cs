using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ValidCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String UserId { get; set; }

        public String Code { get; set; }
    }

    public class SendCodeByEmail
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class GetCode
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
    }

}