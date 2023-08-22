using Caliburn.Micro;
using CashSwiftDataAccess.Data;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Collections.Generic;

namespace CashSwiftDeposit.ViewModels
{
    public abstract class FormViewModelBase : Conductor<Screen>, IDepositorForm, IPermissionRequired
    {
        protected bool isNew = false;
        public string _formErrorText;
        protected ApplicationViewModel ApplicationViewModel;
        protected DepositorDBContext DBContext = new DepositorDBContext();

        public string ScreenTitle { get; set; }

        protected List<FormListItem> Fields { get; set; } = new List<FormListItem>();

        public string FormErrorText
        {
            get => _formErrorText;
            set
            {
                _formErrorText = value;
                NotifyOfPropertyChange(nameof(FormErrorText));
            }
        }

        protected ICashSwiftWindowConductor Conductor { get; set; }

        protected object CallingObject { get; set; }

        public string NextCaption { get; set; }

        public string BackCaption { get; set; }

        public string CancelCaption { get; set; }

        public FormViewModelBase(
          ApplicationViewModel applicationViewModel,
          ICashSwiftWindowConductor conductor,
          object callingObject,
          bool isNewEntry)
        {
            isNew = isNewEntry;
            ApplicationViewModel = applicationViewModel;
            Conductor = conductor;
            CallingObject = callingObject;
        }

        public void SelectFormField(int fieldID) => SelectFormField(Fields[fieldID]);

        public void SelectFormField(FormListItem field)
        {
            if ((field.FormListItemType & FormListItemType.TEXTBOX) > FormListItemType.NONE)
                ActivateItemAsync(new FormFieldViewModel(this, field));
            else if ((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE)
            {
                ActivateItemAsync(new FormPasswordFieldViewModel(this, field));
            }
            else
            {
                if ((field.FormListItemType & FormListItemType.LISTBOX) <= FormListItemType.NONE)
                    return;
                ActivateItemAsync(new FormListboxFieldViewModel(this, field));
            }
        }

        public void FormHome(bool success) => ActivateItemAsync(new FormListViewModel(this));

        public List<FormListItem> GetFields() => Fields;

        public virtual void FormClose(bool success) => Conductor.ShowDialog(CallingObject);

        public string GetErrors() => FormErrorText;

        public virtual string SaveForm() => throw new NotImplementedException();

        public virtual int FormValidation() => throw new NotImplementedException();

        public virtual void HandleAuthorisationResult(PermissionRequiredResult result) => throw new NotImplementedException();
    }
}
