using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UPS_EmployeeDetails.Model;

namespace UPS_EmployeeDetails.Repository
{
    public class EmployeeService
    {
        public async void GetAllEmployee(string url)
        {
            List<Employee> employees = new List<Employee>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileJsonString = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<ResponseObj>(fileJsonString);

                        //dataGridView1.DataSource = JsonConvert.DeserializeObject<ServerFileInformation[]>(fileJsonString).ToList();
                    }
                }
            }
            //return await employees;
        }
    }
}
