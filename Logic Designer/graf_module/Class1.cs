using System;
using System.Windows.Forms;
using System.Collections;

namespace PluginInterface
{
    [Serializable]
    public class SavedNode
    {
        public string Name = ""; // nazov uzla
        public string Type = "NA"; // typ uzla podla daneho standardu, na ktorom sa dohodneme, napr AND2, AND3, PLACE, atd.
        public int id; // id, pre pripad potreby
        public ArrayList ConIN = new ArrayList();   // pole vstupov, su tam ulozene nazvy vstupov
        public ArrayList ConOut = new ArrayList();  // pole vystupov - nazvy
        public int X = -1;  // suradnice uzla
        public int Y = -1;
    }

    [Serializable]
    public class SavedCon
    {
        public string Name = "";        // nazov prepojenia
        public string StartNode = "";   // Nazov zaciatocneho uzla
        public string EndNode = "";     // nazov koncoveho uzla
    }

    public interface IPlugin
    {
        IPluginHost Host { get; set; }  // definícia hlavnej aplikacie
        string PlugName { get; }        // nazov pluginu
        string Description { get; }     // opis pluginu
        string Author { get; }          // autor
        string Version { get; }         // verzia
        string Type { get; }            // typ (Logic, Petri alebo FSM)
        System.Windows.Forms.UserControl MainInterface { get; } // zaklad pluginu
        void Initialize();              // co sa ma vykonat pri nacitani
        void Dispose();                 // co sa ma vykonat pri ukonceni
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
