<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Burnit.aspx.cs" Inherits="BurnWebApp.Burnit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Burn it!</h1>
        Size in Byte: <asp:TextBox ID="txtByteCount" runat="server" Text="1024" /><br />
        Count:<asp:TextBox ID="txtCount" runat="server" Text="10" /><br />
        <asp:Button Text="Burn via Azure LB" runat="server" name="btnBurnViaAzureLB" OnClick="btnBurnViaAzureLB_Click" /><br />
        <asp:Button Text="Burn via local service" runat="server" name="btnBurnViaLocalService" OnClick="btnBurnViaLocalService_Click" />
    </form>
</body>
</html>
