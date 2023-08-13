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
        private Articulo modificado = null;
        public frmNuevoArticulo()
        {
            InitializeComponent();
        }

        public frmNuevoArticulo(Articulo seleccionado)
        {
            InitializeComponent();
            modificado = seleccionado;

            Text = "Detalle Producto";
            btnAceptar.Text = "Modificar";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            if (modificado == null)
            {
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
            else
            {
                try
                {
                    modificado.Codigo = txtCodigo.Text;
                    modificado.Nombre = txtNombre.Text;
                    modificado.Descripcion = txtDescripcion.Text;
                    modificado.Marca = (Marca)cbMarca.SelectedItem;
                    modificado.Categoria = (Categoria)cbCategoria.SelectedItem;
                    modificado.ImagenUrl = txtImagenUrl.Text;
                    modificado.Precio = decimal.Parse(txtPrecio.Text);

                    negocio.modificar(modificado);
                    MessageBox.Show("Modificado exitosamente!");
                    Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        private void frmNuevoArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio negocio = new MarcaNegocio();
            CategoriaNegocio categoria = new CategoriaNegocio();
            cargarImagen("https://st3.depositphotos.com/17828278/33150/v/450/depositphotos_331503262-stock-illustration-no-image-vector-symbol-missing.jpg");

            try
            {
                cbMarca.DataSource = negocio.listar();
                cbMarca.ValueMember = "Id";
                cbMarca.DisplayMember = "Descripcion";
                cbCategoria.DataSource = categoria.listar();
                cbCategoria.ValueMember = "Id";
                cbCategoria.DisplayMember = "Descripcion";

                if (modificado != null)
                {
                    txtCodigo.Text = modificado.Codigo;
                    txtNombre.Text = modificado.Nombre;
                    txtDescripcion.Text = modificado.Descripcion;
                    cbMarca.SelectedValue = modificado.Marca.Id;
                    cbCategoria.SelectedValue = modificado.Categoria.Id;
                    txtImagenUrl.Text = modificado.ImagenUrl;
                    txtPrecio.Text = modificado.Precio.ToString();

                    cargarImagen(modificado.ImagenUrl);
                }
                else
                {
                    cbMarca.SelectedItem = null;
                    cbCategoria.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }




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

        private void button1_Click(object sender, EventArgs e)
        {
            txtImagenUrl.Text = "";
        }
    }

}
