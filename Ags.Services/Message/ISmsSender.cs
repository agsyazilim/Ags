using System.Threading.Tasks;

namespace Ags.Services.Message
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
