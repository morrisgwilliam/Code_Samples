using Sabio.Models.Domain;
using Sabio.Models.Requests.Email;
using Sabio.Models.Requests.Users;
using Sabio.Models.Requests.RewardRequests;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class EmailService : IEmailService
    {

        SendGridClient _client;
        string appDomainName;


        public EmailService(IKeysService keyService)
        {
            string apiKey = Convert.ToString(@keyService.GetByKeyName<string>("SendGrid"));
            appDomainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            _client = new SendGridClient(apiKey);
        }

        public async Task<bool> TestEmailAsync(IEmail email)
        {
            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(email.From, "William"),
                Subject = email.Subject,
                HtmlContent = email.Content
            };
            msg.AddTo(new EmailAddress(email.To, "William"));

            List<EmailAddress> ccList = new List<EmailAddress>();
            if (email.CcList != null)
            {
                foreach (string emailAddress in email.CcList)
                {
                    EmailAddress ccEmail = new EmailAddress(emailAddress);
                    ccList.Add(ccEmail);
                }
                msg.AddCcs(ccList);
            }
            SendGrid.Response response = await _client.SendEmailAsync(msg);
            bool isSuccessful = false;
            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.Created)
            {
                isSuccessful = true;
            }
            return isSuccessful;
        }

        public async Task<bool> ConfirmAccount(string userEmail, Guid token)
        {
            string path = HttpContext.Current.Server.MapPath("~/Content/Templates/Email_Templates/ConfirmAccount.html");
            string content = System.IO.File.ReadAllText(path);
            Email email = new Email();
            email.To = userEmail;
            email.From = "fakeemail@gmail.com";
            email.Content = @content;
            email.Content = email.Content.Replace("{$DOMAIN}", appDomainName).Replace("{$EMAIL}", userEmail).Replace("{$QUERYPARAMS}", "token=" + token);

            email.Subject = "Confirm GoodDog account email address";

            bool isSuccessful = await TestEmailAsync(email);
            return isSuccessful;
        }

        public async Task<bool> ForgotPassword(User user, Guid token)
        {
            string path = HttpContext.Current.Server.MapPath("~/Content/Templates/Email_Templates/RequestPassword.html");
            string content = System.IO.File.ReadAllText(path);
            Email email = new Email();
            email.To = user.EmailAddress;
            email.From = "fakeemail@gmail.com";
            email.Content = @content;
            email.Content = email.Content.Replace("{$DOMAIN}", appDomainName).Replace("{$USERNAME}", user.UserName).Replace("{$EMAIL}", user.EmailAddress).Replace("{$QUERYPARAMS}", "token=" + token);

            email.Subject = "GoodDog Password Reset";

            bool isSuccessful = await TestEmailAsync(email);
            return isSuccessful;
        }

        public async Task<bool> InviteFriend(UserInvite user, Guid token)
        {
            string path = HttpContext.Current.Server.MapPath("~/Content/Templates/Email_Templates/InviteFriend.html");
            string content = System.IO.File.ReadAllText(path);
            Email email = new Email();
            email.To = user.EmailAddress;
            email.From = "fakeemail@gmail.com";
            email.Content = @content;
            email.Content = email.Content.Replace("{$DOMAIN}", appDomainName).Replace("{$QUERYPARAMS}", "token=" + token);

            email.Subject = "You've been invited to Good Dog";

            bool isSuccessful = await TestEmailAsync(email);
            return isSuccessful;
        }

        public async Task Reactivate(UserStatusUpdateRequest user)
        {
            string path = HttpContext.Current.Server.MapPath("~/Content/Templates/Email_Templates/ReactivateAccount.html");
            string content = System.IO.File.ReadAllText(path);
            string emailEncoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user.Email));
            Email email = new Email();
            email.To = user.Email;
            email.From = "fakeemail@gmail.com";
            email.Content = @content;
            email.Content = email.Content.Replace("{$DOMAIN}", appDomainName).Replace("{$USERNAME}", user.Email).Replace("{$EMAIL_ENCODED}", emailEncoded);

            email.Subject = "GoodDog Deactivation";

            await TestEmailAsync(email);

        }

        public async Task Reactivated(string Email)
        {
            string path = HttpContext.Current.Server.MapPath("~/Content/Templates/Email_Templates/ReactivatedAccountResponse.html");
            string content = System.IO.File.ReadAllText(path);
            Email email = new Email();
            email.To = Email;
            email.From = "fakeemail@gmail.com";
            email.Content = @content;
            email.Content = email.Content.Replace("{$DOMAIN}", appDomainName);

            email.Subject = "GoodDog Account Reactivated";

            await TestEmailAsync(email);
        }


        public async Task<bool> Reward(RewardEmail reward)
        {


            string path = HttpContext.Current.Server.MapPath("~/Content/Templates/Email_Templates/RewardEmail.html");
            string content = System.IO.File.ReadAllText(path);
            Email email = new Email();
            email.To = reward.Email;
            email.From = "fakeemail@gmail.com";
            email.Content = @content;
            email.Content = email.Content.Replace("{$DOMAIN}", appDomainName).Replace("{$NAME}", reward.Name).Replace("{$QUERYPARAMS}", "code=" + reward.QrCode);
            email.Subject = "Your Good Dog Reward!";

            bool isSuccessful = await TestEmailAsync(email);
            return isSuccessful;

        }
    }
}
