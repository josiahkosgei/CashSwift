
//DatabaseUpdate.SystemTextItemUpdate


using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;

namespace CashSwiftCashControlPortal.Module.DatabaseUpdate
{
    internal static class SystemTextItemUpdate
    {
        public static void Update(IObjectSpace ObjectSpace, Version currentDBVersion)
        {
            SystemTextItemType systemTextItemType = ObjectSpace.FindObject<SystemTextItemType>(new BinaryOperator("token", "sys_backendscreen_text"));
            if (systemTextItemType == null)
            {
                systemTextItemType = ObjectSpace.CreateObject<SystemTextItemType>();
                systemTextItemType.name = "BackendScreen";
                systemTextItemType.description = "Backend screens text";
                systemTextItemType.token = "sys_backendscreen_text";
                systemTextItemType.Save();
            }
            SystemTextItemCategory textItemCategory1 = ObjectSpace.FindObject<SystemTextItemCategory>(new BinaryOperator("name", "Backend"));
            if (textItemCategory1 == null)
            {
                textItemCategory1 = ObjectSpace.CreateObject<SystemTextItemCategory>();
                textItemCategory1.name = "Backend";
                textItemCategory1.description = "Text used in the CDM backend Category";
                textItemCategory1.Save();
            }
            SystemTextItemCategory textItemCategory2 = ObjectSpace.FindObject<SystemTextItemCategory>(new BinaryOperator("name", "User Management Screen"));
            if (textItemCategory2 == null)
            {
                textItemCategory2 = ObjectSpace.CreateObject<SystemTextItemCategory>();
                textItemCategory2.name = "User Management Screen";
                textItemCategory2.description = "User Management Screen Category";
                textItemCategory2.ParentID = textItemCategory1;
                textItemCategory2.Save();
            }
            if (ObjectSpace.FindObject<SystemTextItem>(new BinaryOperator("Token", "sys_User_ChangePasswordCommand_Caption")) == null)
            {
                SystemTextItem systemTextItem = ObjectSpace.CreateObject<SystemTextItem>();
                systemTextItem.name = "User_ChangePasswordCommand_Caption";
                systemTextItem.description = "Change Password";
                systemTextItem.DefaultTranslation = "Change Password";
                systemTextItem.Token = "sys_User_ChangePasswordCommand_Caption";
                systemTextItem.TextItemTypeID = systemTextItemType;
                systemTextItem.Category = textItemCategory2;
                systemTextItem.Save();
            }
            if (ObjectSpace.FindObject<SystemTextItem>(new BinaryOperator("Token", "sys_UserManagementScreenTitle_Caption")) == null)
            {
                SystemTextItem systemTextItem = ObjectSpace.CreateObject<SystemTextItem>();
                systemTextItem.name = "UserManagementScreenTitle_Caption";
                systemTextItem.description = "User Management";
                systemTextItem.DefaultTranslation = "User Management";
                systemTextItem.Token = "sys_UserManagementScreenTitle_Caption";
                systemTextItem.TextItemTypeID = systemTextItemType;
                systemTextItem.Category = textItemCategory2;
                systemTextItem.Save();
            }
            SystemTextItemCategory textItemCategory3 = ObjectSpace.FindObject<SystemTextItemCategory>(new BinaryOperator("name", "Device Management Screen"));
            if (textItemCategory3 == null)
            {
                textItemCategory3 = ObjectSpace.CreateObject<SystemTextItemCategory>();
                textItemCategory3.name = "Device Management Screen";
                textItemCategory3.description = "Device Management Screen Category";
                textItemCategory3.ParentID = textItemCategory1;
                textItemCategory3.Save();
            }
            if (ObjectSpace.FindObject<SystemTextItem>(new BinaryOperator("Token", "sys_DeviceStatusScreenTitle")) == null)
            {
                SystemTextItem systemTextItem = ObjectSpace.CreateObject<SystemTextItem>();
                systemTextItem.name = "DeviceStatusScreenTitle";
                systemTextItem.DefaultTranslation = "Device Management";
                systemTextItem.Token = "sys_DeviceStatusScreenTitle";
                systemTextItem.TextItemTypeID = systemTextItemType;
                systemTextItem.Category = textItemCategory3;
                systemTextItem.Save();
            }
            SystemTextItemCategory textItemCategory4 = ObjectSpace.FindObject<SystemTextItemCategory>(new BinaryOperator("name", "Transaction Management Screen"));
            if (textItemCategory4 == null)
            {
                textItemCategory4 = ObjectSpace.CreateObject<SystemTextItemCategory>();
                textItemCategory4.name = "Transaction Management Screen";
                textItemCategory4.description = "Transaction Management Screen Category";
                textItemCategory4.ParentID = textItemCategory1;
                textItemCategory4.Save();
            }
            if (ObjectSpace.FindObject<SystemTextItem>(new BinaryOperator("Token", "sys_TransactionManagementScreenTitle")) == null)
            {
                SystemTextItem systemTextItem = ObjectSpace.CreateObject<SystemTextItem>();
                systemTextItem.name = "TransactionManagementScreenTitle";
                systemTextItem.DefaultTranslation = "Transactions";
                systemTextItem.Token = "sys_TransactionManagementScreenTitle";
                systemTextItem.TextItemTypeID = systemTextItemType;
                systemTextItem.Category = textItemCategory4;
                systemTextItem.Save();
            }
            SystemTextItemCategory textItemCategory5 = ObjectSpace.FindObject<SystemTextItemCategory>(new BinaryOperator("name", "CIT Management Screen"));
            if (textItemCategory5 == null)
            {
                textItemCategory5 = ObjectSpace.CreateObject<SystemTextItemCategory>();
                textItemCategory5.name = "CIT Management Screen";
                textItemCategory5.description = "CIT Management Screen Category";
                textItemCategory5.ParentID = textItemCategory1;
                textItemCategory5.Save();
            }
            if (ObjectSpace.FindObject<SystemTextItem>(new BinaryOperator("Token", "sys_CITManagementScreenTitle")) != null)
                return;
            SystemTextItem systemTextItem1 = ObjectSpace.CreateObject<SystemTextItem>();
            systemTextItem1.name = "CITManagementScreenTitle";
            systemTextItem1.DefaultTranslation = "CIT Management";
            systemTextItem1.Token = "sys_CITManagementScreenTitle";
            systemTextItem1.TextItemTypeID = systemTextItemType;
            systemTextItem1.Category = textItemCategory5;
            systemTextItem1.Save();
        }
    }
}
