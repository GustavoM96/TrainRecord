# TrainRecord

TrainRecord é uma aplicação desenvolvida em ASP.NET CORE C# com a finalidade de gerenciar atividades de academia para alunos e professores.

# Tecnologias

# Rodando a aplicação

Simplismente em seu terminal

```sh
 git clone https://github.com/
 dotnet run --project src/TrainRecord.Api
```

# Arquitetura

## Pastas

### Docs

- release - contém dados de cada release

### Src

- Api - Camada de configuração de api, direcionar os dados de request e response para o usuário

- Application - Camada respnsável por criar os comandos e queries que conecta a api com a Infrastructure no padrão CQRS

- Core - Camada de entidades, enums, exceptions e regras de negócio

- Infrastructure - Camada que obtem dados externos como api, banco de dados, filas e outros

## Prettier

como formatador de C# está sendo utilizado o [csharpier](https://csharpier.com) e suas configurações estão no arquivo .csharpierrc.json localizado na raiz deste projeto

# Documentação API

## Criar conta

- Autenticação - Anônimo
- Detalhes - registrar usuário dentro da plataforma

### Request

```
Post /api/auth/register
```

```json
{
  "email": "josé.silva@gmail.com",
  "password": "sd#fd$904&3jkdf",
  "firstName": "José",
  "LastName": "Silva"
}
```

### Response

```json
201 Created
```

## Login conta

- Autenticação - Anônimo,
- Detalhes - autenticação do usuário

### Request

```
Post /api/auth/login
```

```json
{
  "email": "string",
  "password": "string"
}
```

### Response

```json
200 OK
```

```json
{ "idToken": "fs432jnj543hb-lsdsdasdsadasd-df4545" }
```

## Adicionar registro de alteração de atividade

- Autenticação - Apenas o dono do recurso,
- Detalhes - Adicionar registro de alteração de atividade

```
Post /api/activity/{id}/record
```

### Request

```json
{
  "weight": 20,
  "repetition": 4
}
```

### Response

```json
201 Created
```

## Listar todas as atividades do aluno

- Autenticação - Apenas o dono do recurso,
- Detalhes - Listar todas as atividades do aluno

```
Post /api/activity
```

### Request

```json
sem corpo de requisição
```

### Response

```json
200 OK
```

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "userActivities": [
    {
      "activityDiscription": {
        "id": "00000000-0000-0000-0000-000000000000",
        "name": "flexão de braço",
        "muscles": ["trícipes", "peitoral"]
      },
      "Records": [
        {
          "weight": 20,
          "repetition": 4,
          "createdAt": "2022-03-04 00:00:00"
        }
      ]
    }
  ]
}
```

## Adicionar uma nova atividade

- Autenticação - Apenas Adm,
- Detalhes - Adicionar uma nova atividade

```
Post /api/activity
```

### Request

```json
{
  "name": "flexão de braço",
  "muscles": ["trícipes", "peitoral"]
}
```

### Response

```json
201 Created
```

# Release

# Autores

- Gustavo Henrique Messias [GitHub](https://github.com/GustavoM96)

# License

This project is licensed under the terms of the [MIT]() license.
