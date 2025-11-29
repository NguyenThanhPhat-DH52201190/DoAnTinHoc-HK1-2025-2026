using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnTinHoc_DuLieu
{
    public class NodePure
    {
        public BenhNhan Data;
        public NodePure Trai;
        public NodePure Phai;

        public NodePure(BenhNhan bn)
        {
            Data = bn;
            Trai = null;
            Phai = null;
        }
    }

    public class PureTree
    {
        public NodePure Root;
        public List<BenhNhan> DanhSachLap = new List<BenhNhan>();

        
        private bool IsDuplicate(BenhNhan a, BenhNhan b)
        {
            return a.Age == b.Age;
                   
        }

        public void Insert(BenhNhan data)
        {
            Root = InsertNode(Root, data);
        }

        private NodePure InsertNode(NodePure node, BenhNhan data)
        {
            if (node == null)
                return new NodePure(data);

            
            if (data.Age < node.Data.Age)
            {
                node.Trai = InsertNode(node.Trai, data);
            }
            else if (data.Age > node.Data.Age)
            {
                node.Phai = InsertNode(node.Phai, data);
            }
            else
            {
                
                if (IsDuplicate(data, node.Data))
                {
                    DanhSachLap.Add(data);
                }
                else
                {
                   
                    node.Phai = InsertNode(node.Phai, data);
                }
            }

            return node;
        }

        
        public void InOrder(NodePure node, List<BenhNhan> dsBn)
        {
            if (node == null) return;

            InOrder(node.Trai, dsBn);
            dsBn.Add(node.Data);
            InOrder(node.Phai, dsBn);
        }

        public List<(int Age, int Count)> ThongKeTrungTheoAge()
        {
           
            Dictionary<int, int> demTheoAge = new Dictionary<int, int>();

            
            foreach (var bn in DanhSachLap)
            {
                int age = bn.Age;

                if (demTheoAge.ContainsKey(age))
                    demTheoAge[age]++;          
                else
                    demTheoAge[age] = 1;       
            }

            
            List<(int Age, int Count)> result = new List<(int Age, int Count)>();

            foreach (var kv in demTheoAge)
            {
                result.Add((kv.Key, kv.Value));
            }

            
            for (int i = 0; i < result.Count - 1; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    if (result[i].Age > result[j].Age)
                    {
                       
                        var temp = result[i];
                        result[i] = result[j];
                        result[j] = temp;
                    }
                }
            }

            return result;
        }

    }
}

