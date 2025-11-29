using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnTinHoc_DuLieu
{
        
        public class NodeAVL
        {
            public BenhNhan Data { get; set; }
            public NodeAVL Trai { get; set; }
            public NodeAVL Phai { get; set; }
            public int ChieuCao { get; set; }

        public NodeAVL(BenhNhan bn)
            {
                Data = bn;
                Trai = null;
                Phai = null;
                ChieuCao = 1;
            }
        }
}
