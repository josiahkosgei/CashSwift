using CashSwiftCashControlPortal.Module.BusinessObjects.Screens;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using CashSwiftCashControlPortal.Module.BusinessObjects.Validation;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class NewObjectController : ViewController
    {
        private NewObjectViewController controller;

        protected override void OnActivated()
        {
            base.OnActivated();
            controller = Frame.GetController<NewObjectViewController>();
            if (controller == null)
                return;
            controller.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(controller_ObjectCreated);
        }

        private void controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            if (!(Frame is NestedFrame frame) || !(e.CreatedObject is UserTextItem createdObject))
                return;
            switch (((NestedFrame)Frame).ViewItem.CurrentObject)
            {
                case GUIPrepopItem _:
                    if (!frame.ViewItem.Id.Equals("Value", StringComparison.Ordinal))
                        break;
                    createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIPrepropItem.value"));
                    break;
                case GUIScreen _:
                    if (frame.ViewItem.Id.Equals("GUIScreenText.ScreenTitle", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.screen_title"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("GUIScreenText.BackButtonCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.btn_back_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("GUIScreenText.AcceptButtonCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.btn_accept_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("GUIScreenText.FullInstructions", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.full_instructions"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("GUIScreenText.CancelButtonCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.btn_cancel_caption"));
                        break;
                    }
                    if (!frame.ViewItem.Id.Equals("GUIScreenText.ScreenTitleInstruction", StringComparison.Ordinal))
                        break;
                    createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.screen_title_instruction"));
                    break;
                case TransactionTypeListItem _:
                    if (frame.ViewItem.Id.Equals("TransactionText.AccountNameCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.account_name_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.AccountNumberCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.account_number_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.AliasAccountNameCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.alias_account_name_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.AliasAccountNumberCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.alias_account_number_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.DepositorNameCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.depositor_name_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.Disclaimer", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.disclaimer"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.FullInstructions", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.full_instructions"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.FundsSource_caption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.FundsSource_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.IDNumberCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.id_number_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.ListItemCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.listItem_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.NarrationCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.narration_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.PhoneNumberCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.phone_number_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.ReceiptTemplate", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.receipt_template"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.ReferenceAccountNameCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.reference_account_name_caption"));
                        break;
                    }
                    if (frame.ViewItem.Id.Equals("TransactionText.ReferenceAccountNumberCaption", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.reference_account_number_caption"));
                        break;
                    }
                    if (!frame.ViewItem.Id.Equals("TransactionText.TermsAndConditions", StringComparison.Ordinal))
                        break;
                    createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.terms"));
                    break;
                case ValidationItem _:
                    if (frame.ViewItem.Id.Equals("ValidationText.ErrorMessage", StringComparison.Ordinal))
                    {
                        createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_ValidationText.error_message"));
                        break;
                    }
                    if (!frame.ViewItem.Id.Equals("ValidationText.SuccessMessage", StringComparison.Ordinal))
                        break;
                    createdObject.TextItemTypeID = createdObject.Session.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_ValidationText.success_message"));
                    break;
            }
        }

        protected override void OnDeactivated()
        {
            if (controller != null)
                controller.ObjectCreated -= new EventHandler<ObjectCreatedEventArgs>(controller_ObjectCreated);
            base.OnDeactivated();
        }
    }
}
