using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Gráfico
{
    class Ponto : IComparable<Ponto>, ICriterioDeSeparacao, IRegistro
    {
        private int x, y;
        private Color cor;

        public Ponto(int cX, int cY, Color qualCor)
        {
            X = cX;
            Y = cY;
            Cor = qualCor;
        }

        public int X
        {
            get => x; set => x = value;
        }
        public int Y
        {
            get => y; set => y = value;
        }
        public Color Cor
        {
            get => cor; set => cor = value;
        }

        public virtual void Desenhar(Color cor, Graphics g)
        {
            Pen pen = new Pen(cor);
            g.DrawLine(pen, x, y, x, y);
        }

        //Métodos das interfaces
        public int CompareTo(Ponto other)
        {
            int diferencaX = X - other.X;
            if (diferencaX == 0)
                return Y - other.Y;
            return diferencaX;
        }
        public string FormatoDeRegistro()
        {
            throw new NotImplementedException();
        }
        public bool PodeSeparar()
        {
            throw new NotImplementedException();
        }


        //Métodos para leitura de arquivos do btnAbrir
        public String transformaString(int valor, int quantasPosicoes)
        {
            String cadeia = valor + "";
            while (cadeia.Length < quantasPosicoes)
                cadeia = " " + cadeia;
            return cadeia.Substring(0, quantasPosicoes); // corta, se necessário, para
                                                         //tamanho máximo
        }
        public String transformaString(String valor, int quantasPosicoes)
        {
            String cadeia = valor;
            while (cadeia.Length < quantasPosicoes)
                cadeia = cadeia + " ";
            return cadeia.Substring(0, quantasPosicoes); // corta, se necessário, para
                                                         // tamanho máximo
        }
        public override String ToString()
        {
            return transformaString("p", 5) +
            transformaString(X, 5) +
            transformaString(Y, 5) +
            transformaString(Cor.R, 5) +
            transformaString(Cor.G, 5) +
            transformaString(Cor.B, 5);
        }
    }
}
