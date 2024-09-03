using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RoutePlanning.Middlewares
{
    public class CommonApiResponse
    {
        protected CommonApiResponse(HttpStatusCode statusCode, string version, int successStatus, object result = null, IEnumerable<string> errorMessages = null)
        {
            Version = version;
            SuccessStatus = successStatus;
            StatusCode = (int)statusCode;
            Result = result;
            ErrorMessages = errorMessages;
        }

        public DateTime CreationDate => DateTime.Now;
        public string Version { get; set; }
        public int SuccessStatus { get; set; }
        public int StatusCode { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }

        public object Result { get; set; }

        public static CommonApiResponse Create(HttpStatusCode statusCode, string version, int successStatus, object result = null, IEnumerable<string> errorMessage = null)
        {
            return new CommonApiResponse(statusCode, version, successStatus, result, errorMessage);
        }
    }
}
