using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDijkstra
{
  public class Nodo
  {
    private string nome;
    private List<Vertice> listVert;

    public Nodo(string nome)
    {
      this.nome = nome;
      this.listVert = new List<Vertice>();
    }

    public string Nome
    {
      get { return this.nome; }
    }

    public List<Vertice> ListVert
    {
      get { return this.listVert; }
    }

    public void AddVertice(Vertice vert)
    {
      listVert.Add(vert);
    }
  }
}
