using Gráfico;
using System;
using System.Drawing;
using System.Security.Cryptography.Xml;

namespace Grafico
{
    class Polilinha : Ponto
    {
        private ListaSimples<Ponto> pontos;
        private Reta retaAtual;

        public Reta RetaAtual { get => retaAtual; }
        public ListaSimples<Ponto> Pontos { get => pontos; }

        public Polilinha(int x1, int y1, Color novaCor) : base (x1, y1, novaCor)
        {
            pontos = new ListaSimples<Ponto>();
            pontos.InserirAposFim(new NoLista<Ponto>(new Ponto(base.X,base.Y,novaCor))); //insere o primeiro ponto na lista de pontos
            pontos.IniciarPercursoSequencial(); //começa a percorrer a lista de pontos
        }

        public override void Desenhar(Color corDesenho, Graphics g)
        {
            Pen pen = new Pen(corDesenho);

            if(pontos.PodePercorrer())
            {
                g.DrawLine(pen, pontos.Atual.Info.X, pontos.Atual.Info.Y, pontos.Atual.Prox.Info.X, pontos.Atual.Prox.Info.Y);
                retaAtual = new Reta(pontos.Atual.Info.X, pontos.Atual.Info.Y, pontos.Atual.Prox.Info.X, pontos.Atual.Prox.Info.Y, corDesenho);

                pontos.Prosseguir(); //att
            }
        }
        public void Desenhar(Color corDesenho, Graphics g, bool final) //sobrecarga do método Desenhar para desenhar a ultima reta 
        {
            if(final)
            {
                Pen pen = new Pen(corDesenho);
                g.DrawLine(pen, pontos.Primeiro.Info.X, pontos.Primeiro.Info.Y, pontos.Ultimo.Info.X, pontos.Ultimo.Info.Y); //ultimo ponto se liga ao primeiro
                retaAtual = new Reta(pontos.Primeiro.Info.X, pontos.Primeiro.Info.Y, pontos.Ultimo.Info.X, pontos.Ultimo.Info.Y, corDesenho);
            }
        }

        public int XInicial
        { get => pontos.Primeiro.Info.X; 
        }

        public int YInicial
        { get => pontos.Primeiro.Info.Y; }


        //Não fizemos o ToString() pois a polilinha é salva como varias retas
        public override String ToString()
        {
            return retaAtual.ToString();
        }
    }
}
