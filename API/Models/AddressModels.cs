using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AddressModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        
        public String UserId { get; set; }

        public String Lat { get; set; }
        public String Lng { get; set; }
        [MaxLength(300)]
        public String FormatedAddress { get; set; }
    }
}