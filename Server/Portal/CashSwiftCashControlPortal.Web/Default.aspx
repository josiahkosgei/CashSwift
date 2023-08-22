<%@ Page Language="C#" AutoEventWireup="true" Inherits="CashSwiftCashControlPortal.Web.Default" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v17.1, Version=17.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v17.1, Version=17.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Main Page</title>
    <meta http-equiv="Expires" content="0" />
    <script type="text/javascript">
        window.xafViewRefreshTimer = null;
        window.startXafViewRefreshTimer = function (delay) {
            if (!window.xafViewRefreshTimer) {
                window.xafViewRefreshTimer = window.setInterval(onXafViewRefreshTimer, delay);
            }
        }
        window.stopXafViewRefreshTimer = function () {
            if (window.xafViewRefreshTimer) {
                window.clearInterval(window.xafViewRefreshTimer);
                window.xafViewRefreshTimer = null;
            }
        }
        window.isValidXafView = function () {
            return !GetActivePopupControl(window.parent) || !GetActivePopupControl(window);
        }
        window.onXafViewRefreshTimer = function () {
            if (isValidXafView()) {
                RaiseXafCallback(globalCallbackControl, 'KA18958', '', '', false);
            }
        }
    </script>

    <style type="text/css">
        .dxm-item.accountItem.dxm-subMenu .dx-vam
        {
            padding-left: 10px;
        }
        .dxm-item.accountItem.dxm-subMenu .dxm-image.dx-vam
        {
            border-radius: 32px;
            -moz-border-radius: 32px;
            -webkit-border-radius: 32px;
            padding-right: 0px !important;
            padding-left: 0px !important;
            max-height: 32px;
            max-width: 32px;
        }
    </style>
</head>
<body class="VerticalTemplate" >
    <form id="form2" runat="server">
    <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
    <div runat="server" id="Content" />
    </form>
</body>
</html>
