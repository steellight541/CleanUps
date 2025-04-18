using System.Threading.Tasks;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Interface for a service that sends emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends a welcome email to a newly registered user.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task SendWelcomeEmailAsync(string toEmail, string userName);

        /// <summary>
        /// Sends a password reset email containing the reset token.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name (for personalization).</param>
        /// <param name="resetToken">The password reset token.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken);

        /// <summary>
        /// Sends a confirmation email after a successful password reset.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task SendPasswordResetConfirmationEmailAsync(string toEmail, string userName);

        /// <summary>
        /// Sends a confirmation email after successful account deletion.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="userName">The recipient's name.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task SendDeletedEmailAsync(string toEmail, string userName);

        /// <summary>
        /// Sends a confirmation email after registering for an event.
        /// </summary>
        /// <param name="toEmail">The user's email address.</param>
        /// <param name="userName">The user's name.</param>
        /// <param name="eventName">The name/title of the event.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        Task SendEventAttendanceConfirmationEmailAsync(string toEmail, string userName, string eventName);
    }
} 