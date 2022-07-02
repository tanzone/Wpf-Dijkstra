using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDijkstra
{
  public class Vertice
  {
    private string nome;
    private int distanza;
   
    public Vertice(string nome, int distanza)
    {
      this.nome     = nome;
      this.distanza = distanza;
    }

    public int Distanza
    {
      get { return this.distanza; }
      set { this.distanza = value; }
    }

    public string Nome
    {
      get { return this.nome; }
    }
  }
}
