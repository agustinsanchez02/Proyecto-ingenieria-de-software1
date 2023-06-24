using Proyecto.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class Inicio : Form
    {
        public Inicio(int a)
        {
            InitializeComponent();
            if (a == 0)
            {
                upload.Hide();
                CerrarSesion.Hide();
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registro registro = new Registro(); 
            registro.Show();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            this.Hide();    
            iniciar_sesion sesion = new iniciar_sesion();
            sesion.Show();  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void CerrarSesion_Click(object sender, EventArgs e)
        {
            this.Hide();
            registrarse.Show();
            Iniciarsesion.Show();
            iniciar_sesion Sesion = new iniciar_sesion();
            Sesion.Show();
        }

        private void upload_Click(object sender, EventArgs e)
        {
            Subir_Archivo subir_Archivo = new Subir_Archivo();
            subir_Archivo.Show();
            this.Hide();
        }
    }
}
