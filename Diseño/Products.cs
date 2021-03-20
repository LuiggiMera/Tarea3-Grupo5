using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Diseño
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        Product product = new Product();
        ProductServices productServices = new ProductServices();
        private bool Modify { get; set; } = false;

        private void fillCategory() //dudo que funcione
        {
            if (CategoryRepository.Instancia.Categories.Count > 0)
            {
                for (int i = 0; i < CategoryRepository.Instancia.Categories.Count; i++)
                {
                    cbCategory.Items.Add(CategoryRepository.Instancia.Categories[i].name);
                }
            }
            else
            {
                MessageBox.Show("No hay categorias existentes, debe crear una categoria, luego el producto");
                this.Close();
            }
        }
        private void Clear()
        {
            txtID.Clear();
            txtName.Clear();
            txtCode.Clear();
            txtStock.Clear();
            txtDescription.Clear();
            cbCategory.Text = "";
            dtpExpireDate.Value = System.DateTime.Today;
            cbStatus.Text = "";
            Modify = false;
        }
        private void GetProduct()
        {
            product = productServices.select(Convert.ToInt32(txtID.Text));
            if (product is null)
            {
                MessageBox.Show("No existe ningun registro con este ID");
                Modify = false;
            }
            else
            {
                txtID.Text = product.id.ToString();
                txtName.Text = product.name;
                txtCode.Text = product.codigo;
                txtStock.Text = product.stock;
                txtDescription.Text = product.description;
                dtpExpireDate.Value = product.exDate;
                cbCategory.Text = CategoryRepository.Instancia.Categories[product.category - 1].name;
                if (product.status)
                {
                    cbStatus.Text = "Activo";
                }
                else
                {
                    cbStatus.Text = "Inactivo";
                }
                Modify = true;
            }
        }

        private void SaveProduct()
        {
            product = new Product();
            product.name = txtName.Text;
            product.codigo = txtCode.Text;
            product.stock = txtStock.Text;
            product.exDate = dtpExpireDate.Value;
            product.description = txtDescription.Text;
            product.category = cbCategory.SelectedIndex + 1;
            if(cbStatus.SelectedIndex == 0)
            {
                product.status = true;
            }
            else
            {
                product.status = false;
            }
            if (!Modify)
            {
                product.id = ProductRepository.Instancia.Products.Count + 1;
                if (productServices.Add(product))
                {
                    MessageBox.Show("Producto agregado correctamente");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El producto no puedo ser agregado");
                }
            }
            else
            {
                if (productServices.Edit(Convert.ToInt32(txtID.Text), product))
                {
                    MessageBox.Show("Producto modificado correctamente");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El producto no pudo ser modificado");
                }
            }

        }

        private void DeleteProduct()
        {
            if (productServices.Delete(Convert.ToInt32(txtID.Text)))
            {
                MessageBox.Show("Producto eliminado correctamente");
            }
            else
            {
                MessageBox.Show("El producto no pudo ser eliminado");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(txtID.Text != "" && txtID.Text != null)
            {
                GetProduct();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtName.Text != "" && txtCode.Text != "" && txtStock.Text != "" && dtpExpireDate.Value != null && txtDescription.Text != "" && cbCategory.Text != "" && cbStatus.Text != "")
            {
                SaveProduct();
                Clear();
            }
            else
            {
                MessageBox.Show("Debe llenar todos los campos");
                txtName.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(txtID.Text != "")
            {
                DialogResult dialogResult = new DialogResult();
                dialogResult = MessageBox.Show("Seguro que desea eliminar este producto?","Atencion",MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    DeleteProduct();
                    Clear();
                }
            }
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            fillCategory();
        }
    }
}
