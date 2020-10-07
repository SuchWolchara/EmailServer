using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Json;
using Web.Services;
using DAL.Entities;
using System.Collections.Generic;
using Web.Enums;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using DAL.Repositories;
using Web.Helpers;
using MimeKit;

namespace Web.Controllers
{
    /// <summary>
    /// Контроллер, позволяющий совершать рассылку писем, сохранять информацию об этих письмах в БД и получать её
    /// </summary>
    [Route("api/mails/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IDbReporitory _dbRepository;

        public HomeController(IEmailService emailService, IDbReporitory dbReporitory)
        {
            _emailService = emailService;
            _dbRepository = dbReporitory;
        }

        /// <summary>
        /// Метод, принимающий GET запрос и возвращающий все записи из таблицы Emails
        /// </summary>
        [HttpGet]
        public IEnumerable<IEntity> GetAll()
        {
            return _dbRepository.GetAll<EmailEntity>().AsEnumerable();
        }

        /// <summary>
        /// Метод, принимающий POST запрос на рассылку писем и возвращающий кол-во успешно добавленных в таблицу Emails записей
        /// </summary>
        [HttpPost]
        public async Task<int> SendMessages([FromBody] EmailJsonModel emailJsonModel)
        {
            if (emailJsonModel.Recipients == null) return 0;

            var distinctRecipients = new List<string>(emailJsonModel.Recipients.Distinct());
            var recipients = new List<MailboxAddress>();
            var emailEntities = new List<EmailEntity>();

            foreach (var recipient in distinctRecipients)
            {
                if (_emailService.EmailRegex.IsMatch(recipient ?? string.Empty))
                    recipients.Add(new MailboxAddress(string.Empty, recipient));
                else
                    emailEntities.Add(EmailHelper.GetEmailEntity(emailJsonModel, recipient, false, FailedMessages.Format));
            }

            var sendingResults = await _emailService.TrySendAsync(recipients, emailJsonModel);

            foreach (var sendingResult in sendingResults)
                emailEntities.Add(EmailHelper.GetEmailEntity(emailJsonModel, sendingResult.Recipient, sendingResult.Result, sendingResult.FailedMessage));

            await _dbRepository.AddRangeAsync(emailEntities);
            return await _dbRepository.SaveChangesAsync();
        }
    }
}
