# Vendinha

O **Vendinha** é uma aplicação desktop desenvolvida no ecossistema .NET, com C#, para o gerenciamento de clientes e dívidas. O sistema conta com um CRUDs - Create(Criar) / Read (Ler) / Update (Atualizar) / Delete (Deletar), paginação, campo de busca, dentre outras features.

---

## Principais Funcionalidades

### Gerenciamento de Clientes
* **Cadastro e Edição:** Cadastre ou altere dados de cada cliente, como Nome e Email.
* **Listagem:** Paginação de registros, ordenada automaticamente de forma decrescente pelo total de dívidas.
* **Busca:** Busca de clientes por nome, facilitando a filtração.
* **Exclusão:** desativação de clientes, sem perder o registro no banco de dados.

### Gerenciamento de Dívidas
* **Novas dívidas:** Cadastro de novas dívidas vinculadas a cada respectivo cliente.
* **Consulta:** Listagem de dívidas com filtro, por cliente.
* **Quitar dívidas:** para quitar e registrar a data de pagamento da dívida, apenas clique em um botão e confirme.

---

## Tecnologias Utilizadas

* **Linguagem:** C#
* **Interface:** Windows Forms
* **ORM:** Entity Framework
* **Banco de Dados:** PostgreSQL

---

## Instruções de Instalação e Execução

Siga os passos abaixo para configurar e rodar o projeto.

### Pré-requisitos
Antes de tudo, você precisará ter instalado:
* *.NET* versão 8.0 ou superior (projeto feito em *.NET* 10).
* *Visual Studio*
* *PostgreSQL* ativo localmente.
* *DBeaver* ativo localmente.

### Passo a Passo

1. **Criar Banco de Dados no DBeaver:**
   
   Após criar uma nova conexão com o PostgreSQL no DBeaver, no menu lateral esquerdo da página principal, clique na seta na esquerda da conexão `postgres`, em seguida clique com o botão direito do mouse e vá em `Criar novo banco de dados`, dê o nome a ele de `vendinha`. Após isso, com o banco criado, pressione com o botão direito em cima do mesmo, e vá em `Definir como padrão`, depois com o direito denovo, _Editor SQL -> Novo script SQL_. Em seguida, copie e cole o código do arquivo `scriptdb.sql` localizado dentro da solução.

3. **Configurar a String de Conexão(Connection String):**
   
   Abra o arquivo `VendinhaDbContext.cs` e altere a string de conexão para apontar corretamente ao seu banco de dados.

   Ache esse código, possivelmente na linha 17 do `VendinhaDbContext.cs`:

   `optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User Id=postgres;Password=senha;Database=vendinha");` 

2. **Instalar Dependências do Projeto:**
   
   Visual Studio: projeto instala todas as depêndencias necessárias ao abrir o projeto.
  
   Terminal: abra o terminal na raiz da pasta e rode:

   `dotnet restore`

4. **Rodar a Aplicação:**
   
   Visual Studio: se o projeto `Vendinha.Desktop` está marcado como projeto de inicialização e clique no botão Iniciar (play verde) ou aperte F5, se não, clique na opção ao lado do _play verde_, selecione `Vendinha.Desktop`, em seguida, clique em Inicar (play verde) ou pressione F5.
    
   Terminal: Execute o comando abaixo:
   
   `dotnet run --project Vendinha.Desktop`

---

## Estrutura do Projeto (Arquitetura)

A aplicação foi estruturada da seguinte forma:

* `Vendinha.Core` (Backend): contém os dados (`Cliente`, `Divida`), contextos do Entity Framework (`VendinhaDbContext`) e os _Services_ contendo as regras de negócio (`ClienteService`, `DividaService`).
* `Vendinha.Desktop` (Frontend): visual, utilizando o Windows Forms contendo as telas de gerenciamento (`ClientesListagem`, `ClientesEditar`, `DividasListagem`, `DividasCadastro`).
