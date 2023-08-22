﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CashSwiftDeposit.UserControls
{
    public partial class NumericKeypad : UserControl, IComponentConnector
    {
        public NumericKeypad() => InitializeComponent();

        private void btnDigits_Click(object sender, RoutedEventArgs e)
        {
            Button originalSource = e.OriginalSource as Button;
            TextBox dataContext = DataContext as TextBox;
            int caretIndex = dataContext.CaretIndex;
            if (dataContext.SelectedText.Length > 0)
                deleteText();
            dataContext.Text = dataContext.Text.Insert(dataContext.CaretIndex, originalSource.Content as string);
            int num;
            dataContext.CaretIndex = num = caretIndex + 1;
            dataContext.Focus();
        }

        private void deleteText()
        {
            TextBox dataContext = DataContext as TextBox;
            int startIndex = dataContext.CaretIndex;
            if (dataContext.SelectedText.Length > 0)
            {
                dataContext.Text = dataContext.Text.Remove(dataContext.SelectionStart, dataContext.SelectionLength);
            }
            else
            {
                startIndex = Math.Max(dataContext.CaretIndex - 1, 0);
                if (dataContext.CaretIndex > 0)
                    dataContext.Text = dataContext.Text.Remove(startIndex, 1);
            }
            dataContext.CaretIndex = startIndex;
            dataContext.Focus();
        }

        private void keydelete_Click(object sender, RoutedEventArgs e) => deleteText();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is TextBox dataContext))
                return;
            TextBox textBox = dataContext;
            string text = dataContext.Text;
            int num = text != null ? text.Length : 0;
            textBox.CaretIndex = num;
        }
    }
}
