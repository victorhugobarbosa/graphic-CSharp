using System;
using System.Drawing;

namespace Gráfico
{
    class Elipse : Ponto
    {
        private int raioX, raioY;
        public int RaioX
        {
            get { return raioX; }
            set { raioX = value; }
        }

        public int RaioY
        {
            get { return raioY; }
            set { raioY = value; }
        }

        public Elipse(int xCentro, int yCentro, int novoRaioX, int novoRaioY, Color novaCor) :
        base(xCentro, yCentro, novaCor) // construtor de Ponto(x,y)
        {
            raioX = novoRaioX;
            raioY = novoRaioY;
        }

        public override void Desenhar(Color corDesenho, Graphics g)
        {
            Pen pen = new Pen(corDesenho);
            g.DrawEllipse(pen, base.X - raioX , base.Y - raioY, // centro - raio
                                         2 * raioX, 2 * raioY); // centro + raio
        }

        //toString para leitura de arquivos
        public override String ToString()
        {
            return transformaString("e", 5) +
            transformaString(X, 5) +
            transformaString(Y, 5) +
            transformaString(Cor.R, 5) +
            transformaString(Cor.G, 5) +
            transformaString(Cor.B, 5) +
            transformaString(RaioX, 5) +
            transformaString(RaioY, 5);
        }
    }
}