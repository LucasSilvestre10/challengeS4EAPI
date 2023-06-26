# Exemplos Para Acesso De Rotas

## Api Associates

JSON PARA TESTES
Caso já tenho cadastrado alguma compania e deseja vincular uma ou mais a novos associados só adicionar a id da mesma no campo CompanyIds!

    {
        "name": "teste",
        "cpf": "8854774178",
        "birthDay": "1996-10-18",
        "CompanyIds": []
    }

#### GET
challenges4eapi.azurewebsites.net/api/associates
localhost:{PORTALOCAL}/api/associates/

#### GET ID
troque :id por um numero desejado!

challenges4eapi.azurewebsites.net/api/associates/:id
localhost:{PORTALOCAL}/api/associates/:id

#### GET COM FILTROS
troque na url os "exemplo" por seu valor de filtro de busca

challenges4eapi.azurewebsites.net/api/associates/search?name=exemplo&cpf=exemplo
localhost:{PORTALOCAL}/api/associates/search?name=exemplo&cpf=exemplo

#### POST

challenges4eapi.azurewebsites.net/api/associates
localhost:{PORTALOCAL}/api/associates

#### PUT
nesse caso é necessario adicionar ao json o campo "id"

JSON:

    {
		"id": 1,
        "name": "teste",
        "cpf": "8854774178",
        "birthDay": "1996-10-18",
        "CompanyIds": []
    }

challenges4eapi.azurewebsites.net/api/associates
localhost:{PORTALOCAL}/api/associates

#### DELETE
troque :id por um numero desejado!
deve retornar o id do associado deletado.

challenges4eapi.azurewebsites.net/api/associates/:id
localhost:{PORTALOCAL}/api/associates/:id




## Api Companies

JSON PARA TESTES:
Caso já tenho cadastrado algum associado e deseja vincular a novas companias só adicionar a id dos mesmos no campo associatesIds!

    {
		"Name": "Company do teste atualizada",
        "Cnpj": "12376789078321",
        "associatesIds" : []
    }


#### GET
challenges4eapi.azurewebsites.net/api/companies
localhost:{PORTALOCAL}/api/companies/

#### GET ID
troque :id por um numero desejado!

challenges4eapi.azurewebsites.net/api/companies/:id
localhost:{PORTALOCAL}/api/companies/:id

#### GET COM FILTROS
troque na url os "exemplo" por seu valor de filtro de busca

challenges4eapi.azurewebsites.net/api/companies/search?name=exemplo&cpf=exemplo
localhost:{PORTALOCAL}/api/companies/search?name=exemplo&cpf=exemplo

#### POST

challenges4eapi.azurewebsites.net/api/companies
localhost:{PORTALOCAL}/api/companies

#### PUT
nesse caso é necessario adicionar ao json o campo "id"

    {
        "Id": 31,
        "Name": "Company do teste atualizada",
        "Cnpj": "12376789078321",
        "associatesIds" : [11,12,32]
    }


challenges4eapi.azurewebsites.net/api/companies
localhost:{PORTALOCAL}/api/companies

#### DELETE
troque :id por um numero desejado!
deve retornar o id do associado deletado.

challenges4eapi.azurewebsites.net/api/companies/:id
localhost:{PORTALOCAL}/api/companies/:id






