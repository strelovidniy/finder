using System.Net;
using System.Net.Mail;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Exceptions;
using Finder.Domain.Models.ViewModels;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;
using Microsoft.Extensions.Logging;

namespace Finder.Domain.Services.Realization;

internal class EmailService(
    IRazorViewToStringRenderer razorViewToStringRenderer,
    IEmailSettings emailSettings,
    ILogger<EmailService> logger
) : IEmailService
{
    public async Task SendEmailAsync<TModel>(
        string toAddresses,
        EmailSubject subject,
        TModel viewModel,
        IEnumerable<(string Path, string FileName)>? attachments = null,
        CancellationToken cancellationToken = default
    ) where TModel : class, IEmailViewModel
    {
        var path = typeof(TModel).Name switch
        {
            nameof(CreateAccountEmailViewModel) => EmailViewLocation.CreateAccountEmail,
            nameof(ResetPasswordEmailViewModel) => EmailViewLocation.ResetPasswordEmail,
            _ => throw new ApiException(StatusCode.InvalidEmailModel)
        };

        var content = await razorViewToStringRenderer.RenderViewToStringAsync(path, viewModel);

        await SendEmailAsync(toAddresses, subject, content, true, attachments, cancellationToken);
    }

    private async Task SendEmailAsync(
        string email,
        string subject,
        string content,
        bool isHtmlContent = false,
        IEnumerable<(string Path, string FileName)>? attachments = null,
        CancellationToken cancellationToken = default
    )
    {
        using var smtpClient = new SmtpClient(emailSettings.Server, emailSettings.Port);

        smtpClient.Credentials = new NetworkCredential(emailSettings.FromEmail, emailSettings.Password);
        smtpClient.EnableSsl = emailSettings.UseSSL;

        using var message = new MailMessage();

        message.Body = content;
        message.IsBodyHtml = isHtmlContent;
        message.To.Add(new MailAddress(email));
        message.Subject = subject;
        message.From = new MailAddress(emailSettings.FromEmail, emailSettings.FromDisplayName);
        message.Priority = MailPriority.High;

        try
        {
            attachments?
                .ToList()
                .ForEach(attachment =>
                    message
                        .Attachments
                        .Add(new Attachment(
                            File.OpenRead(attachment.Path),
                            attachment.FileName
                        )));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorMessage.EmailAttachmentAddingError);
        }

        try
        {
            await smtpClient.SendMailAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ErrorMessage.EmailNotSent, message.Subject, message.To.FirstOrDefault()?.Address);

            logger.LogError(ex, ErrorMessage.EmailSendingError);

            throw new ApiException(StatusCode.EmailSendingError);
        }
    }
}