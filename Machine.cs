using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro
{
    public class Machine
    {
        string displayName;
        List<string> extensions;
        Color colour;
        string emuLocation;
        string extraParams;

        public Color Colour { get => colour; set => colour = value; }
        public List<string> Extensions { get => extensions; set => extensions = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string EmuLocation { get => emuLocation; set => emuLocation = value; }
        public string ExtraParams { get => extraParams; set => extraParams = value; }


        public void AddExtension(string newExtension)
        {
            extensions.Add(newExtension);
        }
    }
}
