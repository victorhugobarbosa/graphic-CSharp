using System;
using System.Drawing;

namespace Gráfico
{
    class Reta : Ponto
    {
        // herda (x, y) da classe Ponto, que são as coordenadas
        // do ponto inicial da reta; também herda a cor e, em
        // seguida define o ponto final:

        private Ponto pontoFinal;

        public Reta(int x1, int y1, int x2, int y2, Color novaCor) : base(x1, y1, novaCor)
        {
            pontoFinal = new Ponto(x2, y2, novaCor);
        }

        public override void Desenhar(Color corDesenho, Graphics g)
        {
            Pen pen = new Pen(corDesenho);
            g.DrawLine(pen, base.X, base.Y, // ponto inicial
                            pontoFinal.X, pontoFinal.Y);
        }

        public int XInicial
        {
            get => base.X; 
            set => base.X = value; 
        }

        public int YInicial
        {
            get => base.Y; 
            set => base.Y = value; 
        }

        public int XFinal
        {
            get => pontoFinal.X; 
            set => pontoFinal.X = value; 
        }

        public int YFinal
        {
            get => pontoFinal.Y; 
            set => pontoFinal.Y = value;
        }

        //toString para leitura de arquivos
        public override String ToString()
        {
            return transformaString("l", 5) +
            transformaString(X, 5) +
            transformaString(Y, 5) +
            transformaString(Cor.R, 5) +
            transformaString(Cor.G, 5) +
            transformaString(Cor.B, 5) +
            transformaString(XFinal, 5) +
            transformaString(YFinal, 5);
        }
    }
}
