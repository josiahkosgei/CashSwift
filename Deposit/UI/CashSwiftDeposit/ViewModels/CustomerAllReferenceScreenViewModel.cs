using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDeposit.Models.Forms;
using CashSwiftDeposit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Threading;

namespace CashSwiftDeposit.ViewModels
{

    [Guid("986A4F69-25C7-45CB-AF0F-A3B98363519E")]
    public class CustomerAllReferenceScreenViewModel : CustomerPrepopReferenceScreenBase
    {
        private TextBox currentTextbox;
        private ImageComboBoxItem<string> selectedPhoneCodeComboBox;
        private FormItem selectedFieldList;
        private bool screenInstructionsIsVisible;
        private string instructionTitleCaption;
        private string screenInstructions;

        public Form Form { get; set; }

        public CustomerAllReferenceScreenViewModel(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          bool required = true)
          : base("", screenTitle, applicationViewModel, true)
        {
            Keyboard = KeyboardType.NONE;
            ScreenInstructionsIsVisible = true;

            using (new DepositorDBContext())
            {
                PhoneCodeComboBox = new List<ImageComboBoxItem<string>>(1);
                PhoneCodeComboBox.Add(new ImageComboBoxItem<string>("{ResourceDir}/Resources\\Flags\\ke.png", "+254", "254"));
                SelectedPhoneCodeComboBox = PhoneCodeComboBox.First();
                Form form = new Form();
                form.DisplayName = screenTitle;
                form.ID = "1";
                form.FullInstructionText = FullInstructions;
                form.Name = "References";
                form.ClientSideValidation = new Dictionary<string, ClientSideValidation>()
        {
          {
            "4E6A4195-141F-4D91-8228-6D6147E523D1",
            new ClientSideValidation()
            {
              Validations = new List<Models.Forms.Validation>()
              {
                new Models.Forms.Validation()
                {
                  ErrorMessage = "Must be 16 or less digits",
                  Values = new List<ValidationValue>()
                  {
                    new ValidationValue() { Order = 0, Value = "^.{1,16}$" }
                  }
                }
              }
            }
          },
          {
            "3ADF4B3DD-68FA-E611-BA85-000C29C9B629",
            new ClientSideValidation()
            {
              Validations = new List<Models.Forms.Validation>()
              {
                new Models.Forms.Validation()
                {
                  ErrorMessage = "Phone Number must be in the form 7xxxxxxxx",
                  Values = new List<ValidationValue>()
                  {
                    new ValidationValue()
                    {
                      Order = 0,
                      Value = "^((?!0)\\d{9})$"
                    }
                  }
                }
              }
            }
          }
        };
                form.FormItems = new List<FormItem>()
        {
          new FormItem()
          {
            Label = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof (CustomerAllReferenceScreenViewModel), "sys_NarrationLabelCaption", "Narration"),
            Name = "Narration",
            Value = string.IsNullOrWhiteSpace(ApplicationViewModel.CurrentTransaction.Narration) ?  null : ApplicationViewModel.CurrentTransaction.Narration,
            InputDataType = 2,
            id = "1",
            IsRequired = true,
            ClientSideValidationID = "4E6A4195-141F-4D91-8228-6D6147E523D1",
            HintText = "e.g. Rent, School Fees, Etc"
          },
          new FormItem()
          {
            Label = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof (CustomerAllReferenceScreenViewModel), "sys_DepositorNameLabelCaption", "Depositor Name"),
            Name = "DepositorName",
            Value = string.IsNullOrWhiteSpace(ApplicationViewModel.CurrentTransaction.DepositorName) ?  null : ApplicationViewModel.CurrentTransaction.DepositorName,
            InputDataType = 2,
            id = "2",
            IsRequired = true,
            ClientSideValidationID = "4E6A4195-141F-4D91-8228-6D6147E523D1",
            HintText = "John Doe"
          },
          new FormItem()
          {
            Label = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof (CustomerAllReferenceScreenViewModel), "sys_PhoneLabelCaption", "Phone Number"),
            Name = "PhoneNumber",
            Value = string.IsNullOrWhiteSpace(ApplicationViewModel.CurrentTransaction.Phone) ?  null : ApplicationViewModel.CurrentTransaction.Phone.Remove(0, 3),
            InputDataType = 1,
            id = "3",
            IsRequired = true,
            ClientSideValidationID = "3ADF4B3DD-68FA-E611-BA85-000C29C9B629",
            HintText = "254712345678"
          },
          new FormItem()
          {
            Label = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof (CustomerAllReferenceScreenViewModel), "sys_IDNumberLabelCaption", "ID Number"),
            Name = "IDNumber",
            Value = string.IsNullOrWhiteSpace(ApplicationViewModel.CurrentTransaction.IDNumber) ?  null : ApplicationViewModel.CurrentTransaction.IDNumber,
            InputDataType = 2,
            id = "4",
            IsRequired = true,
            ClientSideValidationID = "4E6A4195-141F-4D91-8228-6D6147E523D1",
            HintText = "12345678"
          }
        };
                Form = form;
                ScreenInstructions = ApplicationViewModel.CashSwiftTranslationService?.TranslateUserText("NextCaption", CurrentGUIScreenText?.screen_title_instruction, "1.Touch on the field to type.\r\n2.All fields are mandatory.");
                InstructionTitleCaption = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText(nameof(CustomerAllReferenceScreenViewModel), "sys_ScreenInstructionsLabelCaption", "Instructions");
                NotifyStatusChanged();
            }
        }

        public string InstructionTitleCaption
        {
            get => instructionTitleCaption;
            set
            {
                instructionTitleCaption = value;
                NotifyOfPropertyChange(nameof(InstructionTitleCaption));
            }
        }

        public string ScreenInstructions
        {
            get => screenInstructions;
            set
            {
                screenInstructions = value;
                NotifyOfPropertyChange(nameof(ScreenInstructions));
            }
        }

        public string NarrationLabelCaption
        {
            get => Form.FormItems[0].Label;
            set
            {
                Form.FormItems[0].Label = value;
                NotifyOfPropertyChange(() => NarrationLabelCaption);
            }
        }

        public string DepositorNameLabelCaption
        {
            get => Form.FormItems[1].Label;
            set
            {
                Form.FormItems[1].Label = value;
                NotifyOfPropertyChange(() => DepositorNameLabelCaption);
            }
        }

        public string PhoneLabelCaption
        {
            get => Form.FormItems[2].Label;
            set
            {
                Form.FormItems[2].Label = value;
                NotifyOfPropertyChange(() => PhoneLabelCaption);
            }
        }

        public string IDNumberLabelCaption
        {
            get => Form.FormItems[3].Label;
            set
            {
                Form.FormItems[3].Label = value;
                NotifyOfPropertyChange(() => IDNumberLabelCaption);
            }
        }

        public string NarrationTextBoxValue
        {
            get => Form.FormItems[0].Value;
            set
            {
                Form.FormItems[0].Value = value;
                NotifyOfPropertyChange(() => NarrationTextBoxValue);
            }
        }

        public string DepositorNameTextBoxValue
        {
            get => Form.FormItems[1].Value;
            set
            {
                Form.FormItems[1].Value = value;
                NotifyOfPropertyChange(() => DepositorNameTextBoxValue);
            }
        }

        public string PhoneTextBoxValue
        {
            get => Form.FormItems[2].Value;
            set
            {
                Form.FormItems[2].Value = value;
                NotifyOfPropertyChange(() => PhoneTextBoxValue);
            }
        }

        public string IDNumberTextBoxValue
        {
            get => Form.FormItems[3].Value;
            set
            {
                Form.FormItems[3].Value = value;
                NotifyOfPropertyChange(() => IDNumberTextBoxValue);
            }
        }

        public string NarrationErrorText
        {
            get => Form.FormItems[0].ErrorMessage;
            set
            {
                Form.FormItems[0].ErrorMessage = value;
                NotifyOfPropertyChange(() => NarrationErrorText);
            }
        }

        public string DepositorNameErrorText
        {
            get => Form.FormItems[1].ErrorMessage;
            set
            {
                Form.FormItems[1].ErrorMessage = value;
                NotifyOfPropertyChange(() => DepositorNameErrorText);
            }
        }

        public string PhoneErrorText
        {
            get => Form.FormItems[2].ErrorMessage;
            set
            {
                Form.FormItems[2].ErrorMessage = value;
                NotifyOfPropertyChange(() => PhoneErrorText);
            }
        }

        public string IDNumberErrorText
        {
            get => Form.FormItems[3].ErrorMessage;
            set
            {
                Form.FormItems[3].ErrorMessage = value;
                NotifyOfPropertyChange(() => IDNumberErrorText);
            }
        }

        public string NarrationHintCaption
        {
            get => Form.FormItems[0].HintText;
            set
            {
                Form.FormItems[0].HintText = value;
                NotifyOfPropertyChange(() => NarrationHintCaption);
            }
        }

        public string DepositorNameHintCaption
        {
            get => Form.FormItems[1].HintText;
            set
            {
                Form.FormItems[1].HintText = value;
                NotifyOfPropertyChange(() => DepositorNameHintCaption);
            }
        }

        public string PhoneHintCaption
        {
            get => Form.FormItems[2].HintText;
            set
            {
                Form.FormItems[2].HintText = value;
                NotifyOfPropertyChange(() => PhoneHintCaption);
            }
        }

        public string IDNumberHintCaption
        {
            get => Form.FormItems[3].HintText;
            set
            {
                Form.FormItems[3].HintText = value;
                NotifyOfPropertyChange(() => IDNumberHintCaption);
            }
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            SelectedFieldList = null;
            if (string.IsNullOrWhiteSpace(Form?.ErrorMessage))
                ClearErrorText();
            else
                PrintErrorText(Form?.ErrorMessage);
            NotifyOfPropertyChange(() => FieldList);
            return base.OnActivateAsync(cancellationToken);
        }
        private void NotifyStatusChanged()
        {
            NotifyOfPropertyChange(() => PhoneCodeComboBox);
            NotifyOfPropertyChange(() => NarrationLabelCaption);
            NotifyOfPropertyChange(() => DepositorNameLabelCaption);
            NotifyOfPropertyChange(() => PhoneLabelCaption);
            NotifyOfPropertyChange(() => IDNumberLabelCaption);
            NotifyOfPropertyChange(() => NarrationTextBoxValue);
            NotifyOfPropertyChange(() => DepositorNameTextBoxValue);
            NotifyOfPropertyChange(() => PhoneTextBoxValue);
            NotifyOfPropertyChange(() => IDNumberTextBoxValue);
            NotifyOfPropertyChange(() => NarrationErrorText);
            NotifyOfPropertyChange(() => DepositorNameErrorText);
            NotifyOfPropertyChange(() => PhoneErrorText);
            NotifyOfPropertyChange(() => IDNumberErrorText);
            NotifyOfPropertyChange(() => NarrationHintCaption);
            NotifyOfPropertyChange(() => DepositorNameHintCaption);
            NotifyOfPropertyChange(() => PhoneHintCaption);
            NotifyOfPropertyChange(() => IDNumberHintCaption);
        }

        public List<ImageComboBoxItem<string>> PhoneCodeComboBox { get; set; }

        public ImageComboBoxItem<string> SelectedPhoneCodeComboBox
        {
            get => selectedPhoneCodeComboBox;
            set
            {
                selectedPhoneCodeComboBox = value;
                NotifyOfPropertyChange(nameof(SelectedPhoneCodeComboBox));
            }
        }

        public List<FormItem> FieldList
        {
            get
            {
                return Form.FormItems;
            }
        }

        public FormItem SelectedFieldList
        {
            get => selectedFieldList;
            set
            {
                selectedFieldList = value;
                NotifyOfPropertyChange(nameof(SelectedFieldList));
            }
        }

        private void PerformSelectionStatusWorker_DoWork(object sender, DoWorkEventArgs e) => PerformSelection();

        public void Cancel() => ApplicationViewModel.CancelSessionOnUserInput();

        public void PerformSelection() => Keyboard = (KeyboardType)SelectedFieldList.InputDataType;

        public void LabelClickMethod(object source)
        {
            if (!(source is Button button))
                return;
            button.Visibility = Visibility.Hidden;
        }

        public void TextChangedMethod(object source)
        {
            if (!(source is TextBox textBox) || !(textBox.Name == "PhoneTextBoxValue"))
                return;
            long result;
            textBox.Text = !long.TryParse(textBox.Text, out result) ? null : result.ToString("#");
        }

        public void LostFocusMethod(object source)
        {
        }

        public void GotFocusMethod(object source)
        {
            CurrentTextbox = source as TextBox;
            if (CurrentTextbox != null)
            {
                string name = CurrentTextbox.Name;
                if (!(name == "NarrationTextBoxValue"))
                {
                    if (!(name == "DepositorNameTextBoxValue"))
                    {
                        if (!(name == "PhoneTextBoxValue"))
                        {
                            if (name == "IDNumberTextBoxValue")
                            {
                                Keyboard = (KeyboardType)Form.FormItems[3].InputDataType;
                                NumericKeypadIsVisible = true;
                                AlphanumericKeyboardIsVisible = false;
                                ScreenInstructionsIsVisible = false;
                            }
                        }
                        else
                        {
                            Keyboard = (KeyboardType)Form.FormItems[2].InputDataType;
                            NumericKeypadIsVisible = true;
                            AlphanumericKeyboardIsVisible = false;
                            ScreenInstructionsIsVisible = false;
                        }
                    }
                    else
                    {
                        Keyboard = (KeyboardType)Form.FormItems[1].InputDataType;
                        NumericKeypadIsVisible = true;
                        AlphanumericKeyboardIsVisible = false;
                        ScreenInstructionsIsVisible = false;
                    }
                }
                else
                {
                    Keyboard = (KeyboardType)Form.FormItems[0].InputDataType;
                    AlphanumericKeyboardIsVisible = true;
                    NumericKeypadIsVisible = false;
                    ScreenInstructionsIsVisible = false;
                }
            }
            else
            {
                Keyboard = KeyboardType.NONE;

            }
            ScreenInstructionsIsVisible = Keyboard == KeyboardType.NONE;

            NotifyOfPropertyChange(() => Keyboard);
            NotifyOfPropertyChange(() => AlphanumericKeyboardIsVisible);
            NotifyOfPropertyChange(() => ScreenInstructionsIsVisible);
            NotifyOfPropertyChange(() => NumericKeypadIsVisible);
        }

        protected override void RefreshKeyboard()
        {
            base.RefreshKeyboard();
            NotifyOfPropertyChange(() => ScreenInstructionsIsVisible);
        }

        public TextBox CurrentTextbox
        {
            get => currentTextbox;
            set
            {
                currentTextbox = value;
                NotifyOfPropertyChange(() => CurrentTextbox);
            }
        }

        public bool ScreenInstructionsIsVisible
        {
            get => screenInstructionsIsVisible;
            set
            {
                screenInstructionsIsVisible = value;
                NotifyOfPropertyChange(nameof(ScreenInstructionsIsVisible));
            }
        }

        public override bool AlphanumericKeyboardIsVisible
        {
            get
            {
                return base.AlphanumericKeyboardIsVisible;
            }
        }

        public override bool FullAlphanumericKeyboardIsVisible
        {
            get
            {
                return base.FullAlphanumericKeyboardIsVisible;
            }
        }

        public override bool NumericKeypadIsVisible
        {
            get
            {
                return base.NumericKeypadIsVisible;
            }
        }

        public void Next()
        {
            lock (ApplicationViewModel.NavigationLock)
            {
                if (!CanNext)
                    return;
                CanNext = false;
                ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
                BackgroundWorker backgroundWorker = new BackgroundWorker()
                {
                    WorkerReportsProgress = false
                };
                backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
                backgroundWorker.RunWorkerAsync();
            }
        }
        public void Back()
        {

            ApplicationViewModel.CurrentTransaction.Narration = Form.FormItems.First(x => x.id == "1").Value;
            ApplicationViewModel.CurrentTransaction.DepositorName = Form.FormItems.First(x => x.id == "2").Value;
            ApplicationViewModel.CurrentTransaction.Phone = SelectedPhoneCodeComboBox.Value + Form.FormItems.First(x => x.id == "3").Value;
            ApplicationViewModel.CurrentTransaction.IDNumber = Form.FormItems.First(x => x.id == "4").Value;
            ApplicationViewModel.NavigatePreviousScreen();
        }
        private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Form.ErrorMessage = null;
            try
            {
                if (ClientSideValidation())
                {
                    ApplicationViewModel.CurrentTransaction.Narration = Form.FormItems.First(x => x.id == "1").Value;
                    ApplicationViewModel.CurrentTransaction.DepositorName = Form.FormItems.First(x => x.id == "2").Value;
                    ApplicationViewModel.CurrentTransaction.Phone = SelectedPhoneCodeComboBox.Value + Form.FormItems.First(x => x.id == "3").Value;
                    ApplicationViewModel.CurrentTransaction.IDNumber = Form.FormItems.First(x => x.id == "4").Value;
                    ApplicationViewModel.CurrentSession.SaveToDatabase();
                    ApplicationViewModel.NavigateNextScreen();
                    return;
                }
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(CustomerAllReferenceScreenViewModel), 1, nameof(StatusWorker_DoWork), ex.MessageString());
                PrintErrorText("Unexpected Error. Try again later or contact Administrator");
            }
            ApplicationViewModel.CloseDialog(false);
            CanNext = true;
        }

        private bool ClientSideValidation()
        {
            int num = 0;
            foreach (FormItem formItem in Form.FormItems)
            {
                formItem.ErrorMessage = null;
                if (!DoClientSideValidation(formItem))
                    ++num;
            }
            return num == 0;
        }

        protected bool DoClientSideValidation(FormItem item)
        {
            try
            {
                if (item.IsRequired && IsInputNull(item.Value))
                {
                    item.ErrorMessage = ApplicationViewModel.CashSwiftTranslationService.TranslateSystemText("ClientValidation", "sys_ValidationRequiredFieldError", "Field is required")?.ToUpperInvariant();
                    return false;
                }
                ClientSideValidation clientSideValidation;
                Form.ClientSideValidation.TryGetValue(item.ClientSideValidationID, out clientSideValidation);
                if (clientSideValidation != null)
                {
                    foreach (Models.Forms.Validation validation in clientSideValidation.Validations)
                    {
                        if (validation.Values.Count <= 0 || !ClientValidationRules.RegexValidation(item.Value, validation.Values.OrderBy(x => x.Order).ToList()[0].Value.Replace("\r\n", "\n").Replace("\n", "")))
                        {
                            item.ErrorMessage = validation.ErrorMessage;
                            return false;
                        }
                        item.ErrorMessage = null;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.ErrorFormat(GetType().Name, 31, ApplicationErrorConst.ERROR_INVALID_DATA.ToString(), "Error during regex validation: {0}", ex.MessageString());
                item.ErrorMessage = "System error. Contact Administrator";
                return false;
            }
        }

    }
}
