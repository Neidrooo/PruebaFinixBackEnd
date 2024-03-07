using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Banks: ClassBase
    {
        public string Uid { get; set; }
        public string Account_number { get; set; }
        public string Iban { get; set; }
        public string Bank_name { get; set; }
        public string Routing_number { get; set; }
        public string Swift_bic { get; set; }
        [Column(TypeName = "TEXT")]
        public DateTime Created { get; set; }
        [Column(TypeName = "TEXT")]
        public DateTime? LastModified { get; set; }
    }
}
