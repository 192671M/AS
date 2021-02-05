<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITCONNECT.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LcHEegZAAAAAIAPz3ETM6ee5B6IB-YqMk8lu80w"></script>
</head>
<body>
    <fieldset>
        <legend>Login</legend>
        <form id="form1" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbl_email" runat="server" Text="Email: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_pwd" runat="server" Text="Password :"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btn_register" runat="server" Text="Register" OnClick="btn_register_click" />
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btn_resetPwd" runat="server" Text="Reset password" OnClick="btn_resetPwd_Click" />
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <div>
                <asp:Label ID="lbl_error" runat="server" EnableViewState="False"></asp:Label>
                <br />
                <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
                <br />
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            </div>
        </form>
    </fieldset>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LcHEegZAAAAAIAPz3ETM6ee5B6IB-YqMk8lu80w', { action: 'login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
