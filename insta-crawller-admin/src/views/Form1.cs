using Google.Cloud.Firestore;
using insta_crawller_admin.src.dto;
using insta_crawller_admin.src.model;
using insta_crawller_admin.src.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace insta_crawller_admin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            await this.getUsers();
        }

        private void PlotUserOnDataGridView(User user, int i)
        {
            this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[i].Cells[0].Value = user.id;
            this.dataGridView1.Rows[i].Cells[1].Value = user.name;
            this.dataGridView1.Rows[i].Cells[2].Value = user.email;
            this.dataGridView1.Rows[i].Cells[3].Value = user.password;
            this.dataGridView1.Rows[i].Cells[4].Value = user.authorized ? "Sim" : "Não";
        }

        private async Task changeUserAuthorization(string id, bool authorization)
        {
            Load load = new Load();
            load.Show();
            var db = Firestore.getInstance();
            if(db != null)
            {
                var result = await Firestore.updateUserCredential(id, authorization, db);
                if (result)
                {
                    MessageBox.Show("A credencial foi alterada com sucesso");
                    await this.getUsers();
                    load.Close();
                }
                else
                {
                    MessageBox.Show("A credencial não foi alterada, verifique se o usuario que você está tentando alterar está correto");
                    load.Close();
                }
            }
            else
            {
                MessageBox.Show("Erro ao conectar com o banco");
                load.Close();
            }
        }

        private async Task getUsers()
        {
            this.dataGridView1.Rows.Clear();
            Load load = new Load();
            load.Show();
            var db = Firestore.getInstance();
            if (db != null)
            {

                var CurrentUsers = await Firestore.getUsers(db);

                if (CurrentUsers != null)
                {
                    var i = 0;
                    foreach (var user in CurrentUsers)
                    {
                        this.PlotUserOnDataGridView(user, i);
                        i += 1;
                    }
                    load.Close();
                }
                else
                {
                    MessageBox.Show("A lista de usuarios recuperada não era valida");
                    load.Close();
                }

                load.Close();

            }
            else
            {
                MessageBox.Show("Erro ao conectar com o banco");
            }
        }

        private string getSelectedUserOnDataGridView()
        {
            var rowIndex = this.dataGridView1.CurrentCell.RowIndex;
            var selectedRow = this.dataGridView1.Rows[rowIndex];

            if(selectedRow != null)
            {
                return selectedRow.Cells[0].Value.ToString();

            }

            return null;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string selectedUserId = this.getSelectedUserOnDataGridView();
            if(selectedUserId != null)
            {
                await this.changeUserAuthorization(selectedUserId, true);

            }
            else
            {
                MessageBox.Show("Nenhum usuario foi selecionado");
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string selectedUserId = this.getSelectedUserOnDataGridView();
            if (selectedUserId != null)
            {
                await this.changeUserAuthorization(selectedUserId, false);

            }
            else
            {
                MessageBox.Show("Nenhum usuario foi selecionado");
            }
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            await this.getUsers();
        }
    }
}
