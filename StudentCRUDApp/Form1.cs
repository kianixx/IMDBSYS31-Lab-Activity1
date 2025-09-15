using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentCRUDApp
{
    public partial class Form1: Form
    {
        private string connectionString = "Server=MSI\\SQLEXPRESS;Initial Catalog=StudentDB;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
            load_data();
        }

        private void load_data()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Students", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataTable.DataSource = dt;
            }
        }

        //ADD
        private void btn_add_Click(object sender, EventArgs e)
        {
            string fname = tb_fname.Text;
            string lname = tb_lname.Text;
            string age = tb_age.Text;
            string course = tb_course.Text;
            string studentID = tb_studentID.Text;

            if (fname == "" || lname == "" || age == "" || course == "")
            {
                MessageBox.Show("Incomplete values. Fill all inputs.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //MessageBox.Show("Connected!");
                SqlCommand cmd = new SqlCommand("INSERT INTO Students(FirstName, LastName, Age, Course) VALUES (@FirstName, @LastName, @Age, @Course)", conn);
                cmd.Parameters.AddWithValue("@FirstName", tb_fname.Text);
                cmd.Parameters.AddWithValue("@LastName", tb_lname.Text);
                cmd.Parameters.AddWithValue("@Age", tb_age.Text);
                cmd.Parameters.AddWithValue("@Course", tb_course.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student added!");
                
            }
            load_data();
            clear_tb();
        }

        //EDIT
        private void btn_edit_Click(object sender, EventArgs e)
        {
            string fname = tb_fname.Text;
            string lname = tb_lname.Text;
            string age = tb_age.Text;
            string course = tb_course.Text;
            string studentID = tb_studentID.Text;

            if (fname == "" || lname == "" || age == "" || course == "" || studentID == "")
            {
                MessageBox.Show("Incomplete values. Fill all inputs.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, Age = @Age, Course = @Course WHERE StudentID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", fname);
                cmd.Parameters.AddWithValue("@LastName", lname);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Course", course);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student updated!");
            }
            load_data();
            clear_tb();
        }

        //DELETE
        private void btn_delete_Click(object sender, EventArgs e)
        {
            string studentID = tb_studentID.Text;

            if(studentID == "")
            {
                MessageBox.Show("Enter Student ID! to delete");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Deleted!");
            }
            load_data();
            clear_tb();
        }

        //LOAD
        private void btn_load_Click(object sender, EventArgs e)
        {
            string studentID = tb_studentID.Text;

            if(studentID == "")
            {
                MessageBox.Show("Enter student ID to load");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Students WHERE StudentID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@StudentID", studentID);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataTable.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No student found with that ID.");
                    return;
                }

                //kuhaon ang data from database nya ibutang sa texboxes
                tb_fname.Text = dt.Rows[0]["FirstName"].ToString();
                tb_lname.Text = dt.Rows[0]["LastName"].ToString();
                tb_age.Text = dt.Rows[0]["Age"].ToString();
                tb_course.Text = dt.Rows[0]["Course"].ToString();
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_data();
            clear_tb();
        }

        private void clear_tb()
        {
            tb_fname.Text = "";
            tb_lname.Text = "";
            tb_age.Text = "";
            tb_course.Text = "";
            tb_studentID.Text = "";
        }
    }
}
