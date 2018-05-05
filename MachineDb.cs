using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Retro
{
    public class MachineDb
    {
        Dictionary<string, Machine> machineList = new Dictionary<string, Machine>();

        public MachineDb()
        {
            
        }

        public Dictionary<string, Machine> MachineList()
        {
            Dictionary<string, Machine> result = new Dictionary<string, Machine>();

            if (machineList.Count == 0)
            {
                result = GenerateMachineList();
                machineList = result;
            }
            else
            {
                result = machineList;
            }

            return result;
        }

        public Dictionary<string, Machine> GenerateMachineList()
        {
            Dictionary<string, Machine> machineList = new Dictionary<string, Machine>();

            machineList["unknown"] = CreateMachine("Unknown", Color.Black, "","","");

            foreach (XElement machineNode in XElement.Load("machines.xml").Elements("Machine"))
            {
                string machineCode = machineNode.Attribute("machine").Value;
                string displayName = machineNode.Attribute("displayname").Value;
                Color color = Color.FromName(machineNode.Attribute("color").Value);
                string emuLocation = machineNode.Attribute("emuLocation").Value;
                string extraParams = machineNode.Attribute("extraParams").Value;
                string[] extensionsString = machineNode.Attribute("extensionlist").Value.Split(',');

                Machine machineToAdd = CreateMachine(displayName, color, extensionsString[0],emuLocation,extraParams);
                
                if (extensionsString.Length > 1)
                {
                    for (int i = 1; i < extensionsString.Length; i++)
                    {
                        machineToAdd.AddExtension(extensionsString[i]);
                    }
                }

                foreach(string extension in extensionsString)
                {
                    machineList[extension] = machineToAdd;
                }
            }


            return machineList;
        }

        public Machine CreateMachine(string displayName, Color machineColor, string firstExtension, string emuLocation, string extraParams)
        {
            Machine machine = new Machine();
            machine.DisplayName = displayName;
            machine.Colour = machineColor;
            machine.EmuLocation = emuLocation;
            machine.ExtraParams = extraParams;
            List<string> extensions = new List<string>();
            extensions.Add(firstExtension);
            machine.Extensions = extensions;

            return machine;
        }
    }
}

