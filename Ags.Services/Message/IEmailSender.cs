using System.Collections.Generic;
using System.Threading.Tasks;
using Ags.Data.Domain.Message;

namespace Ags.Services.Message
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
             string replyTo = null, string replyToName = null,string attachmentFilePath = null, string attachmentFileName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
           IDictionary<string, string> headers = null);
    }
}
