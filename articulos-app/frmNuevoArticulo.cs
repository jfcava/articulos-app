using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace articulos_app
{
    public partial class frmNuevoArticulo : Form
    {
        public frmNuevoArticulo()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                Articulo nuevo = new Articulo();
                nuevo.Codigo = txtCodigo.Text;
                nuevo.Nombre = txtNombre.Text;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.Marca = (Marca)cbMarca.SelectedItem;
                nuevo.Categoria = (Categoria)cbCategoria.SelectedItem;
                nuevo.ImagenUrl = txtImagenUrl.Text;
                nuevo.Precio = int.Parse(txtPrecio.Text);

                negocio.agregar(nuevo);
                MessageBox.Show("Agregado exitosamente.");
                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void frmNuevoArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio negocio = new MarcaNegocio();
            CategoriaNegocio categoria = new CategoriaNegocio();

            cbMarca.DataSource = negocio.listar();
            cbCategoria.DataSource = categoria.listar();

            cbMarca.SelectedItem = null;
            cbCategoria.SelectedItem = null;
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

        private void cargarImagen(string url)
        {
            try
            {
                pbNuevoArticulo.Load(url);
            }
            catch (Exception)
            {
                pbNuevoArticulo.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }
    }

}
