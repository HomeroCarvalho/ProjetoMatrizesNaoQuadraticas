using System;
using System.Collections.Generic;
using System.Text;
using Testes;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{



    public class TransformacaoIsometricaNaoQuadratica
    {
        public Matrizes m32;
        public Matrizes m23;

        public TransformacaoIsometricaNaoQuadratica()
        {
            this.m32 = this.M32();
            this.m23 = this.M23(m32);
        }

        private Matrizes M32()
        {
            Matrizes m32 = new Matrizes(3, 2);
            m32.SetElement(0, 0, 1.0);
            m32.SetElement(1, 0, 0.0);
            m32.SetElement(2, 0, 0.5);

            m32.SetElement(0, 1, 0.0);
            m32.SetElement(1, 1, 1.0);
            m32.SetElement(2, 1, 0.5);

            return m32;
        }

        private Matrizes M23(Matrizes m32)
        {
            return m32.Inverse();
        }


        public Matrizes MatrizCorrecao()
        {
            Matrizes mt_correcao = new Matrizes(3, 3);
            mt_correcao.matrixMain = (m32.matrixMain * m23.matrixMain).Inverse();
            return mt_correcao;
        }

        public Vector3D AplicaMatrizCorrecao(Vector3D p3_inicial)
        {
            Vector<double> p3_vec_inicial = Vector3DToVectorDouble(p3_inicial);
            Vector<double> p3_vec_final = p3_vec_inicial * MatrizCorrecao().GetMatriz();

            Vector3D p3_final = VectorDoubleToVector3D(p3_vec_final);

            return p3_final;
        }

        public Vector3D GetPonto3D(Vector2D ponto2d)
        {
            // vetor ponto3d_inicial é o vetor de controle.
            Vector<double> vector2d_inicial = Vector2DToVectorDouble(ponto2d);
            Vector<double> ponto3d_final = vector2d_inicial * m23.GetMatriz();


            return VectorDoubleToVector3D(ponto3d_final);
        }

        public Vector2D GetPonto2D(Vector3D ponto3d)
        {
            // vetor p2_inicial é o vetor de controle.
            Vector<double> p2_inicial = Vector3DToVectorDouble(ponto3d) * m32.GetMatriz();
            return VectorDoubleToVector2D(p2_inicial);
        }

        private Vector<double> Vector3DToVectorDouble(Vector3D p3)
        {
            return Vector<double>.Build.Dense(new double[] { p3.X, p3.Y, p3.Z });
        }

        private Vector<double> Vector2DToVectorDouble(Vector2D p3)
        {
            return Vector<double>.Build.Dense(new double[] { p3.X, p3.Y });
        }

        private Vector3D VectorDoubleToVector3D(Vector<double> p3)
        {
            return new Vector3D(p3[0], p3[1], p3[2]);
        }

        private Vector2D VectorDoubleToVector2D(Vector<double> p2)
        {
            return new Vector2D(p2[0], p2[1]);
        }

        public class Testes : SuiteClasseTestes
        {
            public Testes()
            {

            }
     
            public void Teste1_5(AssercaoSuiteClasse assercao)
            {
                TransformacaoIsometricaNaoQuadratica trans = new TransformacaoIsometricaNaoQuadratica();

                for (int x = 0; x < 5; x++)
                {
                    Vector<double> v3_inicial = Vector<double>.Build.Random(3);
                    Vector<double> v3_final = v3_inicial * trans.m32.GetMatriz() * trans.m23.GetMatriz();

                    System.Console.WriteLine(v3_inicial);
                    System.Console.WriteLine(v3_final);
                    System.Console.WriteLine(trans.MatrizCorrecao().matrixMain.ToString());
                   
                    System.Console.ReadLine();
                }
            }
            public void Teste1_2(AssercaoSuiteClasse assercao)
            {
                TransformacaoIsometricaNaoQuadratica trans = new TransformacaoIsometricaNaoQuadratica();

                System.Console.WriteLine(trans.MatrizCorrecao().GetMatriz().Inverse().ToString());

                for (int x = 0; x < 5; x++)
                {
                    Vector3D v3_teste_inicial = new Vector3D(1 + x * 2, 2, 3 + x);
                    Vector2D v2_inicial = trans.GetPonto2D(v3_teste_inicial);


                    Vector3D v3_teste_final = trans.GetPonto3D(v2_inicial);


                    Vector3D v3_teste_final_com_correcao = trans.AplicaMatrizCorrecao(v3_teste_final);
                    Matrizes mt_correcao = trans.MatrizCorrecao();


                    System.Console.WriteLine("vetor 3d inicial: " + v3_teste_inicial);
                    System.Console.WriteLine("vetor 3d final: " + v3_teste_final);

                    System.Console.WriteLine();
                    System.Console.WriteLine("matriz m32 inicial: " + trans.m32.GetMatriz());
                    System.Console.WriteLine("matriz m33 correcao: " + mt_correcao.GetMatriz());
                    System.Console.WriteLine("matriz m33 correcao inversa: " + mt_correcao.GetMatriz().Inverse());

                    System.Console.ReadLine();
                }
            }

            public void Teste1_1(AssercaoSuiteClasse assercao)
            {
                TransformacaoIsometricaNaoQuadratica transformacao = new TransformacaoIsometricaNaoQuadratica();


                Vector3D p3_inicial = new Vector3D(2, 6, 8);
                Vector2D ponto2d_final = transformacao.GetPonto2D(p3_inicial);
                Vector3D p3_final = transformacao.GetPonto3D(ponto2d_final);

                Vector3D p3_final_sem_delta = transformacao.GetPonto3D(ponto2d_final);

                System.Console.WriteLine("ponto 3d inicial: " + p3_inicial);
                System.Console.WriteLine("ponto 3d final com delta: " + p3_final);
                System.Console.WriteLine("ponto 3d final sem delta: " + p3_final_sem_delta);

                for (int x = 0; x < 5; x++)
                    for (int y = 0; y < 5; y++)
                    {
                        Vector2D ponto2D = new Vector2D(x, y);
                        Vector3D ponto3d = transformacao.GetPonto3D(ponto2D);
                        System.Console.WriteLine("2D: " + ponto2D + " 3D: " + ponto3d);

                        System.Console.ReadLine();

                    }

                System.Console.ReadLine();

            }

            public void Teste1(AssercaoSuiteClasse assercao)
            {
                TransformacaoIsometricaNaoQuadratica transformacao = new TransformacaoIsometricaNaoQuadratica();

                Vector2D ponto2d_inicial = new Vector2D(4, 5);
                Vector3D p3_inicial = transformacao.GetPonto3D(ponto2d_inicial);
                Vector2D ponto2d_final = transformacao.GetPonto2D(p3_inicial);
                Vector3D p3_final = transformacao.GetPonto3D(ponto2d_final);

                System.Console.WriteLine("ponto 2d inicial: " + ponto2d_inicial);
                System.Console.WriteLine("ponto 2d final: " + ponto2d_final);

                System.Console.WriteLine("ponto 3d inicial: " + p3_inicial);
                System.Console.WriteLine("ponto 3d final: " + p3_final);


                System.Console.ReadLine();




                Vector3D ponto3d_inicial = new Vector3D(2, 5, 8);
                Vector2D p2d_inicial = transformacao.GetPonto2D(ponto3d_inicial);

                System.Console.WriteLine("ponto 3d inicial: " + ponto3d_inicial.ToString());
                System.Console.WriteLine("ponto 2d inicial: " + p2d_inicial.ToString());

                System.Console.ReadLine();
            }

            public void Teste2(AssercaoSuiteClasse assercao)
            {
                Matrizes m32 = new Matrizes(3, 2);
                m32.SetElement(0, 0, 1.0);
                m32.SetElement(1, 0, 0.0);
                m32.SetElement(2, 0, 0.5);

                m32.SetElement(0, 1, 0.0);
                m32.SetElement(1, 1, 1.0);
                m32.SetElement(2, 1, 0.5);

                Matrizes m23 = m32.Inverse();

                System.Console.WriteLine("matriz original: " + m32.GetMatriz().ToString());
                System.Console.WriteLine("matriz invera:" + m23.GetMatriz().ToString());

                System.Console.WriteLine("matriz auxiliar: " + Matrizes.matrixAux1.GetMatriz().ToString());


                System.Console.ReadLine();
                assercao.IsTrue(true); // teste executado sem erros fatais.
            }


            public void Teste3(AssercaoSuiteClasse assercao)
            {
                Matrizes M32 = new Matrizes(3, 2);
                M32.SetElement(0, 0, 1.0);
                M32.SetElement(1, 0, 0.0);
                M32.SetElement(2, 0, 0.5);

                M32.SetElement(0, 1, 0.0);
                M32.SetElement(1, 1, 1.0);
                M32.SetElement(2, 1, 0.5);

                Matrizes M32_bijetora = M32.TreinamentoMatrizBiJetora();

                Matrizes vetor3_inicial = new Matrizes(1, 3);
                vetor3_inicial.SetElement(0, 0, 1);
                vetor3_inicial.SetElement(0, 1, 2);
                vetor3_inicial.SetElement(0, 2, 3);
                Matrizes vetor2_intermediario = vetor3_inicial * M32_bijetora;
                Matrizes vetor3_final = vetor2_intermediario * M32_bijetora.Inverse();

                Console.WriteLine("vetor 3d inicial: " + vetor3_inicial.GetMatriz());
                Console.WriteLine("vetor 3d final: " + vetor3_final.GetMatriz());
                Console.WriteLine();
                Console.WriteLine("matriz M32 inicial: " + M32.GetMatriz().ToString());
                Console.WriteLine("matriz M32 final: " + M32_bijetora.GetMatriz().ToString());
                Console.WriteLine();
                Console.WriteLine();


                Console.ReadLine();

            }

            public void Teste4(AssercaoSuiteClasse assercao)
            {
                Matrizes m1 = new Matrizes(3, 3);
                Matrizes m2 = new Matrizes(3, 3);
                m1.Fill(11);
                m2.Fill(6);

                double g = Matrizes.GrauSemelhanca(m1, m2, 11);

                Console.WriteLine(m1.GetMatriz());
                Console.WriteLine(m2.GetMatriz());

                Console.WriteLine("grau: " + g);

                Console.ReadLine();

            }

        }



    }

}
