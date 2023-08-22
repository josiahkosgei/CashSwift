using Caliburn.Micro;
using System;
using System.Collections.Generic;

namespace CashSwiftDeposit.ViewModels
{
    public class FormListItem : Screen
    {
        private string _errorMessageTextBlock;

        public string DataLabel { get; set; }

        public string DataTextBoxLabel { get; set; }

        public string ValidatedText { get; set; }

        public string ErrorMessageTextBlock
        {
            get => _errorMessageTextBlock;
            set
            {
                _errorMessageTextBlock = value;
                NotifyOfPropertyChange(() => ErrorMessageTextBlock);
            }
        }

        public List<string> ItemList { get; set; }

        public Func<string, string> Validate { get; set; }

        public FormListItemType FormListItemType { get; set; }
    }
}
