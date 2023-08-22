
//BusinessObjects.Authentication..UserGroup


using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftUtil;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("Name")]
    public class UserGroup : XPLiteObject, ITreeNode, ITreeNodeImageProvider
    {
        private int fid;
        private string fname;
        private string fdescription;
        private UserGroup fparent_group;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public int id
        {
            get => fid;
            set => SetPropertyValue<int>(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Name")]
        [Persistent("name")]
        public string name
        {
            get => fname;
            set => SetPropertyValue(nameof(name), ref fname, value);
        }

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [DisplayName("Description")]
        public string description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(description), ref fdescription, value);
        }

        [Association("UserGroupReferencesUserGroup")]
        public UserGroup parent_group
        {
            get => fparent_group;
            set => SetPropertyValue(nameof(parent_group), ref fparent_group, value);
        }

        [Association("UserGroupReferencesUserGroup")]
        public XPCollection<UserGroup> UserGroupCollection => GetCollection<UserGroup>(nameof(UserGroupCollection));

        [Association("DeviceReferencesUserGroup")]
        public XPCollection<Device> Devices => GetCollection<Device>(nameof(Devices));

        [Association("ApplicationUserReferencesUserGroup")]
        public XPCollection<ApplicationUser> ApplicationUsers => GetCollection<ApplicationUser>(nameof(ApplicationUsers));

        public UserGroup(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        protected override void OnSaving()
        {
            base.OnSaving();
            IEnumerable<UserGroup> userGroups = new XPQuery<UserGroup>(Session).Select(c => c);
            Dictionary<int, UserGroup> lookup = userGroups.ToDictionary(type => type.id);
            if (HierarchyMethods.ContainsCycles(userGroups, type => lookup[type.parent_group.id]))
                throw new Exception("Looped reference detected in UserGroup hierarchy");
        }

        public List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> list = UserGroupCollection.ToList().Flatten(userGroup => userGroup.UserGroupCollection).SelectMany(x => x.ApplicationUsers).ToList();
            list.AddRange(ApplicationUsers);
            return list;
        }

        public IEnumerable<UserGroup> GetAllNodes() => UserGroupCollection == null ? Enumerable.Empty<UserGroup>() : UserGroupCollection.SelectMany(e => e.GetAllNodes());

        public List<Device> GetAllDevices()
        {
            List<Device> list = UserGroupCollection.ToList().Flatten(userGroup => userGroup.UserGroupCollection).SelectMany(x => x.Devices).ToList();
            list.AddRange(Devices);
            return list;
        }

        public IBindingList Children => UserGroupCollection;

        public string Name => name;

        public ITreeNode Parent => parent_group;

        public Image GetImage(out string imageName)
        {
            imageName = Children == null || Children.Count <= 0 ? "BO_Department" : "BO_Category";
            return ImageLoader.Instance.GetImageInfo(imageName).Image;
        }
    }
}
