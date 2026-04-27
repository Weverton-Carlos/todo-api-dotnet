# 🚀 Todo API Desafio

API RESTful para gerenciamento de tarefas (ToDo), desenvolvida com **.NET 8**, aplicando boas práticas de arquitetura, testes e containerização.

---

## 📌 Sobre o projeto

A aplicação permite o gerenciamento de tarefas com operações completas de:

* Criar
* Listar
* Atualizar
* Remover (soft delete)

Cada tarefa possui:

* ID
* Título
* Descrição
* Status (`Pending`, `InProgress`, `Completed`)
* Data de vencimento
* Auditoria (CreatedAt, UpdatedAt)

---

## 🧠 Decisões técnicas e diferenciais

### 🗂 Organização e persistência

* Uso de **Entity Framework Core**
* Configurações separadas com `IEntityTypeConfiguration`

  * Mantém o `DbContext` limpo
  * Facilita manutenção e evolução

---

### 🧹 Soft Delete + Auditoria

* Implementado **soft delete** (`IsDeleted`)
* Filtro global (`QueryFilter`)
* Auditoria automática:

  * `CreatedAt`
  * `UpdatedAt`

---

### ⚡ Performance

* Índices criados para:

  * `Status`
  * `DueDate`

👉 Otimizando consultas com filtros

---

### 🧪 Testes

* Testes unitários na camada de serviço
* Uso de **Moq** para isolamento de dependências
* Uso de **FluentValidation** para validação desacoplada
* Validação de:

  * Regras de negócio
  * Cenários de erro
  * Comportamento do serviço

---

### 📦 Containerização

* Aplicação containerizada com **Docker**
* Banco SQL Server em container
* Execução simplificada via `docker-compose`
* Imagem publicada no Docker Hub

---

### 🔄 Resiliência e inicialização

* Retry configurado no EF Core (`EnableRetryOnFailure`)
* Execução automática de:

  * Migrations
  * Seed de dados

👉 Evita falhas por timing do banco

---

### 📦 Padronização de resposta

* Uso de `ApiResponse<T>`
* Retorno consistente para:

  * Sucesso
  * Erros
  * Validação

---

## 🏗 Arquitetura

Estrutura baseada em separação de responsabilidades:

```
src/
├── TodoDesafio.Api            → Controllers / Configuração
├── TodoDesafio.Application    → Services / DTOs / Validators
├── TodoDesafio.Domain         → Entidades / Interfaces / Regras
└── TodoDesafio.Infrastructure → EF Core / Repositórios / Migrations

tests/
└── TodoDesafio.Tests          → Testes unitários
```

---

## 🛠 Tecnologias utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* AutoMapper
* FluentValidation
* xUnit
* Moq
* Docker / Docker Compose
* Swagger (OpenAPI)

---

## 🐳 Docker Hub

Imagem disponível em:

👉 https://hub.docker.com/r/seu-usuario/todo-api-desafio

---

## ▶️ Como executar o projeto

### 🔹 Pré-requisitos

* Docker
* Docker Compose

---

## 🚀 Subindo com Docker Compose (recomendado)

```bash
docker compose up --build
```

---

## ▶️ Executando com imagem pronta (Docker Hub)

```bash
docker run -d -p 5000:8080 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal,1433;Database=TodoDb;User=sa;Password=Senhaforte@123;TrustServerCertificate=True" \
  wevertonleal/todo-api-desafio:v1
```

---

## 🌐 Acessando a API

Após subir os containers:

* API:

```
http://localhost:5000
```

* Swagger:

```
http://localhost:5000/swagger
```

---

## 🗄 Banco de dados

* SQL Server rodando em container
* Porta:

```
1433
```

* Connection String (interna do container):

```
Server=db;Database=TodoDb;User=sa;Password=Senhaforte@123;TrustServerCertificate=True
```

---

## 🔄 Inicialização automática

Ao subir a aplicação:

✔️ Migrations são aplicadas automaticamente  
✔️ Seed de dados é executado  
✔️ Retry evita falhas de conexão inicial  

---

## 📬 Exemplos de uso

### 🔹 Criar tarefa

```http
POST /api/todo
```

```json
{
  "title": "Estudar .NET",
  "description": "Revisar API",
  "status": 1,
  "dueDate": "2026-12-31"
}
```

---

### 🔹 Listar tarefas com filtro

```http
GET /api/todo?status=1&dueDate=2026-12-31
```

---

### 🔹 Atualizar tarefa

```http
PUT /api/todo/1
```

---

### 🔹 Remover tarefa (soft delete)

```http
DELETE /api/todo/1
```

---

## ✅ Regras de negócio implementadas

### 📌 Título

* Obrigatório
* Máx: 150 caracteres
* Não duplicado (considerando soft delete)

---

### 📌 Descrição

* Obrigatória
* Máx: 500 caracteres

---

### 📌 Data de vencimento

* Data válida
* Pode ser no passado (backlog)

---

### 📌 Status

* Valores permitidos:

  * 1 → Pending
  * 2 → InProgress
  * 3 → Completed

---

## 🧪 Executando os testes

```bash
dotnet test
```

---

## ⚠️ Observações

* O SQL Server pode levar alguns segundos para iniciar no Docker
* A aplicação utiliza retry automático para conexão com o banco
* Migrations e seed são executados automaticamente na inicialização

---

## 📌 Melhorias futuras

* Paginação
* Ordenação
* Autenticação (JWT)
* Logs estruturados
* Testes de integração

---

## 👨‍💻 Autor

Desenvolvido como parte de desafio técnico para vaga de desenvolvedor .NET.