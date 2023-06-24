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

namespace Proyecto
{
    public partial class iniciar_sesion : Form
    {
        public iniciar_sesion()
        {
            InitializeComponent();
        }
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
                            Inicio inicio = new Inicio();  
                            inicio.Show();
                            this.Hide();
                            conn.Close();
                        }
                        else
                        {
                            MessageBox.Show("Datos incorrectos");
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
                    int i = a.Size.Width;
                    a.Size = new Size(i, 24);
                    a.BackColor = pctOn;
                    break;

                case 2:
                    int j = a.Size.Width;
                    a.Size = new Size(j, 23);
                    a.BackColor = pctOff;
                    break;
                case 3:
                    int k = a.Size.Width;
                    a.Size = new Size(k, 24);
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


    }
}

