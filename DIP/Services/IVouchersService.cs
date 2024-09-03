using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DIP.Services
{
    public interface IVouchersService
    {
        Task ProcessXmlAsync(string folderPath);
    }
}
