using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prot3
{
    public partial class agregarComp : Form
    {
        public agregarComp()
        {
            InitializeComponent();
            cancelar.DialogResult = DialogResult.Cancel;
            aceptar.DialogResult = DialogResult.OK;
            idTipo.SelectedIndex = 0;
            estadoCivil.SelectedIndex = 0;
        }

        private void agregarComp_Load(object sender, EventArgs e)
        {

        }
        private string verificarCero(string valor, string respuesta)
        {

            int busquedaCeroIzq = 0;
            //mientras exista un cero al inicio de la cadena se agrega y se corre el indicedel substring
            while (valor.Substring(busquedaCeroIzq, 1) == "0")
            {
                respuesta = respuesta + "CERO ";
                busquedaCeroIzq++;
            }
            double numero = Convert.ToDouble(valor.Substring(busquedaCeroIzq, (valor.Length - busquedaCeroIzq)));
            return (respuesta + NumerosALetras.toText(numero));
        }
        private string tratarId(string Id)
        {
            string res = "";
            string IdSeccionado = "(";//para dar  el formato *valorEnLetras* ( *valorEnNumeros* ) 

            //seccionar los datos en formato 4, 5, 4 para dpi
            if (idTipo.Text == "DPI")
            {
                string seccion = Id.Substring(0, 4);
                IdSeccionado += seccion + " ";
                res = verificarCero(seccion, res) + ", ";//verificacion de Ceros a la izquierda y convercion de valor en texto
                seccion = Id.Substring(4, 5);
                IdSeccionado += seccion + " ";
                res = verificarCero(seccion, res) + ", ";
                seccion = Id.Substring(9, 4);
                IdSeccionado += seccion;
                res = verificarCero(seccion, res) + " ";
            }
            else
            {
                //mientras la longitud tenga almenos 4 digitos se sigue tratando
                int lenght = Id.Length;
                int index = 0;
                while (lenght > 3)
                {
                    string seccion = Id.Substring(index, 4);
                    IdSeccionado += seccion + " ";
                    res = verificarCero(seccion, res) + ", ";
                    index += 4;
                    lenght = Id.Length - index;
                }
                // datos restantes menores a 4 si los hubiese
                if (lenght > 0)
                {
                    string restante = Id.Substring(index, lenght);
                    IdSeccionado = IdSeccionado + restante;
                    res = verificarCero(restante, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2);
                }
            }
            res = res + " " + (IdSeccionado) + "), ";
            return res;
        }
        string ObtenerEdad(DateTime startDate, DateTime endDate)
        {
            return NumerosALetras.toText((endDate.Year - startDate.Year - 1) +
               (((endDate.Month > startDate.Month) ||
               ((endDate.Month == startDate.Month) && (endDate.Day >= startDate.Day))) ? 1 : 0));
        }
        /// <summary>
        /// //////////////////////////////// variable accesible
        /// </summary>
        public string nombreCompareciente
        {
            get
            {
                if (firma.Checked)
                {
                    return compareciente.Text.ToUpper();
                }
                else
                {
                    return "[noFirma]" + compareciente.Text.ToUpper();
                }

            }

        }
        private void aceptar_Click(object sender, EventArgs e)
        {
           //REVISIONES. que se seleccione fecha de nacimiento
            Console.WriteLine(nacimiento.Value);
            if (nacimiento.Value == null)
            {
                MessageBox.Show("debe seleccionar una fecha de nacimiento del compareciente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (id.Text.Length != 13 && idTipo.SelectedIndex == 0)
            {

                MessageBox.Show("DPI inválido, debe contener exactamente 13 dígitos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (revisionDeTextos())
            {
                dialogForm dialog = new dialogForm("Datos Incompletos", "Existen campos sin rellenar,\r\n desea continuar?", "Aceptar", "Cancelar");
                dialog.ShowDialog();
                this.DialogResult = DialogResult.None;
                if (dialog.DialogResult == DialogResult.No)
                {
                    return;
                }
            }
            //si todo esta correcto o se omiten los campos por eleccion se cierra el dialogo y la pantalla principal llamara al resultado
            this.DialogResult = DialogResult.OK;
            Close();
        }
        public string resultado
        {
            get
            {
                // obtener el texto y agregarle paréntesis
                            //booleando de segundo compareciente
                // convertir id en texto de 4 en 4 y agregar parentesis de 4 en 4
                string idC1 = tratarId(id.Text);
                //tipo de identificación dependiente del combobox
                string identificacion1 = idTipo.Text == "DPI" ? "Documento Personal de Identificación (DPI), con código único de identificación (CUI) "
                                                : "Pasaporte ";
                var edad1 = ObtenerEdad(nacimiento.Value, DateTime.Today);
                //armado de datos compareciente
                string datosCompareciente1 = compareciente.Text.ToUpper() +
                             ", de " + edad1 + " años de edad, " + estadoCivil.Text + ", "
                            + profesion.Text + ", " + nacionalidad.Text + " con domicilio en " + domicilio.Text + " quien se identifica con el "
                            + identificacion1 + idC1 + "extendido por " + extendidaPor.Text;

                string resultado = "y por la otra parte: " + datosCompareciente1;
                Console.WriteLine(resultado);
                return resultado;
            }
        }
        private bool revisionDeTextos()
        {
            if (compareciente.Text.Length < 1  || profesion.Text.Length < 1 || domicilio.Text.Length < 1 || nacionalidad.Text.Length < 1
                || id.Text.Length < 1 || extendidaPor.Text.Length < 1)
            {
                return true;
            }
            return false;

        }

        private void id_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(id.Text, "[^0-9]"))
            {
                MessageBox.Show("Este campo es numérico únicamente.");
                id.Text = limpiarNumeros(id.Text);
            }
        }
        private string limpiarNumeros(string s)
        {
            Regex rxNonDigits = new Regex(@"[^\d]+");
            if (string.IsNullOrEmpty(s)) return s;
            string cleaned = rxNonDigits.Replace(s, "");
            return cleaned;
        }
        // split the string into an array of good substrings
        // using the bad substrings as the delimiter. Then use
        // String.Join() to splice things back together.

    }
}
