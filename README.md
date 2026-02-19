# 🚀 Projeto Desbravadores - Fontend
> Preencher depois
--- 
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

O projeto segue os princípios de **Clean Architecture**, dividido em camadas:

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


### 🔹 Domain
- Entidades
- Regras de negócio
- Contratos (interfaces)

### 🔹 Application
- DTOs
- Use Cases
- Serviços de aplicação

### 🔹 Infrastructure
- Implementações de repositórios
- Geração de JWT
- Integrações externas

### 🔹 API
- Controllers
- Configuração de DI
- Middleware
- Autenticação/Autorização

---

## 🔐 Autenticação

A autenticação é feita via **JWT (JSON Web Token)**.

Fluxo:

1. Usuário faz login via `/api/auth/login`
2. Backend valida credenciais
3. Retorna:
   - Access Token
   - Refresh Token
4. O Access Token deve ser enviado no header

---

## ⚙️ Configuração

As configurações do JWT ficam no `appsettings.json`:

```json
"Jwt": {
  "Issuer": "projeto.desbravadores",
  "Audience": "projeto.desbravadores",
  "SigningKey": "CHAVE_SECRETA_AQUI",
  "AccessTokenMinutes": 30,
  "RefreshTokenDays": 7
}


