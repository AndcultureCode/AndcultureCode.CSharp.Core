using MimeKit;

namespace AndcultureCode.CSharp.Core.Interfaces.Providers.Mail
{
    /// <summary>
    /// Generic email provider
    /// </summary>
    public interface IEmailProvider : IProvider
    {
        /// <summary>
        /// Send an email immediately
        /// </summary>
        /// <param name="message">The message to be sent</param>
        IResult<bool> Send(MimeMessage message);

        /// <summary>
        /// Send an email via a background job
        /// </summary>
        /// <param name="message">The message to be sent</param>
        IResult<bool> SendLater(MimeMessage message);

        ///// <summary>
        ///// Send a template email immediately
        ///// </summary>
        // IResult<bool> TemplateSend (string templateId, List<string> recipients, Dictionary<string, string> substitutionData);

        ///// <summary>
        ///// Send a template email via a background job
        ///// </summary>
        ///// <param name="message">The message to be sent</param>
        // IResult<bool> TemplateSendLater (string templateId, List<string> recipients, Dictionary<string, string> substitutionData, Dictionary<string, object> metadata = null);

    }
}
