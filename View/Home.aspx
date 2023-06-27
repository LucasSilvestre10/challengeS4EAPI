<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Home.aspx.vb" Inherits="challengeS4EAPI.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Home</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            padding: 20px;
        }
        
        h1 {
            margin-top: 0;
            font-size: 24px;
            color: #333;
            text-align: center;
        }

        .button-container {
            margin-top: 40px;
            text-align: center;
        }

        .button-container button {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            color: #fff;
            cursor: pointer;
            font-size: 18px;
            font-weight: bold;
            text-transform: uppercase;
            transition: background-color 0.3s;
        }

        .button-container button:nth-child(1) {
            background-color: #4CAF50;
        }

        .button-container button:nth-child(2) {
            background-color: #337ab7;
        }

        .button-container button:hover {
            opacity: 0.8;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Welcome to the ChallengeS4E</h1>
            <div class="button-container">
                <asp:Button ID="btnAssociates" runat="server" Text="Go to Associates" OnClick="btnAssociates_Click" />
                <asp:Button ID="btnCompanies" runat="server" Text="Go to Companies" OnClick="btnCompanies_Click" />
            </div>
        </div>
    </form>
</body>
</html>
