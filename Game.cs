using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retro
{
    public class Game
    {
        string displayName;
        string path;
        string extension;
        string region;
        string uniqueID;

        public string DisplayName { get => displayName; set => displayName = value; }
        public string Path { get => path; set => path = value; }
        public string Extension { get => extension; set => extension = value; }
        public string Region { get => region; set => region = value; }
        public string UniqueID { get => uniqueID; set => uniqueID = value; }
    }
}
