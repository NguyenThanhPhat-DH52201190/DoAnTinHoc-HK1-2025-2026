using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnTinHoc_DuLieu
{
    public class XuLyCSV
    {
        DataTable dt = new DataTable();
        string filecsv = "\\patients.csv";
        List<BenhNhan> dsBN = new List<BenhNhan>();
        public List<BenhNhan> DocFileCSV(string path)
        {
            var dsNV = new List<BenhNhan>();
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

                var bn = new BenhNhan
                {
                    PatientID = parts[0],
                    Name = parts[1],
                    Age = int.Parse(parts[2]),
                    Arrival_Date = DateTime.Parse(parts[3]),
                    Departure_Date = DateTime.Parse(parts[4]),
                    Service = parts[5],
                    Satisfaction = int.Parse(parts[6])

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