using Caliburn.Micro;
using System.Collections.Generic;

namespace CashSwiftDeposit.ViewModels
{
    public class FormListViewModel : Screen
    {
        private FormListItem selected;
        private string _nextCaption = "Next";
        private string _backCaption = "Back";

        public IDepositorForm Form { get; set; }

        public List<FormListItem> FieldList { get; set; }

        public string FormErrorText { get; set; }

        public FormListItem SelectedFieldList
        {
            get => selected;
            set
            {
                selected = value;
                NotifyOfPropertyChange(nameof(SelectedFieldList));
                Form.SelectFormField(value);
            }
        }

        public FormListViewModel(IDepositorForm form)
        {
            Form = form;
            FieldList = form.GetFields();
            FormErrorText = Form.GetErrors();
        }

        public string NextCaption
        {
            get => _nextCaption;
            set
            {
                _nextCaption = value;
                NotifyOfPropertyChange(() => NextCaption);
            }
        }

        public void Save()
        {
            string str = Form.SaveForm();
            if (str == null)
                Form.FormClose(true);
            else
                FormErrorText = str;
        }

        public string BackCaption
        {
            get => _backCaption;
            set
            {
                _backCaption = value;
                NotifyOfPropertyChange(() => BackCaption);
            }
        }

        public void Back() => Form.FormClose(false);
    }
}
