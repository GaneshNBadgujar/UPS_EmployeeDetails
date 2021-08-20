using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UPS_EmployeeDetails.Model;
using UPS_EmployeeDetails.Repository;

namespace UPS_EmployeeDetails
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            ClearForm();
            GetAllEmployee();
        }

        /// <summary>
        /// This method will return all the employees
        /// </summary>
        public async void GetAllEmployee()
        {
            string url = "https://gorest.co.in/public-api/users";
            List<Employee> employees = new List<Employee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Authorization", "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");

                using (var response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileJsonString = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<ResponseObj>(fileJsonString);

                        dgvEmployee.DataSource = result.data;
                        dgvEmployee.ClearSelection();
                    }
                }
            }
        }

        /// <summary>
        /// Grid row selection event to display the selected employee details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblEmployeeId.Text = dgvEmployee.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvEmployee.CurrentRow.Cells[1].Value.ToString();
            txtGender.Text = dgvEmployee.CurrentRow.Cells[2].Value.ToString();
            txtEmail.Text = dgvEmployee.CurrentRow.Cells[3].Value.ToString();
            cmbStatus.SelectedItem = dgvEmployee.CurrentRow.Cells[4].Value.ToString();
        }

        /// <summary>
        /// Clear input controls
        /// </summary>
        private void ClearForm()
        {
            lblEmployeeId.Text = "";
            txtName.Text = "";
            txtGender.Text = "";
            txtEmail.Text = "";
            cmbStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// Trigger the save action to insert new employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            long empId = String.IsNullOrEmpty(lblEmployeeId.Text) ? 0 : Convert.ToInt64(lblEmployeeId.Text);
            if (empId == 0)
                AddEmployee();
            else
                UpdateEmployee(empId);

            GetAllEmployee();
        }

        /// <summary>
        /// Web api call to insert new employee
        /// </summary>
        private async void AddEmployee()
        {
            string url = "https://gorest.co.in/public-api/users/";
            EmployeeObj emp = new EmployeeObj();
            emp.Name = txtName.Text;
            emp.Gender = txtGender.Text;
            emp.Email = txtEmail.Text;
            emp.Status = cmbStatus.SelectedItem.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");

                var serializedProduct = JsonConvert.SerializeObject(emp);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync(url, content);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Employee details inserted successfully!", "Success");
                }
            }
        }

        /// <summary>
        /// Trigger the action to update the employee details
        /// </summary>
        /// <param name="empId"></param>
        private async void UpdateEmployee(long empId)
        {
            string url = "https://gorest.co.in/public-api/users/";
            EmployeeObj emp = new EmployeeObj();
            emp.Id = empId;
            emp.Name = txtName.Text;
            emp.Gender = txtGender.Text;
            emp.Email = txtEmail.Text;
            emp.Status = cmbStatus.SelectedItem.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");

                var serializedProduct = JsonConvert.SerializeObject(emp);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");

                HttpResponseMessage result = await client.PutAsync(String.Format("{0}/{1}", url, empId), content);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Employee details updated successfully!", "Success");
                }
            }
        }

        /// <summary>
        /// Trigger the delete action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            long empId = String.IsNullOrEmpty(lblEmployeeId.Text) ? 0 : Convert.ToInt64(lblEmployeeId.Text);
            if (empId > 0)
            {
                DeleteEmployee(empId);
                GetAllEmployee();
            }
            else
                MessageBox.Show("Please select record from the grid");
        }

        /// <summary>
        /// This method will delete the employee as per employee id
        /// </summary>
        /// <param name="emplId"></param>
        private async void DeleteEmployee(long emplId)
        {
            string url = "https://gorest.co.in/public-api/users/";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");

                HttpResponseMessage result = await client.DeleteAsync(String.Format("{0}/{1}", url, emplId));
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Employee deleted successfully!", "Success");
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            dgvEmployee.ClearSelection();
        }
    }
}
