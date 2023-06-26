Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Results


Public Class EditAssociateForm
    Inherits System.Web.UI.Page

    Private controller As New AssociatesController()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim associateId As Integer = Convert.ToInt32(Request.QueryString("id"))
            If associateId > 0 Then
                Dim response As IHttpActionResult = controller.GetAssociateById(associateId)
                If TypeOf response Is OkNegotiatedContentResult(Of Associates) Then
                    Dim associate As Associates = DirectCast(DirectCast(response, OkNegotiatedContentResult(Of Associates)).Content, Associates)
                    PopulateAssociateForm(associate)
                Else
                    lblMessage.Text = "Error retrieving associate: " + response.ToString()
                End If
            End If
        End If
    End Sub

    Private Sub PopulateAssociateForm(ByVal associate As Associates)
        txtAssociateId.Text = associate.Id.ToString()
        txtName.Text = associate.Name
        txtCpf.Text = associate.Cpf
        txtBirthDay.Text = associate.BirthDay.ToString("yyyy-MM-dd")
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Dim associateId As Integer = Convert.ToInt32(txtAssociateId.Text)
        Dim associate As New Associates()
        associate.Id = associateId
        associate.Name = txtName.Text
        associate.Cpf = txtCpf.Text
        associate.BirthDay = DateTime.ParseExact(txtBirthDay.Text, "yyyy-MM-dd", Nothing)

        Dim response As IHttpActionResult = controller.UpdateAssociate(associate)

        If TypeOf response Is OkResult Then
            lblMessage.Text = "Associate updated successfully."
        Else
            lblMessage.Text = "Error updating associate: "
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Dim associateId As Integer = Convert.ToInt32(txtAssociateId.Text)
        Response.Redirect("AssociatesWebForm.aspx?id=" & associateId)
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        ' Lógica para redirecionar o usuário para a AssociatesWebForm
        Response.Redirect("AssociatesWebForm.aspx")
    End Sub

End Class
