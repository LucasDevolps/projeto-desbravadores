# 🚀 Projeto Desbravadores - Backend

Backend da aplicação **Projeto Desbravadores**, desenvolvido com:

- ✅ .NET 8
- ✅ ASP.NET Core Web API
- ✅ Clean Architecture
- ✅ JWT Authentication
- ✅ Docker
- ✅ GitHub Actions (CI)

---

## 🏗 Arquitetura

O backend segue os princípios de **Clean Architecture**, dividido em camadas:

```text
backend.dotnet/
  projeto.desbravadores.Api/            -> Camada de apresentação (Controllers, Program.cs, DI)
  projeto.desbravadores.Application/    -> Casos de uso, DTOs, interfaces de serviços
  projeto.desbravadores.Domain/         -> Entidades, regras de negócio, Value Objects
  projeto.desbravadores.Infrastructure/ -> Implementações (JWT, repositórios, integrações)
  projeto.desbravadores.sln
```

---

## 🔐 Autenticação

A autenticação é feita via JWT (JSON Web Token).

Fluxo:

 - Usuário faz login via /api/auth/login

 - Backend valida credenciais

 - Retorna:
	- Access Token
	- Refresh Token

 - O Access Token deve ser enviado no header:
 
```bash
Authorization: Bearer {token}
```
---

## 🐳 Executando com Docker
### 📦 Pré-requisitos

- Docker instalado
- Docker Compose instalado

--- 

## > Subir aplicação completa (API + SQL Server)
### Na raiz do projeto, execute:

```bash
docker compose up --build
```
--- 

Esse comando irá:

 - Construir a imagem da API
 - Subir o container do SQL Server
 - Aplicar automaticamente as migrations (caso configurado no startup)
 - Expor a API na porta 8080
 
--- 

## 🌐 Acessos
### API:

```bash
http://localhost:8080
```

### Swagger:
```bash
http://localhost:8080/swagger
```

---
## 🗄 Banco de Dados

O projeto utiliza SQL Server 2022 rodando em container Docker.

As migrations são aplicadas automaticamente no startup da aplicação via:


```C#
db.Database.Migrate();
```

Não é necessário rodar comandos `dotnet ef`.

---

## 🔄 CI - GitHub Actions

### ✅ Etapas da pipeline
- **Restore** (`dotnet restore`)
- **Build** (`dotnet build`)
- **Test** (`dotnet test`)
- **Docker Build** (build da imagem)

### 🚀 Gatilhos
- **Push** na branch `main`
- **Pull Request** para a branch `main`

---