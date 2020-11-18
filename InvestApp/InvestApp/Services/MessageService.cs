using System.Windows;
using InvestApp.Domain.Services;

namespace InvestApp.Services
{
    public class MessageService : IMessageService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
