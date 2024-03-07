using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BankForCountingSpecification : BaseSpecification<Banks>
    {
        public BankForCountingSpecification(BanksSpecificationParams parametros) : base(x =>
                     (!parametros.id.HasValue || x.Id == parametros.id) &&
                   (string.IsNullOrEmpty(parametros.Uid) || x.Uid.Contains(parametros.Uid)) &&
                   (string.IsNullOrEmpty(parametros.Account_number) || x.Account_number == parametros.Account_number) &&
                   (string.IsNullOrEmpty(parametros.Iban) || x.Iban.Contains(parametros.Iban)) &&
                   (string.IsNullOrEmpty(parametros.Bank_name) || x.Bank_name.Contains(parametros.Bank_name)) &&
                   (string.IsNullOrEmpty(parametros.Routing_number) || x.Routing_number.Contains(parametros.Routing_number)) &&
                   (string.IsNullOrEmpty(parametros.Swift_bic) || x.Swift_bic.Contains(parametros.Swift_bic))

               )
        {

        }
    }
}
