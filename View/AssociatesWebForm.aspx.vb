Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Results
Imports System.Web.Script.Serialization
Imports challengeS4EAPI


Public Class AssociatesWebForm
    Inherits System.Web.UI.Page

    Dim controller As New AssociatesController()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click
        Try


            Dim result As IHttpActionResult = controller.GetAssociates()

            If TypeOf result Is OkNegotiatedContentResult(Of List(Of Associates)) Then
                Dim response As OkNegotiatedContentResult(Of List(Of Associates)) = DirectCast(result, OkNegotiatedContentResult(Of List(Of Associates)))
                Dim associates As List(Of Associates) = response.Content

                gridAssociates.DataSource = associates
                gridAssociates.DataBind()
            ElseIf TypeOf result Is BadRequestErrorMessageResult Then
                Dim response As BadRequestErrorMessageResult = DirectCast(result, BadRequestErrorMessageResult)
                Dim errorMessage As String = response.Message
            End If


        Catch ex As Exception
            lblMessage.Text = ex.Message
            gridAssociates.DataSource = Nothing
            gridAssociates.DataBind()

        End Try

    End Sub

    Protected Sub btnGetAssociate_Click(sender As Object, e As EventArgs) Handles btnGetAssociate.Click
        Try
            Dim associateId As Integer = Integer.Parse(txtAssociateId.Text)
            Dim result As IHttpActionResult = controller.GetAssociateById(associateId)


            If TypeOf result Is OkNegotiatedContentResult(Of Associates) Then
                Dim response As OkNegotiatedContentResult(Of Associates) = DirectCast(result, OkNegotiatedContentResult(Of Associates))
                Dim associate As Associates = response.Content

                Dim associatesList As New List(Of Associates)()
                associatesList.Add(associate)

                gridAssociates.DataSource = associatesList
                gridAssociates.DataBind()

                lblMessage.Text = ""
            ElseIf TypeOf result Is BadRequestErrorMessageResult Then
                Dim response As BadRequestErrorMessageResult = DirectCast(result, BadRequestErrorMessageResult)
                Dim errorMessage As String = response.Message

                lblMessage.Text = "Associate not found."
                gridAssociates.DataSource = Nothing
                gridAssociates.DataBind()
            End If
        Catch ex As Exception
            lblMessage.Text = "Associate not found." & ex.Message
            gridAssociates.DataSource = Nothing
            gridAssociates.DataBind()
        End Try
    End Sub

    Protected Sub btnGetAssociatesByFilters_Click(sender As Object, e As EventArgs) Handles btnGetAssociatesByFilters.Click
        Try
            Dim name As String = txtName.Text
            Dim cpf As String = txtCpf.Text
            Dim result As IHttpActionResult = controller.GetAssociatesByFilters(name, cpf)

            If TypeOf result Is OkNegotiatedContentResult(Of List(Of Associates)) Then
                Dim response As OkNegotiatedContentResult(Of List(Of Associates)) = DirectCast(result, OkNegotiatedContentResult(Of List(Of Associates)))
                Dim associates As List(Of Associates) = response.Content
                gridAssociates.DataSource = associates
                gridAssociates.DataBind()
                lblMessage.Text = ""

                If associates.Count <= 0 Then
                    lblMessage.Text = "No associates found with the given filters."
                End If
            ElseIf TypeOf result Is BadRequestErrorMessageResult Then
                Dim response As BadRequestErrorMessageResult = DirectCast(result, BadRequestErrorMessageResult)
                Dim errorMessage As String = response.Message

                lblMessage.Text = "No associates found with the given filters."
                gridAssociates.DataSource = Nothing
                gridAssociates.DataBind()
            End If
        Catch ex As Exception
            lblMessage.Text = "No associates found with the given filters." & ex.Message
            gridAssociates.DataSource = Nothing
            gridAssociates.DataBind()
        End Try
    End Sub

    Protected Sub gridAssociates_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridAssociates.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim companiesList As IEnumerable(Of Object) = TryCast(DataBinder.Eval(e.Row.DataItem, "CompaniesList"), IEnumerable(Of Object))
            If companiesList IsNot Nothing Then
                Dim litCompanies As Literal = DirectCast(e.Row.FindControl("litCompanies"), Literal)
                For Each company As Object In companiesList
                    Dim companyName As String = TryCast(company.GetType().GetProperty("Name")?.GetValue(company, Nothing), String)
                    If Not String.IsNullOrEmpty(companyName) Then
                        litCompanies.Text += "<li>" & companyName & "</li>"
                    End If
                Next
            End If
        End If
    End Sub

    Protected Sub btnAddAssociate_Click(sender As Object, e As EventArgs) Handles btnAddAssociate.Click

        Try
            ' Obtenha os valores inseridos nos campos de entrada do novo associado
            Dim newName As String = txtNewName.Text
            Dim newCpf As String = txtNewCpf.Text
            Dim newBirthDay As Date = Date.Parse(txtNewBirthDay.Text)
            Dim newCompanyIds As New List(Of Integer)
            Dim outCompanyId As Integer

            ' Obtenha os IDs das empresas fornecidos na entrada e divida-os em uma lista
            Dim companyIdsInput As String = txtNewCompanyIds.Text
            Dim companyIdsArray As String() = companyIdsInput.Split(","c)
            For Each companyIdStr As String In companyIdsArray
                If Integer.TryParse(companyIdStr.Trim(), outCompanyId) Then
                    newCompanyIds.Add(outCompanyId)
                End If
            Next

            ' Crie um novo objeto de associado com os valores inseridos
            Dim newAssociate As New Associates()
            newAssociate.Name = newName
            newAssociate.Cpf = newCpf
            newAssociate.BirthDay = newBirthDay
            newAssociate.CompanyIds = newCompanyIds


            ' Chame o método AddAssociate da controller e manipule a resposta
            Dim response As IHttpActionResult = controller.AddAssociate(newAssociate)
            If TypeOf response Is OkResult Then
                ' Cadastro bem-sucedido, limpe os campos de entrada e exiba uma mensagem de sucesso
                ClearNewAssociateFields()
                lblMessage.Text = "Associado cadastrado com sucesso!"
                lblMessage.CssClass = "success-message"
            Else
                ' Ocorreu um erro no cadastro, exiba uma mensagem de erro
                Dim errorMessage As String = GetErrorMessage(response)
                lblMessage.Text = "Erro ao cadastrar o associado: " & errorMessage
                lblMessage.CssClass = "error-message"
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ClearNewAssociateFields()
        ' Limpe os campos de entrada do novo associado
        txtNewName.Text = String.Empty
        txtNewCpf.Text = String.Empty
        txtNewBirthDay.Text = String.Empty
        txtNewCompanyIds.Text = String.Empty
    End Sub

    Private Function GetErrorMessage(response As IHttpActionResult) As String
        ' Obtenha a mensagem de erro a partir da resposta da API
        Dim errorMessage As String = String.Empty
        If TypeOf response Is BadRequestErrorMessageResult Then
            Dim badRequestResult As BadRequestErrorMessageResult = DirectCast(response, BadRequestErrorMessageResult)
            errorMessage = badRequestResult.Message
        End If
        Return errorMessage
    End Function

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles gridAssociates.SelectedIndexChanged
        Dim btnDelete As Button = TryCast(sender, Button)
        If btnDelete IsNot Nothing Then
            Dim associateId As Integer = Convert.ToInt32(btnDelete.CommandArgument)

            ' Chamar o método DeleteAssociate da controller passando o associateId '
            Dim controller As New AssociatesController()
            Dim result As IHttpActionResult = controller.DeleteAssociate(associateId)

            ' Verificar o resultado da ação e atualizar a exibição do GridView se necessário '
            If TypeOf result Is OkNegotiatedContentResult(Of Integer) Then
                Dim deletedAssociateId As Integer = DirectCast(result, OkNegotiatedContentResult(Of Integer)).Content

                ' Atualizar a mensagem de sucesso com a ID do associado deletado '
                lblMessage.Text = "Associado " & deletedAssociateId & " deletado com sucesso."

                ' Chamar o método Button1_Click para atualizar o GridView após a exclusão '
                Button1_Click(sender, e)
            Else
                ' Tratar o erro de exclusão, se necessário '
                Dim errorMessage As String = "Erro ao deletar associado."
                lblMessage.Text = errorMessage
            End If
        End If
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles gridAssociates.SelectedIndexChanged
        Dim btnUpdate As Button = TryCast(sender, Button)
        If btnUpdate IsNot Nothing Then
            Dim row As GridViewRow = TryCast(btnUpdate.NamingContainer, GridViewRow)
            If row IsNot Nothing Then
                Dim associateId As Integer = Convert.ToInt32(btnUpdate.CommandArgument)

                ' Redirecione para o formulário de edição com o ID do associado como parâmetro '
                Response.Redirect($"EditAssociateForm.aspx?id={associateId}")
            End If
        End If
    End Sub

End Class