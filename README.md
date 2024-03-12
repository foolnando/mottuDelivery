# README

Este projeto contém uma aplicação ASP.NET Core dockerizada que pode ser executada em um contêiner Docker.

## Pré-requisitos

Antes de começar, certifique-se de ter os seguintes pré-requisitos instalados em sua máquina:

- Docker: [Instalação do Docker](https://docs.docker.com/get-docker/)
- .NET Core SDK: [Instalação do .NET Core SDK](https://dotnet.microsoft.com/download)

## Como usar

Siga estas etapas para executar a aplicação em um contêiner Docker:

1. Clone este repositório em sua máquina local:

```
git clone <URL_do_repositório>

```

2. Navegue até o diretório do projeto:

```
cd MottuService
```


3. Execute o seguinte comando para construir a imagem Docker:

```
docker build -t nome-da-imagem .

```
Substitua `nome-da-imagem` pelo nome que você deseja dar à sua imagem Docker.

4. Depois que a imagem for construída com sucesso, execute o seguinte comando para iniciar o contêiner:
```
docker run -d -p 8080:8080 nome-da-imagem
```
obs: se o projeto não carregar, verifique em que porta o projeto está rodando na imagem construida através do comando `docker logs <id-container>` e substitua no comando acima.



