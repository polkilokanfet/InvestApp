﻿using System.Windows;
using System.Windows.Input;
using Infragistics.Controls.Menus;
using InvestApp.Core.Behaviors;
using Prism.Commands;

namespace InvestApp.Core.Mvvm
{
    public class XamDataTreeItemSelected
    {
        public static readonly DependencyProperty SelectedCommandBehaviorProperty = DependencyProperty
            .Register("SelectedCommandBehavior", typeof(XamDataTreeCommandBehavior), typeof(XamDataTree), new PropertyMetadata());





        public static readonly DependencyProperty CommandProperty = DependencyProperty
            .Register("Command", typeof(ICommand), typeof(XamDataTree), new PropertyMetadata(OnCommandChangeCallback));

        public static ICommand GetCommand(XamDataTree menuItem)
        {
            return menuItem.GetValue(CommandProperty) as ICommand;
        }
        public static void SetCommand(XamDataTree menuItem, ICommand command)
        {
            menuItem.SetValue(CommandProperty, command);
        }
        private static void OnCommandChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is XamDataTree menuItem)
            {
                var behavior = GetOrCreateCommandBehavior(menuItem);
                behavior.Command = e.NewValue as ICommand;
            }
        }


        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty
            .Register("CommandParameter", typeof(object), typeof(XamDataTree), new PropertyMetadata(OnCommandParameterChangeCallback));

        public static object GetCommandParameter(XamDataTree menuItem)
        {
            return menuItem.GetValue(CommandParameterProperty);
        }
        public static void SetCommandProperty(XamDataTree menuItem, object parameter)
        {
            menuItem.SetValue(CommandParameterProperty, parameter);
        }

        private static void OnCommandParameterChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is XamDataTree menuItem)
            {
                var behavior = GetOrCreateCommandBehavior(menuItem);
                behavior.CommandParameter = e.NewValue;
            }
        }




        private static XamDataTreeCommandBehavior GetOrCreateCommandBehavior(XamDataTree menuItem)
        {
            if (!(menuItem.GetValue(SelectedCommandBehaviorProperty) is XamDataTreeCommandBehavior behavior))
            {
                behavior = new XamDataTreeCommandBehavior(menuItem);
                menuItem.SetValue(SelectedCommandBehaviorProperty, behavior);
            }
            return behavior;
        }
    }
}
