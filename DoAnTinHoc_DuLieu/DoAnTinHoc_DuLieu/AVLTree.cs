using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnTinHoc_DuLieu
{
    public class AVLTree
    {
        public NodeAVL Root;
        private int ChieuCao_Cay(NodeAVL n)
        {
            if (n == null)
                return 0;
            else
                return n.ChieuCao;
        }

        private int Canbang_Cay(NodeAVL n)
        {
            if (n == null)
                return 0;
            else
                return ChieuCao_Cay(n.Trai) - ChieuCao_Cay(n.Phai);
            
        }

        private NodeAVL XoayPhai(NodeAVL n)
        {
            if (n == null || n.Trai == null) return n;

            NodeAVL y = n.Trai;
            NodeAVL T2 = y.Phai; // T2 là cây con phải của y

            // Thực hiện xoay
            y.Phai = n;
            n.Trai = T2;

            // Cập nhật lại chiều cao
            n.ChieuCao = Math.Max(ChieuCao_Cay(n.Trai), ChieuCao_Cay(n.Phai)) + 1;
            y.ChieuCao = Math.Max(ChieuCao_Cay(y.Trai), ChieuCao_Cay(y.Phai)) + 1;

            return y; // y trở thành gốc mới
        }

        private NodeAVL Xoay_Trai(NodeAVL n)
        {
            if (n == null || n.Phai == null) return n;

            NodeAVL y = n.Phai;
            NodeAVL T2 = y.Trai; // T2 là cây con trái của y

            // Thực hiện xoay
            y.Trai = n;
            n.Phai = T2;

            // Cập nhật lại chiều cao
            n.ChieuCao = Math.Max(ChieuCao_Cay(n.Trai), ChieuCao_Cay(n.Phai)) + 1;
            y.ChieuCao = Math.Max(ChieuCao_Cay(y.Trai), ChieuCao_Cay(y.Phai)) + 1;

            return y; // y trở thành gốc mới
        }
        public NodeAVL Insert(NodeAVL node, BenhNhan data)
        {
            if (node == null)
                return new NodeAVL(data);

            if (string.Compare(data.PatientID, node.Data.PatientID) < 0)
            {
                node.Trai = Insert(node.Trai, data);
            }
            else if (string.Compare(data.PatientID, node.Data.PatientID) > 0)
            {
                node.Phai = Insert(node.Phai, data);
            }

            else
                return node; // Không chèn trùng Id

            node.ChieuCao = 1 + Math.Max(ChieuCao_Cay(node.Trai), ChieuCao_Cay(node.Phai));
            int canbang = Canbang_Cay(node);

            // 4 trường hợp mất cân bằng
            if (canbang > 1 && string.Compare(data.PatientID, node.Trai.Data.PatientID)<0)
                return XoayPhai(node);

            if (canbang < -1 && string.Compare(data.PatientID, node.Phai.Data.PatientID)>0)
                return Xoay_Trai(node);

            if (canbang > 1 && string.Compare(data.PatientID, node.Trai.Data.PatientID)>0)
            {
                node.Trai = Xoay_Trai(node.Trai);
                return XoayPhai(node);
            }

            if (canbang < -1 && string.Compare(data.PatientID,node.Phai.Data.PatientID)<0)
            {
                node.Phai = XoayPhai(node.Phai);
                return Xoay_Trai(node);
            }

            return node;
        }

        public NodeAVL Insert1(NodeAVL node, BenhNhan data)
        {
            if (node == null)
                return new NodeAVL(data);

            if (data.Age<node.Data.Age || (data.Age==node.Data.Age && string.Compare(data.PatientID, node.Data.PatientID) < 0))
            {
                node.Trai = Insert1(node.Trai, data);
            }
            else
            {
                node.Phai = Insert1(node.Phai, data);
            }

            node.ChieuCao = 1 + Math.Max(ChieuCao_Cay(node.Trai), ChieuCao_Cay(node.Phai));
            int canbang = Canbang_Cay(node);

            // 4 trường hợp mất cân bằng
            if (canbang > 1 && node.Trai != null && (data.Age < node.Trai.Data.Age || data.Age == node.Trai.Data.Age && string.Compare(data.PatientID, node.Trai.Data.PatientID) < 0))
                return XoayPhai(node);
            
            if (canbang < -1 && node.Phai != null && (data.Age > node.Phai.Data.Age || data.Age == node.Phai.Data.Age && string.Compare(data.PatientID, node.Phai.Data.PatientID) > 0))
                return Xoay_Trai(node);

            if (canbang > 1 && node.Trai != null &&  (data.Age > node.Trai.Data.Age || data.Age == node.Trai.Data.Age && string.Compare(data.PatientID, node.Trai.Data.PatientID) > 0))
            {
                node.Trai = Xoay_Trai(node.Trai);
                return XoayPhai(node);
            }

            if (canbang < -1 && node.Phai != null && (data.Age < node.Phai.Data.Age || data.Age == node.Phai.Data.Age && string.Compare(data.PatientID, node.Phai.Data.PatientID) < 0))
            {
                node.Phai = XoayPhai(node.Phai);
                return Xoay_Trai(node);
            }

            return node;
        }



        public void InOrder(NodeAVL node, List<BenhNhan> dsBN)
        {
            if (node != null)
            {
                InOrder(node.Trai, dsBN);
                dsBN.Add(node.Data);
                InOrder(node.Phai, dsBN);
            }
        }

        public void LNR(NodeAVL node,List<BenhNhan> dsBN)
        {
            if (node != null)
            {
                LNR(node.Trai,dsBN);
                dsBN.Add(node.Data);
                LNR(node.Phai,dsBN);
            }
            
        }

        public void RLN(NodeAVL node, List<BenhNhan> dsBN)
        {
            if (node != null)
            {
                RLN(node.Phai, dsBN);
                dsBN.Add(node.Data);
                RLN(node.Trai, dsBN);
            }

        }
        public int Lay_Chieucao_Cay()
        {
            return ChieuCao_Cay(Root);
        }

        public int DemLa(NodeAVL root)
        {
            if (root == null)
                return 0;
            if (root.Trai == null && root.Phai == null)
                return 1;
            return DemLa(root.Trai) + DemLa(root.Phai);
        }

        public int Lay_Soluong_La()
        {
            return DemLa (Root);
        }

        public int DemSoNode(NodeAVL root)
        {
            if (root==null)
                return 0;
            return 1 + DemSoNode(root.Trai) + DemSoNode(root.Phai); 
        }

        public int Lay_SoLuong_Node()
        {
            return DemSoNode (Root);
        }

        public int DemNodeHaiCon(NodeAVL node)
        {
            if (node == null)
                return 0;

            int dem = 0;

            
            if (node.Trai != null && node.Phai != null)
                dem = 1;

            
            return dem + DemNodeHaiCon(node.Trai) + DemNodeHaiCon(node.Phai);
        }

        public int Lay_Node2con()
        {
            return DemNodeHaiCon(Root);
        }

        public int DemNodeMotCon(NodeAVL node)
        {
            if (node == null)
                return 0;

            int dem = 0;

           
            if ((node.Trai == null && node.Phai != null) ||
                (node.Trai != null && node.Phai == null))
            {
                dem = 1;
            }

            return dem + DemNodeMotCon(node.Trai) + DemNodeMotCon(node.Phai);
        }
        public int Lay_Node1Con()
        {
            return DemNodeMotCon(Root);
        }

        public int DemNodeTaiTang(NodeAVL node, int tang)
        {
            if (node == null)
                return 0;

           
            if (tang == 0)
                return 1;

            
            return DemNodeTaiTang(node.Trai, tang - 1) + DemNodeTaiTang(node.Phai, tang - 1);
        }

        public int Lay_SoNode_Tang(int tang)
        {
            return DemNodeTaiTang(Root, tang);
        }

        public int DemNodeBenTrai(NodeAVL node)
        {
            if (node == null)
                return 0;

            return 1 + DemNodeBenTrai(node.Trai) + DemNodeBenTrai(node.Phai);
        }

        public int Lay_SoNodeBenTrai()
        {
            if (Root == null || Root.Trai == null)
                return 0;

            
            return DemNodeBenTrai(Root.Trai);
        }

        public int DemNodeBenPhai(NodeAVL node)
        {
            if (node == null)
                return 0;

            return 1 + DemNodeBenPhai(node.Trai) + DemNodeBenPhai(node.Phai);
        }

        public int Lay_SoNodeBenPhai()
        {
            if (Root == null || Root.Phai == null)
                return 0;

            return DemNodeBenPhai(Root.Phai);
        }





    }
}
