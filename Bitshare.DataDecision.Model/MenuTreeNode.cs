using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitshare.DataDecision.Model
{
    public class MenuTreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MenuTreeNode> Nodes = new List<MenuTreeNode>();
    }
}
