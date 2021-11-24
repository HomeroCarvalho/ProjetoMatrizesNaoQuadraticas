using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{
    public class Matrizes
    {
        public Matrix<double> matrixMain;
        public static Matrizes matrixAux1 = null;
        static Matrizes matrixAux2 = null;
        private int linhas;
        private int colunas;
        public Matrizes(int lin, int col)
        {
            this.matrixMain = Matrix<double>.Build.Dense(lin, col);
            this.linhas = lin;
            this.colunas = col;
          
        }

        public Matrix<double> GetMatriz()
        {
            return this.matrixMain;
        }

        public void SetElement(int lin, int col, double valor)
        {
            if ((lin < this.linhas) && (col < this.colunas))
                this.matrixMain[lin, col] = valor;
            else
                throw new Exception("numero de linhas ou numero de colunas invalido para o valor a ser atribuido ");
        }

        public void Fill(int fatorIndice)
        {
            for (int linha = 0; linha < this.linhas; linha++)
                for (int coluna = 0; coluna < this.colunas; coluna++)
                    this.matrixMain[linha, coluna] = fatorIndice++;
        }
    
        
        /// <summary>
        /// Calcula a Matriz Inversa nao quadrática, se a matriz que chamou este metodo
        /// tenha numero de linhas <> numero de colunas.
        /// Se tiver numero de linhas==numero de colunas, calcula a inversa quadrática.
        /// </summary>
        public Matrizes Inverse()
        {
            if (this.linhas > this.colunas)
            {
                /// exemplo do calculo da matriz nao quadratica, com utilizacao de matriz auxliar.
                /// M[3,2]*M[2,3]=I(3), M[3,2] é conhecida.
                /// Aux1[2,3]*M[3,2]*M[2,3]=Aux1[2,3]*I(3),
                /// Aux1M[2,2]*M[2,3]=Aux1[2,3]
                /// Aux1M32(-1)*Aux1M[2,2]*M[2,3]=Aux1M32(-1)*Aux1[2,3]
                /// M[2,3]=Aux1M32(-1)*Aux1[2,3]
                matrixAux1 = new Matrizes(this.colunas, this.linhas);
                matrixAux1.Fill(5);


                Matrix<double> MAuxM32Inv = (matrixAux1.matrixMain * this.matrixMain).Inverse();
                Matrix<double> MInv = MAuxM32Inv * matrixAux1.matrixMain;

                Matrizes MInversa = new Matrizes(this.colunas, this.linhas);
                MInversa.matrixMain = MInv;

                return MInversa;
            }
            else
            if (this.linhas < this.colunas)
            {
                /// exemplo do calculo da matriz nao quadratica, com utilizacao de matriz auxliar.
                /// M[3,2]*M[2,3]=I(3); M[2,3] é conhecida.
                /// M[3,2]*M[2,3]*Aux1[3,2]=I(3)*Aux1[3,2]
                /// M[3,2]*MAux1[2,2]=Aux1[3,2]
                /// M[3,2]*MAux1[2,2]*MAux1[2,2](-1)=Aux1[3,2]*MAux1[2,2](-1)
                /// M[3,2]=Aux1[3,2]*MAux1[2,2](-1)

                matrixAux1 = new Matrizes(this.colunas, this.linhas);
                matrixAux1.Fill(5);
                Matrix<double> MAuxM1Inv = (this.matrixMain * matrixAux1.matrixMain).Inverse();
                Matrix<double> MInv = matrixAux1.matrixMain * MAuxM1Inv;

                Matrizes MInversa = new Matrizes(this.colunas, this.linhas);
                MInversa.matrixMain = MInv;

                return MInversa;
            }
            else
            if (this.linhas == this.colunas)
            {
                Matrix<double> mInv = this.matrixMain.Inverse();

                Matrizes MInversa = new Matrizes(this.linhas, this.colunas);
                MInversa.matrixMain = mInv;
                return MInversa;
            }
            else
                return null;
        }

        /// <summary>
        /// calcula o quanto m1 está próxima de m2, em porcentagem: 0..1.
        /// matrizes m1 e m2 devem ter o mesmo numero de linhas e colunas.
        /// numero de linhas pode ser diferente do numero de colunas.
        /// </summary>
        public static double GrauSemelhanca(Matrizes m1, Matrizes m2, int rangeRandom)
        {
            if ((m1.linhas != m2.linhas) || (m1.colunas != m2.colunas))
                return -1.0;
            matrixAux1 = new Matrizes(m1.linhas, m1.colunas);
            matrixAux2 = new Matrizes(m1.linhas, m1.colunas);

            matrixAux1.Fill(rangeRandom);
            matrixAux2.Fill(rangeRandom);
            double d1 = (matrixAux1.matrixMain * m1.matrixMain * matrixAux2.matrixMain)[0, 0];
            double d2 = (matrixAux1.matrixMain * m2.matrixMain * matrixAux2.matrixMain)[0, 0];

            return d1 / d2;

        }

        /// <summary>
        /// calcula uma matriz bijetora, tal que t(t(-1)(x))=x, t é a função matriz bijetora,
        /// </summary>
        /// <param name="M32_inicial">matriz inicial, de quaisquer dimensões</param>
        /// <returns></returns>
        public Matrizes TreinamentoMatrizBiJetora()
        {
            Matrizes M32_inicial = this;

            for (int x = 0; x < 5; x++) 
            {
                Matrizes M23_intermediaria = M32_inicial.Inverse();
                Matrizes M32_final = M23_intermediaria.Inverse();
        
                Matrizes M_erro = M32_final - M32_inicial;


                M32_inicial = M32_final - M_erro;
            }

            return M32_inicial;
        }

        public static Matrizes operator -(Matrizes m1, Matrizes m2)
        {
            if ((m1.linhas != m2.linhas) || (m1.colunas != m2.colunas))
                throw new Exception("matrizes de dimensoes diferentes, em operacao menos.");
            else
            {
                Matrizes mResult = new Matrizes(m1.linhas, m1.colunas);
                mResult.matrixMain = m1.matrixMain - m2.matrixMain;

                return mResult;

            }
        }

        public static Matrizes operator +(Matrizes m1, Matrizes m2)
        {
            if ((m1.linhas != m2.linhas) || (m1.colunas != m2.colunas))
                throw new Exception("matrizes de dimensoes diferentes, em operacao menos.");
            else
            {
                Matrizes mResult = new Matrizes(m1.linhas, m1.colunas);
                mResult.matrixMain = m1.matrixMain + m2.matrixMain;

                return mResult;

            }
        }

        public static Matrizes operator *(Matrizes m1, Matrizes m2)
        {
            if (m1.colunas != m2.linhas)
                throw new Exception("matrizes de dimensoes nao compativel com operacao de multiplicacao de matrizes");
            else
            {
                Matrizes mResult = new Matrizes(m1.linhas, m2.colunas);
                mResult.matrixMain = m1.matrixMain * m2.matrixMain;
                return mResult;
            }
        }
    }
}
