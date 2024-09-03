using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FormsAPI.Dtos
{
    public class CompansationFormDto : BasicFormDto
    {
        
        public string description { get; set; }
        
        public string address1_city { get; set; }
        
        public string address1_postalcode { get; set; }
        
        public string address1_postofficebox { get; set; }
        
        [EmailAddress]
        public string emailaddress1 { get; set; }
        
        public string address1_line1 { get; set; }
        
        public string new_ravkavnumber { get; set; }
       

    }
}
