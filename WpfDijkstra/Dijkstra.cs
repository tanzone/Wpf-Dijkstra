using System.Windows.Forms;

namespace WpfDijkstra
{
  class Dijkstra
  {
    private readonly long[] spf;
    private readonly int[] prev;
    private int a;
    private int i;
    private int peso;
    private int salti;
    private readonly int[] cammino;

    public int Peso
    {
      get { return this.peso; }
    }
    public int Salti
    {
      get { return this.salti; }
    }
    public int[] Cammino
    {
      get { return this.cammino; }
    }

    public Dijkstra(int dim)
    {
      this.spf = new long[dim];
      this.prev = new int[dim];
      this.a = 0;
      this.i = 0;
      this.peso = 0;
      this.salti = 0;
      this.cammino = new int[dim];
    }

    public void CalcolaPercorso(int nodopartenza, int nododestinazione, int n, long[][] adiacenze, int[] camm)
    {
      // Inizializzazione del vettore di output del percorso
      for (int i = 0; i < n; ++i)
        camm[i] = -1;

      Algoritmo(nodopartenza, n, ref adiacenze);
      this.peso = (int)spf[nododestinazione];
      this.salti = StampaPercorso(nododestinazione, camm);
    }


    private void Algoritmo(int source, int n, ref long[][] adiacenze)
    {
      int i, k, mini;
      bool[] visitato = new bool[100];

      // Inizializzazione dei vettori di supporto
      for (i = 0; i < n; ++i)
      {
        spf[i] = 999999;
        prev[i] = -1;
        visitato[i] = false;
      }

      spf[source] = 0;

      for (k = 0; k < n; ++k)
      {
        mini = -1;
        for (i = 0; i < n; ++i)
        {
          if (!visitato[i] && ((mini == -1) || (spf[i] < spf[mini])))
            mini = i;
        }

        visitato[mini] = true;

        for (i = 0; i < n; ++i)
        {
          if (adiacenze[mini][i] != 0)
            if (spf[mini] + adiacenze[mini][i] < spf[i])
            {
              spf[i] = spf[mini] + adiacenze[mini][i];
              prev[i] = mini;
            }
        }
      }
    }

    private int StampaPercorso(int dest, int[] camm)
    {
      a++;
      if (prev[dest] != -1)
        StampaPercorso(prev[dest], camm);

      if (a == 1) { }
      else
      {
        camm[i] = dest;
        i++;
      }
      a = 0;

      return i;
    }
  }
}
