﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace CashSwiftDeposit.Views
{
    public partial class UserFormView : UserControl, IComponentConnector
    {
        public UserFormView()
        {
            InitializeComponent();
            new Style().Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
        }

        private void FieldsListbox_PreviewMouseUp(object sender, MouseButtonEventArgs e) => tabControl.SelectedIndex = FieldsListbox.SelectedIndex + 1;
    }
}
