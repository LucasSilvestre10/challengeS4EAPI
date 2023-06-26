Imports System.Web.Http

<RoutePrefix("api/companies")>
Public Class CompaniesController
    Inherits ApiController

    Private repository As New CompaniesRepository()

    <HttpGet>
    Public Function GetCompanies() As IHttpActionResult
        Dim companies = repository.GetCompanies()
        Return Ok(companies)
    End Function

    <HttpGet>
    Public Function GetCompanyById(id As Integer) As IHttpActionResult

        Try
            Dim company = repository.GetCompanyById(id)
            If company Is Nothing Then
                Return NotFound()
            End If
            Return Ok(company)

        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try


    End Function
    ' GET /api/companies/search?name=Example&cnpj=12345
    <HttpGet>
    <Route("search")>
    Public Function GetCompaniesByFilters(<FromUri> name As String, <FromUri> cnpj As String) As IHttpActionResult
        Try
            Dim companies = repository.GetCompaniesByFilters(name, cnpj)
            Return Ok(companies)
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try

    End Function

    <HttpPost>
    Public Function AddCompany(companyData As Companies) As IHttpActionResult
        Try
            If ModelState.IsValid Then
                Dim company As New Companies()
                company.Name = companyData.Name
                company.Cnpj = companyData.Cnpj
                repository.AddCompany(company, companyData.AssociatesIds)
                Return Ok()
            Else
                Return BadRequest(ModelState)
            End If
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try
    End Function


    <HttpPut>
    Public Function UpdateCompany(company As Companies) As IHttpActionResult
        Try
            If ModelState.IsValid Then
                repository.UpdateCompany(company)
                Return Ok()
            Else
                Return BadRequest(ModelState)
            End If
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try

    End Function

    <HttpDelete>
    Public Function DeleteCompany(id As Integer) As IHttpActionResult
        Try
            repository.DeleteCompany(id)
            Return Ok(id)
        Catch ex As Exception
            Return BadRequest(ex.Message)
        End Try

    End Function

End Class
