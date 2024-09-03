using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FormsAPI.Dtos
{
    public class DamageCompansationDto : BasicFormDto
    {
        
        [IsraeliIdAuthentication]
        public string new_customerid { get; set; }
        
        public string address1_city { get; set; }
        
        public string address1_line1 { get; set; }
        
        [Range(minimum: 1000000, maximum: 9999999)]
        public int address1_postalcode { get; set; }
        
        [EmailAddress]
        public string emailaddress1 { get; set; }
        
        public string description { get; set; }

    }
}
