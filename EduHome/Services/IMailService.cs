using P512FiorelloBack.Models;

namespace EduHome.Services;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}