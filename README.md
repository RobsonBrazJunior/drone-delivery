# drone-delivery

## Projeto WebApi

Projeto construído para acessar uma WebApi pública, ler um grafo em formato JSON
e com base em três parâmetros de entrada calcular a rota mais rápida para entrega do package pelo drone.

 - url pública para acesso do endpoint em: dronedelivery.azurewebsites.net/swagger

 - Como testar:
No corpo da requisição informe um objeto com uma lista de strings:
[
    "A1",
    "F4",
    "B8"
]