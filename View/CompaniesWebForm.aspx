<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CompaniesWebForm.aspx.vb" Inherits="challengeS4EAPI.CompaniesWebForm" %>

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
            <h1>Companies</h1>

            <div class="form-group">
                <asp:Button ID="btnDisplay" runat="server" Text="Exibir Todas" CssClass="btn" />
            </div>

            <div class="form-group">
                <label for="txtCompanyId">Buscar Empresa por ID:</label>
                <asp:TextBox ID="txtCompanyId" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Button ID="btnGetCompany" runat="server" Text="Buscar" OnClick="btnGetCompany_Click" CssClass="btn" />
            </div>

            <div class="form-group">
                <label for="txtName">Busca por filtro:</label>
                <div class="form-inline">
                    <label for="txtName">Name:</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                    <label for="txtCnpj">CNPJ:</label>
                    <asp:TextBox ID="txtCnpj" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Button ID="btnGetCompaniesByFilters" runat="server" Text="Buscar" OnClick="btnGetCompaniesByFilters_Click" CssClass="btn" />
                </div>
            </div>
        </div>

        <div class="container">
            <div class="form-group">
                <h3>Cadastrar Nova Empresa</h3>
                <div class="form-inline">
                    <label for="txtNewName">Name:</label>
                    <asp:TextBox ID="txtNewName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-inline">
                    <label for="txtNewCnpj">CNPJ:</label>
                    <asp:TextBox ID="txtNewCnpj" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="form-group">
                <label for="txtAssociateIds">Associates IDs (separados por vírgula):</label>
                <asp:TextBox ID="txtAssociateIds" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
                </div>
                <asp:Button ID="btnAddCompany" runat="server" Text="Cadastrar" OnClick="btnAddCompany_Click" CssClass="btn" />
            </div>
        </div>

        <div class="container">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

            <div class="grid-container">
                <asp:GridView ID="gridCompanies" runat="server" AutoGenerateColumns="false" CssClass="gridview">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header" />
                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header" />
                        <asp:BoundField DataField="Cnpj" HeaderText="CNPJ" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header" />
                        <asp:TemplateField HeaderText="Associates" ItemStyle-CssClass="grid-cell" HeaderStyle-CssClass="grid-header">
                            <ItemTemplate>
                                <ul>
                                    <asp:Literal ID="litAssociates" runat="server"></asp:Literal>
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
