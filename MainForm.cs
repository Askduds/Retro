using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retro
{
    public partial class Form1 : Form
    {
        // Owned by the entire system for caching purposes.
        List<FileInfo> masterFilesList;
        Dictionary<string, Game> masterGamesList;
        Dictionary<string, Machine> machineList = new Dictionary<string, Machine>();
        Dictionary<string, int> regionRanking = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
            this.Visible = true;
            masterFilesList = new List<FileInfo>();
            masterGamesList = new Dictionary<string, Game>();
            regionRanking = new RegionRanking().RegionDictionary;
            MachineDb machineDb = new MachineDb();
            machineList = machineDb.GenerateMachineList();
            masterGamesList = ReadRomsFolders();
            PopulateListView();
        }

        public void status(string status)
        {
            label1.Text = status;
        }

        public Dictionary<string, Game> ReadRomsFolders()
        {
            status("Reading Files");
            Dictionary<string, Game> result = new Dictionary<string, Game>();

            if (masterFilesList.Count == 0)
            {
                string path = Application.StartupPath + "\\roms";

                foreach (string directory in Directory.GetDirectories(path))
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    masterFilesList.AddRange(dir.GetFiles("*.*"));
                }
            }

            if (masterGamesList.Count == 0)
            {
                foreach (FileInfo file in masterFilesList)
                {
                    Game game = new Game();
                    game.Path = file.DirectoryName + "\\" + file.Name;

                    string[] fileSections = file.Name.Split('.');

                    StringBuilder displayName = new StringBuilder();

                    for (int i = 0; i < fileSections.Count() - 1; i++)
                    {
                        displayName.Append(fileSections[i]);
                    }

                    game.DisplayName = displayName.ToString().Split('(')[0];
                    game.Extension = fileSections[fileSections.Count() - 1];
                    game.Region = GetRegionString(file);
                    string machineName = GetMachine(game).DisplayName;
                    game.UniqueID = game.DisplayName + machineName;

                    if (game.Extension != "sav" && machineName != "Unknown")
                    {
                        Game existingGame;
                        if (!result.TryGetValue(game.DisplayName, out existingGame) || GetRegionInt(game.Region) < GetRegionInt(existingGame.Region))
                        {
                            result[game.UniqueID] = game;
                        }
                    }
                }
            }

            status("Files Read : " + result.Count());

            return result;
        }

        /// <summary>
        /// Attempts to get the region for the game.  It assumes that region info is in () in the file name
        /// It looks at all such occurances and assumes the one that has the best score is the one to go with.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>The region we've gone with for the game</returns>
        private string GetRegionString(FileInfo file)
        {
            string result = "ERR";
            string[] regionList = file.Name.Split('(');

            if (regionList.Length > 1)
            {
                int regionScore = 10000;
                for (int i = 1; i < regionList.Count(); i++)
                {
                    string region = file.Name.Split('(')[i];
                    int newRegionScore;
                    region = region.Split(')')[0];
                    newRegionScore = GetRegionInt(region);
                    if (newRegionScore < regionScore)
                    {
                        result = region;
                        regionScore = newRegionScore;
                    }
                }
            }

            return result;
        }

        /// <summary>Gets the "score" for the region of this file.  Retro uses this to determine the best version of the game to show.</summary>
        /// <param name="regionString"></param>
        /// <returns>The "score" (lowest will win) of the particular region of the file.</returns>
        private int GetRegionInt(string regionString)
        {
            int result;

            if (regionRanking.TryGetValue(regionString, out result))
            {
                return result;
            }
            else
            {
                return 999;
            }
        }

        public void PopulateListView()
        {
            status("Refreshing games list...");
            string searchString = txtSearch.Text.ToUpper();
            bool searching = !String.IsNullOrEmpty(txtSearch.Text);

            // Skip the whole damn thing if we didn't search last time and aren't now.
            if (searching || listView1.Items.Count != masterGamesList.Count)
            {
                listView1.Items.Clear();
                List<ListViewItem> items = new List<ListViewItem>();
                foreach (Game file in masterGamesList.Values)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = file.DisplayName;

                    // Here's where the narrowing down happens, we add it to the list if we're not searching or if there's a case insensitive match for the search string
                    // This is where you'd refactor out less dumb searching.
                    if (!searching || item.Text.ToUpper().Contains(searchString))
                    {
                        Machine machine = GetMachine(file);
                        string machineDisplayName = machine.DisplayName;

                        item.SubItems.Add(machineDisplayName);
                        item.ForeColor = machine.Colour;
                        item.Tag = file;
                        items.Add(item);
                    }
                }
                listView1.Items.AddRange(items.ToArray());
            }
            status("Done : " + listView1.Items.Count + " shown of " + masterGamesList.Count());
        }

        private Machine GetMachine(Game file)
        {
            Machine machine;
            
            if (!machineList.TryGetValue(file.Extension, out machine))
            {
                machine = machineList["unknown"];
            }

            return machine;
        }

        public void Search()
        {
            using (Cursors.WaitCursor)
            {
                PopulateListView();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Game gameTorun = (Game)listView1.SelectedItems[0].Tag;
            status("Launching : " + gameTorun.DisplayName);
            // An asumption is made here that the emulators are all under /emu but given we're portable
            // and it'd be really ugly if you didn't, you will.
            string emuPath = Application.StartupPath + "\\emu\\" + GetMachine(gameTorun).EmuLocation;
            string romname = gameTorun.Path;
            // The assumption here is that extra parmas can go before the gam's file name.
            System.Diagnostics.Process.Start(emuPath, GetMachine(gameTorun).ExtraParams + " \""+romname);
            status("Ready.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
             Search();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }

        }
    }
}
