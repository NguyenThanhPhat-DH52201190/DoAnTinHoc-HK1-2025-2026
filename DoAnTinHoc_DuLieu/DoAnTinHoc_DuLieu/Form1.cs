using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DoAnTinHoc_DuLieu;
using static DoAnTinHoc_DuLieu.XuLyCSV;
using System.Linq;



namespace DoAnTinHoc_DuLieu
{
    public partial class Form1 : Form
    {
        XuLyCSV xl = new XuLyCSV();
        string filecsv = Application.StartupPath + "\\patients.csv";
        AVLTree tree= new AVLTree();
        PureTree pureTree = new PureTree();
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

       
        private void btnAddform1_Click(object sender, EventArgs e)
        {
            var newbn = new FormAdd();
            if (newbn.ShowDialog() == DialogResult.OK)
            {
                dsBN.Add(newbn.newBenhNhan);
                tree.Root = tree.Insert1(tree.Root, newbn.newBenhNhan);// bệnh nhân mới từ form con
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dsBN;

                // Lưu lại ra file CSV
                xl.GhiFile(filecsv, dsBN);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;
            var bn = (BenhNhan)dataGridView1.CurrentRow.DataBoundItem;
            dsBN.Remove(bn);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dsBN;
            xl.GhiFile(filecsv, dsBN);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) 
                return;

            var bn = (BenhNhan)dataGridView1.CurrentRow.DataBoundItem;
            var a = new FormAdd(bn); // form con có constructor nhận bệnh nhân
            if (a.ShowDialog() == DialogResult.OK)
            {
                // Cập nhật giá trị
                int index = dsBN.IndexOf(bn);
                dsBN[index] = a.newBenhNhan;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dsBN;
                xl.GhiFile(filecsv, dsBN);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnDocFile_Click(object sender, EventArgs e)
        {
            try 
            {
                if(!File.Exists(filecsv))
                {
                    MessageBox.Show("File CSV không tông tại!");
                    return;
                }

                
                dsBN = xl.DocFileCSV(filecsv);

                tree.Root = null;
                foreach (var item in dsBN)
                {
                   
                    tree.Root = tree.Insert1(tree.Root, item);
                }


                dataGridView1.DataSource= null;
                dataGridView1.DataSource = dsBN;
            }
            catch ( Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file: " +ex.Message);
            }
        }

        private void btnHienthiAVL_Click(object sender, EventArgs e)
        {
            try
            {
                List<BenhNhan> dsBN = new List<BenhNhan>();
                tree.LNR(tree.Root, dsBN);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dsBN;
                MessageBox.Show(" Hiển thị dữ liệu duyệt theo thứ tự LNR!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Lỗi khi hiển thị cây AVL: " + ex.Message);
            }
        }

        private void btnDocao_Click(object sender, EventArgs e)
        {
            int h = tree.Lay_Chieucao_Cay();
            MessageBox.Show(" Chiều cao của cây AVL là: " + h);
        }

        private void btn_demla_Click(object sender, EventArgs e)
        {
            int demla = tree.Lay_Soluong_La();
            MessageBox.Show("Số lá của cây là: " + demla);
        }

        private void btn_demnode_Click(object sender, EventArgs e)
        {
            int demnode = tree.Lay_SoLuong_Node();
            MessageBox.Show("Số node của cây là: "+demnode);
        }

        private void btn_Node2con_Click(object sender, EventArgs e)
        {
            List<BenhNhan> list = new List<BenhNhan>();
            Lay_Node2con(tree.Root, list);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }
        public void Lay_Node2con(NodeAVL root, List<BenhNhan> list)
        {
            if (root == null) return;

            // node có đúng 2 con
            if (root.Trai != null && root.Trai != null)
            {
                list.Add(root.Data);
            }

            Lay_Node2con(root.Trai, list);
            Lay_Node2con(root.Phai, list);
        }

        private void XayCayTinhKhietVaHienThi()
        {
           
            pureTree = new PureTree();

            foreach (var bn in dsBN)
            {
                pureTree.Insert(bn);
            }

            

            List<BenhNhan> dsTinhKhiet = new List<BenhNhan>();
            pureTree.InOrder(pureTree.Root, dsTinhKhiet);

            
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dsTinhKhiet;

            
            dataGridViewTrung.DataSource = null;
            dataGridViewTrung.DataSource = pureTree.DanhSachLap
            .OrderBy(x => x.Age)
            .ThenBy(x => x.Service)
            .ToList();


            var thongKe = pureTree.ThongKeTrungTheoAge()
            .Select(x => new { Age = x.Age, SoLuongTrung = x.Count })
            .ToList();
            dataGridViewDemTrung.DataSource = thongKe;
            if (thongKe.Count > 0)
            {
                var maxItem = thongKe.OrderByDescending(x => x.SoLuongTrung).First();

                MessageBox.Show(
                    $"Tuổi trùng nhiều nhất: {maxItem.Age}\nSố lượng: {maxItem.SoLuongTrung}",
                    "Thống kê",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show("Không có node nào bị trùng.", "Thống kê");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dsBN == null || dsBN.Count == 0)
            {
                MessageBox.Show("Chưa có dữ liệu.");
                return;
            }

            XayCayTinhKhietVaHienThi();
            MessageBox.Show("Đã xây cây tinh khiết.");
        }

        private void btn_Node1Con_Click(object sender, EventArgs e)
        {
            List<BenhNhan> list = new List<BenhNhan>();
            Lay_Node1Con(tree.Root, list);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }
        public void Lay_Node1Con(NodeAVL root, List<BenhNhan> list)
        {
            if (root == null) return;

            if ((root.Trai == null && root.Phai != null) ||
                (root.Trai != null && root.Phai == null))
            {
                list.Add(root.Data);
            }

            Lay_Node1Con(root.Trai, list);
            Lay_Node1Con(root.Phai, list);
        }


        private void btn_DemNodeTrai_Click(object sender, EventArgs e)
        {
            int demnodetrai = tree.Lay_SoNodeBenTrai();
            MessageBox.Show("Số node có 2 con là: " + demnodetrai);
        }

        private void btnDem_NodePhai_Click(object sender, EventArgs e)
        {
            int demnodephai = tree.Lay_SoNodeBenPhai();
            MessageBox.Show("Số node có 2 con là: " + demnodephai);
        }

        private void btnDem_NodeTang_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTang.Text.Trim(), out int level) || level < 1)
            {
                MessageBox.Show("Vui lòng nhập số tầng hợp lệ!");
                return;
            }

            List<BenhNhan> ds = tree.GetNodesAtSpecificLevel(level);

            if (ds.Count == 0)
            {
                MessageBox.Show($"Tầng {level} không có node nào!");
                return;
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ds;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            MessageBox.Show($"Đã hiển thị {ds.Count} node ở tầng {level}!");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dsBN == null || dsBN.Count == 0)
            {
                MessageBox.Show("Chưa có dữ liệu! Vui lòng đọc file hoặc thêm dữ liệu trước.");
                return;
            }

            // Nếu cây tinh khiết chưa được xây thì tự xây
            if (pureTree == null || pureTree.Root == null)
            {
                pureTree = new PureTree();

                foreach (var bn in dsBN)
                {
                    pureTree.Insert(bn);
                }
            }

            // Tổng số node của cây tinh khiết
            List<BenhNhan> dsOrder = new List<BenhNhan>();
            pureTree.InOrder(pureTree.Root, dsOrder);
            int tongNode = dsOrder.Count;

            // Tổng số node bị trùng
            int tongTrung = pureTree.DanhSachLap.Count;

            // Hiển thị kết quả
            MessageBox.Show(
                $"Tổng số node của cây tinh khiết: {tongNode}\n" +
                $"Tổng số node bị trùng: {tongTrung}",
                "Thống kê cây tinh khiết",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
