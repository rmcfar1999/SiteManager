
namespace SiteManager.V4.Application.Common.Interfaces
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html);
    }
}
