using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Domin.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Application.Services
{
    public interface IMiscellaneousDataService
    {
        Task<string?> GetValueAsync(string key);
        Task<bool> SaveValueAsync(string key, string value);
    }

    public class MiscellaneousDataService : IMiscellaneousDataService
    {
        private readonly IDbAmpmiContext _context;

        public MiscellaneousDataService(IDbAmpmiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// دریافت مقدار مرتبط با یک کلید خاص
        /// </summary>
        public async Task<string?> GetValueAsync(string key)
        {
            var data = await _context.MiscellaneousDatas.FirstOrDefaultAsync(x => x.Key == key);
            return data?.Value;
        }

        /// <summary>
        /// ذخیره یا به‌روزرسانی مقدار مرتبط با یک کلید خاص
        /// </summary>
        public async Task<bool> SaveValueAsync(string key, string value)
        {
            try
            {
                var data = await _context.MiscellaneousDatas.FirstOrDefaultAsync(x => x.Key == key);

                if (data == null)
                {
                    data = new MiscellaneousData
                    {
                        Key = key,
                        Value = value
                    };
                    _context.MiscellaneousDatas.Add(data);
                }
                else
                {
                    data.Value = value;
                    _context.MiscellaneousDatas.Update(data);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
