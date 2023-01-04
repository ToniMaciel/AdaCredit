# AdaCredit

Voc� foi contratado para constru��o de um sistema de controle para uma pequena cooperativa digital de cr�dito local, a Ada Credit (*C�digo Banc�rio 777*).

Para garantir a seguran�a das informa��es, o sistema deve exigir login e senha para que seja poss�vel a sua opera��o.
O usu�rio e senha "iniciais" (na primeira execu��o do programa) devem ser *"user"* e *"pass". *Essa senha padr�o deve ser trocada no primeiro login**.

Quando o login for bem sucedido, o sistema exibe um menu com op��es para:

- Clientes
    - Cadastrar Novo Cliente
    - Consultar os Dados de um Cliente existente
    - Alterar o Cadastro de um Cliente existente
    - Desativar Cadastro de um Cliente existente
- Funcion�rios
    - Cadastrar Novo Funcion�rio
    - Alterar Senha de um Funcion�rio existente
    - Desativar Cadastro de um Funcion�rio existente
- Transa��es
    - Processar Transa��es (Reconcilia��o Banc�ria)
- Relat�rios
    - Exibir Todos os Clientes Ativos com seus Respectivos Saldos
    - Exibir Todos os Clientes Inativos
    - Exibir Todos os Funcion�rios Ativos e sua �ltima Data e Hora de Login
    - Exibir Transa��es com Erro (Detalhes da transa��o e do Erro)

Ao ser cadastrado, o cliente recebe um n�mero de conta de 5 d�gitos e um d�gito verificador, ambos aleat�rios, formando o padr�o XXXXX-X.
Por ser uma cooperativa digital, todos os clientes possuem o mesmo n�mero de ag�ncia, que � 0001.

As senhas devem ser armazenadas de forma segura. Para isso, nosso cliente solicitou a utiliza��o do mecanismos de seguran�a BCRYPT com salto (veja Anexo C) para criptografia da senha (Veja este exemplo. Entretanto, use o salto em vez do WorkFactor).

A Ada Credit recebe, diariamente, do "Sistema Nacional de Pagamentos Integrado" m�ltiplos arquivos representando transa��es banc�rias que envolvam seus clientes. Essas transa��es podem ser de Entrada (Cr�dito) ou Sa�da (D�bito). Os arquivos possuem o padr�o de nomenclatura "nome-do-banco-parceiro-aaaammdd.csv", em que aaaa, mm e dd representam, respectivamente, o ano com quatro d�gitos, o m�s com dois d�gitos e o dia com dois d�gitos da data em que o arquivo foi gerado.

Quando o usu�rio selecionar a op��o "Processar Transa��es" no menu principal, o sistema buscar� pelos arquivos de transa��o que ficam na pasta "Desktop/Transactions" (ou seu equivalente "~/home/Transactions/Pending" em sistemas *nix) e os processar�, respeitando a tabela de tarifas em vigor. Verifique os detalhes sobre o layout do arquivo de transa��es no Anexo A ao final do enunciado, bem como As Tabelas de Tarifas no Anexo B.

� importante mantermos o registro das transa��es que n�o puderam ser processadas que falharam. Essas falhas podem acontecer, por exemplo, por insufici�ncia de saldo, n�mero da conta inv�lido ou inexistente, tipo de transa��o incompat�vel (no caso de TEFs), etc. Nesses casos, o registro da transa��o deve ser movido para um arquivo cujo padr�o de nomenclatura � "nome-do-banco-parceiro-aaaammdd-failed.csv" que deve ser armazenado na pasta "~/home/Transactions/Failed".

Caso a transa��o tenha sido processada com sucesso, o registro da transa��o deve ser movido para um arquivo cujo padr�o de nomenclatura � "nome-do-banco-parceiro-aaaammdd-completed.csv" e que deve ser armazenado na pasta "~/home/Transactions/Completed". � importante que o saldo do cliente tenha sido atualizado de forma correta, inclusive com as cobran�as das devidas taxas.

### ANEXO A - Layoute do Arquivo de Transa��es
Cada linha no arquivo de transa��es � composta pelas seguintes informa��es
AAA,BBBB,CCCCCC,DDD,EEEE,FFFFFF,GGG,H,I

Sendo que:
AAA N�mero com 3 d�gitos representando o C�digo do Banco de Origem
BBBB N�mero com 4 d�gitos representando a Ag�ncia do Banco de Origem
CCCCCC N�mero com 6 d�gitos representando o n�mero da conta do Banco de Origem

DDD N�mero com 3 d�gitos representando o C�digo do Banco de Destino
EEEE N�mero com 4 d�gitos representando a Ag�ncia do Banco de Destino
FFFFFF N�mero com 6 d�gitos representando o n�mero da conta do Banco de Destino

GGG Tipo da Transa��o (DOC, TED, TEF).

H N�mero representando o sentido da transa��o (0 - D�bito/Sa�da, 1 - Cr�dito/Entrada)

I n�mero real com duas casas decimais, separadas por um . e sem separador de milhar

Obs: TEFs s� podem ser realizadas entre clientes do mesmo banco.

### ANEXO B - Tabelas de Tarifas
Transa��es a Cr�dito
Todas isentas de Tarifas

Transa��es a D�bito realizadas/recebidas at� 30/11/2022
Todas isentas de Tarifas

Transa��es a D�bito realizadas/recebidas a partir de 01/12/2022
TED - Tarifa �nica de R$5,00
DOC - Tarifa de R$1,00 + (1% da Transa��o limitado a R$5,00)
TEF - Isenta

### ANEXO C - Salt (Salto)
Um salt �, basicamente, uma cadeia de caracteres aleat�ria que � concatenada ao come�o ou ao final da senha fornecida pelo usu�rio antes de aplicarmos a fun��o de Criptografia/Hash. O uso do salt permite que o hash gerado seja completamente diferente, mesmo que duas ou mais senhas sejam id�nticas. Uma vez que cada uma delas tem seu pr�prio salto, os hashs ser�o diferentes.

Para que esse mecanismo funcione, al�m de armazenar o hash da senha do usu�rio, precisamos tamb�m armazenar o Salt, para que, no momento do login, possamos fazer a concatena��o do hash daquele usu�rio espec�fico com a senha fornecida no login a fim de comparar o resultado com o hash armazenado no "banco de dados".