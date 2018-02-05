<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BurnWebApp.Burnit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Burn it!</h1>
        <table>
            <tr>
                <td>
                    Size in Byte: 
                </td>
                <td>
                    <asp:TextBox ID="txtByteCount" runat="server" Text="1024" />
                </td>
            </tr>
            <tr>
                <td>
                    Count:
                </td>
                <td>
                    <asp:TextBox ID="txtCount" runat="server" Text="10" />
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td>
                    <asp:Button Text="Burn via Azure LB" runat="server" name="btnBurnViaAzureLB" Width="150" OnClick="btnBurnViaAzureLB_Click" />
                </td>
            </tr>
            <tr>
                <td>

                </td>
                <td>
                    <asp:Button Text="Burn via local service" runat="server" name="btnBurnViaLocalService" Width="150" OnClick="btnBurnViaLocalService_Click" />
                </td>
            </tr>
            <tr>
                <td>

                </td>
                <td>
                    <asp:Button Text="Burn via WCF Service" runat="server" name="btnBurnViaWcfService" Width="150" OnClick="btnBurnViaWcfService_Click" />
                </td>
            </tr>
            <tr>
                <td></td>
                 <td>
                    <asp:Label Id="lblStatus" runat="server" />
                 </td>
            </tr>
        </table>      
    </form>
</body>
</html>
