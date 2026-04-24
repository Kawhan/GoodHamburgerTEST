# 🍔 Good Hamburger — API de Pedidos

API REST para gerenciamento de pedidos de uma lanchonete, com cálculo automático de descontos por combo.

Desenvolvido com **.NET 10**, **Entity Framework Core**, **PostgreSQL** e **Scalar** para documentação interativa.

---

## Sobre o Projeto

A **Good Hamburger** é uma API que permite registrar e gerenciar pedidos de uma lanchonete, aplicando regras de negócio como limite de itens por pedido e descontos progressivos por combinação de produtos (combos).

### Cardápio

| Sanduíches       | Preço  | Acompanhamentos | Preço  |
|------------------|--------|-----------------|--------|
| X Burger         | R$ 5,00 | Batata Frita    | R$ 2,00 |
| X Egg            | R$ 4,50 | Refrigerante    | R$ 2,50 |
| X Bacon          | R$ 7,00 |                 |        |

### Regras de Desconto

| Combo                              | Desconto |
|-------------------------------------|----------|
| Sanduíche + Batata + Refrigerante   | **20%**  |
| Sanduíche + Refrigerante            | **15%**  |
| Sanduíche + Batata                  | **10%**  |

---

## Stack & Tecnologias

- **.NET 10** / C# 14
- **Entity Framework Core 10** (Code First + Migrations + Seed Data)
- **PostgreSQL** como banco de dados
- **Scalar** para documentação OpenAPI interativa
- **Middleware** customizado para tratamento global de exceções
- **Docker + Docker Compose**

---

## Arquitetura

```
GoodHamburgerProject/
├── Controllers/         # Endpoints da API (Order, Burger, Accompaniment, Menu)
├── DTOs/                # Data Transfer Objects (Request/Response)
├── Data/                # DbContext + EntityTypeConfigurations com Seed Data
├── Enums/               # ProductTypeEnum (Burger, Accompaniment)
├── Exceptions/          # Exceções de domínio tipadas com hierarquia
├── Mappers/             # Extension methods para conversão Model → DTO
├── Middlewares/         # ExceptionMiddleware (tratamento global de erros)
├── Models/              # Entidades (Order, Burger, Accompaniment, Discount)
├── Repositories/        # Camada de acesso a dados com interfaces
├── Services/            # Regras de negócio com interfaces
├── Migrations/          # EF Core Migrations
└── Program.cs           # Configuração e DI
```

O projeto segue uma **arquitetura em camadas** com separação clara de responsabilidades:

- **Controller** → recebe a requisição e delega para o Service
- **Service** → aplica regras de negócio (validações, cálculos de desconto)
- **Repository** → encapsula o acesso ao banco de dados
- **Mapper** → converte entidades para DTOs de resposta

Todas as dependências são injetadas via **DI nativa do ASP.NET Core** com escopo `Scoped`.

---

## Endpoints

### 📋 Menu

| Método | Rota           | Descrição                     |
|--------|----------------|-------------------------------|
| GET    | `/api/Menu`    | Retorna o cardápio completo   |

### 🍔 Burgers

| Método | Rota               | Descrição                    |
|--------|---------------------|------------------------------|
| GET    | `/api/Burger`       | Lista todos os sanduíches    |
| GET    | `/api/Burger/{id}`  | Busca sanduíche por ID       |
| POST   | `/api/Burger`       | Cria um novo sanduíche       |
| PUT    | `/api/Burger`       | Atualiza um sanduíche        |
| DELETE | `/api/Burger/{id}`  | Remove um sanduíche          |

### 🍟 Acompanhamentos

| Método | Rota                       | Descrição                       |
|--------|-----------------------------|---------------------------------|
| GET    | `/api/Accompaniment`       | Lista todos os acompanhamentos  |
| GET    | `/api/Accompaniment/{id}`  | Busca acompanhamento por ID     |
| POST   | `/api/Accompaniment`       | Cria um novo acompanhamento     |
| PUT    | `/api/Accompaniment`       | Atualiza um acompanhamento      |
| DELETE | `/api/Accompaniment/{id}`  | Remove um acompanhamento        |

### 🧾 Pedidos

| Método | Rota              | Descrição                                      |
|--------|--------------------|-------------------------------------------------|
| GET    | `/api/Order`       | Lista todos os pedidos                          |
| GET    | `/api/Order/{id}`  | Busca pedido por ID                             |
| POST   | `/api/Order`       | Cria um pedido com cálculo automático de combo  |
| PUT    | `/api/Order/{id}`  | Atualiza pedido (recalcula totais e descontos)  |
| DELETE | `/api/Order/{id}`  | Remove um pedido                                |

---

## Exemplos de Uso

### Criar um pedido (Combo Completo — 20% de desconto)

**Request:**
```http
POST /api/Order
Content-Type: application/json
```
```json
{
  "items": [
    { "productType": 1, "productId": "11111111-1111-1111-1111-111111111111" },
    { "productType": 2, "productId": "44444444-4444-4444-4444-444444444444" },
    { "productType": 2, "productId": "55555555-5555-5555-5555-555555555555" }
  ]
}
```

> `productType`: 1 = Burger, 2 = Accompaniment

**Response (201 Created):**
```json
{
  "id": "a1b2c3d4-...",
  "totalAmount": 9.50,
  "discount": 1.90,
  "finalAmount": 7.60,
  "createdAt": "2026-04-22T14:00:00Z",
  "active": true,
  "items": [
    { "productType": 1, "productId": "11111111-...", "name": "X Burger", "price": 5.00 },
    { "productType": 2, "productId": "44444444-...", "name": "Batata Frita", "price": 2.00 },
    { "productType": 2, "productId": "55555555-...", "name": "Refrigerante", "price": 2.50 }
  ]
}

```

---

### Atualizar um pedido

Permite atualizar os itens de um pedido existente.  
O sistema recalcula automaticamente o total e o desconto com base na nova combinação.

**Request:**
```http
PUT /api/Order/{id}
Content-Type: application/json
```

```json
{
  "items": [
    { "productType": 1, "productId": "11111111-1111-1111-1111-111111111111" },
    { "productType": 2, "productId": "55555555-5555-5555-5555-555555555555" }
  ]
}
```

> `productType`: 1 = Burger, 2 = Accompaniment

**Response (200 Ok):**

```json
{
  "id": "a1b2c3d4-...",
  "totalAmount": 7.50,
  "discount": 1.13,
  "finalAmount": 6.38,
  "createdAt": "2026-04-22T14:00:00Z",
  "active": true,
  "items": [
    { "productType": 1, "productId": "11111111-...", "name": "X Burger", "price": 5.00 },
    { "productType": 2, "productId": "55555555-...", "name": "Refrigerante", "price": 2.50 }
  ]
}
```

---

## Regras de Negócio

- Cada pedido aceita **no máximo 1 sanduíche**
- **Itens duplicados** não são permitidos (retorna erro 400)
- O sistema aplica automaticamente o **maior desconto disponível** para a combinação de itens
- Descontos são configurados como entidades no banco, via **Seed Data**, o que permite extensibilidade

---

## Tratamento de Erros

A API utiliza um **ExceptionMiddleware** que intercepta exceções de domínio e retorna respostas HTTP padronizadas:

| Exceção                          | HTTP Status | Cenário                                   |
|----------------------------------|-------------|-------------------------------------------|
| `BurgerNotFound`                 | 404         | Sanduíche não encontrado                  |
| `AccompanimentNotFound`          | 404         | Acompanhamento não encontrado             |
| `OrderNotFoundException`         | 404         | Pedido não encontrado                     |
| `BurgersLimitException`          | 400         | Mais de 1 sanduíche no pedido             |
| `DuplicateAccompanimentsException` | 400       | Itens duplicados no pedido                |
| `BurgerAlreadyExistsException`   | 409         | Sanduíche com mesmo nome já existe        |
| `AccompanimentAlreadyExistsException` | 409    | Acompanhamento com mesmo nome já existe   |
| `DatabaseException`              | 500         | Erro de banco de dados                    |

---

## Como Executar o Projeto

### Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- EF Core CLI (caso ainda não tenha):
- PostgreSQL rodando localmente

```bash
dotnet tool install --global dotnet-ef
```

```sql
CREATE DATABASE goodhamburger;
```

### Rodar o projeto

```bash
# 1. Clonar o repositório
git clone https://github.com/Kawhan/GoodHamburgerTEST.git
cd GoodHamburgerTEST

# 2. Acessar o projeto da API
cd GoodHamburgerProject

# 3. Restaurar dependências
dotnet restore

# 4. Compilar o projeto
dotnet build

# 5. Aplicar migrations e criar o banco SQLite
dotnet ef database update

# 6. Executar a API
dotnet run
```

A API estará disponível em `https://localhost:7162` ou `http://localhost:5202`

⚠️ As portas podem variar conforme a configuração do `launchSettings.json`.

### 📄 Documentação Interativa

Com a API rodando, acesse a documentação Scalar em:

```
https://localhost:7162/scalar/v1
```

### 🐳 Execução com Docker (RECOMENDADO)

#### Pré-requisitos

##### 🪟 Windows
- Docker Desktop instalado
- WSL2 habilitado (necessário para rodar containers Linux)

> O Docker Desktop utiliza o WSL2 internamente para executar containers.

##### 🐧 Linux
- Docker instalado
- Docker Compose instalado (ou plugin `docker compose`)

---

#### ▶️ Rodar a aplicação (API + PostgreSQL)

```bash
docker-compose up --build
```


#### 🌐 Acesso
API: http://localhost:5000
Documentação (Scalar): http://localhost:5000/scalar

#### 🧹 Parar os containers
```bash
docker-compose down
```

#### 🗑️ Remover também os dados do banco
```bash
docker-compose down -v
```

### ℹ️ Observações

- O banco PostgreSQL roda em um container separado (`db`)
- A API se conecta usando `Host=db` (rede interna do Docker)
- As migrations são aplicadas automaticamente na inicialização
- Não é necessário rodar `dotnet ef` dentro do container

---

## Seed Data

O banco é populado automaticamente via **EF Core Migrations** com:

- **3 Sanduíches**: X Burger (R$ 5,00), X Egg (R$ 4,50), X Bacon (R$ 7,00)
- **2 Acompanhamentos**: Batata Frita (R$ 2,00), Refrigerante (R$ 2,50)
- **9 Regras de Desconto**: 3 combos por sanduíche (completo 20%, +refri 15%, +batata 10%)

---

## Decisões Técnicas

| Decisão | Justificativa |
|---------|---------------|
| **SQLite** | Zero configuração, ideal para o escopo do desafio. Migração para SQL Server/PostgreSQL requer apenas trocar o provider no `Program.cs` |
| **Seed Data via Configuration** | Dados iniciais versionados junto com as migrations, garantindo reprodutibilidade |
| **Descontos como entidade** | Permite criar, editar e desativar combos sem alterar código-fonte |
| **ExceptionMiddleware** | Tratamento centralizado de erros, eliminando try-catch repetitivo nos controllers |
| **Interfaces em Service e Repository** | Facilita testes unitários e substituição de implementações |
| **Soft Delete (Active flag)** | Preserva histórico de pedidos e itens sem perda de dados |
| **Scalar (OpenAPI)** | Documentação interativa moderna como alternativa ao Swagger UI |
| **.NET 10 + C# 14** | Primary constructors, target-typed new, init-only setters |

| Decisão | Justificativa |
|---------|---------------|
| **Arquitetura em Camadas (Controller → Service → Repository)** | Separação clara de responsabilidades, facilitando manutenção, testes e evolução do sistema |
| **Service Layer com regras de negócio** | Centraliza lógica (validações, descontos, regras de pedido), evitando controllers “gordos” |
| **Repository Pattern** | Abstrai o acesso a dados, desacoplando o EF Core da regra de negócio |
| **DTOs (Data Transfer Objects)** | Evita exposição direta das entidades, permitindo controle total do contrato da API |
| **Mappers (ToDTO / ToModel)** | Isola transformação de dados, mantendo services limpos e organizados |
| **Domain Models ricos (Order, OrderItem)** | Entidades representam o domínio com propriedades como `Active`, `FinalAmount`, etc. |
| **Validações no Service (não no Controller)** | Garante reutilização das regras e evita duplicação em múltiplos endpoints |
| **Exception-driven flow** | Uso de exceptions para controle de fluxo de erro, integrado com middleware global |
| **Baixo acoplamento entre camadas** | Cada camada depende de abstrações (interfaces), não de implementações |
| **Preparado para Clean Architecture** | Estrutura atual permite evolução futura para separação em projetos (Domain, Application, Infrastructure) |

---

## Estrutura de Branches

- `main` — código estável e entregável

---

## 🧩 Modelagem de Domínio

A modelagem de domínio foi projetada com foco em simplicidade, escalabilidade e representação clara das regras de negócio de um sistema de pedidos de hamburgueria.

---

### 📌 Entidades Principais

#### **Burger e Accompaniment**
Representam os itens disponíveis no cardápio.

- Propriedades: `Id`, `Name`, `Price`, `Active`
- Utiliza **Data Annotations** para validações básicas
- A flag `Active` implementa **soft delete**, permitindo desativar itens sem perder histórico

---

#### **Order**
Entidade central do domínio, responsável por agregar os itens e gerenciar os cálculos.

- `Items`: lista de itens do pedido  
- `TotalAmount`: soma dos itens ativos  
- `Discount`: valor do desconto aplicado  
- `FinalAmount`: valor final após desconto  
- `CreatedAt`: controle temporal  
- `Active`: suporte a soft delete  

---

#### **OrderItem**
Representa um produto dentro de um pedido.

- Utiliza:
  - `ProductId`
  - `ProductType` (enum)

**Decisão de design:**
Ao invés de relacionamentos diretos com tabelas de produtos, foi adotada uma abordagem polimórfica.

Isso evita joins complexos e permite suportar múltiplos tipos de produto de forma escalável.

---

#### **Discount**
Representa regras promocionais (combos).

- `Percentage`: percentual de desconto  
- `Items`: produtos necessários para ativar o desconto  
- `Active`: ativa/desativa o desconto  
- `DeletedAt`: controle de ciclo de vida  

---

#### **DiscountItem**
Define quais produtos fazem parte de um desconto.

- Utiliza a mesma estratégia polimórfica:
  - `ProductId`
  - `ProductType`

---

## 🧠 Decisões de Modelagem

| Decisão | Justificativa |
|---------|--------------|
| **ProductId + ProductType** | Evita múltiplas chaves estrangeiras e permite suportar novos tipos de produto |
| **Soft Delete (flag Active)** | Preserva o histórico mesmo quando itens são removidos do cardápio |
| **Snapshot de preço no OrderItem** | Garante consistência histórica mesmo que o preço do produto mude |
| **Desconto como entidade** | Permite criação e gerenciamento dinâmico de promoções sem alterar código |
| **Order como raiz de agregação** | Centraliza regras de negócio como cálculo de total e descontos |
| **Sem FK direta de OrderItem para produtos** | Reduz acoplamento e evita problemas com o ciclo de vida dos produtos |
| **Validação básica via DataAnnotations** | Garante integridade dos dados sem misturar regras de negócio nos controllers |

---

## 🔄 Fluxo de Dados (Simplificado)

1. O pedido é criado com os itens  
2. Cada item consulta seu preço atual  
3. Os itens são adicionados ao pedido  
4. O valor total é calculado  
5. O melhor desconto aplicável é identificado  
6. O valor final é calculado  

---

## 🚀 Benefícios

- Baixo acoplamento entre entidades  
- Alta flexibilidade para evolução futura  
- Estrutura limpa e de fácil manutenção  
- Preparado para novos tipos de produto  
- Garante consistência histórica dos dados  

## O que ficou de fora

- 

## Autor

**Kawhan** — [GitHub](https://github.com/Kawhan)
