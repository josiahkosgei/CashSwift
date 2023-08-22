
//DatabaseUpdate.UserTextItemUpdater


using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;

namespace CashSwiftCashControlPortal.Module.DatabaseUpdate
{
    internal static class UserTextItemUpdater
    {
        public static void Update(IObjectSpace ObjectSpace, Version currentDBVersion)
        {
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIPrepropItem.value")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "GUIPrePropListItem_Value";
                userTextItemType.description = "A GUIPrePropListItem Value";
                userTextItemType.token = "sys_GUIPrepropItem.value";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.screen_title")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "GUIScreenText_ScreenTitle";
                userTextItemType.description = "A GUIScreenText Screen Title";
                userTextItemType.token = "sys_GUIScreen.screen_title";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.btn_back_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "GUIScreenText_BackButtonCaption";
                userTextItemType.description = "A GUIScreenText Back button caption text";
                userTextItemType.token = "sys_GUIScreen.btn_back_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.btn_cancel_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "GUIScreenText_CancelButtonCaption";
                userTextItemType.description = "A GUIScreenText Cancel button caption text";
                userTextItemType.token = "sys_GUIScreen.btn_cancel_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.full_instructions")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "GUIScreenText_FullInstructions";
                userTextItemType.description = "A GUIScreenText Full Instructions button caption text";
                userTextItemType.token = "sys_GUIScreen.full_instructions";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.btn_accept_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "GUIScreenText_AcceptButtonCaption";
                userTextItemType.description = "A GUIScreenText Accept button caption text";
                userTextItemType.token = "sys_GUIScreen.btn_accept_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_GUIScreen.screen_title_instruction")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "GUIScreenText_ScreenTitleInstruction";
                userTextItemType.description = "A GUIScreenText showing a brief summary of the screen instructions";
                userTextItemType.token = "sys_GUIScreen.screen_title_instruction";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.account_name_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Account Name Caption";
                userTextItemType.description = "Account Name";
                userTextItemType.token = "sys_TransactionText.account_name_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.account_number_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Account Number Caption";
                userTextItemType.description = "Account Number";
                userTextItemType.token = "sys_TransactionText.account_number_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.alias_account_name_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Alias Account Name Caption";
                userTextItemType.description = "Alias Account Name";
                userTextItemType.token = "sys_TransactionText.alias_account_name_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.alias_account_number_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Alias Account Number Caption";
                userTextItemType.description = "Alias Account Number";
                userTextItemType.token = "sys_TransactionText.alias_account_number_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.depositor_name_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Depositor Name Caption";
                userTextItemType.description = "Depositor Name Caption";
                userTextItemType.token = "sys_TransactionText.depositor_name_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.disclaimer")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Disclaimer Text";
                userTextItemType.description = "Disclaimer";
                userTextItemType.token = "sys_TransactionText.disclaimer";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.full_instructions")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "TxText Full Instructions";
                userTextItemType.description = "Full Instructions";
                userTextItemType.token = "sys_TransactionText.full_instructions";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.FundsSource_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Funds Source Caption";
                userTextItemType.description = "NULL";
                userTextItemType.token = "sys_TransactionText.FundsSource_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.id_number_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "ID Number Caption";
                userTextItemType.description = "ID Number";
                userTextItemType.token = "sys_TransactionText.id_number_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.listItem_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "ListItem Caption";
                userTextItemType.description = "ListItem Caption";
                userTextItemType.token = "sys_TransactionText.listItem_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.narration_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Narration Caption";
                userTextItemType.description = "Narration Caption";
                userTextItemType.token = "sys_TransactionText.narration_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.phone_number_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Phone Number Caption";
                userTextItemType.description = "Phone Number";
                userTextItemType.token = "sys_TransactionText.phone_number_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.receipt_template")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Receipt Template";
                userTextItemType.description = "Receipt Template";
                userTextItemType.token = "sys_TransactionText.receipt_template";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.reference_account_name_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Reference Account Name Caption";
                userTextItemType.description = "Reference Account Name Caption";
                userTextItemType.token = "sys_TransactionText.reference_account_name_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.reference_account_number_caption")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Reference Account Number Caption";
                userTextItemType.description = "Reference Account Number";
                userTextItemType.token = "sys_TransactionText.reference_account_number_caption";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_TransactionText.terms")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Terms and Conditions Text";
                userTextItemType.description = "Terms";
                userTextItemType.token = "sys_TransactionText.terms";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_ValidationText.error_message")) == null)
            {
                UserTextItemType userTextItemType = ObjectSpace.CreateObject<UserTextItemType>();
                userTextItemType.name = "Validation Error Message";
                userTextItemType.description = "Error Messages";
                userTextItemType.token = "sys_ValidationText.error_message";
                userTextItemType.Save();
            }
            if (ObjectSpace.FindObject<UserTextItemType>(new BinaryOperator("token", "sys_ValidationText.success_message")) != null)
                return;
            UserTextItemType userTextItemType1 = ObjectSpace.CreateObject<UserTextItemType>();
            userTextItemType1.name = "Validation Success Message";
            userTextItemType1.description = "Success Message";
            userTextItemType1.token = "sys_ValidationText.success_message";
            userTextItemType1.Save();
        }
    }
}
