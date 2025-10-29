using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DoAnTinHoc_DuLieu;


namespace DoAnTinHoc_DuLieu
{
    public partial class Form1 : Form
    {
        XuLyCSV xl = new XuLyCSV();
        string filecsv = "patients.csv";
        List<BenhNhan> dsBN = new List<BenhNhan>();

        public Form1()
        {
            InitializeComponent();
        }
        private void btnXuLyCSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.DataSource == null)
                {
                    MessageBox.Show("Không có dữ liệu để ghi!");
                    return;
                }

                dsBN = (List<BenhNhan>)dataGridView1.DataSource;
                xl.GhiFile(filecsv, dsBN);
                MessageBox.Show(" Đã ghi lại file CSV thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Lỗi khi ghi file CSV: " + ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(filecsv))
                {
                    MessageBox.Show("File CSV không tồn tại!");
                    return;
                }

                dsBN = xl.DocFileCSV(filecsv);
                dataGridView1.DataSource = dsBN;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file CSV: " + ex.Message);
            }
        }
    }
}
