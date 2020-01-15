﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmurfWalletOW.Behavior
{
    public class ClosingBehavior : BehaviorBase<Window>
    {
        private static void OnHandleCommandChanged(DependencyObject owner, DependencyPropertyChangedEventArgs e)
        {
            var element = owner as Window;
            if (element == null) return;
            element.Closing -= OnCommand;

            if (e.NewValue == null) return;
            element.Closing += OnCommand;
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
                                                typeof(ICommand),
                                                typeof(ClosingBehavior),
                                                new FrameworkPropertyMetadata(null, OnHandleCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(ClosingBehavior),
                                                new FrameworkPropertyMetadata(null));

        public static ICommand GetCommand(DependencyObject owner)
        {
            return (ICommand)owner.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject owner, ICommand value)
        {
            owner.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(DependencyObject element)
        {
            return element.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject element, object value)
        {
            element.SetValue(CommandParameterProperty, value);
        }

        private static void OnCommand(object sender, CancelEventArgs e)
        {
            var frameWorkElement = sender as FrameworkElement;
            if (frameWorkElement == null) return;

            var command = (ICommand)frameWorkElement.GetValue(CommandProperty);

            if (command == null || !command.CanExecute(null)) return;
            command.Execute(new[] { sender, e });
        }
    }
}

