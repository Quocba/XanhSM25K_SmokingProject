using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailService.DTO;

namespace EmailService.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailRequest<string> request);
    }
}
