using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VetClinicApp
{
    public partial class AddClient : Form
    {
        private List<TextBox> textBoxes = new List<TextBox>(); 

        public AddClient(string FIO, string phone)
        {
            InitializeComponent();

            textBoxes.AddRange(new List<TextBox>() { textBox1, textBox2, textBox3, textBox4 });

            InitFields(FIO, phone);
        }

        private void InitFields(string FIO, string phone)
        {
            string[] fioFields = FIO.Trim().Split(new char[] { ' ' });

            textBox1.Text = fioFields[0];
            textBox2.Text = fioFields[1];
            textBox3.Text = fioFields[2];
            maskedTextBox3.Text = phone;
        }

        private bool ValidateNewCilentFields()
        {
            foreach(TextBox textBox in textBoxes)
                if (String.IsNullOrEmpty(textBox.Text) || String.IsNullOrWhiteSpace(textBox.Text))
                    return false;
            if (String.IsNullOrEmpty(maskedTextBox3.Text))
                return false;
            return true;
        }

        private void ClearInputs()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            maskedTextBox3.Text = string.Empty;

            MessageBox.Show(StaticData.saved);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(ValidateNewCilentFields())
            {
                var fieldsData = new List<string>() { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, maskedTextBox3.Text };

                Client client = new Client(fieldsData);
                client.Serialize();

                ClearInputs();
            }
            else
            {
                MessageBox.Show(StaticData.invalidFields);
                return;
            }
        }
    }
}
