using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DoAnTinHoc_DuLieu.Form1;

namespace DoAnTinHoc_DuLieu
{
    public partial class FormAdd : Form
    {
        public BenhNhan newBenhNhan { get; set; }
        public FormAdd()
        {
            InitializeComponent();
        }

        // ✅ Constructor thứ hai (dùng khi nhấn nút "Sửa")
        public FormAdd(BenhNhan bn) : this()
        {
            txtId.Text = bn.PatientID;
            txtName.Text = bn.Name;
            txtAge.Text = bn.Age.ToString();
            Time_Arrival.Text = bn.Arrival_Date.ToString();
            Time_Departure.Text = bn.Departure_Date.ToString();
            txtService.Text = bn.Service;
            txtSatis.Text = bn.Satisfaction.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                newBenhNhan = new BenhNhan
                {
                    PatientID = txtId.Text,
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    Arrival_Date = DateTime.Parse( Time_Arrival.Text),
                    Departure_Date = DateTime.Parse(Time_Departure.Text),
                    Service = txtService.Text,
                    Satisfaction = int.Parse(txtSatis.Text)
                };

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
