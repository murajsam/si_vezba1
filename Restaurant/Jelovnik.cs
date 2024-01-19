using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer.Models;
using BusinessLayer;

namespace Restaurant
{
    public partial class Jelovnik : Form
    {
        private readonly MenuItemBusiness menuItemBusiness;
        private List<DataLayer.Models.MenuItem> menuItems;
        private DataLayer.Models.MenuItem menuItem;

        public Jelovnik()
        {
            menuItem = new DataLayer.Models.MenuItem();
            menuItems = new List<DataLayer.Models.MenuItem>();
            menuItemBusiness = new MenuItemBusiness();
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            menuItem.title = textBoxTitle.Text;
            menuItem.price = Convert.ToDouble(textBoxPrice.Text);
            menuItem.description = textBoxDescription.Text;
            if (menuItemBusiness.InsertMenuItem(menuItem))
            {
                MessageBox.Show("Uspesan unos!!!");
                UpdateSpisak();
            }
            else
            {
                MessageBox.Show("Neuspesan unos!!!");
            }
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            spisak.Items.Clear();
            var fromRange = Convert.ToDouble(textBoxFrom.Text);
            var toRange = Convert.ToDouble(textBoxTo.Text);
            menuItems = menuItemBusiness.GetAllMenuItems().Where(m => m.price > fromRange && m.price < toRange).ToList();
            foreach (var menuItem in menuItems)
            {
                spisak.Items.Add(menuItem.title + " " + menuItem.description + " " + menuItem.price.ToString());
            }

        }

        private void UpdateSpisak()
        {
            spisak.Items.Clear();
            menuItems = menuItemBusiness.GetAllMenuItems();
            foreach (var menuItem in menuItems)
            {
                spisak.Items.Add(menuItem.menuItemId + "\t" + menuItem.title + "\t" + menuItem.description + "\t" + menuItem.price.ToString());
            }
        }

        private void Jelovnik_Load(object sender, EventArgs e)
        {

            UpdateSpisak();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(spisak.SelectedItem.ToString().Split('\t')[0]);
            menuItemBusiness.RemoveMenuItem(menuItemBusiness.GetMenuItemById(id));
            UpdateSpisak();
        }
    }
}
