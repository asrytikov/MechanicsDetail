using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MechanicsDetails
{

    class Controller
    {
        Model model = new Model();
        public Dictionary<int, TreeNode> getTree() {
            return model.getDataAtDatabase();

        }
        public int createNewParentsNode(String name) {
            return model.createParentsNode(name);
        }

        public int getIdParents(String name) {
            return model.getIdParents(model.checkName(name));
        }

        public void addNode(int id, TreeNode node) {
            model.addNode(id, node);
        }

        public String renameNode(String newName, TreeNode node) {
           return model.renameItem(newName, node);
        }

        public int deleteNode(TreeNode node) {
            return model.deleteNode(node);
        }

        public void createChild(string nameNode, int count, TreeNode selectedNode)
        {
            model.createChild(nameNode, count, selectedNode);
        }
    }
}
