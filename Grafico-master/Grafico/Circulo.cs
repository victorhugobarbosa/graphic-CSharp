using Gráfico;
using System;
using System.Drawing;
namespace Grafico
{
    class Circulo : Ponto
    {
        // herda o ponto central (x, y) da classe Ponto
        private int raio;
        public int Raio
        {
            get { return raio; }
            set { raio = value; }
        }
        public Circulo(int xCentro, int yCentro, int novoRaio, Color novaCor) :
        base(xCentro, yCentro, novaCor) // construtor de Ponto(x,y)
        {
            raio = novoRaio;
        }

        public override void Desenhar(Color corDesenho, Graphics g)
        {
            Pen pen = new Pen(corDesenho);
            g.DrawEllipse(pen, base.X - raio, base.Y - raio, // centro - raio
            2 * raio, 2 * raio); // centro + raio
        }

        //toString para leitura de arquivos
        public override String ToString()
        {
            return transformaString("c", 5) +
            transformaString(X, 5) +
            transformaString(Y, 5) +
            transformaString(Cor.R, 5) +
            transformaString(Cor.G, 5) +
            transformaString(Cor.B, 5) +
            transformaString(Raio, 5);
        }
    }
}