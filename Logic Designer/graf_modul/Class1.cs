using System;
using System.Windows.Forms;
using System.Collections;

namespace PluginInterface
{
    [Serializable]
    public class SavedNode
    {
        // nazov uzla
        public string Name = "";
        // typ uzla podla daneho standardu, na ktorom 
        // sa dohodneme, napr AND2, AND3, PLACE, atd.
        public string Type = "NA";
        // id, pre pripad potreby
        public int id;
        // pole vstupov, su tam ulozene nazvy vstupov
        public ArrayList ConIN = new ArrayList();
        // pole vystupov - nazvy
        public ArrayList ConOut = new ArrayList();
        // suradnice uzla
        public int X = -1;
        public int Y = -1;
    }

    [Serializable]
    public class SavedCon
    {
        // nazov prepojenia
        public string Name = "";
        //Nazov zaciatocneho uzla
        public string StartNode = "";
        // nazov koncoveho uzla
        public string EndNode = "";
    }

    public interface IPlugin
    {
        // definícia hlavnej aplikacie
        IPluginHost Host { get; set; }

        // nazov pluginu
        string PlugName { get; }
        // opis pluginu
        string Description { get; }
        // autor
        string Author { get; }
        // verzia
        string Version { get; }
        // typ (Logic, Petri alebo FSM)
        string Type { get; }

        // zaklad pluginu
        System.Windows.Forms.UserControl MainInterface { get; }

        // co sa ma vykonat pri nacitani
        void Initialize();
        // co sa ma vykonat pri ukonceni
        void Dispose();
    }

    // zakladne metody, cez ktore sa komunikuje s hlavnou aplikaciu
    public interface IPluginHost
    {
        // vrati konkretny objekt vyhladany podla mena
        Object ReturnObject(string Name);
        // spoji vsetky porty s rovnakym nazvom
        void ConnectAll();
        // vytvory uzol s danymi parametrami
        void CreateNode(int x, int y, int ID, string Name, string Type, string[] portsIN, string portsOUT);
        // spoji uzly n1 a n2
        void ConnectNodes(string n1, string n2);
        // pretazena metoda, s pridanym argumentom pre nastavenie nazvu spojenia
        void ConnectNodes(string n1, string n2, string name);
        // ulozi aktualny obvod do docasneho suboru
        bool UlozUzly();
        // vymaze sa vsetko (ako keby sa vytvoril novy projekt)
        void Cleanup();
        // vrati nazov modelu
        string GetModelName();
        // nastavi nazov modelu
        void SetModelName(string Name);
        // vrati vystupne funkcie
        ArrayList GetFunctions();
        // nastavi sirku vstupu a vystupu pre stavove automaty
        void SetStateInputs(int first, int second);
        //vrati zoznam aktualnych funkcii
        ArrayList GetFunctionsContent();
        //nastavi zoznam zjednodusenu funkciu
        void SetFunctionsContent(ArrayList setter);
        //vrati hodnotu prepinaca changed
        bool GetChanged();
        //nastavi prepinac changed
        void SetChanged(bool setter);
    }
}
