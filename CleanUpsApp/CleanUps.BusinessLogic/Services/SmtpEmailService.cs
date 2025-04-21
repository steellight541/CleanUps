using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; // Added
using System.Net;
using System.Net.Mail;

namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Provides functionality to send emails using SMTP (Simple Mail Transfer Protocol).
    /// Retrieves SMTP server configuration from <see cref="SmtpSettings"/>.
    /// Handles basic error logging for email sending failures.
    /// </summary>
    internal class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<SmtpEmailService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailService"/> class.
        /// </summary>
        /// <param name="smtpSettings">The SMTP configuration settings, injected via IOptions.</param>
        /// <param name="logger">Logger for recording email service operations and errors.</param>
        public SmtpEmailService(IOptions<SmtpSettings> smtpSettings, ILogger<SmtpEmailService> logger)
        {
            // Using IOptions<T> to access configured settings
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends a welcome email to a newly registered user.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name for personalization.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        public async Task SendWelcomeEmailAsync(string toEmail, string userName)
        {
            string subject = "Welcome to CleanUps App!";
            
            // HTML body: Centered container with white background, green header, purple username highlight.
            // Content includes welcome message and encouragement to find events.
            string body =
        "<!DOCTYPE html>" +
        "<html>" +
        "<head>" +
        "  <style>" +
        "    body { font-family: Arial, sans-serif; line-height:1.6; color:#333333; text-align:center; max-width:600px; margin:0 auto; padding:20px; }" +
        "    h1 { color:#2E8B57; font-size:2rem; margin-bottom:20px; }" +
        "  </style>" +
        "</head>" +
        "<body>" +
        "  <div style=\"background-color:#ffffff;border:1px solid #e0e0e0;border-radius:5px;padding:25px;margin:0 auto;max-width:600px;text-align:center;\">" +
        "    <h1>Welcome to CleanUps App!</h1>" +
        "    <div style=\"text-align:left;margin:0 auto;max-width:90%;\">" +
        $"      <p style=\"margin:15px 0;font-size:0.8rem;\">Hi <span style=\"color:#663399;font-weight:600;\">{userName}</span>,</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">Thank you for registering with the CleanUps App.</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">We're excited to have you join our community dedicated to cleaning up our environment.</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">Get started by finding events near you!</p>" +
        "    </div>" +
        "    <div style=\"text-align:left;margin-top:30px;border-top:1px solid #e0e0e0;padding-top:15px;font-size:14px;color:#666;\">" +
        "      <p>Thanks,<br>The CleanUps Team</p>" +
        "    </div>" +
        "  </div>" +
        "</body>" +
        "</html>";

            await SendEmailAsync(toEmail, subject, body, true);
        }

        /// <summary>
        /// Sends a password reset email containing a unique token to the user.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name for personalization.</param>
        /// <param name="resetToken">The generated password reset token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken)
        {
            string subject = "Reset Your CleanUps App Password";
            
            // HTML body: Centered container, green header, monospace token container with grey background.
            // Content explains the reset request, displays the token, highlights expiration, and offers support info.
            string body =
        "<!DOCTYPE html>" +
        "<html>" +
        "<head>" +
        "<meta charset=\"UTF-8\">" +
        "  <style>" +
        "    body { font-family: Arial, sans-serif; line-height: 1.6; color: #333333; text-align: center; max-width: 600px; margin: 0 auto; padding: 20px; }" +
        "    h1 { color: #2E8B57; font-size: 1.4rem; margin-bottom: 20px; }" +
        "    .token-container { background-color: #f5f5f5; border: 1px solid #e0e0e0; border-radius: 4px; padding: 15px; margin: 15px auto; font-family: monospace; font-size: 24px; font-weight: bold; text-align: center; max-width: 80%; }" +
        "  </style>" +
        "</head>" +
        "<body>" +
        "  <div style=\"background-color:#ffffff;border:1px solid #e0e0e0;border-radius:5px;padding:25px;margin:0 auto;max-width:600px;text-align:center;\">" +
        "    <h1>CleanUps App Password Reset</h1>" +
        // wrapper for left‑aligned paragraphs
        "    <div style=\"text-align:left;margin:0 auto;max-width:90%;\">" +
        $"      <p style=\"margin:15px 0;font-size:0.8rem;\">Hi <span style=\"color:#663399;font-weight:600;\">{userName}</span>,</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">You requested a password reset. Use the following token to reset your password:</p>" +
        "    </div>" +
        // token box stays centered
        $"    <div class=\"token-container\">{resetToken}</div>" +
        // another left‑aligned block for warnings & extra copy
        "    <div style=\"text-align:left;margin:0 auto;max-width:90%;\">" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\"><span style=\"font-weight:bold;color:#d32f2f;\">This token will expire in 15 minutes.</span></p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">If you did not request this, please ignore this email.</p>" +
        "    </div>" +
        // footer also left‑aligned
        "    <div style=\"text-align:left;margin-top:30px;border-top:1px solid #e0e0e0;padding-top:15px;font-size:14px;color:#666;\">" +
        "      <p class=\"text-align:center\">Thanks,<br/>The CleanUps Team</p>" +
        "    </div>" +
        "  </div>" +
        "</body>" +
        "</html>";

    await SendEmailAsync(toEmail, subject, body, true);
}


        /// <summary>
        /// Sends a confirmation email after a user's password has been successfully reset.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        public async Task SendPasswordResetConfirmationEmailAsync(string toEmail, string userName)
        {
            string subject = "Your CleanUps App Password Has Been Reset";
            
            // HTML body: Centered container, green header, confirmation message.
            // Includes an alert box prompting the user to contact support if they didn't perform the action.
            string body =
        "<!DOCTYPE html>" +
        "<html>" +
        "<head>" +
        "  <style>" +
        "    body { font-family: Arial, sans-serif; line-height:1.6; color:#333333; text-align:center; max-width:600px; margin:0 auto; padding:20px; }" +
        "    h1 { color:#2E8B57; font-size:24px; margin-bottom:20px; }" +
        "  </style>" +
        "</head>" +
        "<body>" +
        "  <div style=\"background-color:#ffffff;border:1px solid #e0e0e0;border-radius:5px;padding:25px;margin:0 auto;max-width:600px;text-align:center;\">" +
        "    <h1>CleanUps App Password Confirmation</h1>" +
        "    <div style=\"text-align:left;margin:0 auto;max-width:90%;\">" +
        $"      <p style=\"margin:15px 0;font-size:0.8rem;\">Hi <span style=\"color:#663399;font-weight:600;\">{userName}</span>,</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">Your password for the CleanUps App was successfully reset.</p>" +
        "    </div>" +
        "    <div style=\"background-color:#f8f8f8;border-left:4px solid #2E8B57;padding:12px 15px;margin:15px auto;max-width:80%;text-align:left;\">" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\"><strong>If you did not perform this action, please contact support immediately.</strong></p>" +
        "    </div>" +
        "    <div style=\"text-align:left;margin-top:30px;border-top:1px solid #e0e0e0;padding-top:15px;font-size:14px;color:#666;\">" +
        "      <p>Thanks,<br>The CleanUps Team</p>" +
        "    </div>" +
        "  </div>" +
        "</body>" +
        "</html>";

            await SendEmailAsync(toEmail, subject, body, true);
        }

        /// <summary>
        /// Sends a confirmation email after a user's account has been deleted.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        public async Task SendDeletedEmailAsync(string toEmail, string userName)
        {
            string subject = "Your CleanUps App Account Has Been Deleted";
            
            // HTML body: Centered container, green header, confirmation of account deletion.
            // Expresses regret and offers support if the deletion was a mistake.
            string body =
        "<!DOCTYPE html>" +
        "<html>" +
        "<head>" +
        "  <style>" +
        "    body { font-family: Arial, sans-serif; line-height:1.6; color:#333333; text-align:center; max-width:600px; margin:0 auto; padding:20px; }" +
        "    h1 { color:#2E8B57; font-size:24px; margin-bottom:20px; }" +
        "  </style>" +
        "</head>" +
        "<body>" +
        "  <div style=\"background-color:#ffffff;border:1px solid #e0e0e0;border-radius:5px;padding:25px;margin:0 auto;max-width:600px;text-align:center;\">" +
        "    <h1>Account Deletion Confirmation</h1>" +
        "    <div style=\"text-align:left;margin:0 auto;max-width:90%;\">" +
        $"      <p style=\"margin:15px 0;font-size:0.8rem;\">Hi <span style=\"color:#663399;font-weight:600;\">{userName}</span>,</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">Your account with the CleanUps App has been successfully deleted as requested.</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">We're sorry to see you go. If this was a mistake, please contact support.</p>" +
        "    </div>" +
        "    <div style=\"text-align:left;margin-top:30px;border-top:1px solid #e0e0e0;padding-top:15px;font-size:14px;color:#666;\">" +
        "      <p>Thanks,<br>The CleanUps Team</p>" +
        "    </div>" +
        "  </div>" +
        "</body>" +
        "</html>";

            await SendEmailAsync(toEmail, subject, body, true);
        }

        /// <summary>
        /// Sends a confirmation email after a user successfully registers for an event.
        /// </summary>
        /// <param name="toEmail">The user's email address.</param>
        /// <param name="userName">The user's name.</param>
        /// <param name="eventName">The name/title of the event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        public async Task SendEventAttendanceConfirmationEmailAsync(string toEmail, string userName, string eventName)
        {
            string subject = $"Event Registration Confirmation: {eventName}";
            
            // HTML body: Centered container, green header, confirmation of event registration.
            // Includes the event name highlighted in green and expresses anticipation.
            string body =
        "<!DOCTYPE html>" +
        "<html>" +
        "<head>" +
        "  <style>" +
        "    body { font-family: Arial, sans-serif; line-height:1.6; color:#333333; text-align:center; max-width:600px; margin:0 auto; padding:20px; }" +
        "    h1 { color:#2E8B57; font-size:24px; margin-bottom:20px; }" +
        "  </style>" +
        "</head>" +
        "<body>" +
        "  <div style=\"background-color:#ffffff;border:1px solid #e0e0e0;border-radius:5px;padding:25px;margin:0 auto;max-width:600px;text-align:center;\">" +
        "    <h1>Event Registration Confirmation</h1>" +
        "    <div style=\"text-align:left;margin:0 auto;max-width:90%;\">" +
        $"      <p style=\"margin:15px 0;font-size:0.8rem;\">Hi <span style=\"color:#663399;font-weight:600;\">{userName}</span>,</p>" +
        $"      <p style=\"margin:15px 0;font-size:0.8rem;\">Thank you for registering for the event: <span style=\"color:#2E8B57;font-weight:bold;\">\"{eventName}\"</span>.</p>" +
        "      <p style=\"margin:15px 0;font-size:0.8rem;\">We look forward to seeing you there!</p>" +
        "    </div>" +
        "    <div style=\"text-align:left;margin-top:30px;border-top:1px solid #e0e0e0;padding-top:15px;font-size:14px;color:#666;\">" +
        "      <p>Thanks,<br>The CleanUps Team</p>" +
        "    </div>" +
        "  </div>" +
        "</body>" +
        "</html>";

            await SendEmailAsync(toEmail, subject, body, true);
        }

        /// <summary>
        /// Private helper method to construct and send an email using configured SMTP settings.
        /// Logs success or failure.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="isHtml">Whether the body content is HTML formatted.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        private async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            try
            {
                // Step 1: Create a new SMTP client with configured host and port.
                using (SmtpClient client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    // Step 2: Configure SSL and authentication credentials.
                    client.EnableSsl = _smtpSettings.EnableSsl;
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                    // Step 3: Create a new mail message with sender, recipient, subject and body.
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = isHtml, // Set based on the parameter
                    };

                    // Step 4: Add recipient email address.
                    mailMessage.To.Add(toEmail);

                    // Step 5: Send email asynchronously.
                    await client.SendMailAsync(mailMessage);
                    
                    // Step 6: Log successful email delivery.
                    _logger.LogInformation("Email sent successfully to {Email}", toEmail);
                }
            }
            catch (Exception ex)
            {
                // Step 7: Log email sending failure with detailed information.
                _logger.LogError(ex, "Failed to send email to {Email}. Host: {Host}, Port: {Port}, SSL: {SSL}, User: {User}",
                    toEmail, _smtpSettings.Host, _smtpSettings.Port, _smtpSettings.EnableSsl, _smtpSettings.Username);
                // Do not re-throw here, email sending failure shouldn't block the main operation (like password reset)
                // The caller (AuthService) already logs if token creation/update fails.
            }
        }
    }
} 