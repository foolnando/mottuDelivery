# README

Este projeto contém uma aplicação ASP.NET Core dockerizada que pode ser executada em um contêiner Docker.

## Pré-requisitos

Antes de começar, certifique-se de ter os seguintes pré-requisitos instalados em sua máquina:

- Docker: [Instalação do Docker](https://docs.docker.com/get-docker/)
- .NET Core SDK: [Instalação do .NET Core SDK](https://dotnet.microsoft.com/download)

## Como rodar o projeto

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

5. Após iniciar o contêiner, você pode acessar a aplicação em seu navegador da web usando o seguinte URL:
`http://localhost:8080`
E o swagger do projeto no URL: `http://localhost:8080/swagger/index.html`

## Suporte

Se você encontrar algum problema ou tiver alguma dúvida sobre este projeto, sinta-se à vontade para abrir uma issue neste repositório. Teremos o prazer de ajudar!



## Rotas implementadas
Essas são as rotas implementadas para os casos de uso do projeto. 
### POST /vehicle

Esta rota é usada para criar um novo veículo no sistema.

### GET /vehicle/{plate}

Esta rota permite recuperar informações sobre um veículo específico com base na placa.

### PATCH /vehicle/{id}

Esta rota permite atualizar a placa de um veículo específico com base no ID fornecido.

### DELETE /vehicle/{id}

Esta rota é usada para excluir um veículo do sistema com base no ID fornecido.

### POST /vehicle/rent

Esta rota é usada para alugar um veículo.

### POST /vehicle/rent/excharge

Esta rota é usada para calcular o valor do aluguel baseado em uma data de devolução do veículo.

### POST /driver

Esta rota é usada para cadastrar um novo motorista no sistema.

### PATCH /driver/cnhImage/{id}

Esta rota é usada para atualizar a imagem da CNH de um motorista específico com base no ID.

### GET /driver/{cnpj}

Esta rota permite recuperar informações sobre um motorista específico com base no CNPJ.

### POST /delivery

Esta rota é usada para criar uma nova entrega no sistema Mottu.

### POST /delivery/accept

Esta rota é usada para aceitar uma entrega.




### POST /delivery/confirm

Esta rota é usada para confirmar uma entrega.



## Esquema do Banco de Dados

Um pouco sobre o esquema definido:

### Driver
Relação que corresponde ao motorista, que armazenas os dados de nome, cnpj, tipo da cnh, path da foto da cnh no S3, numero da cnh e data de nascimento.


### Vehicle
Relação que corresponde ao veículo, que armazenas os dados de placa, modelo, ano e status de disponibilidade para aluguel.


### Rent_driver_vehicle
Relação que corresponde a locação do veículo e que relaciona o motorista com  o veículo alugago, armazena os dados de motorista e veículo, inicio da local, numero de dias de locação (7 dias, 15 dias ou 30 dias), data esperada da devolução do veículo, data de devolução do veículo, valor do aluguel e status da locação que corresponde se aquele motorista com aquele veículo está disponível para realizar entregas.

### Order
Relação que corresponde ao pedido, com as informações de data de criacao, valor da corrida, situacao do pedido (disponivel, aceito e entregue).

### Order_driver_notification
Relação que corresponde as notificações de pedidos disponiveis para entrega enviados para motoristas disponiveis para realizar entrega

![image](https://github.com/foolnando/mottuDelivery/assets/47675174/a7848cb5-6efa-4826-bb34-4f4542547223)

## Arquitetura

A decisão arquitetural tomada levando em consideração o tempo de desenvolvimento e os recursos disponíveis foi trabalhar num monolito que produz e consome uma fila, bem como tem acesso direto ao banco. 
O monolito foi moduralizado pensando em 3 módulos principais, que facilitariam uma possível re-estruturação em uma arquitetura de microsserviços.






