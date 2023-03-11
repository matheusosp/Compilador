# Analizador Lexico

# Como rodar a aplicação WindowsForms no Linux e Windows com .NET 6

## Pré-requisitos

Você precisará ter instalado o seguinte:

- .NET 6 SDK
  https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- Posssiveis comandos para instalar o dotnet 6 no ubuntu 22
```
wget https://packages.microsoft.com/config/ubuntu/22.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
```
- Comando para checkar a instalaçao do .NET
  ```--list-sdks```
## Passo a Passo

### 1. Acessar o diretório do projeto

Abra o terminal e navegue até o diretório da aplicação de console existente.(Pasta \ContactBook\ContactBook)

### 2. Restaurar as dependências

Execute o seguinte comando para restaurar as dependências do projeto:

```dotnet restore```

### 3. Compilar o projeto

Execute o seguinte comando para compilar o projeto:

```dotnet build```

### 4. Rodar a aplicação

Para executar a aplicação, execute o seguinte comando:

```dotnet run```

A aplicação de console será executada no terminal.

Para rodar no VSCODE é preciso:
- Extensão C# para o Visual Studio Code
- .NET 6 SDK

Pode ser executada pelo Visual Studio tambem, basta abrir o .csproj ou .sln (não tem Visual Studio para o Linux)
https://visualstudio.microsoft.com/pt-br/downloads/