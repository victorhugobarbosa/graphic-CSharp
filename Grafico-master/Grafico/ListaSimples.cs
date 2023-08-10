using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

public class ListaSimples<Dado> where Dado : IComparable<Dado>,
    ICriterioDeSeparacao, IRegistro
{
    NoLista<Dado> primeiro, ultimo, atual, anterior;
    int quantosNos;

    public ListaSimples()
    { 
        primeiro = ultimo = anterior = atual = null; 
        quantosNos = 0; 
    }

    public bool EstaVazia
    {
        get => primeiro == null;
    }

    public NoLista<Dado> Primeiro { get => primeiro; set =>  primeiro = value; }
    public NoLista<Dado> Ultimo { get => ultimo; set => ultimo = value; }
    public NoLista<Dado> Atual { get => atual; set => atual = value;  }
    public NoLista<Dado> Anterior { get => anterior; set => anterior = value;  }
    public int QuantosNos { get => quantosNos; } //fiz esse getter pra verificação da polilinha

    public List<Dado> Lista() 
    {
        var lista = new List<Dado>();
        atual = primeiro; 
        while (atual != null) 
        { 
            lista.Add(atual.Info);
            atual = atual.Prox; 
        }
        return lista;
    }

    public void InserirAposFim(Dado novoDado)
    {
        var novoNo = new NoLista<Dado>(novoDado);
        if (EstaVazia)
            primeiro = novoNo;
        else
            ultimo.Prox = novoNo;

        ultimo = novoNo;
        ultimo.Prox = null;
        quantosNos++;
    }

    public void InserirAposFim(NoLista<Dado> noExistente)
    {
        if (EstaVazia)
            primeiro = noExistente;
        else
            ultimo.Prox = noExistente;

        ultimo = noExistente;
        ultimo.Prox = null;

        quantosNos++;
    }

    public void IniciarPercursoSequencial()
    {
        anterior = null;
        atual = primeiro;
        //o nome do método é bem sugestivo kkkk
    }

    public void GravarArquivo(string nomeArquivo, string bgColor)
    {
        var arquivo = new StreamWriter(nomeArquivo);
        atual = primeiro;
        while (atual != null)
        {
            arquivo.WriteLine(atual.Info.ToString());
            atual = atual.Prox;
        }

        //to save the bg color
        string rgbText = "";
        bgColor += "     ";
        if (bgColor.IndexOf("=") == -1) //is a name
        {
            rgbText += "c";
            for(int i = 7; i < bgColor.Length; i++)
            {
                string text = bgColor[i].ToString();
                if (text == "]")
                    break;
                else{
                    rgbText += bgColor[i].ToString();
                }
            }
            rgbText += "     ";
        }
        else
        {
            rgbText += "C";
            for (int i = 0; i < bgColor.Length; i++)
            {
                string text = bgColor[i].ToString();
                if (text == "R" || text == "G" || text == "B")
                {
                    string colorValue = bgColor[i + 2].ToString();
                    if(bgColor[i + 3].ToString() != ",")
                        colorValue += bgColor[i + 3].ToString();
                    if (bgColor[i + 3].ToString() != " " && bgColor[1+3].ToString() != "]")
                        colorValue += bgColor[i + 4].ToString();

                    switch (colorValue.Length)
                    {
                        case 1:
                            colorValue += "    "; 
                            break;
                        case 2:
                            colorValue += "   ";
                            break;
                        case 3:
                            colorValue += "  ";
                            break;
                    }

                    rgbText += colorValue.Replace(",", "").Replace("=", "").Replace("R", "").Replace("G", "").Replace("B", "").Replace("]", "");
                }
            }
        }
        
        arquivo.WriteLine(rgbText);
        arquivo.Close();
    }

    public bool PodePercorrer()
    {
        if (atual == null) return false; //significa q chegou no ponteiro do ultimo, n tem mais nada pra percorrer
        
        return true;
    }

    public void Prosseguir() //método que att os atributos da lista pra prosseguir percorrendo-a
    {
        atual = atual.Prox;
    }
}

