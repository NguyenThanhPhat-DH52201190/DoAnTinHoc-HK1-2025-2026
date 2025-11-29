using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CsvHelper.Configuration.Attributes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DoAnTinHoc_DuLieu
{
    public class XuLyCSV
    {
        

        public List<BenhNhan> DocFileCSV(string path)
        {
            var dsBN = new List<BenhNhan>();
            bool boQuaTieuDe = true;
            

            


            foreach (var line in File.ReadLines(path))
            {

                if (boQuaTieuDe)
                {
                    boQuaTieuDe = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line)) continue;

                // Xử lý chuỗi có dấu phẩy trong dữ liệu (dạng "text, text")
                var parts = line.Split(',');
                
                if (!int.TryParse(parts[2], out int Age)) Age = 0; // Gán 0 nếu lỗi

                // 2. Xử lý Satisfaction (parts[6])
                if (!int.TryParse(parts[6], out int Satisfaction)) Satisfaction = 0; // Gán 0 nếu lỗi

                var bn = new BenhNhan
                {
                    PatientID = parts[0],
                    Name = parts[1],
                    Age = Age,
                    Arrival_Date =DateTime.Parse(parts[3]),
                    Departure_Date =DateTime.Parse( parts[4]),
                    Service = parts[5],
                    Satisfaction = Satisfaction
                    
                };
                dsBN.Add(bn);
                
            }

            return dsBN;
        }

        public void GhiFile(string gf, List<BenhNhan> dsBN)
        {
            using (var writer = new StreamWriter(gf))
            {
                // Ghi dòng tiêu đề
                writer.WriteLine("PatientID,Name,Age,Arrival_Date,Departure_Date,Service,Satisfaction");

                // Ghi từng dòng dữ liệu
                foreach (var bn in dsBN)
                {
                    writer.WriteLine($"{bn.PatientID},{bn.Name},{bn.Age},{bn.Arrival_Date},{bn.Departure_Date},{bn.Service},{bn.Satisfaction}");
                }
            }
        }
    }
}