using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Retro
{
    class RegionRanking
    {
        public Dictionary<string, int> RegionDictionary = new Dictionary<string, int>();

        public void ReadRegionDictionary()
        {
            foreach (XElement scoreNode in XElement.Load("regions.xml").Elements("Score")) 
            {
                int currentScore = Convert.ToInt32(scoreNode.Attribute("score").Value);
                foreach(XElement regionListNode in scoreNode.Elements("RegionList"))
                {
                    string[] regionsString = regionListNode.Value.Split(',');
                    foreach(string region in regionsString)
                    {
                        RegionDictionary[region] = currentScore;
                    }
                }
            }
        }

        // Lower Number good?
        public RegionRanking()
        {
            if (RegionDictionary.Count == 0)
            {
                ReadRegionDictionary();
            }
        }
    }
}
