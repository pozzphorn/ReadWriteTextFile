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

namespace ReadWriteDataTextFile
{
    public partial class Form1 : Form
    {
        // Define the relative path
        static string folderPath = Path.Combine(Application.StartupPath, "Data");
        string filePath = Path.Combine(folderPath, "products.txt");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckTextFileExist();
            ReadProductsFromFile(filePath);
            LoadStatus();
        }

        private void CheckTextFileExist()
        {
            // Create folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create file if it doesn't exist
            if (!File.Exists(filePath))
            {
                // Create an empty file (and close the stream to avoid locking it)
                File.Create(filePath).Close();
            }
        }

        private void LoadStatus()
        {
            cboStatus.DataSource = new List<KeyValuePair<int, string>> {
                new KeyValuePair<int, string>(1, "In Stock"),
                new KeyValuePair<int, string>(2, "Out Of Stock")
            };
            cboStatus.DisplayMember = "Value";
            cboStatus.ValueMember = "Key";
        }

        private void WriteProductsToFile(List<Product> products, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, append: true))
            {
                foreach (var product in products)
                {
                    string line = String.Format("{0}|{1}|{2}|{3}|{4}", product.Id, product.Name, product.Status, product.QTY, product.Price);
                    writer.WriteLine(line);
                }
            }
        }

        private void ReadProductsFromFile(string filePath)
        {
            List<Product> products = new List<Product>();

            foreach(var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split('|');
                if(parts.Length == 5){
                    products.Add(new Product { 
                        Id = parts[0],
                        Name = parts[1],
                        Status = parts[2],
                        QTY = parts[3],
                        Price = parts[4]
                    });
                }
            }

            dgvProducts.DataSource = products;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<Product> products = new List<Product>() { 
                new Product { Id = txtID.Text, Name = txtName.Text, Status = cboStatus.Text, QTY = txtQTY.Text, Price = txtPrice.Text }
            };

            WriteProductsToFile(products, filePath);
            ReadProductsFromFile(filePath);
        }

    }
}
