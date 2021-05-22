using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Autoservisas.Models
{
    public class PayInfo
    {
        [DisplayName("Vardas, pavardė")]
        [Required]
        public string FName { get; set; }

        [DisplayName("Kortelės numeris")]
        [Required]
        public int Number { get; set; }

        [DisplayName("Kodas")]
        [Required]
        public int Code { get; set; }
    }
}