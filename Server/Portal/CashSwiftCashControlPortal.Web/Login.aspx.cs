using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Templates;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace CashSwiftCashControlPortal.Web
{
    public class LoginPage : BaseXafPage
    {
        protected HtmlHead Head1;
        protected HtmlForm form1;
        protected ASPxProgressControl ProgressControl;
        protected HtmlGenericControl Content;

        public override Control InnerContentPlaceHolder =>
            Content;
    }
}