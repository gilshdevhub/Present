using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FormsAPI.Dtos
{
    public class LostAndFoundDto : BasicFormDto
    {
        
        public string description { get; set; }
    }
}
