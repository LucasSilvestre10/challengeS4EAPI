Public Class Home
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnAssociates_Click(sender As Object, e As EventArgs) Handles btnAssociates.Click
        Response.Redirect("/View/AssociatesWebForm.aspx")
    End Sub

    Protected Sub btnCompanies_Click(sender As Object, e As EventArgs) Handles btnCompanies.Click
        Response.Redirect("/View/CompaniesWebForm.aspx")
    End Sub
End Class