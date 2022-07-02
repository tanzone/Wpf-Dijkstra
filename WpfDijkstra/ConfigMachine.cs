using System;
using System.Xml;

namespace WpfDijkstra
{
  public class ConfigMachine
  {
    private string machine;
    private string systemOS;
    private string xmlSystem;
    private string userLog;
    private string lastUser;
    private string xmlData;

    private const string XML_DECLARATION01 = "1.0";
    private const string XML_DECLARATION02 = "UTF-8";
    private const string XML_NODOPRIMARIO  = "config";
    private const string XML_LASTUSER      = "lastUser";

    public ConfigMachine()
    {
      this.machine   = "Unknown";
      this.systemOS  = "Unknown";
      this.xmlSystem = "Unknown";
      this.userLog   = "Unknown";
      this.lastUser  = "Unknown";
      this.xmlData   = "Unknown";
    }

    public string Machine
    {
      get { return machine; }
    }
    public string SystemOS
    {
      get { return systemOS; }
    }
    public string XmlSystem
    {
      get { return xmlSystem; }
    }
    public string UserLog
    {
      get { return userLog; }
    }
    public string LastUser
    {
      get { return lastUser; }
    }
    public string XmlData
    {
      get { return xmlData; }
    }

    public void InizializzaConfig(string xmlConfig, string xmlData)
    {
      this.machine   = Environment.MachineName;
      this.systemOS  = Environment.OSVersion + "";
      this.userLog   = Environment.UserName;
      this.xmlSystem = xmlConfig;
      this.xmlData   = xmlData;

      LeggiConfig(xmlConfig);
      ScriviConfig(xmlConfig);
    }

    private void LeggiConfig(string xmlConfig)
    {
      if (!System.IO.File.Exists(xmlConfig))
        return;

      XmlDocument doc = new XmlDocument();
      doc.Load(xmlConfig);
      XmlNodeList reminders = doc.SelectNodes(XML_NODOPRIMARIO);

      foreach (XmlNode reminder in reminders)
        this.lastUser = reminder.SelectSingleNode(XML_LASTUSER).InnerText;
    }

    private void ScriviConfig(string xmlConfig)
    {
      XmlDocument doc = new XmlDocument();
      XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration(XML_DECLARATION01, XML_DECLARATION02, null);

      XmlElement nodoPrimario = doc.CreateElement("", XML_NODOPRIMARIO, "");
      XmlElement nodoUser = doc.CreateElement("", XML_LASTUSER, "");
      XmlText text1 = doc.CreateTextNode(this.UserLog);

      doc.AppendChild(xmlDeclaration);
      doc.AppendChild(nodoPrimario);

      nodoUser.AppendChild(text1);
      nodoPrimario.AppendChild(nodoUser);

      doc.Save(xmlConfig);
    }
  }
}