using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Controllers {
    [Route("/api/skineroVehicles/contactUs")]
    [ApiController]
    public class ContactUsController : Controller {
        private IContactUsRepository _repository { get; }
        private IUnitOfWork _unitOfWork { get; }
        private IMapper _mapper { get; }
        public ContactUsController (IContactUsRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateContactUs(ContactUsResource contactUsResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var contactUs = _mapper.Map<ContactUsResource, ContactUs>(contactUsResource);
            _repository.Add(contactUs);

            await _unitOfWork.CompleteAsync();
            contactUs = await _repository.GetContact(contactUs.Id);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Skinero Motors", "info@skineromotors.com"));
            message.To.Add(new MailboxAddress(contactUs.Name, contactUs.Email));
            message.Subject = contactUs.Name;
            message.Body = new TextPart("plain"){
                Text = $"{contactUs.Message} Contact Details {contactUs.Phone}  {contactUs.Email}"
            };
            using (var client = new SmtpClient()) {
                client.Connect("smtp.gmail.com", 587);
                await client.AuthenticateAsync("adesokantayyib@gmail.com", "Adenike1913");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return Ok(contactUs);
        }
        [HttpGet]
        public async Task<IEnumerable<ContactUsResource>> GetContacts ()
        {
            var contactUs = await _repository.GetContacts();
            return _mapper.Map<IEnumerable<ContactUs>, IEnumerable<ContactUsResource>>(contactUs);
        }
    }
}