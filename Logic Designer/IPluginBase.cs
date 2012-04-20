using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginInterface
{
    public interface IPluginBase
    {
	//První metoda, kterou jádro volá a předává i instanci jádra.
	void Load(IApplicationBase app);
	//Touto metodou předá jádro pluginu pole všech pluginů. Díky tomu může probíhat komunikace mezi pluginy.
	void PluginsLoaded(IPluginBase[] plugins);
	//Demonstrační metoda, kterou jádro volá po kliknutí v hlavním menu aplikace.
	void DelejNeco();

	//Metody sloužící k vzájemné komunikaci mezi pluginy.
	object Metoda1(object param);
	object Metoda2(object param);

	//Vlastnost určující název pluginu.
	string Nazev
	{
		get;
	}

	//Vlastnost určující titulek pluginu.
    string Titulek
    {
        get;
    }
    }
}
