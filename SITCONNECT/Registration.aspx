<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITCONNECT.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Form</title>

    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

            if (str.length < 8) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length must be at least 8 characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short;");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[^A-Za-z0-9_]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }
            document.getElementById("lbl_pwdchecker").innerHTML = "Excellent!";
            document.getElementById("lbl_pwdchecker").style.color = "Blue";
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 470px;
        }

        .auto-style2 {
            width: 238px;
        }

        .auto-style3 {
            width: 238px;
            height: 33px;
        }

        .auto-style4 {
            height: 33px;
        }

        .auto-style5 {
            width: 470px;
            height: 33px;
        }
    </style>
</head>
<body>
    <fieldset>
        <legend>Register</legend>
        <form id="form1" runat="server">
            <div>

                <br />
                <table>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lbl_fname" runat="server" Text="First Name: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_fname" runat="server" Width="218px"></asp:TextBox>
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lbl_lname" runat="server" Text="Last Name: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_lname" runat="server" Width="218px"></asp:TextBox>
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="lbl_bdate" runat="server" Text="Birthdate: "></asp:Label>
                        </td>
                        <td class="auto-style4">
                            <asp:TextBox ID="tb_bdate" runat="server" Width="218px" placeholder="MM/DD/YYYY" TextMode="Date"></asp:TextBox>
                        </td>
                        <td class="auto-style5"></td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lbl_cardno" runat="server" Text="Card Number: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_cardNo" runat="server" Width="218px" autocomplete="cc-number" placeholder="•••• •••• •••• ••••" TextMode="Password"></asp:TextBox>
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">
                            <asp:RegularExpressionValidator ID="validator_card" runat="server" ControlToValidate="tb_cardNo" ErrorMessage="Please enter a valid credit card number" ForeColor="Red" ValidationExpression="^4[0-9]{12}(?:[0-9]{3})?$|^5[1-5][0-9]{14}$|^2(?:2(?:2[1-9]|[3-9][0-9])|[3-6][0-9][0-9]|7(?:[01][0-9]|20))[0-9]{12}$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lbl_expiryDate" runat="server" Text="Expiry date:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="tb_expiryDate" runat="server" Width="218px" placeholder="••/••"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">
                            <asp:RegularExpressionValidator ID="validator_expiryDate" runat="server" ControlToValidate="tb_expiryDate" ErrorMessage="Please enter a valid expiry date" ForeColor="Red" ValidationExpression="^(0[1-9]|1[0-2])\/(2|3)\d{1}$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lbl_cvc" runat="server" Text="CVC Number:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="tb_cvc" runat="server" Width="218px" placeholder="•••"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">
                            <asp:RegularExpressionValidator ID="validator_cvc" runat="server" ControlToValidate="tb_cvc" ErrorMessage="Please enter a valid cvc" ForeColor="Red" ValidationExpression="^[0-9]{3}$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lbl_email" runat="server" Text="Email: "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_email" runat="server" Width="218px"></asp:TextBox>
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">
                            <asp:RegularExpressionValidator ID="validator_email" runat="server" ControlToValidate="tb_email" ErrorMessage="Please enter a valid email" ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Label ID="lbl_pwd" runat="server" Text="Password :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_pwd" runat="server" onkeyup="javascript:validate()" TextMode="Password" Width="218px"></asp:TextBox>
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td colspan="2">
                            <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Button ID="btn_register" runat="server" Text="Register" OnClick="btn_register_Click" />
                        </td>
                        <td>&nbsp;</td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
                        </td>
                        <td>&nbsp;</td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                </table>
                &nbsp;<asp:Label ID="lbl_msg" runat="server"></asp:Label>
                <br />
            </div>
        </form>
    </fieldset>
</body>
</html>
