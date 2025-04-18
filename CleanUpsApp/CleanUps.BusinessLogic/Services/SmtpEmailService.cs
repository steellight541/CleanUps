using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; // Added
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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
            string body = $"Hi {userName},\n\n" +
                        $"Thank you for registering with the CleanUps App.\n\n" +
                        "We're excited to have you join our community dedicated to cleaning up our environment.\n\n" +
                        "Get started by finding events near you!\n\n" +
                        "Thanks,\nThe CleanUps Team";

            await SendEmailAsync(toEmail, subject, body);
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
            // TODO: Create a link to MAUI app's reset page, embedding the token
            // Example: string resetUrl = $"myapp://reset-password?token={resetToken}";
            string body = $"Hi {userName},\n\n" +
                        $"You requested a password reset. Use the following token to reset your password:\n\n" +
                        $"{resetToken}\n\n" +
                        // $"Or click this link: {resetUrl}\n\n" + // Uncomment when you have a reset URL
                        $"This token will expire in 30 minutes.\n\n" +
                        "If you did not request this, please ignore this email.\n\n" +
                        "Thanks,\nThe CleanUps Team";

            await SendEmailAsync(toEmail, subject, body);
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
            string body = $"Hi {userName},\n\n" +
                        $"Your password for the CleanUps App was successfully reset.\n\n" +
                        "If you did not perform this action, please contact support immediately.\n\n" +
                        "Thanks,\nThe CleanUps Team";

            await SendEmailAsync(toEmail, subject, body);
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
            string body = $"Hi {userName},\n\n" +
                        $"Your account with the CleanUps App has been successfully deleted as requested.\n\n" +
                        "We're sorry to see you go. If this was a mistake, please contact support.\n\n" +
                        "Thanks,\nThe CleanUps Team";

            await SendEmailAsync(toEmail, subject, body);
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
            string body = $"Hi {userName},\n\n" +
                        $"Thank you for registering for the event: \"{eventName}\".\n\n" +
                        "We look forward to seeing you there!\n\n" +
                        "Thanks,\nThe CleanUps Team";

            await SendEmailAsync(toEmail, subject, body);
        }

        /// <summary>
        /// Private helper method to construct and send an email using configured SMTP settings.
        /// Logs success or failure.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="body">The plain text body content of the email.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous email sending operation.</returns>
        private async Task SendEmailAsync(string toEmail, string subject, string body)
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
                        IsBodyHtml = false, // Sending plain text for simplicity
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