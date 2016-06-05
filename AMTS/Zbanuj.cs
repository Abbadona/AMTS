﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMTS
{
    public partial class Zbanuj: AbstractForm
    {
        SqlConnection conn;
        AbstractForm mainForm;
        DataSet dataSet;
        SqlDataAdapter dataAdapter;
        SqlCommandBuilder commandBuilder;

        public Zbanuj(SqlConnection connection, AbstractForm MF)
        {
            mainForm = MF;
            conn = connection;
            InitializeComponent();
            dataAdapter = new SqlDataAdapter("SELECT Nazwisko, Imie FROM UZYTKOWNICY", conn);
            commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "UZYTKOWNICY");
            List<string> osoby = new List<string>();

            foreach(DataRow dataRow in dataSet.Tables["UZYTKOWNICY"].Rows)
            {
                string dane = dataRow["Nazwisko"].ToString() + " " + dataRow["Imie"].ToString();
                spisOsob.Items.Add(dane);
                osoby.Add(dane);
                string[] list = osoby.ToArray<string>();
                var autoComplete = new AutoCompleteStringCollection();
                autoComplete.AddRange(osoby.ToArray());
                spisOsob.AutoCompleteCustomSource = autoComplete;
            }
        }

        private void banowanie_Click(object sender, EventArgs e)
        {
            DialogResult choose = MessageBox.Show("Czy na pewno chcesz zablokować tego użytkownika?", "Blokowanie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(choose == DialogResult.Yes)
            {
                string[] nazwiskoImie = spisOsob.SelectedItem.ToString().Split(' ');
                string mail = "SELECT Mail FROM UZYTKOWNICY WHERE Nazwisko LIKE '" + nazwiskoImie[0] + "' AND Imie LIKE '" + nazwiskoImie[1] + "'";
                User osoba = new User(conn, mail);
                SqlCommand sqlcomm = new SqlCommand("UPDATE UZYTKOWNICY SET Ban = ~Ban WHERE Mail = @mail");
                sqlcomm.Parameters.Add("@mail", SqlDbType.VarChar, 50).Value = mail;
                sqlcomm.Connection = conn;
                sqlcomm.ExecuteNonQuery();
                this.Close();
            }
        }

        private void banowanie_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.changeOpenedWindow();
        }
    }
}
