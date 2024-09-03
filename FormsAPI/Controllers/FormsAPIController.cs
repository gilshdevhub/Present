using AutoMapper;
using Core.Entities.Forms;
using Core.Interfaces;
using FormsAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FormsAPI.Controllers
{
    public class FormsAPIController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAppFormsService _appFormsService;
        private readonly Dictionary<string, string>  map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [".png"] = "image/png",
            [".jpg"] = "image/jpeg",
            [".gif"] = "image/gif",
            [".doc"] = "application/msword",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            [".pdf"] = "application/pdf",
            [".png"] = "image/png",
            [".txt"] = "text/*"
        };


        public FormsAPIController(IAppFormsService appFormsService ,IMapper mapper)
        {
            _mapper = mapper;
            _appFormsService = appFormsService;
        }
        
        [HttpPost("PostCompansationForm")]
        public async Task<ActionResult<string>> PostCompansationForm([FromForm] CompansationFormDto compansationForm)
        {
            CompansationForm _compansationForm = _mapper.Map<CompansationForm>(compansationForm);
            var response = await _appFormsService.PostCompansationFormAsync(_compansationForm).ConfigureAwait(false);
            return Ok(response);
        }
        
        [HttpPost("PostContactForm")]
        public async Task<ActionResult<string>> PostContactForm([FromForm] ContactFormDto contactForm)
        {
            ContactForm _contactForm = _mapper.Map<ContactForm>(contactForm);
            var response = await _appFormsService.PostContactFormAsync( _contactForm).ConfigureAwait(false);
            return Ok(response);
        }
        
        [HttpPost("PostDamageCompansation")]
        public async Task<ActionResult<string>> PostDamageCompansation([FromForm] DamageCompansationDto damageCompansation)
        {
            DamageCompansation _damageCompansation = _mapper.Map<DamageCompansation>(damageCompansation);
            var response = await _appFormsService.PostDamageCompansationAsync(_damageCompansation).ConfigureAwait(false);
            return Ok(response);
        }
        
        [HttpPost("LostAndFoundDto")]
        public async Task<ActionResult<string>> PostLostAndFound([FromForm] LostAndFoundDto lostAndFound)
        {
            LostAndFound _lostAndFound = _mapper.Map<LostAndFound>(lostAndFound);
            var response = await _appFormsService.PostLostAndFoundAsync(_lostAndFound).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPost("PostPhotoShoot")]
        public async Task<ActionResult<string>> PostPhotoShoot([FromForm] PhotoShootDto photoShoot)
        {
            PhotoShoot _photoShoot = _mapper.Map<PhotoShoot>(photoShoot);
            var response = await _appFormsService.PostPhotoShootAsync(_photoShoot).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
