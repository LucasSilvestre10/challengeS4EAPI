<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AssociatesWebForm.aspx.vb" Inherits="challengeS4EAPI.AssociatesWebForm" %>

<!DOCTYPE html>

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 800px;
            margin: 20px auto;
            padding: 20px;
            background-color: #fff;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
        }

        h1 {
            font-size: 24px;
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 10px;
        }

        label {
            display: block;
            margin-bottom: 5px;
        }

        .form-control {
            width: 100%;
            padding: 5px;
            font-size: 14px;
        }

        .btn {
            display: inline-block;
            padding: 5px 10px;
            background-color: #007bff;
            color: #fff;
            text-decoration: none;
            border: none;
            cursor: pointer;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .grid-container {
            margin-top: 20px;
        }

        .grid-header {
            background-color: #007bff;
            color: #fff;
            font-weight: bold;
            text-align: left;
            padding: 10px;
        }

        .grid-row {
            background-color: #f9f9f9;
        }

        .grid-cell {
            padding: 5px;
        }

        .btn-delete {
            background-color: #dc3545;
        }

        .btn-delete:hover {
            background-color: #c82333;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:Button ID="btnDisplay0" runat="server" Text="Home" CssClass="btn" />
            <h1>Associates</h1>

            <div class="form-group">
                <asp:Button ID="btnDisplay" runat="server" Text="Exibir Todos" CssClass="btn" />
            </div>

            <div class="form-group">
                <label for="txtAssociateId">Buscar Associado por ID:</label>
                <asp:TextBox ID="txtAssociateId" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Button ID="btnGetAssociate" runat="server" Text="Buscar" OnClick="btnGetAssociate_Click" CssClass="btn" />
            </div>

            <div class="form-group">
                <label for="txtName">Busca por filtro:</label>
                <div class="form-inline">
                    <label for="txtName">Name:</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                    <label for="txtCpf">CPF:</label>
                    <asp:TextBox ID="txtCpf" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Button ID="btnGetAssociatesByFilters" runat="server" Text="Buscar" OnClick="btnGetAssociatesByFilters_Click" CssClass="btn" />
                </div>
            </div>
        </div>

        <div class="container">
            <div class="form-group">
                <h3>Cadastrar Novo Associado</h3>
                <div class="form-inline">
                    <label for="txtNewName">Name:</label>
                    <asp:TextBox ID="txtNewName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-inline">
                    <label for="txtNewCpf">CPF:</label>
                    <asp:TextBox ID="txtNewCpf" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-inline">
                    <label for="txtNewBirthDay">BirthDay:</label>
                    <asp:TextBox type="date" ID="txtNewBirthDay" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-inline">
                    <label for="txtNewCompanyIds">Company IDs (separados por vírgula):</label>
                    <asp:TextBox ID="txtNewCompanyIds" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button ID="btnAddAssociate" runat="server" Text="Cadastrar" OnClick="btnAddAssociate_Click" CssClass="btn" />
            </div>
        </div>

        <div class="container">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

            <div class="grid-container">
                <asp:GridView ID="gridAssociates" runat="server" AutoGenerateColumns="false" CssClass="gridview">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header" />
                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header" />
                        <asp:BoundField DataField="Cpf" HeaderText="CPF" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header" />
                        <asp:BoundField DataField="BirthDay" HeaderText="BirthDay" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header" />
                        <asp:TemplateField HeaderText="Companies" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header">
                            <ItemTemplate>
                                <ul>
                                    <asp:Literal ID="litCompanies" runat="server"></asp:Literal>
                                </ul>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header">
                            <ItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="Editar" CommandName="Update" CommandArgument='<%# Eval("Id") %>' OnClick="btnEdit_Click" CssClass="btn" />
                                <asp:Button ID="btnDelete" runat="server" Text="Deletar" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' OnClick="btnDelete_Click" CssClass="btn-delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="grid-row" />
                    <HeaderStyle CssClass="grid-header" />
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
