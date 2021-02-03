# Cache

## Tipos de memória

Antes de falarmos de cache precisamos expor o conceito sobre os dois grupos básicos de memória:

**1. Memória Temporária**:  
São memórias voláteis, isto é, perdem seus dados com ausência de energia. Como exemplos temos a memória RAM e a memória Cache (aqui estamos falando da memória cache física que fica junto ao processador na placa do microcomputador e não da estratégia de cache em aplicações, que falaremos mais adiante).  

**2. Memória Permanente**:
São memórias não voltáteis, isto é, não perdem seus dados na ausência de energia. Como exemplos temos o disco rígido (HD) e sua evolução, o SSD (Solid State Drive).
<br>
<br>


## Cache, em termos gerais
Cache é qualquer tipo de implementação de memória cuja o objetivo seja aumentar o tempo de resposta.
Com os avanços tecnológicos, vários tipos de cache foram desenvolvidos. Atualmente há cache em processadores, discos rígidos, sistemas, servidores, nas placas-mãe, clusters de bancos de dados, entre outros. 

- No caso dos **processadores**, o cache disponibiliza alguns dados já requisitados anteriormente à memoria RAM/Disco Rígido; 

- No caso dos **navegadores web**, o cache mantém o conteúdo estático (HTML, CSS, JavaScript, imagens, etc) em memória local para evitar consultas constantes à rede;

- Os **servidores de aplicação** também podem dispor de caches configurados pelo administrador. Neste caso o cache mantém dados em memória para melhorar o desempenho de consultas à base de dados, por exemplo.
<br>
<br>


## Cache de aplicação
É sobre cache de aplicação e/ou servidores que vamos tratar.  
Como podemos melhorar o desempenho de nossas aplicações utilizando estratégias de cache?

Dependendo da quantidade e/ou do tipo de informações pesquisadas, uma consulta no banco de dados pode se tornar lenta. Repetir essas consultas constantemente certamente se tornará algo inviável.

### Como o cache de aplicação funciona?
Vamos montar um cenário para entender como o cache de aplicação é implementado na sua forma mais básica:

Os dados são gravados em uma memória permanente (banco de dados por exemplo). O Administrador configura que uma determinada consulta de dados precisará primeiro passar pelo cache. Existindo as informações nesse cache, devem elas serem retornadas ao usuário dali mesmo. Não existindo, o sistema deverá buscá-las no banco de dados, então gravá-las em cache e retornar ao usuário. Na próxima consulta as informações já estarão em cache. 

### Dados passíveis de alteração
Caso estes dados sejam passíveis de alteração, um tempo deve ser configurado para que o cache seja invalidado e renovado, ou seja, efetuar uma nova consulta a base de dados e gravar um novo cache. 

Essa estratégia de cache é conhecida como **Write-Around**.

Perceba que desta forma as alterações efetuadas na base de dados podem não ficar imediatamente disponíveis, afinal, existe um tempo que o cache deve respeitar para ser atualizado. 

Em alguns cenários isso não acarretaria em muitos problemas, e, dependendo do negócio, o tempo de cache pode ser ajustado e pronto, tudo certo. Em outros casos, porém, a inconsistência destes dados podem causar sérios problemas. 

Entramos em um conceito importante: **Inconsistência de dados**.


### Inconsistência de dados
Para resolver o problema de Inconsistência de Dados descrito acima, existem outras estratégias de cache que podem ser implementadas:

- **Write-Through**:  
Uma alteração é gravada tanto na base de dados quanto no cache.   
Vantagem: Alto nível de disponibilidade e consistência de dados.  
Desvantagem: Aumento de latência na escrita.

- **Write-Back**
Escreve apenas no cache. As informações são gravadas em uma base de dados de tempos em tempos, conforme configurado.  

Para o segundo cenário o risco é mais alto, uma vez que um desligamento de máquina resultaria em perda total dos dados ainda não persistidos em uma base segura. 

Por este motivo essa estratégia só possui sentido se utilizarmos **Servidores de Cache**, ou seja, o cache fica na memória de servidores próprios para esta tarefa. Eles replicam a mesma informação. Se possível, estes servidores devem ser mantidos em diferentes áreas geográficas.

Perceba agora que podemos dividir em duas novas categorias os tipos/estratégias de cache: **Cache Local** e **Cache Distribuido**


1. **Cache Local**   
O cache está na memória do mesmo servidor que está rodando a aplicação.

2. **Cache Distribuído**  
O cache está na memória de servidor(s) separado(s) do servidor que está rodando a aplicação.

É importante observar que qualquer uma das estratégias de cache descritas anteriormente (Write-Around, Write-Through e Write-Back) podem ser aplicadas tanto em Cache Local como em Cache Distribuído. Cada uma, porém, tem suas vantagens, desvantagens e cuidados a serem observados.
<br>
<br>


## Cache no .NET Core
