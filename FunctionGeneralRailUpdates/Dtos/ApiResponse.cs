using System;
using System.Collections.Generic;
using System.Text;

namespace AzureGeneralRailUpdates.Dtos
{
    public class ApiResponse
    {
        public DateTime CreationDate => DateTime.Now;
        public string Version { get; set; }
        public int SuccessStatus { get; set; }
        public int StatusCode { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }

        public bool Result { get; set; }
    }
}
