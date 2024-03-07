using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class DataBanks
    {
        private readonly HttpClient _httpClient;
        private readonly BanksDBContext _context;
       
        public DataBanks(BanksDBContext context)
        {
            _httpClient = new HttpClient();
            _context = context;
        }
        public async Task LoadDataFromApiAsync()
        {
            TimeZoneInfo chileTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
            DateTime chileDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, chileTimeZone);
            var delayBetweenRequests = TimeSpan.FromSeconds(4); // Tiempo de espera entre llamadas
            const int MaxBanksCount = 200;
            var ApiUrl = "https://random-data-api.com/api/v2/banks?size=100";

            var currentBanksCount = await _context.Banks.CountAsync();
            var slotsAvailable = MaxBanksCount - currentBanksCount;

            int numberOfRequestsNeeded = (slotsAvailable > 100) ? 2 : 1;

            for (int i = 0; i < numberOfRequestsNeeded; i++)
            {
                if (slotsAvailable <= 0) break;

                var response = await _httpClient.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var banksToAdd = JsonSerializer.Deserialize<List<Banks>>(content, options);

                if (banksToAdd != null)
                {
                    int recordsToInsert = Math.Min(slotsAvailable, banksToAdd.Count);
                    for (int j = 0; j < recordsToInsert; j++)
                    {
                        var bank = banksToAdd[j];
                        bank.Id = 0; 
                        bank.Created = chileDateTime;
                        _context.Banks.Add(bank);
                        slotsAvailable--;
                    }
                    await _context.SaveChangesAsync();
                }

                if (i < numberOfRequestsNeeded - 1)
                {
                    await Task.Delay(delayBetweenRequests);
                }
            }
        }


    }
}
