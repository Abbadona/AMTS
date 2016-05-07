﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace AMTS
{
    public partial class Regulamin:AbstractForm
    {
        AbstractForm form;
        public Regulamin(bool AdminLogged, AbstractForm form)
        {
            InitializeComponent();
            this.form = form;
            if(AdminLogged)
            {
                wczytaj.Visible = true;
            }
            PdfReader file = new PdfReader("C:\\Users\\Klaudia\\Desktop\\AMTS-master\\Regulamin.pdf");
            for(int i = 1; i <= file.NumberOfPages; i++)
                tresc.Text += PdfTextExtractor.GetTextFromPage(file, i, new SimpleTextExtractionStrategy());
        }

        private void Zamknij_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Regulamin_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.changeOpenedWindow();
        }

        private void wczytaj_Click(object sender, EventArgs e)
        {

        }
    }
}
