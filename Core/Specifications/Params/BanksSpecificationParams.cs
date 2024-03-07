using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Params
{
    public class BanksSpecificationParams
    {
        public int? id { get; set; }
        public string Uid { get; set; }
        public string Account_number { get; set; }
        public string Iban { get; set; }
        public string Bank_name { get; set; }
        public string Routing_number { get; set; }
        public string Swift_bic { get; set; }
        public string Created { get; set; }
        public string Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 50;
        private int _pageSize = 3;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
