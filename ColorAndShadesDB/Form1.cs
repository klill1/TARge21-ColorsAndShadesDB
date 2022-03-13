using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorAndShadesDB
{
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ColorAndShadesDB.Properties.Settings.ColorsConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateColorsTable();
        }

        private void PopulateColorsTable()
        {
            using(connection = new SqlConnection(connectionString))
            using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PrimaryColor", connection))
            {
                DataTable colorTable = new DataTable();
                adapter.Fill(colorTable);

                listPrimaryColor.DisplayMember = "PrimaryColorName";
                listPrimaryColor.ValueMember = "Id";
                listPrimaryColor.DataSource = colorTable;
            }
        }

        private void listPrimaryColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateColorShades();
        }

        private void PopulateColorShades()
        {            
            string query = "SELECT Shades.ColorName FROM PrimaryColor INNER JOIN Shades ON Shades.PrimaryColorId = PrimaryColor.Id WHERE PrimaryColor.Id = @PrimaryColorId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@PrimaryColorId", listPrimaryColor.SelectedValue);
                DataTable colorNameTable = new DataTable();
                adapter.Fill(colorNameTable);

                listShades.DisplayMember = "ColorName";
                listShades.ValueMember = "Id";
                listShades.DataSource = colorNameTable;
            }
        }
    }
}
