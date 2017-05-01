## Sobre ##
Esse projeto visa expor dois endpoints para gerenciamento de pontuações de jogos.
Foi utilizado asp.net **WEB API** para expor os dois endpoints, ambos abaixo do endereço *servidor*/**api/game**, sendo que para envio de pontuações dos jogadores será usado o método **POST** e para receber o ranking os 100 melhores é utilizado o método **GET**.

## Código fonte ##
Subi apenas as classes e demais arquivos de importância do projeto, as dependências deve ser obtidas através do comando **nuget restore** no **Package Manager Console** ou clicando com o botão direto na solution e selecionando **Restore Nuget Packages**

## Algumas informações ##
Visando a simplicidade de acesso à dados que não é o foco desse teste optei por utilizar um arquivo de **SQLite** como banco de dados.
Visando a performance das queries e como o foco não é trabalhar profundamente com ORM optei pelo **Micro ORM Dapper** que é o mais rápido atualmente.
Se rodar o projeto, a única página exitente será o Home/Index que conterá uma breve **documentação** sobre o mesmo.

## Testes ##
Além do projeto de testes unitários, foram utilizados para testes das apis o POSTMAN e para testes de carga o Apache JMeter, o qual na raiz desse repositório existe o arquivo de configuração para testes utilizando essa ferramenta, chamado Requisição HTTP.jmx, basta abri-lo no Apache jMeter e rodar