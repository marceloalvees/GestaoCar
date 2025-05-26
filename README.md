# 📘 Documentação Técnica - Sistema de Gestão de Concessionárias

---

## 🧾 Visão Geral do Projeto

Sistema web robusto para gerenciamento de fabricantes, veículos, concessionárias e vendas, com autenticação de usuários, geração de relatórios e arquitetura escalável baseada em DDD (Domain-Driven Design).

---

## 🎯 Objetivo

Desenvolver um sistema web para:

- Gerenciamento de fabricantes, veículos, concessionárias e vendas
- Autenticação e controle de acesso de usuários
- Geração de relatórios dinâmicos
- Arquitetura escalável e de fácil manutenção (DDD)

---

## 🏗️ Arquitetura da Aplicação

### 🧠 Abordagem DDD (Domain-Driven Design)

O projeto é organizado em camadas, seguindo os princípios de DDD para maior clareza, testabilidade e manutenção.

| Camada         | Responsabilidade                                                             |
| -------------- | --------------------------------------------------------------------------- |
| **Domain**         | Entidades, value objects, agregados e regras de negócio.                   |
| **Application**    | Casos de uso e orquestração entre domínio e infraestrutura.                |
| **Infrastructure** | Repositórios, serviços externos, acesso a dados e Identity.                |
| **WebInterface**   | Apresentação: Views, Controllers, validações client-side, lógica AJAX.     |

A camada **WebInterface** controla toda a experiência do usuário, incluindo formulários, validações e exibição dinâmica de dados.

---

## ⚙️ Tecnologias Utilizadas

- .NET 9
- ASP.NET MVC
- Entity Framework Core
- SQL Server (LocalDB)
- Bootstrap, JavaScript (AJAX), Chart.js
- ASP.NET Identity (autenticação/autorização)

---

## 🔐 Autenticação e Autorização

### 🔧 ASP.NET Identity

- Perfis de usuário: **Administrador**, **Gerente**, **Vendedor**
- Login/Registro com criptografia de senhas
- Autorização baseada em papéis via `[Authorize(Roles = "...")]`

---

## 🧩 Funcionalidades Principais

### 🏭 Cadastro de Fabricantes
- Nome (único), país de origem, ano de fundação, website (URL válida)

### 🚗 Cadastro de Veículos
- Modelo, ano de fabricação (não pode ser futuro), preço positivo, descrição opcional, vínculo obrigatório com fabricante

### 🏢 Cadastro de Concessionárias
- Nome (único), endereço completo (CEP validado), telefone, e-mail, capacidade máxima de veículos

### 🧾 Venda de Veículos
- Associada a veículo, cliente e concessionária
- Validação de CPF, data de venda (não pode ser futura)
- Preço de venda ≤ valor do veículo
- Geração de protocolo de venda único

### 👤 Cadastro de Clientes
- Nome, CPF (único e validado), telefone

### 📈 Relatórios e Dashboard
- Relatórios gráficos de vendas por tipo/fabricante/concessionária
- Seleção de período (mês/ano)
- Visualização dinâmica (Chart.js + AJAX)
- Exportação para PDF/Excel (opcional)

---

## 🔗 Integrações

- API de CEP (autopreenchimento de endereço)
- Chamadas AJAX para carregamento dinâmico de dados

---

## 🧪 Testes e Documentação

### ✔️ Testes
- Testes unitários e de integração (negócio e autenticação)

### 📚 Documentação
- API documentada com Swagger
- Documentação técnica em Markdown
- Estrutura clara e separação por camadas

---

## 🗃️ Modelagem de Dados

Tabelas relacionalmente normalizadas, com validações integradas:

- **Fabricantes**
- **Veiculos**
- **Concessionarias**
- **Clientes**
- **Vendas**
- **Usuarios**

Relacionamentos com chaves estrangeiras, integridade referencial e exclusões lógicas (soft delete).

---

## 🛠️ Execução do Projeto

### 📌 Pré-requisitos:
- .NET 9 SDK instalado
- SQL Server LocalDB configurado
- Editor (Visual Studio, Rider ou VS Code)

### ▶️ Instruções

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/seu-repositorio.git
   ```
2. **Restaurar dependências:**
   ```bash
   dotnet restore
   ```
3. **Aplicar as migrações:**
   ```bash
   dotnet ef database update
   ```
4. **Executar a aplicação:**
   ```bash
   dotnet run
   ```

---

## 🗂️ Estrutura de Pastas

```
/Domain
  └── Entities, ValueObjects, Interfaces

/Application
  └── UseCases, DTOs, Services

/Infrastructure
  └── EF Migrations, Identity, Repositories

/WebInterface
  └── Controllers, Views, ViewModels, wwwroot (JS/CSS)
```

---

## 📦 Publicação

O projeto deve ser publicado em um repositório público no GitHub com instruções detalhadas neste README.

---

## 📊 Possíveis Extensões

- Diagramas de camadas, ER, fluxo de vendas (solicite se desejar!)
- Exportação de relatórios em PDF/Excel

---
