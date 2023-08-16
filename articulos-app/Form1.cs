using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace articulos_app
{
    public partial class frmPrincipal : Form
    {
        private List<Articulo> listaArticulos;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cargar();

            cboCampo.Items.Add("Código");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");

        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulos = negocio.listar();

            dgvArticulos.DataSource = negocio.listar();
            ocultarColumnas();
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "C";

        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvArticulos.CurrentRow != null)
                {
                    Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    cargarImagen(seleccionado.ImagenUrl);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cargarImagen(string url)
        {
            try
            {
                if(dgvArticulos.CurrentRow != null)
                    pbImagenArticulo.Load(url);
                else
                    pbImagenArticulo.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
            catch (Exception)
            {
                pbImagenArticulo.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmNuevoArticulo nuevo = new frmNuevoArticulo();
            nuevo.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                DialogResult respuesta = MessageBox.Show("¿Estas seguro de eliminarlo?", "Eliminando...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    Articulo seleccionado = new Articulo();

                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado);
                    cargar();
                    MessageBox.Show("Producto eliminado.");
                }                
            }
            else
                MessageBox.Show("Debes seleccionar un producto.");
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvArticulos.CurrentRow != null)
                {
                    Articulo seleccionado = new Articulo();
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                    frmNuevoArticulo modificado = new frmNuevoArticulo(seleccionado);
                    modificado.ShowDialog();
                    cargar();

                }
                else
                    MessageBox.Show("Debes seleccionar un producto.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> filtrada;
            string filtro = txtBuscar.Text;

            filtrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = filtrada;
            ocultarColumnas();
            cargarImagen("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");

            //Validacion para que no se pueda presionar en el btn Ver Detalle, y al no tener nada seleccionado
            //la app se rompa

            if (dgvArticulos.SelectedRows.Count == 0)
            {
                btnVerDetalle.Enabled = false;
                btnEliminar.Enabled = false;
            }
            else
            {
                btnVerDetalle.Enabled = true;
                btnEliminar.Enabled = true;
                dgvArticulos.ClearSelection();
            }

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccionado = cboCampo.SelectedItem.ToString();

            switch (seleccionado)
            {
                case "Código":
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Empieza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                    break;
                case "Marca":
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Empieza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                    break;
                case "Nombre":
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Empieza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                    break;
                case "Precio":
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Mayor a");
                    cboCriterio.Items.Add("Menor a");
                    cboCriterio.Items.Add("Igual a");
                    break;
                default:
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Empieza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                    break;
            }
        }
        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Debes seleccionar un campo para buscar.");
                return true;
            }

            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Debes seleccionar un criterio para buscar.");
                return true;
            }

            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (!(esNumero(txtFiltroAvanzado.Text)) || txtFiltroAvanzado.Text == "")
                {
                    MessageBox.Show("Debes ingresar números para buscar por precio.");
                    return true;
                }
            }
            return false;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (validarFiltro())
                {
                    return;
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool esNumero(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
        }
    }
}
