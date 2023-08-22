using CashSwiftCashControlPortal.Module.Controllers;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.TestScripts;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CashSwiftCashControlPortal.Web
{
    public class ErrorPage : Page
    {
        protected HtmlHead Head1;
        protected HtmlForm form1;
        protected Literal ApplicationTitle;
        protected Literal InfoMessagesPanel;
        protected Table Table1;
        protected TableRow TableRow2;
        protected TableCell ViewSite;
        protected Literal ErrorTitleLiteral;
        protected Panel ErrorPanel;
        protected PlaceHolder ApologizeMessage;
        protected HyperLink HyperLink1;
        protected LinkButton NavigateToStart;
        protected Panel Details;
        protected Literal DetailsText;
        protected Panel ReportForm;
        protected TextBox DescriptionTextBox;
        protected Button ReportButton;

        private void ErrorPage_PreRender(object sender, EventArgs e)
        {
            RegisterThemeAssemblyController.RegisterThemeResources((Page)sender);
        }

        private void InitializeComponent()
        {
            Load += new EventHandler(Page_Load);
            PreRender += new EventHandler(ErrorPage_PreRender);
        }

        protected override void InitializeCulture()
        {
            if (WebApplication.Instance != null)
            {
                WebApplication.Instance.InitializeCulture();
            }
        }

        protected void NavigateToStart_Click(object sender, EventArgs e)
        {
            WebApplication.Instance.LogOff();
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            BaseXafPage.SetupCurrentTheme();
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (TestScriptsManager.EasyTestEnabled)
            {
                TestScriptsManager manager = new TestScriptsManager(Page);
                manager.RegisterControl("Label", "FormCaption", TestControlType.Field, "FormCaption");
                manager.RegisterControl("Label", "DescriptionTextBox", TestControlType.Field, "Description");
                manager.RegisterControl("DefaultControl", "ReportButton", TestControlType.Action, "Report");
                manager.AllControlRegistered();
                ClientScript.RegisterStartupScript(GetType(), "EasyTest", manager.GetScript(), true);
            }
            ApplicationTitle.Text = (WebApplication.Instance == null) ? "No application" : WebApplication.Instance.Title;
            Header.Title = "Application Error - " + ApplicationTitle.Text;
            ErrorInfo applicationError = ErrorHandling.GetApplicationError();
            if (applicationError == null)
            {
                ErrorPanel.Visible = false;
            }
            else
            {
                if (!ErrorHandling.CanShowDetailedInformation)
                {
                    Details.Visible = false;
                }
                else
                {
                    Exception exception = CustomErrorController.HandleException(applicationError.Exception);
                    DetailsText.Text = exception.Message;
                }
                ReportForm.Visible = ErrorHandling.CanSendAlertToAdmin;
            }
        }

        protected void ReportButton_Click(object sender, EventArgs e)
        {
            ErrorInfo applicationError = ErrorHandling.GetApplicationError();
            if (applicationError != null)
            {
                ErrorHandling.SendAlertToAdmin(applicationError.Id, DescriptionTextBox.Text, applicationError.Exception.Message);
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Your report has been sent. Thank you.');", true);
            }
        }
    }
}