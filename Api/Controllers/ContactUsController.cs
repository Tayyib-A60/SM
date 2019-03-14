using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;
namespace SkineroMotors.Controllers {
    [Route ("/api/skineroVehicles/contactUs")]
    [ApiController]
    public class ContactUsController : Controller {
        private IContactUsRepository _repository { get; }
        private IUnitOfWork _unitOfWork { get; }
        private IMapper _mapper { get; }
        private IConfiguration _configuration { get; }
        public ContactUsController (IContactUsRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactUs (ContactUsResource contactUsResource) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var contactUs = _mapper.Map<ContactUsResource, ContactUs> (contactUsResource);
            _repository.Add (contactUs);
            await _unitOfWork.CompleteAsync ();

            // var apiKey = System.Environment.GetEnvironmentVariable ("SkineroMotorsSendGridApi");
            var apiKey = _configuration.GetSection ("SkineroMotorsSendGridApiKey").Value;
            var sendGridclient = new SendGridClient (apiKey);
            var from = new EmailAddress ("info@skineromotors.com", "SkineroMotors");
            var subject = "Skinero Motors Contact us";
            var to = new EmailAddress ("adesokantayyib@gmail.com", "Skinero");
            var plainTextContent = $"";
            var htmlContent = $"<strong>Customer Details:</strong> {contactUs.Name} {contactUs.Email} {contactUs.Phone}<br/><strong>Message</strong> {contactUs.Message}";
            var msg = MailHelper.CreateSingleEmail (from, to, subject, plainTextContent, htmlContent);
            var response = await sendGridclient.SendEmailAsync (msg);

            return Ok (contactUs);
        }

        [HttpGet]
        public async Task<IEnumerable<ContactUsResource>> GetContacts () {
            var contactUs = await _repository.GetContacts ();
            return _mapper.Map<IEnumerable<ContactUs>, IEnumerable<ContactUsResource>> (contactUs);
        }
    }
}