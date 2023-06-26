Imports System.Net
Imports System.Web.Http
Imports Newtonsoft.Json.Linq

<RoutePrefix("api/associates")>
Public Class AssociatesController
    Inherits ApiController

    Private repository As New AssociatesRepository()

    <HttpGet>
    Public Function GetAssociates() As IHttpActionResult
        Try
            Dim associates = repository.GetAssociates()
            Return Ok(associates)
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try

    End Function

    <HttpGet>
    Public Function GetAssociateById(id As Integer) As IHttpActionResult

        Try
            Dim associate = repository.GetAssociateById(id)
            Return Ok(associate)
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try

    End Function


    'GET /api/associates/search?name=John&cpf=1234567890    
    <HttpGet>
    <Route("search")>
    Public Function GetAssociatesByFilters(<FromUri> name As String, <FromUri> cpf As String) As IHttpActionResult
        Dim associates = repository.GetAssociatesByFilters(name, cpf)

        Return Ok(associates)
    End Function


    <HttpPost>
    Public Function AddAssociate(associate As Associates) As IHttpActionResult
        Try
            If ModelState.IsValid Then
                Dim companyIds As List(Of Integer) = associate.CompanyIds

                repository.AddAssociate(associate, companyIds)
                Return Ok()
            Else
                BadRequest(ModelState)
            End If
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try

    End Function



    <HttpPut>
    Public Function UpdateAssociate(associate As Associates) As IHttpActionResult
        Try
            If ModelState.IsValid Then
                Dim companyIds As List(Of Integer) = associate.CompanyIds
                repository.UpdateAssociate(associate, companyIds)
                Return Ok()
            Else
                Return BadRequest(ModelState)
            End If
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try

    End Function

    <HttpDelete>
    Public Function DeleteAssociate(id As Integer) As IHttpActionResult

        Try
            repository.DeleteAssociate(id)
            Return Ok(id)
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try


    End Function
End Class
