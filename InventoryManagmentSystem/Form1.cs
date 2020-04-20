using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace InventoryManagmentSystem
{
    public partial class Form1 : Form
    {
        public InventoryItems InventoryManager = new InventoryItems();
        public string SaveFile = Directory.GetCurrentDirectory() + "/Inventory.dat";

        public Form1()
        {
            InitializeComponent();
        }

        //When the main form is loaded at the start of the program, deserialize the stored inventory data and load it into the program.
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(SaveFile))
                {
                    
                }
                else
                {
                    BinaryFormatter SaveManager = new BinaryFormatter();
                    FileStream FileManager = new FileStream(SaveFile, FileMode.Open);
                    InventoryManager = (InventoryItems)SaveManager.Deserialize(FileManager);

                    foreach (KeyValuePair<string, InventoryItem> InventoryItem in InventoryManager.LocalInventory)
                    {
                        listBox1.Items.Add(InventoryItem.Key);
                    }

                    FileManager.Close();
                    FileManager.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        //When the listbox selection is changed, update the items information in the inventory management screen.
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                InventoryItemNametxt.Text = InventoryManager.LocalInventory[listBox1.SelectedItem.ToString()].ItemName;
                InventoryItemQtytxt.Text = InventoryManager.LocalInventory[listBox1.SelectedItem.ToString()].ItemCount.ToString();
                InventoryItemValuetxt.Text = InventoryManager.LocalInventory[listBox1.SelectedItem.ToString()].ItemCost.ToString();
                InventoryItemTotalValuetxt.Text = InventoryManager.LocalInventory[listBox1.SelectedItem.ToString()].InventoryItemsValue.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        //Resaves the inventory item or adds a new item to the local inventory.
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    InventoryItem NewInventoryItem = new InventoryItem();
                    NewInventoryItem.ItemName = InventoryItemNametxt.Text;
                    NewInventoryItem.ItemCount = float.Parse(InventoryItemQtytxt.Text);
                    NewInventoryItem.ItemCost = float.Parse(InventoryItemValuetxt.Text);
                    NewInventoryItem.InventoryItemsValue = NewInventoryItem.ItemCost * NewInventoryItem.ItemCount;

                    InventoryItemNametxt.Text = "";
                    InventoryItemQtytxt.Text = "";
                    InventoryItemValuetxt.Text = "";
                    InventoryItemTotalValuetxt.Text = "";

                    InventoryManager.LocalInventory.Add(NewInventoryItem.ItemName, NewInventoryItem);

                    listBox1.Items.Add(NewInventoryItem.ItemName.ToString());
                }
                else
                {
                    InventoryItem NewInventoryItem = new InventoryItem();

                    NewInventoryItem.ItemName = InventoryItemNametxt.Text;
                    NewInventoryItem.ItemCount = float.Parse(InventoryItemQtytxt.Text);
                    NewInventoryItem.ItemCost = float.Parse(InventoryItemValuetxt.Text);
                    NewInventoryItem.InventoryItemsValue = NewInventoryItem.ItemCost * NewInventoryItem.ItemCount;

                    InventoryItemNametxt.Text = "";
                    InventoryItemQtytxt.Text = "";
                    InventoryItemValuetxt.Text = "";
                    InventoryItemTotalValuetxt.Text = "";

                    InventoryManager.LocalInventory[listBox1.SelectedItem.ToString()] = NewInventoryItem;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        //Save the inventory items object to file.
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(SaveFile))
                {
                    BinaryFormatter SaveManager = new BinaryFormatter();
                    FileStream FileManager = new FileStream(SaveFile, FileMode.Open);

                    SaveManager.Serialize(FileManager, InventoryManager);

                    FileManager.Close();
                    FileManager.Dispose();
                }
                else
                {
                    BinaryFormatter SaveManager = new BinaryFormatter();
                    FileStream NewFileManager = new FileStream(SaveFile, FileMode.OpenOrCreate);

                    SaveManager.Serialize(NewFileManager, InventoryManager);

                    NewFileManager.Close();
                    NewFileManager.Dispose();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        //Delete the inventory item that is currently selected.
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryManager.LocalInventory.Remove(listBox1.SelectedItem.ToString());
                listBox1.Items.Remove(listBox1.SelectedItem);
                InventoryItemNametxt.Text = "";
                InventoryItemQtytxt.Text = "";
                InventoryItemValuetxt.Text = "";
                InventoryItemTotalValuetxt.Text = "";
            }
            catch (Exception ex)
            {
                
            }
        }
    }

    [Serializable]
    public class InventoryItems
    {
        public Dictionary<string, InventoryItem> LocalInventory = new Dictionary<string, InventoryItem>();
    }

    //Structure to hold the data of each inventory item.
    [Serializable]
    public struct InventoryItem
    {
        public string ItemName;
        public float ItemCount;
        public float ItemCost;
        public float InventoryItemsValue;
    }
}
