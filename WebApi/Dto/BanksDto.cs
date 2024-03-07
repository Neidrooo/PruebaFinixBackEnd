using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace WebApi.Dto
{
    public class BanksDto
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Account_number { get; set; }
        public string Iban { get; set; }
        public string Bank_name { get; set; }
        public string Routing_number { get; set; }
        public string Swift_bic { get; set; }
        public string Created { get; set; }
    }
}
