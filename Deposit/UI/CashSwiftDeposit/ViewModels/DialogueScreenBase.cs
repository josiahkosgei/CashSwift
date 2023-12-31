﻿using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CashSwiftDeposit.ViewModels
{
    public abstract class DialogueScreenBase : Screen, IDisposable
    {
        private int _timerDuration;
        private string _button1Caption;
        private string _button2Caption;
        private string _button3Caption;
        private string _button4Caption;
        private string _button5Caption;
        private string _helpText;
        private MessageBoxButton _messageBoxButton;
        private string _screenTitle;
        private string _dialogImage;
        private string _dialogBoxMessage;

        public DialogueButton OK { get; }

        public DialogueButton Cancel { get; }

        public DialogueButton Yes { get; }

        public DialogueButton No { get; }

        public DialogueButton Accept { get; }

        public DialogueButton Decline { get; }

        protected bool HasReturned { get; set; }

        protected ApplicationViewModel ApplicationViewModel { get; }

        protected MessageBoxButton MessageBoxButton
        {
            get => _messageBoxButton;
            set
            {
                _messageBoxButton = value;
                switch (_messageBoxButton)
                {
                    case MessageBoxButton.OK:
                        DialogueButton1 = OK;
                        Button1Caption = DialogueButton1.name;
                        break;
                    case MessageBoxButton.OKCancel:
                        DialogueButton1 = OK;
                        Button1Caption = DialogueButton1.name;
                        DialogueButton2 = Cancel;
                        Button2Caption = DialogueButton2.name;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        DialogueButton1 = Yes;
                        Button1Caption = DialogueButton1.name;
                        DialogueButton2 = No;
                        Button2Caption = DialogueButton2.name;
                        DialogueButton3 = Cancel;
                        Button3Caption = DialogueButton3.name;
                        break;
                    case MessageBoxButton.YesNo:
                        DialogueButton1 = Yes;
                        Button1Caption = DialogueButton1.name;
                        DialogueButton2 = No;
                        Button2Caption = DialogueButton2.name;
                        break;
                }
            }
        }

        protected MessageBoxImage MessageBoxImage { get; }

        public string ScreenTitle
        {
            get => _screenTitle;
            set
            {
                _screenTitle = value;
                NotifyOfPropertyChange(() => ScreenTitle);
            }
        }

        public string DialogImage
        {
            get => _dialogImage;
            set
            {
                _dialogImage = value;
                NotifyOfPropertyChange(() => DialogImage);
            }
        }

        public string DialogBoxMessage
        {
            get => _dialogBoxMessage;
            set
            {
                _dialogBoxMessage = value;
                NotifyOfPropertyChange(() => DialogBoxMessage);
            }
        }

        public string HelpText
        {
            get => _helpText;
            set
            {
                _helpText = value;
                NotifyOfPropertyChange(() => Button1Caption);
                NotifyOfPropertyChange(() => CanHelp);
                NotifyOfPropertyChange(() => HelpButtonIsVisible);
            }
        }

        public int DefaultButton { get; set; }

        protected MessageBoxResult MessageBoxResult { get; set; }

        public bool CanButton1 => Button1IsVisible;

        public bool CanButton2 => Button2IsVisible;

        public bool CanButton3 => Button3IsVisible;

        public bool CanButton4 => Button4IsVisible;

        public bool CanButton5 => Button5IsVisible;

        public bool CanHelp => !string.IsNullOrWhiteSpace(HelpText);

        public bool HelpButtonIsVisible => !HasReturned && !string.IsNullOrWhiteSpace(HelpText);

        public bool Button1IsVisible => !HasReturned && !string.IsNullOrWhiteSpace(Button1Caption);

        public bool Button2IsVisible => !HasReturned && !string.IsNullOrWhiteSpace(Button2Caption);

        public bool Button3IsVisible => !HasReturned && !string.IsNullOrWhiteSpace(Button3Caption);

        public bool Button4IsVisible => !HasReturned && !string.IsNullOrWhiteSpace(Button4Caption);

        public bool Button5IsVisible => !HasReturned && !string.IsNullOrWhiteSpace(Button5Caption);

        public string Button1Caption
        {
            get => _button1Caption;
            set
            {
                _button1Caption = value;
                NotifyOfPropertyChange(() => Button1Caption);
                NotifyOfPropertyChange(() => CanButton1);
                NotifyOfPropertyChange(() => Button1IsVisible);
            }
        }

        public string Button2Caption
        {
            get => _button2Caption;
            set
            {
                _button2Caption = value;
                NotifyOfPropertyChange(() => Button2Caption);
                NotifyOfPropertyChange(() => CanButton2);
                NotifyOfPropertyChange(() => Button2IsVisible);
            }
        }

        public string Button3Caption
        {
            get => _button3Caption;
            set
            {
                _button3Caption = value;
                NotifyOfPropertyChange(() => Button1Caption);
                NotifyOfPropertyChange(() => CanButton3);
                NotifyOfPropertyChange(() => Button3IsVisible);
            }
        }

        public string Button4Caption
        {
            get => _button4Caption;
            set
            {
                _button4Caption = value;
                NotifyOfPropertyChange(() => Button1Caption);
                NotifyOfPropertyChange(() => CanButton4);
                NotifyOfPropertyChange(() => Button4IsVisible);
            }
        }

        public string Button5Caption
        {
            get => _button5Caption;
            set
            {
                _button5Caption = value;
                NotifyOfPropertyChange(() => Button1Caption);
                NotifyOfPropertyChange(() => CanButton5);
                NotifyOfPropertyChange(() => Button5IsVisible);
            }
        }

        public DialogueButton DialogueButton1 { get; set; }

        public DialogueButton DialogueButton2 { get; set; }

        public DialogueButton DialogueButton3 { get; set; }

        public DialogueButton DialogueButton4 { get; set; }

        public DialogueButton DialogueButton5 { get; set; }

        protected int TimerDuration
        {
            get => _timerDuration;
            set => _timerDuration = value;
        }

        protected DialogueScreenBase(
          ApplicationViewModel applicationViewModel,
          int timerDuration = 30,
          int defaultButton = 0)
        {
            DialogueButton dialogueButton = new DialogueButton();
            ref DialogueButton local1 = ref dialogueButton;
            if (!(Application.Current.FindResource("Dialog_OK_Caption") is string str1))
                str1 = nameof(OK);
            local1.name = str1;
            dialogueButton.type = MessageBoxResult.OK;
            OK = dialogueButton;
            dialogueButton = new DialogueButton();
            ref DialogueButton local2 = ref dialogueButton;
            if (!(Application.Current.FindResource("Dialog_Cancel_Caption") is string str2))
                str2 = nameof(Cancel);
            local2.name = str2;
            dialogueButton.type = MessageBoxResult.Cancel;
            Cancel = dialogueButton;
            dialogueButton = new DialogueButton();
            ref DialogueButton local3 = ref dialogueButton;
            if (!(Application.Current.FindResource("Dialog_Yes_Caption") is string str3))
                str3 = nameof(Yes);
            local3.name = str3;
            dialogueButton.type = MessageBoxResult.Yes;
            Yes = dialogueButton;
            dialogueButton = new DialogueButton();
            ref DialogueButton local4 = ref dialogueButton;
            if (!(Application.Current.FindResource("Dialog_No_Caption") is string str4))
                str4 = nameof(No);
            local4.name = str4;
            dialogueButton.type = MessageBoxResult.No;
            No = dialogueButton;
            dialogueButton = new DialogueButton();
            ref DialogueButton local5 = ref dialogueButton;
            if (!(Application.Current.FindResource("Dialog_Accept_Caption") is string str5))
                str5 = nameof(Accept);
            local5.name = str5;
            dialogueButton.type = MessageBoxResult.Yes;
            Accept = dialogueButton;
            dialogueButton = new DialogueButton();
            ref DialogueButton local6 = ref dialogueButton;
            if (!(Application.Current.FindResource("Dialog_Decline_Caption") is string str6))
                str6 = nameof(Decline);
            local6.name = str6;
            dialogueButton.type = MessageBoxResult.No;
            Decline = dialogueButton;
            // ISSUE: explicit constructor call
            //base.c();
            ApplicationViewModel = applicationViewModel;
            DefaultButton = defaultButton;
            TimerDuration = timerDuration;
            Deactivated += new AsyncEventHandler<DeactivationEventArgs>(DialogueScreenBase_Deactivated);
            Activated += new EventHandler<ActivationEventArgs>(DialogueScreenBase_Activated);
        }

        private void DialogueBoxWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            if (DefaultButton <= 0)
                return;
            switch (DefaultButton)
            {
                case 1:
                    Button1();
                    break;
                case 2:
                    Button2();
                    break;
                case 3:
                    Button3();
                    break;
                case 4:
                    Button4();
                    break;
                case 5:
                    Button5();
                    break;
            }
        }

        private void TimerUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            double d = 0.0;
            while (true)
            {
                switch (DefaultButton)
                {
                    case 1:
                        Button1Caption = string.Format("{0} ({1:0})", Button1Caption, TimerDuration - Math.Floor(d));
                        break;
                    case 2:
                        Button2Caption = string.Format("{0} ({1:0})", Button2Caption, TimerDuration - Math.Floor(d));
                        break;
                    case 3:
                        Button3Caption = string.Format("{0} ({1:0})", Button3Caption, TimerDuration - Math.Floor(d));
                        break;
                    case 4:
                        Button4Caption = string.Format("{0} ({1:0})", Button4Caption, TimerDuration - Math.Floor(d));
                        break;
                    case 5:
                        Button5Caption = string.Format("{0} ({1:0})", Button5Caption, TimerDuration - Math.Floor(d));
                        break;
                }
                Thread.Sleep(1000);
            }
        }

        public void Button1()
        {
            HasReturned = true;
            MessageBoxResult = DialogueButton1.type;
        }

        public void Button2()
        {
            HasReturned = true;
            MessageBoxResult = DialogueButton2.type;
        }

        public void Button3()
        {
            HasReturned = true;
            MessageBoxResult = DialogueButton3.type;
        }

        public void Button4()
        {
            HasReturned = true;
            MessageBoxResult = DialogueButton4.type;
        }

        public void Button5()
        {
            HasReturned = true;
            MessageBoxResult = DialogueButton5.type;
        }

        private async Task DialogueScreenBase_Deactivated(object sender, DeactivationEventArgs e)
        {
            if (HasReturned)
                return;
            ApplicationViewModel.ShowDialogBox(this);
        }

        private void DialogueScreenBase_Activated(object sender, ActivationEventArgs e)
        {
            if (TimerDuration <= 0)
                return;
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(DialogueBoxWorker_DoWork);
            backgroundWorker1.RunWorkerAsync();
            BackgroundWorker backgroundWorker2 = new BackgroundWorker();
            backgroundWorker2.DoWork += new DoWorkEventHandler(TimerUpdater_DoWork);
            backgroundWorker2.RunWorkerAsync();
        }

        public void Dispose() => ApplicationViewModel.CloseDialog();
    }
}
