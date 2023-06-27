Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports System.Web.Routing

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        ' Configuração de Web API e serviços

        ' Rotas de Web API
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
)

        RouteTable.Routes.MapPageRoute("", "", "~/View/Home.aspx")
    End Sub
End Module
