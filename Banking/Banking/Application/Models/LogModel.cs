using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Log")]
    public class LogModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        public DateTime Created { get; set; }

        public string UserName { get; set; }

        public string ErrorMessage { get; set; }

        public string AdditionalData { get; set; }
    }
}