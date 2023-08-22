using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Templates;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace CashSwiftCashControlPortal.Web
{
    public class Default : BaseXafPage
    {
        protected HtmlForm form2;
        protected ASPxProgressControl ProgressControl;
        protected HtmlGenericControl Content;

        protected override ContextActionsMenu CreateContextActionsMenu()
        {
            string[] containerNames = new string[] { "Edit", "RecordEdit", "ObjectsCreation", "ListView", "Reports" };
            return new ContextActionsMenu(this, containerNames);
        }

        public override Control InnerContentPlaceHolder =>
            Content;
    }
}