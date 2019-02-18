using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Ags.Data.Core;
using Ags.Data.Domain.Message;

namespace Ags.Services.Message
{

    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IAgsFileProvider _fileProvider;

        public AuthMessageSender(IAgsFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public Task SendEmailAsync(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
             string replyTo = null, string replyToName = null, string attachmentFilePath = null, string attachmentFileName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
           IDictionary<string, string> headers = null)
        {
            MailMessage message = new MailMessage
            {
                //from, to, reply to
                From = new MailAddress(fromAddress, fromName)
            };
            message.To.Add(new MailAddress(toAddress, toName));
            if (!string.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            //BCC
            if (bcc != null)
            {
                foreach (string address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            //CC
            if (cc != null)
            {
                foreach (string address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            //content
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            //headers
            if (headers != null)
                foreach (KeyValuePair<string, string> header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }

            //create the file attachment for this e-mail message
            if (!string.IsNullOrEmpty(attachmentFilePath) &&
                _fileProvider.FileExists(attachmentFilePath))
            {
                Attachment attachment = new Attachment(attachmentFilePath);
                attachment.ContentDisposition.CreationDate = _fileProvider.GetCreationTime(attachmentFilePath);
                attachment.ContentDisposition.ModificationDate = _fileProvider.GetLastWriteTime(attachmentFilePath);
                attachment.ContentDisposition.ReadDate = _fileProvider.GetLastAccessTime(attachmentFilePath);
                if (!string.IsNullOrEmpty(attachmentFileName))
                {
                    attachment.Name = attachmentFileName;
                }

                message.Attachments.Add(attachment);
            }


            //send email
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                smtpClient.EnableSsl = emailAccount.EnableSsl;
                smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                    CredentialCache.DefaultNetworkCredentials :
                    new NetworkCredential(emailAccount.Username, emailAccount.Password);
                smtpClient.Send(message);
            }

            return Task.CompletedTask;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }


    }
}
