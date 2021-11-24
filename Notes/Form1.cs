using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Notes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadAllNotes();
        }

        private void titleTB_Click(object sender, EventArgs e)
        {
            this.titleTB.Text = string.Empty;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if(this.titleTB.Text != string.Empty)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MARGOPC;Initial Catalog=Note;Integrated Security=True;"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand($"INSERT INTO Notes(title, body, creation_date) VALUES ('{this.titleTB.Text}','{this.bodyTB.Text}','{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}')", connection))
                    {
                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show($"{rows} added!");
                        else
                            MessageBox.Show($"Error!");

                    }
                }
            }
            else
                MessageBox.Show($"Add title!");
        }

        private void noteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ListBox).Text != string.Empty)
            {
                ChangeVisible();
                LoadNote((sender as ListBox).Text);
            }
        }
        private void LoadNote(string noteName)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MARGOPC;Initial Catalog=Note;Integrated Security=True;"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT body FROM [Notes] WHERE [Notes].title = '{noteName}';", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    this.titleTB.Text = noteName;
                    this.bodyTB.Text = reader[0].ToString();
                    reader.Close();
                }
            }
        }
        private void ChangeVisible()
        {
            this.noteList.Visible = !this.noteList.Visible;
            this.addBtn.Visible = !this.addBtn.Visible;

            this.titleTB.Visible = !this.titleTB.Visible;
            this.bodyTB.Visible = !this.bodyTB.Visible;
            this.saveBtn.Visible = !this.saveBtn.Visible;
            this.backBtn.Visible = !this.backBtn.Visible;
        }
        private void LoadAllNotes()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MARGOPC;Initial Catalog=Note;Integrated Security=True;"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT title FROM [Notes];", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        this.noteList.Items.Add(reader.GetValue(0));
                    }
                    reader.Close();
                }
            }
            if (this.noteList.Items.Count == 0)
                ChangeVisible();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            ChangeVisible();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            ChangeVisible();
        }
    }
}
