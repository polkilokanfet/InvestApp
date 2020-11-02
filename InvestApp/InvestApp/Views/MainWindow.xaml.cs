using Infragistics.Windows.OutlookBar.Events;
using InvestApp.Core.Mvvm;

namespace InvestApp.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void XamOutlookBar_OnSelectedGroupChanging(object sender, SelectedGroupChangingEventArgs e)
        {
            if (e.NewSelectedOutlookBarGroup is IOutlookBarGroup selectedOutlookBarGroup)
            {
                Commands.NavigateCommand.Execute(selectedOutlookBarGroup.DefaultViewUri);
            }
        }

    }
}
