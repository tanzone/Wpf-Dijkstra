using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WpfDijkstra
{
  public partial class MainWindow : Window
  {
    private ConfigMachine cfgMachine;
    private ConfigData cfgData;

    private const string XML_CONFIG = "Config.xml";
    private const string XML_DATA   = "Citta.xml";

    public MainWindow()
    {
      ConfigWindow.addEventForm += new ConfigWindow.AddSettingNotify(AggiungiDati);
      ConfigWindow.modEventForm += new ConfigWindow.ModSettingNotify(ModificaDati);

      cfgMachine = new ConfigMachine();
      cfgData = new ConfigData();

      InitializeComponent();

      cfgMachine.InizializzaConfig(XML_CONFIG, XML_DATA);
      cfgData.InizializzaData(XML_DATA);

      AsserimentoDati();
    }

    #region inserimento dati nelle label della config e dei dati all'interno della data grid e per la ricerca con Dijkstra

    private void AsserimentoDati()
    {
      InserimentoDatiConfig();
      InserimentoDatiDijkstra();
      InserimentoDatiGrid();
    }

    private void InserimentoDatiConfig()
    {
      MachineName.Content = cfgMachine.Machine;
      SystemVers.Content  = cfgMachine.SystemOS;
      XmlSystem.Content   = cfgMachine.XmlSystem;
      UserLog.Content     = cfgMachine.UserLog;
      UserPrec.Content    = cfgMachine.LastUser;
      XmlDataGrid.Content = cfgMachine.XmlData;
    }

    private void InserimentoDatiDijkstra()
    {
      Partenza.ItemsSource = cfgData.ListProv;
      Partenza.SelectedIndex = 0;
      Arrivo.ItemsSource = cfgData.ListProv;
      Arrivo.SelectedIndex = 0;
    }

    private void InserimentoDatiGrid()
    {
      DataGrid.DataContext = CreaDataTable().DefaultView;
    }

    private DataTable CreaDataTable()
    {
      DataTable dt = new DataTable();
      DataRow dr;

      dt.TableName = "EmiliaRomagna";
      dt.Columns.Add("############");

      foreach (string i in cfgData.ListProv)
        dt.Columns.Add(i);

      for(int i = 0; i < cfgData.ListProv.Count; i++)
      {
        dr = dt.NewRow();
        dr[0] = cfgData.ListProv[i];
        for(int j = 1; j < cfgData.ListNodi.Count+1; j++)
        {
          dr[j] = ((Nodo)cfgData.ListNodi[i]).ListVert[j-1].Distanza;
        }
        dt.Rows.Add(dr);
      }
      return dt;
    }
    #endregion

    #region eventi bottoni presenti nella fornm prinicipale
    private void Config_Click(object sender, RoutedEventArgs e)
    {
      new ConfigWindow(cfgData).ShowDialog();
    }

    private void Dijkstra_Click(object sender, RoutedEventArgs e)
    {
      string partenza = Partenza.SelectedItem as string;
      string arrivo   = Arrivo.SelectedItem as string;
      int nPartenza   = cfgData.ListProv.IndexOf(partenza);
      int nArrivo     = cfgData.ListProv.IndexOf(arrivo);

      long[][] matrix = cfgData.Wrapper(partenza.Equals(arrivo), ref nArrivo);

      Dijkstra dij = new Dijkstra(matrix.Length);
      dij.CalcolaPercorso(nPartenza, nArrivo, matrix.Length, matrix, dij.Cammino);

      StampaPercorso(dij.Cammino, dij.Salti, dij.Peso, partenza, arrivo, nPartenza, nArrivo, matrix);
    }

    private void StampaPercorso(int[] cammino, int salti, int peso, string partenza, string arrivo, int nPart, int nArr, long[][] matrix)
    {
      string strada = "";
      for (int i = 0; i < salti - 1; i++)
        strada += cfgData.ListProv[cammino[i]] + " - " + cfgData.ListProv[cammino[i + 1]] + " : " + matrix[cammino[i]][cammino[i + 1]] + "\n";

      if(partenza.Equals(arrivo))
        cfgData.AutoNodoDel();

      MessageBox.Show("Partenza : " + partenza + "\t\t\nArrivo : " + arrivo + "\n\nNumero Salti : " + (salti-1) + "\n\nPercorso : \n" + strada + "\nPeso Totale : " + peso);
    }
    #endregion

    /**
    *FUNZIONI PER IL FORM SECONDARIO
    */
    private void AggiungiDati(string stringa)
    {
      cfgData.AddProv(stringa);
      Aggiorna();
    }

    private void ModificaDati(string part, string arr, int dist)
    {
      cfgData.CercaNodo(part, arr, dist);
      Aggiorna();
    }

    private void Aggiorna()
    {
      InserimentoDatiGrid();
      Partenza.Items.Refresh();
      Arrivo.Items.Refresh();
    }
  }
}
