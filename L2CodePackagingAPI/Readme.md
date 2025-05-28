# 📦 API de Embalagem - Loja do Seu Manoel

Esta API automatiza o processo de embalagem de pedidos da Loja do Seu Manoel. Dada uma lista de produtos com dimensões variadas, o sistema determina a melhor combinação de caixas para otimizar espaço e reduzir o número total de embalagens utilizadas.

---

## ✅ Tecnologias Utilizadas

- **.NET 8** – Framework principal
- **Entity Framework Core** – ORM
- **SQL Server** – Banco de dados
- **Swagger** – Documentação e testes da API
- **xUnit** – Testes unitários
- **JWT Bearer (opcional)** – Autenticação

---

## 📋 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server local (pode usar SQL Server Express ou LocalDB)
- Git (para clonar o repositório)

---

## 🚀 Como Executar a Aplicação (sem Docker)

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/packaging-api.git
cd packaging-api
```

### 2. Configure o banco de dados

Edite o arquivo `appsettings.json` com sua string de conexão:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LojaManoelDB;User Id=sa;Password=SuaSenhaAqui;"
}
```

> Dica: se estiver usando o SQL Server Express local, pode usar:  
> `"Server=(localdb)\MSSQLLocalDB;Database=LojaManoelDB;Trusted_Connection=True;"`

### 3. Crie o banco de dados

```bash
dotnet ef database update
```

> Isso criará automaticamente o banco com as tabelas e dados iniciais (as 3 caixas padrão).

### 4. Execute a API

```bash
dotnet run
```

Acesse o Swagger em:  
🔗 [http://localhost:5000/swagger](http://localhost:5000/swagger)  
ou [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## 📦 Caixas Disponíveis

| Caixa   | Altura | Largura | Comprimento | Volume (cm³) |
|---------|--------|---------|-------------|---------------|
| Caixa 1 | 30 cm  | 40 cm   | 80 cm       | 96.000        |
| Caixa 2 | 80 cm  | 50 cm   | 40 cm       | 160.000       |
| Caixa 3 | 50 cm  | 80 cm   | 60 cm       | 240.000       |

---

## 🔧 Endpoints Principais

### `POST /api/packaging/process`

Processa pedidos e retorna a embalagem otimizada.

**Exemplo de entrada:**
```json
{
  "pedidos": [
    {
      "id": "PEDIDO-001",
      "produtos": [
        {
          "id": "PRODUTO-001",
          "altura": 20,
          "largura": 30,
          "comprimento": 40
        }
      ]
    }
  ]
}
```

**Exemplo de saída:**
```json
{
  "pedidos": [
    {
      "id": "PEDIDO-001",
      "caixas": [
        {
          "id": "Caixa 1_1",
          "produtos": ["PRODUTO-001"],
          "dimensoes": {
            "altura": 30,
            "largura": 40,
            "comprimento": 80
          },
          "volumeUtilizado": 24000,
          "volumeTotal": 96000,
          "taxaOcupacao": 25.0
        }
      ]
    }
  ]
}
```

---

### `GET /api/packaging/boxes`

Retorna as caixas disponíveis no sistema.

---

### `GET /api/packaging/health`

Verifica se a API está no ar.

---

## 🔐 Autenticação (Opcional)

A API pode usar JWT. Para ativar:

1. Defina as chaves `Jwt:Key` e `Jwt:Issuer` no `appsettings.json`
2. Use `POST /api/auth/login` com:

```json
{
  "username": "admin",
  "password": "admin123"
}
```

---

## 🧪 Rodar Testes Unitários

```bash
dotnet L2CodePackagingTests
```

---

## 🧠 Algoritmo de Otimização

O sistema:

- Ordena os produtos por volume (maior primeiro)
- Avalia combinações que maximizem ocupação e minimizem número de caixas
- Permite rotações dos produtos para encaixe ideal
- Prioriza uso de caixas menores, se possível

---

## 📁 Estrutura do Projeto

```
L2CodePackagingAPI/
├── Controllers/         # Endpoints da API
├── Data/                # DbContext e Migrations
├── DTOs/                # Objetos de entrada e saída
├── Models/              # Entidades da base de dados
├── Services/            # Lógica de negócios
└── README.md            # Este arquivo
```

---

## 🧩 Contribuições

Pull requests são bem-vindos! Sinta-se à vontade para propor melhorias ou novos recursos.