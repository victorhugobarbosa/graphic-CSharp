using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grafico;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Diagnostics.Eventing.Reader;

namespace Gráfico
{
    public partial class frmGrafico : Form
    {
        public frmGrafico()
        {
            InitializeComponent();
        }

        private ListaSimples<Ponto> figuras = new ListaSimples<Ponto>();
        Color corAtual = Color.Black;
        private static Ponto p1 = new Ponto(0, 0, Color.Black);

        Point coordenadaMouse; //usada no efeito de desenhar a figura durante o processo de cria-la

        //Ponto
        bool esperaPonto = false;

        //Reta
        bool esperaInicioReta = false;
        bool esperaFimReta = false;

        //Circulo
        bool esperaCentroCirculo = false;
        bool esperaRaioCirculo = false;

        //Elipse
        bool esperaCentroElipse = false;
        bool esperaRaioElipse = false;

        //Retângulo
        bool esperaInicioRetangulo = false;
        bool esperaTamRetangulo = false;

        //Polilinha 
        bool esperaInicioPolilinha = false;
        bool esperaFimPolilinha = false;
        Polilinha polilinha;


        private void limparEsperas()
        {
            //Primeiro ponto
            p1 = new Ponto(0, 0, Color.Black);
            //Ponto
            esperaPonto = false;
            //Reta
            esperaInicioReta = false;
            esperaFimReta = false;
            //Circulo
            esperaCentroCirculo = false;
            esperaRaioCirculo = false;
            //Elipse
            esperaCentroElipse = false;
            esperaRaioElipse = false;
            //Retangulo
            esperaInicioRetangulo = false;
            esperaTamRetangulo = false;
            //Polilinha
            esperaInicioPolilinha = false;
            esperaFimPolilinha = false;
            polilinha = null;
        }

        private void pbAreaDesenho_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // acessa contexto gráfico
            figuras.IniciarPercursoSequencial();
            while (figuras.PodePercorrer())
            {
                Ponto figuraAtual = figuras.Atual.Info;
                figuraAtual.Desenhar(figuraAtual.Cor, g);

                figuras.Prosseguir(); //att pra continuar o fluxo
            }

            /*As verificações abaixo possiblitam o efeito de desenhar a figura durante o processo de cria-la,
             juntamente com os dados fonecidos do MouseMove() e o Refresh() dado a cada movimento do mouse para chamar o Paint()*/

            //Reta
            if (esperaFimReta)
            {
                Reta desenhaReta = new Reta(p1.X, p1.Y, coordenadaMouse.X, coordenadaMouse.Y, corAtual);
                desenhaReta.Desenhar(desenhaReta.Cor, g);
            }

            //Circulo
            else if(esperaRaioCirculo)
            {
                           //distância entre dois pontos: d=√((x2-x1)²+(y2-y1)²)
                int raio = (int)Math.Sqrt(Math.Pow(coordenadaMouse.X - p1.X, 2) + Math.Pow(coordenadaMouse.Y - p1.Y, 2));
                Circulo desenhaCirculo = new Circulo(p1.X, p1.Y, raio, corAtual);
                desenhaCirculo.Desenhar(desenhaCirculo.Cor, g);
            }

            //Elipse
            else if (esperaRaioElipse)
            {
                Elipse desenhaElipse = new Elipse(p1.X, p1.Y, Math.Abs(coordenadaMouse.X - p1.X), Math.Abs(coordenadaMouse.Y - p1.Y), corAtual);
                desenhaElipse.Desenhar(desenhaElipse.Cor, g);
            }

            //Retangulo
            else if(esperaTamRetangulo)
            {
                Retangulo desenhaRetangulo = new Retangulo(p1.X, p1.Y, Math.Abs(coordenadaMouse.X - p1.X), Math.Abs(coordenadaMouse.Y - p1.Y), corAtual);
                desenhaRetangulo.Desenhar(desenhaRetangulo.Cor, g);
            }

            //Polilinha
            else if(esperaFimPolilinha)
            {
                Reta retaPolilinha = new Reta(polilinha.Pontos.Atual.Info.X, polilinha.Pontos.Atual.Info.Y, coordenadaMouse.X, coordenadaMouse.Y, corAtual);
                retaPolilinha.Desenhar(retaPolilinha.Cor, g);
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                figuras = new ListaSimples<Ponto>(); //limpa a lista ligada de figuras
                pbAreaDesenho.BackColor = Color.White;
                pbAreaDesenho.Refresh(); //limpa a tela

                try
                {
                    StreamReader arqFiguras = new StreamReader(dlgAbrir.FileName);
                    string linha;
                    while ((linha = arqFiguras.ReadLine()) != null)
                    {
                        string tipo = linha.Substring(0, 5).Trim();
                        if (tipo[0].ToString() == "C" || tipo[0].ToString() == "c") //if is the bg color
                        {
                            if(tipo[0].ToString() == "c")
                            {
                                pbAreaDesenho.BackColor = Color.FromName(linha.Replace("c","").Trim());
                            }
                            else //ARRUMAR SUBSTRING 
                            {
                                var rgbCode = linha.Replace("C", "").Replace("]","");
                                int red = Convert.ToInt32(rgbCode.Substring(0, 3));
                                int green = Convert.ToInt32(rgbCode.Substring(5, 3));
                                int blue = Convert.ToInt32(rgbCode.Substring(10, 3));
                                pbAreaDesenho.BackColor = Color.FromArgb(red, green, blue);
                            }
                        }
                        else
                        {
                            int xBase = Convert.ToInt32(linha.Substring(5, 5).Trim());
                            int yBase = Convert.ToInt32(linha.Substring(10, 5).Trim());
                            int corR = Convert.ToInt32(linha.Substring(15, 5).Trim());
                            int corG = Convert.ToInt32(linha.Substring(20, 5).Trim());
                            int corB = Convert.ToInt32(linha.Substring(25, 5).Trim());
                            Color cor = new Color();
                            cor = Color.FromArgb(255, corR, corG, corB);

                            switch (tipo[0])
                            {
                                case 'p': //Ponto
                                    figuras.InserirAposFim(new Ponto(xBase, yBase, cor));
                                    break;
                                case 'l': //Reta ('l' de "linha")
                                    int xFinal = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                    int yFinal = Convert.ToInt32(linha.Substring(35, 5).Trim());
                                    figuras.InserirAposFim(new Reta(xBase, yBase, xFinal, yFinal, cor));
                                    break;
                                case 'c': //Círculo
                                    int raio = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                    figuras.InserirAposFim(new Circulo(xBase, yBase, raio, cor));
                                    break;
                                case 'e': //Elipse
                                    int raioX = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                    int raioY = Convert.ToInt32(linha.Substring(35, 5).Trim());
                                    figuras.InserirAposFim(new Elipse(xBase, yBase, raioX, raioY, cor));
                                    break;
                                case 'r': //Retangulo
                                    int largura = Convert.ToInt32(linha.Substring(30, 5).Trim());
                                    int altura = Convert.ToInt32(linha.Substring(35, 5).Trim());
                                    figuras.InserirAposFim(new Retangulo(xBase, yBase, largura, altura, cor));
                                    break;
                                    // Não fizemos da polilinha pq ela é salva como várias retas
                            }
                        }
                    }
                    arqFiguras.Close();
                    Text = dlgAbrir.FileName;
                    pbAreaDesenho.Invalidate();
                }
                catch (IOException)
                {
                    Console.WriteLine("Erro de leitura no arquivo");
                }
            }
        }

        private void pbAreaDesenho_MouseClick(object sender, MouseEventArgs e)
        {
            //Ponto
            if (esperaPonto)
            {
                esperaPonto = false;
                Ponto novoPonto = new Ponto(e.X, e.Y, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(novoPonto, null));
                novoPonto.Desenhar(novoPonto.Cor, pbAreaDesenho.CreateGraphics());
                stMensagem.Items[1].ForeColor = Color.Black;
                stMensagem.Items[1].Text = "escolha uma opção:";
            }

            //Reta
            else if (esperaInicioReta)
            {
                esperaInicioReta = false;
                esperaFimReta = true;
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                stMensagem.Items[1].Text = "clique no local do ponto final da reta:";
            }
            else if(esperaFimReta)
            {
                esperaFimReta = false;
                Reta novaLinha = new Reta(p1.X, p1.Y, e.X, e.Y, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(novaLinha, null));
                novaLinha.Desenhar(novaLinha.Cor, pbAreaDesenho.CreateGraphics());
                stMensagem.Items[1].ForeColor = Color.Black;
                stMensagem.Items[1].Text = "escolha uma opção:";
            }

            //Circulo
            else if(esperaCentroCirculo)
            {
                esperaCentroCirculo = false;
                esperaRaioCirculo = true;
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                stMensagem.Items[1].Text = "clique no local que definirá o raio do circulo:";
            }
            else if (esperaRaioCirculo)
            {
                esperaRaioCirculo = false;

                //distância entre dois pontos: d=√((x2-x1)²+(y2-y1)²)
                int raio = (int)Math.Sqrt(Math.Pow(e.X - p1.X, 2) + Math.Pow(e.Y - p1.Y, 2));

                Circulo novoCirculo = new Circulo(p1.X, p1.Y, raio, corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(novoCirculo, null));
                novoCirculo.Desenhar(novoCirculo.Cor, pbAreaDesenho.CreateGraphics());
                stMensagem.Items[1].ForeColor = Color.Black;
                stMensagem.Items[1].Text = "escolha uma opção:";
            }

            //Elipse
            else if(esperaCentroElipse)
            {
                esperaCentroElipse = false;
                esperaRaioElipse = true;
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                stMensagem.Items[1].Text = "clique no local que definirá os raios da elipse:";
            }
            else if(esperaRaioElipse)
            {
                esperaRaioElipse = false;
                Elipse novaElipse = new Elipse(p1.X, p1.Y, Math.Abs(e.X - p1.X), Math.Abs(e.Y - p1.Y), corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(novaElipse, null));
                novaElipse.Desenhar(novaElipse.Cor, pbAreaDesenho.CreateGraphics());
                stMensagem.Items[1].ForeColor = Color.Black;
                stMensagem.Items[1].Text = "escolha uma opção:";
            }

            //Retangulo
            else if (esperaInicioRetangulo)
            {
                esperaInicioRetangulo = false;
                esperaTamRetangulo = true;
                p1.Cor = corAtual;
                p1.X = e.X;
                p1.Y = e.Y;
                stMensagem.Items[1].Text = "clique no local que definirá o tamanho do retângulo(largura e altura):";
            }
            else if (esperaTamRetangulo)
            {
                esperaTamRetangulo = false;
                Retangulo novoRetangulo = new Retangulo(p1.X, p1.Y, Math.Abs(e.X - p1.X), Math.Abs(e.Y - p1.Y), corAtual);
                figuras.InserirAposFim(new NoLista<Ponto>(novoRetangulo));
                novoRetangulo.Desenhar(novoRetangulo.Cor, pbAreaDesenho.CreateGraphics());
                stMensagem.Items[1].ForeColor = Color.Black;
                stMensagem.Items[1].Text = "escolha uma opção:";
            }

            //Polilinha
            else if(esperaInicioPolilinha)
            {
                esperaInicioPolilinha = false;
                esperaFimPolilinha = true;
                stMensagem.Items[1].Text = "escolha o próximo ponto:";
                polilinha = new Polilinha(e.X, e.Y, corAtual);
            }
            else if(esperaFimPolilinha)
            {
                stMensagem.Items[1].Text = "(botão direito do mouse finaliza a polilinha)escolha o próximo ponto:";

                if (e.Button.Equals(MouseButtons.Right)) //se clickar no botão direito do mouse, finaliza a polilinha
                {
                    esperaFimPolilinha = false;
                    stMensagem.Items[1].ForeColor = Color.Black;
                    stMensagem.Items[1].Text = "escolha uma opção:";

                    polilinha.Desenhar(polilinha.Cor, pbAreaDesenho.CreateGraphics(), true); //é a ultima reta a ser desenhada
                    figuras.InserirAposFim(new NoLista<Ponto>(polilinha.RetaAtual));
                    limparEsperas();
                }
                else 
                {
                    polilinha.Pontos.InserirAposFim(new NoLista<Ponto>(new Ponto(e.X, e.Y, corAtual))); //insere um novo ponto
                    polilinha.Desenhar(polilinha.Cor, pbAreaDesenho.CreateGraphics());
                    figuras.InserirAposFim(new NoLista<Ponto>(polilinha.RetaAtual));
                }
            }
        }

        private void btnPonto_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].Text = "clique no local do ponto desejado:";
            limparEsperas();
            esperaPonto = true;
        }

        private void pbAreaDesenho_MouseMove(object sender, MouseEventArgs e)
        {
            stMensagem.Items[3].Text = "x(" + e.X + ")" + " | " + "y(" + e.Y + ")";

            //os comandos abaixo auxiliam no efeito de desenhar a figura durante o processo de cria-la
            coordenadaMouse.X = e.X;
            coordenadaMouse.Y = e.Y;
            Refresh();
        }

        private void btnReta_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].ForeColor = Color.DarkBlue;
            stMensagem.Items[1].Text = "clique no local do ponto inicial da reta:";
            limparEsperas();
            esperaInicioReta = true;
        }

        private void btnCirculo_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].ForeColor = Color.Crimson;
            stMensagem.Items[1].Text = "clique no local do ponto central do circulo:";
            limparEsperas();
            esperaCentroCirculo = true;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja fechar o programa?", "Fechar Programa", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            Close();
        }

        private void btnElipse_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].ForeColor = Color.Green;
            stMensagem.Items[1].Text = "clique no local do ponto central da elipse:";
            limparEsperas();
            esperaCentroElipse = true;
        }

        private void btnRetangulo_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].ForeColor = Color.Magenta;
            stMensagem.Items[1].Text = "clique no local do ponto inicial do retângulo(canto superior esquerdo):";
            limparEsperas();
            esperaInicioRetangulo = true;
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            //verifica se o usuário deseja mesmo apagar o desenho
            if(MessageBox.Show("Deseja apagar todo seu desenho?", "Apagar Desenho", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                figuras = new ListaSimples<Ponto>(); //limpa a lista ligada de figuras
                pbAreaDesenho.BackColor = Color.White;
                pbAreaDesenho.Refresh(); //limpa a tela
                

                limparEsperas();
                stMensagem.Items[1].ForeColor = Color.Black;
                stMensagem.Items[1].Text = "escolha uma opção:";
            }
        }

        private void btnCor_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].ForeColor = Color.Peru;
            stMensagem.Items[1].Text = "escolha uma cor:";
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                corAtual = colorDialog1.Color;
            }

            stMensagem.Items[1].ForeColor = Color.Black;
            stMensagem.Items[1].Text = "escolha uma opção:";
        }

        private void btnPolilinha_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].ForeColor = Color.Orange;
            stMensagem.Items[1].Text = "clique no local do ponto inicial da reta:";
            limparEsperas();
            esperaInicioPolilinha = true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            dlgSalvar.Filter = "Arquivo de Texto | *.txt"; //salva como arquivo de texto
            if (dlgSalvar.ShowDialog() == DialogResult.OK)
            {
                figuras.GravarArquivo(dlgSalvar.FileName, pbAreaDesenho.BackColor.ToString());
            }
        }

        private void btnCorDeFundo_Click(object sender, EventArgs e)
        {
            stMensagem.Items[1].ForeColor = Color.Indigo;
            stMensagem.Items[1].Text = "escolha uma cor de fundo:";
            if(colorDialog2.ShowDialog() == DialogResult.OK)
            {
                pbAreaDesenho.BackColor = colorDialog2.Color;
            }

            stMensagem.Items[1].ForeColor = Color.Black;
            stMensagem.Items[1].Text = "escolha uma opção:";
        }
    }
}
