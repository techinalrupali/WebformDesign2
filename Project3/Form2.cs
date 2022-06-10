using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Project3
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        DataSet ds;
        SqlCommandBuilder scb;
        public Form2()
        {
            InitializeComponent();
            string strConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            con = new SqlConnection(strConnection);
        }

        private DataSet GetEmpData()
        {
            da = new SqlDataAdapter("select * from Employee", con);
            //add primary key constraint to the col(id)which is in Dataset
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //build the command for DataAdpater
            scb = new SqlCommandBuilder(da);
            ds = new DataSet();
            //fill() fire the select qry in DB & load data in Dataset
            da.Fill(ds, "employee");//employee is the name given to the table which get loaded in the dataset
            return ds;
            
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmpData();
                DataRow row = ds.Tables["employee"].NewRow();
                row["Name"] = txtEmpName.Text;
                row["Salary"] = txtSalary.Text;
                ds.Tables["employee"].Rows.Add(row);
                //reflect the changes from dataset to DB
                int result = da.Update(ds.Tables["employee"]);
                if(result==1)
                {
                    MessageBox.Show("Record Inserted");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmpData();
                DataRow row = ds.Tables["employee"].Rows.Find(txtEmpId.Text);
                if (row != null)
                {
                    row["Name"] = txtEmpName.Text;
                    row["Salary"] = txtSalary.Text;
                    int result = da.Update(ds.Tables["employee"]);
                    if (result == 1)
                    {
                        MessageBox.Show("Record Updated");
                    }

                }
                else
                {
                    MessageBox.Show("Id does not exists to update");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmpData();
                DataRow row = ds.Tables["employee"].Rows.Find(txtEmpId.Text);
                if (row != null)
                {
                    row.Delete();//delete the row from dataset
                    int result = da.Update(ds.Tables["employee"]);
                    if (result == 1)
                    {
                        MessageBox.Show("Record Deleted");
                    }

                }
                else
                {
                    MessageBox.Show("Id does not exists to Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmpData();
                DataRow row = ds.Tables["employee"].Rows.Find(txtEmpId.Text);
                if (row != null)
                {
                    txtEmpName.Text= row["Name"].ToString();
                    txtSalary.Text = row["Salary"].ToString();

                }
                else
                {
                    MessageBox.Show("Record does not exists ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            ds = GetEmpData();
            dataGridView1.DataSource = ds.Tables["employee"];
        }
    }
}
