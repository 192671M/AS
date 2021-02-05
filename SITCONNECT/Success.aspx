<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="SITCONNECT.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Success</title>
    <style type="text/css">
        .auto-style1 {
            width: 144px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <p>
            <asp:Label ID="lbl_success" runat="server" Text="Congralution. You have logged in."></asp:Label>
        </p>
        <p>
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1">Email:</td>
                    <td>
                        <asp:Label ID="lbl_email" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Card Number:</td>
                    <td>
                        <asp:Label ID="lbl_cardNo" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Expiry Date:</td>
                    <td>
                        <asp:Label ID="lbl_expiryDate" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">CVC:</td>
                    <td>
                        <asp:Label ID="lbl_cvc" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </p>
        <div>
            <asp:Button ID="btnLogout" runat="server" OnClick="btnLogout_Click" Text="Logout" Visible="false" />
            <br />
            <br />
            <p>Countdown timer:</p>
            <p id="timer"></p>
        </div>
    </form>
</body>
<script>
    // Set the date we're counting down to
    var countDownDate = new Date().getTime() + 60000;

    // Update the count down every 1 second
    var x = setInterval(function () {

        // Get today's date and time
        var now = new Date().getTime();

        // Find the distance between now and the count down date
        var distance = countDownDate - now;

        // Time calculations for seconds
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Output the result in an element with id="timer"
        document.getElementById("timer").innerHTML = seconds + "s ";

        // If the count down is over, write some text 
        if (distance < 0) {
            clearInterval(x);
            document.getElementById("timer").innerHTML = "EXPIRED";
        }
    }, 1000);
</script>
</html>

