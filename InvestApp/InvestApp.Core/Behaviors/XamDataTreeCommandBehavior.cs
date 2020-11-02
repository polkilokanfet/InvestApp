using Infragistics.Controls.Menus;
using InvestApp.Core.Mvvm;
using Prism.Interactivity;

namespace InvestApp.Core.Behaviors
{
    public class XamDataTreeCommandBehavior : CommandBehaviorBase<XamDataTree>
    {
        public XamDataTreeCommandBehavior(XamDataTree tree) : base(tree)
        {
            tree.ActiveNodeChanged += (object sender, ActiveNodeChangedEventArgs eventArgs) =>
            {
                var param = eventArgs.NewActiveTreeNode.Data as NavigationItem;
                CommandParameter = param?.NavigationUri;
                ExecuteCommand(CommandParameter);
            };
        }
    }
}