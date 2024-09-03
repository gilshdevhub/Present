using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FormsAPI.Dtos
{
    public class BasicFormDto
    {
        
        [MaxLength(12)]
        [MinLength(10)]
        public string address1_telephone1 { get; set; } 
        
        public string lastname { get; set; } 
        
        public string firstname { get; set; } 
        public DateTime new_occasiondate { get; set; }
        
        [Range(minimum: 1000, maximum: 9999)]
        public int new_destination { get; set; } 
        
        [Range(minimum: 1000, maximum: 9999)]
        public int new_initialstation { get; set; }
        public IFormFile[] files { get; set; }
        public string new_casesubject1 { get; set; }
        public string new_casesubject2 { get; set; }
        public string new_casesubject3 { get; set; }

    }
}
