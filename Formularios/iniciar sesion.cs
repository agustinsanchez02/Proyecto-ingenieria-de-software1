﻿using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Web.ModelBinding;
using COMUN;
using System.Data.SqlClient;
using System.Configuration;
using vista.Login;
using System.Net.Mail;
using FontAwesome.Sharp;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Proyecto
{
    public partial class iniciar_sesion : Form
    {
        public iniciar_sesion()
        {
            InitializeComponent();
            int a = 0;
        }
        [DllImport("User32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("User32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        int bandera = 1;
        EventArgs v;
        Color pctOn = Color.FromArgb(65, 168, 95);
        Color pctOff = Color.DarkGray;
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registro registro = new Registro(); 
            registro.Show();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
           
            string pass = MetodosComunes.Encrypt.GetSHA256(Contraseña.Text.Trim());
            try
            { 
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-J6FH9OJ\\SQLEXPRESS; Initial Catalog = Studysphere; Integrated Security = True");
               
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Usuario , Contraseña FROM Clientes where Usuario ='" + Usuario.Text + "' AND Contraseña='" + pass + "'OR Email ='" + Usuario.Text + "' AND Contraseña='" + pass + "'", conn))
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            int a = 1;
                            Inicio inicio = new Inicio(a);  
                            inicio.Iniciarsesion.Hide();
                            inicio.registrarse.Hide();
                            inicio.Show();
                            this.Hide();
                            conn.Close();
                        }
                        else
                        {
                            MessageBox.Show("Datos incorrectos");
                            pctLineDecoration(pctContraseña, 3);
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Usuario.Text == "" || MetodosComunes.ValidacionEMAIL(v, Usuario.Text) == false)
            {
                MessageBox.Show("Porfavor ingrese un mail para recuperar su cuenta", "Error", MessageBoxButtons.OK ,MessageBoxIcon.Warning);
            }
            else
            {

                Controladora.usuarios1 controladora = new Controladora.usuarios1();

                string mail = Usuario.Text;
                Random r = new Random();
                int codigoVER = r.Next(10000, 99999);
                CodigoConfirmacion formConfirmacion = new CodigoConfirmacion(codigoVER, mail);
                try
                {
                    SmtpClient cliente = controladora.SmtpClient();

                    MailMessage correo = controladora.MailVerificar(mail, codigoVER);

                    cliente.Send(correo);
                }
                catch (Exception ex)
                {
                    DialogResult dialog = MessageBox.Show(ex.Message, "Se produjo un error al enviar el código de verificación, por favor revisar si escribio bien su correo electronico.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                formConfirmacion.ShowDialog();
                if (formConfirmacion.Confirmacion())
                {
                    Form1 form1 = new Form1(mail);
                    form1.Show();
                    this.Hide();
                }
            }
           
        }
        public void pctLineDecoration(PictureBox a, int caso)
        {
            //Esta función la uso para las lineas de color verde y gris, una decoración bastante moderna//
            //Caso 1 significa que el usuario entro al TextBox y Caso 2 Significa que el usuario salió del TextBox//
            switch (caso)
            {
                case 1:
                    a.BackColor = pctOn;
                    break;
                case 2:
                    a.BackColor = pctOff;
                    break;
                case 3:
                    a.BackColor = Color.Red;
                    break;
                default:
                    MessageBox.Show("Error");
                    break;
            }
        }
        private void Usuario_TextChanged(object sender, EventArgs e)
        {
            v = e;
            if (Usuario.Text != "" && Usuario.ForeColor != Color.Silver)
            {
                if (COMUN.MetodosComunes.ValidacionEMAIL(e, Usuario.Text))
                {
                    pctLineDecoration(pctCorreo, 1);
                }
                else
                {
                    pctLineDecoration(pctCorreo, 3);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Ojo_Click(object sender, EventArgs e)
        {
            {
                if (bandera == 0)
                {
                    Ojo.BackgroundImage = Properties.Resources.Ojo_cerrado;
                    Contraseña.UseSystemPasswordChar = true;
                    bandera = 1;
                    Ojo.Height = 30;
                    Ojo.Width = 26;
                }
                else
                {
                    Ojo.BackgroundImage = Properties.Resources.Ojoabierto;
                    bandera = 0;
                    Contraseña.UseSystemPasswordChar = false;
                    Ojo.Width = 25;
                    Ojo.Height = 25;

                }
            }
        }

        private void Contraseña_Enter(object sender, EventArgs e)
        {
            if (Contraseña.Text == "Contraseña")
            {
                Contraseña.Text = "";
                Contraseña.ForeColor = Color.White;
                Contraseña.UseSystemPasswordChar = true;
            }
        }

        private void Usuario_Enter(object sender, EventArgs e)
        {
            if (Usuario.Text == "Usuario/Mail")
            {
                Usuario.Text = "";
                Usuario.ForeColor = Color.White;
            }
        }

        private void Usuario_Leave(object sender, EventArgs e)
        {
            if (Usuario.Text == "")
            {
                Usuario.Text = "Usuario/Mail";
                Usuario.ForeColor = Color.Gray;
            }
        }

        private void Contraseña_Leave(object sender, EventArgs e)
        {
            if (Contraseña.Text == "")
            {
                Contraseña.Text = "Contraseña";
                Contraseña.ForeColor = Color.Gray;
                Contraseña.UseSystemPasswordChar = false;
            }
        }

        private void iniciar_sesion_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            iniciar_sesion_MouseDown((object)sender, e);
        }
    }
}

