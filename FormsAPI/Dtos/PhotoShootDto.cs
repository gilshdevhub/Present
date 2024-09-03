using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FormsAPI.Dtos
{
    public class PhotoShootDto : BasicFormDto
    {
        
        [IsraeliIdAuthentication]
        public string new_customerid { get; set; }
        
        [EmailAddress]
        public string emailaddress1 { get; set; }
    }
}
