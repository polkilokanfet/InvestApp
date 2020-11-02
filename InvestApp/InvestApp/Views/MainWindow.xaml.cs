using Infragistics.Windows.OutlookBar.Events;
using InvestApp.Core.ApplicationCommands;
using InvestApp.Core.Mvvm;

namespace InvestApp.Views
{
    public partial class MainWindow
    {
        private readonly IApplicationCommands _applicationCommands;

        public MainWindow(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
            InitializeComponent();
        }

        private void XamOutlookBar_OnSelectedGroupChanging(object sender, SelectedGroupChangingEventArgs e)
        {
            if (e.NewSelectedOutlookBarGroup is IOutlookBarGroup selectedOutlookBarGroup)
            {
                _applicationCommands.NavigateCommand.Execute(selectedOutlookBarGroup.DefaultViewUri);
            }
        }

    }
}
