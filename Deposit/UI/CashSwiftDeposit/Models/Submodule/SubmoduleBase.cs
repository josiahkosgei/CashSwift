﻿using CashSwiftDeposit.ViewModels;
using CashSwiftUtil.Licensing;
using System;

namespace CashSwiftDeposit.Models.Submodule
{
    public abstract class SubmoduleBase
    {
        public CDMLicense License { get; set; }

        public string ModuleName { get; set; }

        public Guid ModuleID { get; set; }

        protected ApplicationViewModel ApplicationViewModel { get; }

        public SubmoduleBase(
          ApplicationViewModel applicationViewModel,
          CDMLicense license,
          Guid subModuleID,
          string subModuleName)
        {
            ApplicationViewModel = applicationViewModel;
            License = license;
            ModuleID = subModuleID;
            ModuleName = subModuleName;
        }
    }
}
