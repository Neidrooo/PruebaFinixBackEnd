using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BankSpecification : BaseSpecification<Banks>
    {
        public BankSpecification(BanksSpecificationParams parametros) : base(x =>
                     (!parametros.id.HasValue || x.Id == parametros.id) &&
                   (string.IsNullOrEmpty(parametros.Uid) || x.Uid.Contains(parametros.Uid))&&
                   (string.IsNullOrEmpty(parametros.Account_number) || x.Account_number == parametros.Account_number) &&
                   (string.IsNullOrEmpty(parametros.Iban) || x.Iban.Contains(parametros.Iban)) &&
                   (string.IsNullOrEmpty(parametros.Bank_name) || x.Bank_name.Contains(parametros.Bank_name)) &&
                   (string.IsNullOrEmpty(parametros.Routing_number) || x.Routing_number.Contains(parametros.Routing_number)) &&
                   (string.IsNullOrEmpty(parametros.Swift_bic) || x.Swift_bic.Contains(parametros.Swift_bic))
               )
        {
            ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
            if (!string.IsNullOrEmpty(parametros.Sort))
            {
                switch (parametros.Sort)
                {
                    case "uidAsc":
                        AddOrderBy(x => x.Uid);
                        break;
                    case "uidDesc":
                        AddOrderByDescending(x => x.Uid);
                        break;
                    case "account_numberAsc":
                        AddOrderBy(x => x.Account_number);
                        break;
                    case "account_numberDesc":
                        AddOrderByDescending(x => x.Account_number);
                        break;
                    case "ibanAsc":
                        AddOrderBy(x => x.Iban);
                        break;
                    case "ibanDesc":
                        AddOrderByDescending(x => x.Iban);
                        break;
                    case "bank_nameAsc":
                        AddOrderBy(x => x.Bank_name);
                        break;
                    case "bank_nameDesc":
                        AddOrderByDescending(x => x.Bank_name);
                        break;
                    case "routing_numberAsc":
                        AddOrderBy(x => x.Routing_number);
                        break;
                    case "routing_numberDesc":
                        AddOrderByDescending(x => x.Routing_number);
                        break;
                    case "swift_bicAsc":
                        AddOrderBy(x => x.Swift_bic);
                        break;
                    case "swift_bicDesc":
                        AddOrderByDescending(x => x.Swift_bic);
                        break;
                    case "createdAsc":
                        AddOrderBy(x => x.Created);
                        break;
                    case "createdDesc":
                        AddOrderByDescending(x => x.Created);
                        break;
                    default:
                        AddOrderBy(x => x.Id);
                        break;

                }
               
            }
            else
            {
                AddOrderBy(x => x.Id);
            }

        }
        public BankSpecification(string uid) : base(x => x.Uid == uid)
        {

        }
    }
}
