using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MechanicsDetails
{
    public partial class Form1 : Form
    {
        Controller controller = new Controller();
        Dictionary<int, TreeNode> nodes;
        private String name;
        private int status;

        public string Name { get => name; set => name = value; }
        public int Status { get => status; set => status = value; }
        public Dictionary<int, TreeNode> Nodes { get => nodes; set => nodes = value; }

        public Form1()
        {
            InitializeComponent();
            Nodes = controller.getTree();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            foreach (KeyValuePair<int, TreeNode> each in Nodes) {
                if (each.Value.Parent == null) {
                    treeView1.Nodes.Add(each.Value);
                }
            }
            
        }

        private void КомпонентВерхнегоУровняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form2 form2 = new Form2();
            form2.ShowDialog();
            if (form2.Flag == true)
            {
                String str = form2.NameNode;
                status = controller.createNewParentsNode(str);
                if (status > 0)
                {
                    TreeNode node = new TreeNode(str);
                    controller.addNode(controller.getIdParents(str), node);
                    treeView1.Nodes.Add(node);
                }
            }
        }

        private bool checkNamechildNode(String name, TreeNode parentNode) {
            TreeNode nodeP = parentNode;
            bool flag = false;
            while (nodeP != null) {
                String nodePText="";
                if (nodeP.Text.IndexOf("(") > 0)
                {
                    nodePText = nodeP.Text.Substring(0, nodeP.Text.IndexOf("("));
                }
                else {
                    nodePText = nodeP.Text;
                }
                if (name == nodePText) {
                    flag = true;
                }
                nodeP = nodeP.Parent;
            }
            return flag;
        }

        private void ВToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
            if (form3.Flag == true) {
                String nameNode = form3.Name;
                int count = form3.Count;
                if (checkNamechildNode(nameNode, treeView1.SelectedNode) == false)
                {
                    controller.createChild(nameNode, count, treeView1.SelectedNode);
                }
                else
                {
                    MessageBox.Show("Рекурсивное вложение не возможно");
                }
            }
        }


        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.ShowDialog();
            if (form5.Flag == true) {
                TreeNode nodeDel = treeView1.SelectedNode;
                if (nodeDel != null)
                {
                    TreeNode child = nodeDel;
                    while (getChild(child) != null) {
                        child = getChild(child);
                    }
                    while (child.Parent != nodeDel.Parent)
                    {
                        TreeNode parent = child.Parent;
                        status = controller.deleteNode(child);
                        if (status > 0)
                        {
                            treeView1.Nodes.Remove(child);
                        }
                        child = parent;
                    }
                    status = controller.deleteNode(nodeDel);
                    if (status > 0)
                    {
                        treeView1.Nodes.Remove(nodeDel);
                    }
                }
                else {
                    MessageBox.Show("Элемент для удаления не выбран");
                }
            }
        }

     

        private TreeNode getChild(TreeNode node) {
            TreeNode child = null;
            foreach (var each in nodes)
            {
                if (each.Value.Parent == node) {
                    child = each.Value;
                }
            }
            return child;
            }

        private void ПереименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
            if (form4.Flag == true)
            {
                String str = form4.NameNode;
                TreeNode node = treeView1.SelectedNode;
                if (node != null)
                {
                    String oldName = node.Text;
                    str = controller.renameNode(str, node);

                    if (str.Length > 0)
                    {
                        //treeView1.SelectedNode.Text = str;
                        foreach (var each in nodes)
                        {
                            if ((each.Value.Text.IndexOf("(") > 0) & (oldName.IndexOf("(") > 0))
                            {
                                String str22 = each.Value.Text.Substring(0, each.Value.Text.IndexOf("("));
                                String oldShortName = oldName.Substring(0, oldName.IndexOf("("));
                                if (str22 == oldShortName)
                                {
                                    each.Value.Text = str;
                                }
                            }
                            else
                            {
                                if (each.Value.Text == oldName)
                                {
                                    Console.Write("oldName=" + str + "\n");
                                    each.Value.Text = str;
                                }
                            }
                        }
                    }
                }
                else {
                    MessageBox.Show("Элемент для переименования не выбран");
                }
            }
        }
    }
}
