namespace CleanUps.BusinessLogic.Helpers
{
    /// <summary>
    /// Represents the configuration settings required for connecting to an SMTP server.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>
        /// Gets or sets the hostname or IP address of the SMTP server.
        /// </summary>
        /// <value>The SMTP server host.</value>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the port number for the SMTP server.
        /// Common ports are 25, 587 (TLS), or 465 (SSL - less common now).
        /// </summary>
        /// <value>The SMTP server port. Defaults to 587.</value>
        public int Port { get; set; } = 587; // Default SMTP port, thats what simply.com gave us

        /// <summary>
        /// Gets or sets the email address used for sending emails.
        /// </summary>
        /// <value>The sender's email address.</value>
        public string SenderEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name displayed as the sender.
        /// </summary>
        /// <value>The sender's display name. Defaults to "CleanUps App".</value>
        public string SenderName { get; set; } = "CleanUps App";

        /// <summary>
        /// Gets or sets the username for authenticating with the SMTP server.
        /// </summary>
        /// <value>The authentication username.</value>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for authenticating with the SMTP server.
        /// </summary>
        /// <value>The authentication password.</value>
        public string Password { get; set; } = string.Empty; // Use App Password for Gmail

        /// <summary>
        /// Gets or sets a value indicating whether SSL/TLS should be enabled for the connection.
        /// </summary>
        /// <value><c>true</c> to enable SSL/TLS; otherwise, <c>false</c>. Defaults to <c>true</c>.</value>
        public bool EnableSsl { get; set; } = true;
    }
} 