# ProjetoMatrizesNaoQuadraticas

O Projeto visa discutir e encontrar solução de cálculo de matrizes não quadráticas inversas.
Na atual situação, a matriz não quadrática funciona  numa determinada posição de matrizes na equação do projeto.
Discute também o emprego do artifício matemático na solução de matrizes inversas não quadráticas, para outras
equações, como sistemas lineares impossível de se resolver.

Mas há boas notícias! Como o vetor escolhido anteriormente, resultou na validação (o vetor 3d: (1,2,3)),
e outros vetores com intervalos entre os elementos do vetor, diferente do vetor (1,2,3), resolvi checar
se os intervalos dos elementos da matriz auxiliar, fosse arranjado tal que fosse igual aos intervalos de
um outro vetor, (3,6,9). Resultado: uma validação na transformação 3d-->2d-->3d, que era a transformação
faltante para transformações isométricas como função bijetora. A transformação 2d-->3d-->2d já era bijetora.

No aguardo, de como modificar a matriz auxiliar de tal modo que resulte numa matriz bijetora, para qualquer
vetor 3d, numa transformação 3d-->2d-->3d.

Motivo? A matriz que era para ser uma matriz Identidade, não é identidade, para por exemplo: m32*m23<>I(3).
Creio que se mudar a biblioteca matemática, haja resultados melhores, pois em um dos testes, a matriz m32*m23 
foi escrita na tela não como uma matriz impossível (com números NAN, not a number), mas uma matriz com elementos:
+8, e -8, o que é ultrajante, nós esperamos resultados nos cálculos, e alguém insiste em "brincar" alterando
os valores de vetores e matrizes..
