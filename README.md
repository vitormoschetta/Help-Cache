# Cache

## Tipos de memória

Antes de falaros de cache precisamos falar sobre os  tipos de memória existentes. Basicamente elas subdividem em dois principais grupos:

1. **Memória Temporária**:  
São memórias voláteis, isto é, perdem seus dados com ausência de energia. Como exemplos temos a memória RAM e a memória Cache (aqui estamos falando da memória cache física que fica junto ao processador na placa do microcomputador e não da estratégia de cache em aplicações, que falaremos mais adiante).  

A segunda foi projetada para obter maior desempenho, por isso fica junto ao processador.

2. **Memória Permanente**:
São memórias não voltáteis, isto é, não perdem seus dados na ausência de energia. Como exemplos temos o disco rígido (HD) e sua evolução, o SSD (Solid State Drive).
<br>


## Cache, o que é?
Cache é qualquer tipo de implementação de memória cuja o objetivo seja aumentar o tempo de resposta.
Com os avanços tecnológicos, vários tipos de cache foram desenvolvidos. Atualmente há cache em processadores, discos rígidos, sistemas, servidores, nas placas-mãe, clusters de bancos de dados, entre outros. 

- No caso dos processadores, em que cache disponibiliza alguns dados já requisitados e outros a processar;
- No caso dos navegadores web, em que as páginas são guardadas localmente para evitar consultas constantes à rede (especialmente úteis quando se navega por páginas estáticas);
- Os servidores de aplicação também podem dispor de caches configurados pelo administrador. Neste caso o cache mantém dados de uma base em memória para melhorar o desempenho de consultas.


## Cache de sistemas
É sobre cache de sistemas e/ou servidores que vamos tratar.  
Como podemos melhorar o desempenho de nossas aplicações utilizando cache?

Dependendo da quantidade e/ou do tipo de informações pesquisadas, uma consulta no banco de dados pode se tornar lenta. Repetir essas consultas constantemente certamente se tornará algo inviável.

#### Como o cache funciona?
A forma mais básica de uso de cache em sistemas ocorre da seguinte forma:
Os dados são gravados em uma memória permanente (banco de dados por exemplo). O Administrador configura que uma determinada consulta de dados precisará primeiro passar pelo cache. As informações existindo em cache devem ser retornadas ao usuário dali mesmo. Não existindo as informações em cache, o sistema deverá buscá-las no banco de dados, gravá-las em cache e retornar ao usuário. Na próxima consulta as informações já estarão em cache. 

Caso estes dados sejam passíveis de alteração, um tempo deve ser configurado para que o cache seja invalidado e renovado, ou seja, efetuar uma nova consulta a base de dados e gravar um novo cache. 

Essa estratégia de cache é conhecida como **Write-Around**.

Perceba que desta forma as alterações efetuadas na base de dados podem não ficarem imediatamente disponíveis, afinal, existe um tempo que o cache deve respeitar para ser atualizado.

Entramos em um conceito importante: Inconsistência de dados.


#### Inconsistência de dados
Para resolver o problema de Inconsistência de Dados descrito acima, existem outras estratégias de cache que podem ser implementadas:

- **Write-Through**:  
Uma alteração é gravada tanto na base de dados quanto no cache.   
Vantagem: Alto nível de disponibilidade e consistência de dados.
Desvantagem: Aumento de latência na escrita.

- **Write-Back**
Escreve apenas no cache. As informações são gravadas em uma base de dados de tempos em tempos, conforme configurado.  

Para este cenário o risco é mais alto,uma vez que um desligamento de máquina resultaria em perda totoal dos dados ainda não persistidos em uma base segura. 

Por este motivo essa estratégia só possui sentido se utilizarmos **Servidores de Cache**, ou seja, o cache fica na memória de servidores próprios para esta tarefa. Eles replicam a mesma informação. Se possível, estes servidores devem ser mantidos em áreas geográficas diferentes.

Perceba agora que podemos dividir em duas novas categorias os tipos/estratégias de cache: **Cache Local** e **Cache Distribuido**


1. **Cache Local**   
O cache está na memória do mesmo servidor que está rodando a aplicação.

2. **Cache Distribuído**
O cache está na memória de servidor(s) separado(s) do servidor que está rodando a aplicação.

É importante observar que qualquer uma das estratégias de cache descritas anteriormente (Write-Around, Write-Through e Write-Back) podem ser aplicadas tanto em Cache Local como em Cache Distribuído. Cada uma, porém, tem suas vantagens, desvantagens e cuidados a serem observados.
<br>
<br>


## Cache no .NET Core
