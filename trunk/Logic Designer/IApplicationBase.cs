using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginInterface
{
    public interface IApplicationBase
    {
        //bool UlozUzly();
        void Cleanup();
        //void CreateNode(int x, int y, int ID, string Name, string Type, string[] portsIN, string portsOUT);
        void CreateNode2(int x, int y);
        void ConnectAll();
        bool UlozUzly(string FileName);
    }
}
