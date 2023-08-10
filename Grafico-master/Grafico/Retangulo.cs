using Gráfico;
using System;
using System.Drawing;

namespace Gráfico
{
    internal class Retangulo : Ponto
    {
        private int largura, altura;
        public int Largura { get => largura; set => largura = value; }
        public int Altura { get => altura; set => altura = value; }

        public Retangulo(int xCentro, int yCentro, int largura, int altura, Color novaCor) : base(xCentro, yCentro, novaCor)
        {
            this.largura = largura;
            this.altura = altura;
        }
        public override void Desenhar(Color corDesenho, Graphics g)
        {
            Pen pen = new Pen(corDesenho);
            g.DrawRectangle(pen, base.X, base.Y, largura, altura);
        }

        //toString para leitura de arquivos
        public override String ToString()
        {
            return transformaString("r", 5) +
            transformaString(X, 5) +
            transformaString(Y, 5) +
            transformaString(Cor.R, 5) +
            transformaString(Cor.G, 5) +
            transformaString(Cor.B, 5) +
            transformaString(Largura, 5) +
            transformaString(Altura, 5);
        }
    }
}
