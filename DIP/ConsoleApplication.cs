using DIP.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DIP
{
    public class ConsoleApplication
    {
        private readonly IVouchersService _vouchersService;
        private readonly IConfiguration _configuration;

        public ConsoleApplication(IVouchersService vouchersService, IConfiguration configuration)
        {
            _vouchersService = vouchersService;
            _configuration = configuration;
        }

        public async Task RunAsync()
        {
            string xmlFolder = _configuration.GetSection("XmlFolder").Value;
            await _vouchersService.ProcessXmlAsync(xmlFolder);
        }
    }
}
