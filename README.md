# TodoDesafio.Api

API RESTful para gerenciamento de tarefas, desenvolvida com .NET 8 e SQL Server, seguindo boas práticas de arquitetura em camadas, testes unitários e organização do domínio.

---

## 🚀 Tecnologias utilizadas

* .NET 8 (ASP.NET Core Web API)
* Entity Framework Core
* SQL Server
* xUnit (testes unitários)
* Moq (mock de dependências)
* FluentAssertions (assertions fluentes)

---

## 🧱 Arquitetura do projeto

O projeto foi estruturado em camadas para garantir separação de responsabilidades e facilitar manutenção:

```
src/
├── TodoDesafio.Api
├── TodoDesafio.Application
├── TodoDesafio.Domain
├── TodoDesafio.Infrastructure
teste/
└──TodoDesafio.Tests
```

---

## 🧩 Decisões de arquitetura e implementação

### 🔹 Organização do Entity Framework

As configurações das entidades foram separadas utilizando `IEntityTypeConfiguration<T>`, mantendo o `DbContext` mais limpo e organizado.

Isso facilita:

* Manutenção do modelo
* Evolução do domínio
* Separação de responsabilidades

---

### 🔹 Soft Delete e Auditoria

Foi implementado:

* **Soft delete** com query filter global (`IsDeleted`)
* **Auditoria automática de datas** no `DbContext`

  * `CreatedAt`
  * `UpdatedAt`

Isso garante rastreabilidade dos registros sem exclusão física no banco.

---

### 🔹 Performance e otimização

Foram adicionados índices no banco para otimizar consultas frequentes:

* Filtros por status
* Filtros por data de vencimento

---

### 🔹 Testes unitários

A camada de serviço possui testes unitários com:

* Moq para isolamento de dependências
* FluentAssertions para validações mais legíveis
* xUnit como framework de testes

Isso garante validação das regras de negócio sem dependência de infraestrutura.

---

## ⚙️ Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

* .NET SDK 8+
* SQL Server (local ou via Docker)

---

## 🛠️ Configuração do banco de dados

Atualize a connection string no arquivo:

```
src/TodoDesafio.Api/appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TodoDb;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True;"
  }
}
```

---

## ▶️ Como executar a aplicação

### 1. Restaurar dependências

```bash
dotnet restore
```

### 2. Executar migrations

```bash
dotnet ef database update \
--project src/TodoDesafio.Infrastructure \
--startup-project src/TodoDesafio.Api
```

### 3. Rodar a API

```bash
dotnet run --project src/TodoDesafio.Api
```

### 4. Acessar a aplicação

Swagger:

* [http://localhost:5000/swagger](http://localhost:5000/swagger)
* [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## 🧪 Executando os testes

```bash
dotnet test
```

---

## 📌 Funcionalidades

* CRUD de tarefas
* Soft delete (exclusão lógica)
* Auditoria de criação e atualização
* Filtros otimizados com índices
* Validação de regras de negócio via testes unitários

---

## 📈 Melhorias futuras

* Autenticação JWT
* Versionamento de API
* Paginação e ordenação avançada
* Logging estruturado
* Dockerização da aplicação

---

## 👨‍💻 Autor

Projeto desenvolvido como desafio técnico, focado em boas práticas de arquitetura, organização de código e testes automatizados.
