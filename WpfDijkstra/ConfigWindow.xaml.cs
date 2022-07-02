using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace WpfDijkstra
{
  public partial class ConfigWindow : Window
  {
    internal static event AddSettingNotify addEventForm;
    internal static event ModSettingNotify modEventForm;
    internal delegate void AddSettingNotify(string stringa);
    internal delegate void ModSettingNotify(string part, string arr, int dist);

    private ConfigData cfgData;

    public ConfigWindow(ConfigData cfgData)
    {
      this.cfgData = cfgData;
      InitializeComponent();
      InizializzaConDati();
    }

    private void InizializzaConDati()
    {
      PartenzaMod.ItemsSource = cfgData.ListProv;
      PartenzaMod.SelectedIndex = 0;
      ArrivoMod.ItemsSource = cfgData.ListProv;
      ArrivoMod.SelectedIndex = 0;
    }

    private void Aggiungi_Click(object sender, RoutedEventArgs e)
    {
      string newProv = TxtProvincia.Text;
      if (!cfgData.CercaProv(newProv.ToUpper()))
        addEventForm(newProv);

      PartenzaMod.Items.Refresh();
      ArrivoMod.Items.Refresh();
    }

    private void Modifica_Click(object sender, RoutedEventArgs e)
    {
      string part = PartenzaMod.Text;
      string arr = ArrivoMod.Text;
      int dist = 0;
      try{ dist = Int32.Parse(TxtDistanza.Text); }
      catch (Exception)
      {
        MessageBox.Show("Valore numerico errato!!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }
      if ( (dist > 0 && dist < 999999) || (dist == 0 && part.Equals(arr)) )
        modEventForm(part, arr, dist);
      else
        MessageBox.Show("Dati inseriti non sono corretti !!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
    }


    private void Chiudi_Click(object sender, EventArgs e)
    {
      //SCRIVI SU FILE XML NUOVO I DATI 
      XDocument doc = new XDocument(
          new XElement("emilia-romagna",
            new XElement("headers", cfgData.ListProv.Select(x => new XElement("citta", x))))
      );

      for (int i = 0; i < cfgData.ListProv.Count; i++)
      {
        doc.Descendants("emilia-romagna").Last().Add(new XElement("nodo", new XElement("nome", cfgData.ListProv[i])));
        for (int j = 0; j < cfgData.ListProv.Count; j++)
          doc.Descendants("nodo").Last().Add(new XElement("vertice", cfgData.ListNodi[i].ListVert[j].Distanza));
      }
      doc.Save("Citta.xml");

      this.Close();
    }
  }
}
