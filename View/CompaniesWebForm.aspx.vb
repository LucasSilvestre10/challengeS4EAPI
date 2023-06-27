Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Results
Imports System.Web.Script.Serialization
Imports challengeS4EAPI

Public Class CompaniesWebForm
    Inherits System.Web.UI.Page

    Dim controller As New CompaniesController()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisplay.Click
        Try
            Dim result As IHttpActionResult = controller.GetCompanies()

            If TypeOf result Is OkNegotiatedContentResult(Of List(Of Companies)) Then
                Dim response As OkNegotiatedContentResult(Of List(Of Companies)) = DirectCast(result, OkNegotiatedContentResult(Of List(Of Companies)))
                Dim companies As List(Of Companies) = response.Content

                gridCompanies.DataSource = companies
                gridCompanies.DataBind()
            ElseIf TypeOf result Is BadRequestErrorMessageResult Then
                Dim response As BadRequestErrorMessageResult = DirectCast(result, BadRequestErrorMessageResult)
                Dim errorMessage As String = response.Message
            End If
        Catch ex As Exception
            lblMessage.Text = ex.Message
            gridCompanies.DataSource = Nothing
            gridCompanies.DataBind()
        End Try
    End Sub

    Protected Sub btnGetCompany_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetCompany.Click
        Try
            Dim companyId As Integer = Integer.Parse(txtCompanyId.Text)
            Dim result As IHttpActionResult = controller.GetCompanyById(companyId)

            If TypeOf result Is OkNegotiatedContentResult(Of Companies) Then
                Dim response As OkNegotiatedContentResult(Of Companies) = DirectCast(result, OkNegotiatedContentResult(Of Companies))
                Dim company As Companies = response.Content

                Dim companiesList As New List(Of Companies)()
                companiesList.Add(company)

                gridCompanies.DataSource = companiesList
                gridCompanies.DataBind()

                lblMessage.Text = ""
            ElseIf TypeOf result Is BadRequestErrorMessageResult Then
                Dim response As BadRequestErrorMessageResult = DirectCast(result, BadRequestErrorMessageResult)
                Dim errorMessage As String = response.Message

                lblMessage.Text = "Company not found."
                gridCompanies.DataSource = Nothing
                gridCompanies.DataBind()
            End If
        Catch ex As Exception
            lblMessage.Text = "Company not found." & ex.Message
            gridCompanies.DataSource = Nothing
            gridCompanies.DataBind()
        End Try
    End Sub

    Protected Sub btnGetCompaniesByFilters_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetCompaniesByFilters.Click
        Try
            Dim companyName As String = txtName.Text
            Dim companyCnpj As String = txtCnpj.Text
            Dim result As IHttpActionResult = controller.GetCompaniesByFilters(companyName, companyCnpj)

            If TypeOf result Is OkNegotiatedContentResult(Of List(Of Companies)) Then
                Dim response As OkNegotiatedContentResult(Of List(Of Companies)) = DirectCast(result, OkNegotiatedContentResult(Of List(Of Companies)))
                Dim companies As List(Of Companies) = response.Content

                gridCompanies.DataSource = companies
                gridCompanies.DataBind()
                lblMessage.Text = ""

                If companies.Count <= 0 Then
                    lblMessage.Text = "No companies found with the given filters."
                End If
            ElseIf TypeOf result Is BadRequestErrorMessageResult Then
                Dim response As BadRequestErrorMessageResult = DirectCast(result, BadRequestErrorMessageResult)
                Dim errorMessage As String = response.Message

                lblMessage.Text = "No companies found with the given filters."
                gridCompanies.DataSource = Nothing
                gridCompanies.DataBind()
            End If
        Catch ex As Exception
            lblMessage.Text = "No companies found with the given filters." & ex.Message
            gridCompanies.DataSource = Nothing
            gridCompanies.DataBind()
        End Try
    End Sub

    Protected Sub gridCompanies_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gridCompanies.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim associatesList As IEnumerable(Of Object) = TryCast(DataBinder.Eval(e.Row.DataItem, "associatesList"), IEnumerable(Of Object))
            If associatesList IsNot Nothing Then
                Dim litCompanies As Literal = DirectCast(e.Row.FindControl("litAssociates"), Literal)
                For Each associate As Object In associatesList
                    Dim associateName As String = TryCast(associate.GetType().GetProperty("Name")?.GetValue(associate, Nothing), String)
                    If Not String.IsNullOrEmpty(associateName) Then
                        litCompanies.Text += "<li>" & associateName & "</li>"
                    End If
                Next
            End If
        End If
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnDelete As Button = TryCast(sender, Button)
        If btnDelete IsNot Nothing Then
            Dim companyId As Integer = Convert.ToInt32(btnDelete.CommandArgument)

            ' Call the DeleteCompany method in the controller passing the companyId '
            Dim result As IHttpActionResult = controller.DeleteCompany(companyId)

            ' Check the result of the action and update the GridView display if needed '
            If TypeOf result Is OkNegotiatedContentResult(Of Integer) Then
                Dim deletedCompanyId As Integer = DirectCast(result, OkNegotiatedContentResult(Of Integer)).Content

                ' Update the success message with the ID of the deleted company '
                lblMessage.Text = "Company " & deletedCompanyId & " deleted successfully."

                ' Call the btnDisplay_Click method to update the GridView after deletion '
                btnDisplay_Click(sender, e)
            Else
                ' Handle the delete error, if necessary '
                Dim errorMessage As String = "Error deleting company."
                lblMessage.Text = errorMessage
            End If
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btnUpdate As Button = TryCast(sender, Button)
        If btnUpdate IsNot Nothing Then
            Dim row As GridViewRow = TryCast(btnUpdate.NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                Dim companyId As Integer = Convert.ToInt32(btnUpdate.CommandArgument)

                ' Redirect to the edit form with the company ID as a parameter '
                Response.Redirect($"EditCompanyForm.aspx?id={companyId}")
            End If
        End If
    End Sub

    Protected Sub btnAddCompany_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddCompany.Click
        Try
            Dim company As New Companies()
            company.Name = txtNewName.Text
            company.Cnpj = txtNewCnpj.Text

            ' Get the IDs of the associates from the input fields
            Dim associateIds As New List(Of Integer)()
            For Each associateIdText As String In txtAssociateIds.Text.Split(","c)
                Dim associateId As Integer
                If Integer.TryParse(associateIdText.Trim(), associateId) Then
                    associateIds.Add(associateId)
                End If
            Next

            company.AssociatesIds = associateIds

            Dim result As IHttpActionResult = controller.AddCompany(company)

            If TypeOf result Is OkResult Then
                ' Reset the input fields
                txtNewName.Text = String.Empty
                txtNewCnpj.Text = String.Empty
                txtAssociateIds.Text = String.Empty

                ' Display success message
                lblMessage.Text = "Company added successfully."

                ' Refresh the grid view
                btnDisplay_Click(sender, e)
            ElseIf TypeOf result Is BadRequestErrorMessageResult Then
                Dim response As BadRequestErrorMessageResult = DirectCast(result, BadRequestErrorMessageResult)
                Dim errorMessage As String = response.Message

                lblMessage.Text = errorMessage
            End If
        Catch ex As Exception
            lblMessage.Text = "Error adding company: " & ex.Message
        End Try
    End Sub

    Protected Sub btnDisplay0_Click(sender As Object, e As EventArgs) Handles btnDisplay0.Click
        Response.Redirect("Home.aspx")
    End Sub
End Class
