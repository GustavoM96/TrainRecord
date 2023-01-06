# TrainRecord 🏋️‍♀️

TrainRecord é uma aplicação desenvolvida em ASP.NET CORE C# com a finalidade de gerenciar atividades de academia para alunos e professores.

# Tecnologias 💻

- AspNet.Core SDK 7.0.100-rc.2.22477.23

## Pacotes

- Mediatr: para gerenciar comandos e queries no padrão CQRS.
- FluentValidation: para validação dos campos de request.
- Mapster: para mapeamento dos objetos com Adapt<>().
- ErrorOr: para retorno de api com erro ou o response.
- Jwt: para autenticação do usuário.
- PasswordHasher: hasher de senha do usuário.
- Sqlite: apenas como banco de dados de desenvolvimento.

## Prettier

Como formatador de C# está sendo utilizado o [csharpier](https://csharpier.com) e suas configurações estão no arquivo .csharpierrc.json localizado na raiz deste projeto.

## Husky

As mensagens de commits deste repo seguem o seguinte regex
`^(feat|fix|ci|chore|docs|test|style|refactor|perf|build|revert)(\(.+?\))?: .{1,}$`.

No pré-commit executamos o comando `dotnet csharpier .` para formatar antes do commit.

# Rodando a aplicação ⚙️

Simplismente em seu terminal.

```sh
 git clone https://github.com/GustavoM96/TrainRecord.git
 dotnet build
 dotnet run --project src/TrainRecord.Api
```

# Arquitetura 🏛️

## Pastas

### Docs

- release: contém dados de cada release.
- todo: contendo os requesitos de desenvolvimento.

### Src

<img src="assets/Arch.png" alt="drawing" width="500px"/>

- Api - Camada de configuração de api, direcionar os dados de request e response para o usuário.

- Application - Camada responsável por criar os comandos e queries que conecta a api com a Infrastructure no padrão CQRS.

- Core - Camada de entidades, enums, exceptions e regras de negócio.

- Infrastructure - Camada que obtem dados externos como api, banco de dados, filas e outros.

### Assets

- contém imagens utilizadas.

# Documentação API 💼

Mapeamento das rotas

<img src="assets/RouteApi.jpg" alt="drawing" width="500px"/>

Entidades de Banco de dados

<img src="assets/TrainRecordDB.png" alt="drawing" width="500px"/>

## Criar conta

- Autenticação Anônimo
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
  "lastName": "Silva"
}
```

### Response 201 Created

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "email": "josé.silva@gmail.com",
  "firstName": "José",
  "lastName": "Silva"
}
```

## Login conta

- Autenticação Anônimo,
- Detalhes - autenticação do usuário

### Request

```
Post /api/auth/login
```

```json
{
  "email": "josé.silva@gmail.com",
  "password": "sd#fd$904&3jkdf"
}
```

### Response 200 Ok

```json
{ "idToken": "fs432jnj543hb-lsdsdasdsadasd-df4545" }
```

## Adicionar registro de alteração de atividade

- Autenticação Apenas o dono do recurso,
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

### Response 201 Created

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "activityId": "00000000-0000-0000-0000-000000000000",
  "weight": 20,
  "repetition": 4
}
```

## Listar todas as atividades do aluno

- Autenticação Apenas o dono do recurso,
- Detalhes - Listar todas as atividades do aluno

```
Post /api/activity
```

### Request

```json
sem corpo de requisição
```

### Response 200 Ok

```json
{
  [
    {
      "userId": "00000000-0000-0000-0000-000000000000",
      "activityId": "00000000-0000-0000-0000-000000000000",
      "weight": 20,
      "repetition": 4
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
  "name": "flexão de braço"
}
```

### Response 201 Created

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "name": "flexão de braço"
}
```

# Release 📦

- 0.1.0
  - Public release

# Autores 🧑‍🤝‍🧑

- Gustavo Henrique Messias [GitHub](https://github.com/GustavoM96) | [Porttfolio](https://gustavo-messias.vercel.app)

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/GustavoM96/TrainRecord/blob/main/LICENSE) license.
