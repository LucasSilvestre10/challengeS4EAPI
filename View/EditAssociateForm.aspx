<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditAssociateForm.aspx.vb" Inherits="challengeS4EAPI.EditAssociateForm" %>

<!DOCTYPE html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Edit Associate</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            padding: 20px;
        }

        h1 {
            margin-top: 0;
        }

        .form-container {
            background-color: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.3);
        }

        .form-container label {
            display: block;
            margin-bottom: 10px;
        }

        .form-container input[type="text"],
        .form-container input[type="date"] {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .form-container .btn-container {
            margin-top: 20px;
        }

        .form-container .btn-container button {
            padding: 8px 16px;
            border: none;
            border-radius: 4px;
            color: #fff;
            cursor: pointer;
            font-weight: bold;
        }

        .form-container .btn-container button[type="submit"] {
            background-color: #4CAF50;
        }

        .form-container .btn-container button[type="button"] {
            background-color: #e74c3c;
        }

        .message-label {
            margin-top: 10px;
            display: block;
            color: #f00;
            font-weight: bold;
        }

        .btn-back {
            padding: 8px 16px;
            background-color: #337ab7;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: bold;
        }

        .btn-back:hover {
            background-color: #286090;
        }
        
        .btn-update {
            background-color: #4CAF50;
        }

        .btn-cancel {
            background-color: #e74c3c;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h1>Edit Associate</h1>
            <div>
                <label for="txtAssociateId">Associate ID:</label>
                <asp:TextBox ID="txtAssociateId" runat="server" Enabled="false"></asp:TextBox>
            </div>
            <br />
            <div>
                <label for="txtName">Name:</label>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </div>
            <br />
            <div>
                <label for="txtCpf">CPF:</label>
                <asp:TextBox ID="txtCpf" runat="server"></asp:TextBox>
            </div>
            <br />
            <div>
                <label for="txtBirthDay">BirthDay:</label>
                <asp:TextBox ID="txtBirthDay" runat="server"></asp:TextBox>
            </div>
            <br />
            <div class="btn-container">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn-update" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" OnClick="btnCancel_Click" />
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>
            <br />
            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn-back" OnClick="btnBack_Click" />
        </div>
    </form>
</body>
</html>
