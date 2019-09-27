using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicsDetails
{
    class Nodes
    {
        private int id;
        private int partsId;
        private String name;
        private int? count;
        private int? parentId;

        public int Id { get => id; set => id = value; }
        public int PartsId { get => partsId; set => partsId = value; }
        public string Name { get => name; set => name = value; }
        public int? ParentId { get => parentId; set => parentId = value; }
        public int? Count { get => count; set => count = value; }
    }
}
