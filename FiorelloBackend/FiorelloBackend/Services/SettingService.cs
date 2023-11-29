using FiorelloBackend.Data;
using FiorelloBackend.Services.Interfaces;

namespace FiorelloBackend.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.Where(m=> !m.SoftDeleted)
                                    .AsEnumerable()
                                    .ToDictionary(m=>m.Key, m=> m.Value);
        }
    }
}
