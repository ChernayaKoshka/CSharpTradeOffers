using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using CSharpTradeOffers;
using CSharpTradeOffers.Trading;

namespace GetAssetClassInfo
{
    public partial class InventoryInfoForm : Form
    {
        private static SteamEconomyHandler econHandler = new SteamEconomyHandler("no key needed");
        private Dictionary<string, Item> items = new Dictionary<string, Item>();
        Inventory loadedInventory;

        public InventoryInfoForm()
        {
            InitializeComponent();
        }

        private void itemLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            assetClassInfoTB.Text = items[itemLB.Text].SerializeToXml();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            itemLB.DataSource = null;
            loadedInventory = new Inventory(Convert.ToUInt64(steamIdNUD.Value), Convert.ToUInt32(appIdNUD.Value));
            items.Clear();
            List<string> l1 = new List<string>();
            foreach(Item i in loadedInventory.Items.Values)
            { 
                if(!items.ContainsKey(i.Description.MarketHashName))
                    items.Add(i.Description.MarketHashName, i);
            }
            itemLB.DataSource = items.Keys.ToList();
        }
    }
}
