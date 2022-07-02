using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace WpfDijkstra
{
  public class ConfigData
  {
    private List<string> listProv;
    private List<Nodo> listNodi;

    private readonly List<string> costProv;

    public ConfigData()
    {
      listProv = new List<string>();
      listNodi = new List<Nodo>();

      costProv = new List<string>
      {
        "Parma", "Piacenza", "Ferrara", "Bologna", "Modena", "Forli-Cesena", "Ravenna", "Rimini", "Reggio Emilia"
      };
    }

    public List<String> ListProv
    {
      get { return this.listProv; }
    }

    public List<Nodo> ListNodi
    {
      get { return this.listNodi; }
    }

    public void InizializzaData(string xmlData)
    {
      if (!System.IO.File.Exists(xmlData))
        ScriviXml(xmlData);

      LeggiXml(xmlData);
    }

    #region APERTURA scrivo su file xml dei dati nel caso in cui non esistesse

    private void ScriviXml(string xmlData)
    {
      Random rnd = new Random();

      XDocument doc = new XDocument(
          new XElement("emilia-romagna",
            new XElement("headers", costProv.Select(x => new XElement("citta", x))))
      );

      for (int i = 0; i < costProv.Count; i++)
      {
        doc.Descendants("emilia-romagna").Last().Add(new XElement("nodo", new XElement("nome", costProv[i])));
        for (int j = 0; j < costProv.Count; j++)
          if (j == i)
            doc.Descendants("nodo").Last().Add(new XElement("vertice", "" + "0"));
          else
            doc.Descendants("nodo").Last().Add(new XElement("vertice", "" + rnd.Next(1, 100)));
      }
      doc.Save(xmlData);
    }

    #endregion

    #region APERTURA leggo il file xml dei dati
    private void LeggiXml(string xmlData)
    {
      XDocument objDoc = XDocument.Load(xmlData);
      int i;

      foreach (string provincia in objDoc.Descendants("citta"))
        listProv.Add(provincia);

      foreach (var citta in objDoc.Descendants("nodo"))
      {
        Nodo tmp = new Nodo(citta.Element("nome").Value);

        i = 0;
        foreach (string vertice in citta.Descendants("vertice"))
        {
          try
          {
            tmp.AddVertice(new Vertice(listProv[i], Int32.Parse(vertice)));
            i++;
          }
          catch (Exception)
          {
            MessageBox.Show("Errore durante la lettura del file xml dei dati...", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
            Application.Current.Shutdown();
          }
        }
        listNodi.Add(tmp);
      }
    }
    #endregion

    public bool CercaProv(string prov)
    {
      foreach (string i in listProv)
        if (i.ToUpper().Equals(prov))
          return true;
      return false;
    }

    public void AddProv(string prov)
    {
      Random rnd = new Random();

      this.listProv.Add(prov);
      Nodo tmp = new Nodo(prov);
      foreach (Nodo i in listNodi)
      {
        i.AddVertice(new Vertice(prov, rnd.Next(1, 100)));
        tmp.AddVertice(new Vertice(i.Nome, rnd.Next(1, 100)));
      }
      tmp.AddVertice(new Vertice(prov, 0));

      this.listNodi.Add(tmp);
    }

    public void CercaNodo(string part, string arr, int dist)
    {
      foreach (Nodo n in listNodi)
        if (n.Nome.Equals(part))
          ModificaDistVert(arr, dist, n);
    }

    public void ModificaDistVert(string arr, int dist, Nodo n)
    {
      foreach (Vertice v in n.ListVert)
        if (v.Nome.Equals(arr))
          v.Distanza = dist;
    }

    #region Wrapper dei miei dati in una matrice per usare l'algoritmo di djikstra del prof
    public long[][] Wrapper(bool autoNodo, ref int nodoDoppio)
    {
      if (autoNodo)
      {
        AutoNodoAdd(nodoDoppio);
        nodoDoppio = listProv.Count - 1;
      }
      long[][] matrix = new long[ListProv.Count][];

      for(int i = 0; i < listNodi.Count; i++)
      {
        matrix[i] = new long[ListProv.Count];
        for (int j = 0; j < ListNodi[i].ListVert.Count; j++)
          matrix[i][j] = ListNodi[i].ListVert[j].Distanza;
      }
      return matrix;
    }

    private void AutoNodoAdd(int nodoDoppio)
    {
      listProv.Add(listProv[nodoDoppio]);
     
      foreach (Nodo x in listNodi)
        x.ListVert.Add(x.ListVert[nodoDoppio]);

      listNodi.Add(ListNodi[nodoDoppio]);
    }

    public void AutoNodoDel()
    {
      listProv.RemoveAt(listProv.Count - 1);
      listNodi.RemoveAt(listNodi.Count - 1);
      foreach(Nodo x in listNodi)
        x.ListVert.RemoveAt(x.ListVert.Count - 1);
    }
    #endregion
  }
}
