Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Http
Imports System.Web.Http.Results
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports challengeS4EAPI

Public Class EditCompanyForm
    Inherits System.Web.UI.Page

    Private controller As New CompaniesController()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim companyId As Integer = Convert.ToInt32(Request.QueryString("id"))
            If companyId > 0 Then
                Dim response As IHttpActionResult = controller.GetCompanyById(companyId)
                If TypeOf response Is OkNegotiatedContentResult(Of Companies) Then
                    Dim company As Companies = DirectCast(DirectCast(response, OkNegotiatedContentResult(Of Companies)).Content, Companies)
                    PopulateCompanyForm(company)
                Else
                    lblMessage.Text = "Error retrieving company: " + response.ToString()
                End If
            End If
        End If
    End Sub

    Private Sub PopulateCompanyForm(ByVal company As Companies)
        txtCompanyId.Text = company.Id.ToString()
        txtCompanyName.Text = company.Name
        txtCompanyCnpj.Text = company.Cnpj

        Dim associates As List(Of Associates) = company.AssociatesList
        Dim associateIds As String = String.Join(", ", associates.Select(Function(a) a.Id.ToString()))
        txtAssociateIds.Text = associateIds


    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Dim companyId As Integer = Convert.ToInt32(txtCompanyId.Text)
        Dim company As New Companies()
        company.Id = companyId
        company.Name = txtCompanyName.Text
        company.Cnpj = txtCompanyCnpj.Text
        Dim associateIds As String = txtAssociateIds.Text.Trim()

        ' Limpar a lista atual de associados
        company.AssociatesList.Clear()

        ' Verificar se a string de IDs não está vazia
        If Not String.IsNullOrEmpty(associateIds) Then
            ' Dividir a string em IDs individuais
            Dim ids As String() = associateIds.Split(","c)
            Dim associateIdList As New List(Of Integer)()

            For Each id As String In ids
                Dim associateId As Integer
                If Integer.TryParse(id, associateId) Then
                    associateIdList.Add(associateId)
                End If
            Next

            company.AssociatesIds = associateIdList
        End If

        Dim response As IHttpActionResult = controller.UpdateCompany(company)

        If TypeOf response Is OkResult Then
            lblMessage.Text = "Company updated successfully."
        Else
            lblMessage.Text = "Error updating company: "
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Dim companyId As Integer = Convert.ToInt32(txtCompanyId.Text)
        Response.Redirect("CompaniesWebForm.aspx?id=" & companyId)
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        ' Lógica para redirecionar o usuário para a CompaniesWebForm
        Response.Redirect("CompaniesWebForm.aspx")
    End Sub

End Class
