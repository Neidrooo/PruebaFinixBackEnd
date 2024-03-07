using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Core.Specifications.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dto;
using WebApi.Errors;

namespace WebApi.Controllers
{
    [ApiExplorerSettings(GroupName = "APIBanks")]
    public class BanksController : BaseApiController
    {

        private readonly IGenericRepository<Banks> _banksRepository;
        private readonly IMapper _mapper;

        public BanksController(IGenericRepository<Banks> banksRepository, IMapper mapper)
        {

            _banksRepository = banksRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Obtiene todos los bancos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Pagination<BanksDto>>> GetBanks([FromQuery] BanksSpecificationParams parametros)
        {
            var spec = new BankSpecification(parametros);
            var banks = await _banksRepository.GetAllWithSpec(spec);

            var specCount = new BankForCountingSpecification(parametros);
            var totalBanks = await _banksRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalBanks / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);
            var data = _mapper.Map<IReadOnlyList<Banks>, IReadOnlyList<BanksDto>>(banks);

            return Ok(
                new Pagination<BanksDto>
                {
                    Count = totalBanks,
                    Data = data,
                    PageIndex = parametros.PageIndex,
                    PageSize = parametros.PageSize,
                    PageCount = totalPages
                }


                );
        }
        /// <summary>
        /// Obtiene un banco por su UID
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("{uid}")]
        [Authorize]
        public async Task<ActionResult<BanksDto>> GetBank(string uid)
        {
            var spec = new BankSpecification(uid);
            var bank = await _banksRepository.GetByIdWithSpec(spec);
            if (bank == null)
            {
                return NotFound(new CodeErrorResponse(404));
            }

            return _mapper.Map<Banks, BanksDto>(bank);
        }
        /// <summary>
        /// Crear un banco
        /// </summary>
        /// <param name="createBankDto"></param>
        /// <returns></returns>
        [HttpPost("createBank")]
        [Authorize]
        public async Task<ActionResult<Banks>> createBank(createBankDto createBankDto)
        {
            TimeZoneInfo chileTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
            DateTime chileDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, chileTimeZone);
            var bank = new Banks
            {
                Uid = createBankDto.Uid,
                Account_number = createBankDto.Account_number,
                Iban = createBankDto.Iban,
                Bank_name = createBankDto.Bank_name,
                Routing_number = createBankDto.Routing_number,
                Swift_bic = createBankDto.Swift_bic,
                Created = chileDateTime,
            };
            if (!await _banksRepository.SaveBD(bank))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha agregado el banco"));
            }
            return Ok(new { Message = "Operacion Existosa" });


        }
        /// <summary>
        /// Elimina un banco por su UID
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpDelete("{uid}")]
        [Authorize]
        public async Task<ActionResult> DeleteBank(string uid)
        {
            var spec = new BankSpecification(uid);
            var bank = await _banksRepository.GetByIdWithSpec(spec);
            if (bank == null)
            {
                return NotFound(new CodeErrorResponse(404, "El banco con el UID especificado no se encontró."));
            }


            var result = await _banksRepository.RemoveAsync(bank);

            if (!result)
            {
                return BadRequest(new CodeErrorResponse(500, "Error en la operación, no se ha eliminado/desactivado"));
            }
            else
            {
                return Ok(new { Message = "Operacion Existosa" });
            }
        }
        /// <summary>
        /// Edita el nombre de un banco
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="updateBankDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateBank/{uid}")]
        [Authorize]
        public async Task<ActionResult> UpdateBank(string uid, UpdateBankNameDto updateBankDto)
        {
            TimeZoneInfo chileTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
            DateTime chileDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, chileTimeZone);
            var spec = new BankSpecification(uid);
            var bank = await _banksRepository.GetByIdWithSpec(spec);
            if (bank == null)
            {
                return NotFound(new CodeErrorResponse(404, "El banco con el UID especificado no se encontró."));
            }

            bank.Bank_name = updateBankDto.Bank_name;
            bank.LastModified = chileDateTime;
            var result = await _banksRepository.UpdateAsync(bank);

            if (!result)
            {
                return BadRequest(new CodeErrorResponse(400, "Error al actualizar el nombre del banco."));
            }

            return Ok(new { Message = "Nombre del banco actualizado exitosamente" });
        }

    }
}
