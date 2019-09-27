using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace MechanicsDetails
{

    class Model
    {
        private Dictionary<int, TreeNode> nodes;

        public Model() => Nodes = new Dictionary<int, TreeNode>();

        public Dictionary<int, TreeNode> Nodes { get => nodes; set => nodes = value; }
        public Database dt = new Database();

        public void addNode(int id, TreeNode node)
        {
            Nodes.Add(id, node);
        }
        public Dictionary<int, TreeNode> getDataAtDatabase() {
           
            nodes = new Dictionary<int, TreeNode>();
            List<Nodes> list = dt.executeCommand(@"SELECT[dbo].Parents.id as id, [dbo].Parents.partsid as partsidid, [dbo].PartsSpare.name as name, [dbo].Parents.count as count, [dbo].Parents.parentid FROM[dbo].Parents INNER JOIN[dbo].PartsSpare ON[dbo].Parents.partsid = [dbo].PartsSpare.id");
            int count = 0;
            for (int i = list.Count - 1; i>=0; i--)
            {
                TreeNode node;
                if (list[i].ParentId == null)
                {
                    node = new TreeNode(list[i].Name);
                    nodes.Add(list[i].Id, node);
                    list.Remove(list[i]);
                }
            }
            
            while (list.Count !=0) {
                addChild(list);
            }
                return nodes;
        }

        private void addChild(List<Nodes> list) {

            for (int i = list.Count - 1; i >= 0; i--)
            {
                TreeNode newNode;
                TreeNode parentNode;
                if (list[i].ParentId != null)
                {
                    newNode = new TreeNode(list[i].Name + "(" + list[i].Count + " шт.)");
                    if (nodes.TryGetValue(list[i].ParentId.Value, out parentNode))
                    {
                        parentNode.Nodes.Add(newNode);
                        nodes.Add(list[i].Id, newNode);
                        list.Remove(list[i]);
                    }
                }
               
            }

        }

        public void createChild(String nameNode, int count, TreeNode selectedNode)
        {
            TreeNode newNode=null;
            SqlCommand command = new SqlCommand();
            command.CommandText = @"Select [dbo].PartsSpare.id From [dbo].PartsSpare Where [dbo].PartsSpare.name = @name";
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = nameNode;
            int c = dt.getIdPartsSpare(command);
            int parentid = 0;
            if (c > 0)
            {
                foreach (KeyValuePair<int, TreeNode> node in nodes)
                {
                    if (node.Value == selectedNode)
                    {
                        parentid = node.Key;
                        SqlCommand command1 = new SqlCommand();
                        command1.CommandText = @"INSERT INTO [dbo].Parents(partsid, parentid, count) VALUES (@id, @parentid, @count)";
                        command1.Parameters.Add("@id", SqlDbType.Int).Value = c;
                        command1.Parameters.Add("@parentid", SqlDbType.Int).Value = parentid;
                        command1.Parameters.Add("@count", SqlDbType.Int).Value = count;
                        dt.exCommand(command1);
                        newNode = new TreeNode(nameNode + "(" + count + " шт.)");
                        node.Value.Nodes.Add(newNode);
                    }
                }

            }
            else {
                foreach (KeyValuePair<int, TreeNode> node in nodes)
                {
                    if (node.Value == selectedNode)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = @"INSERT INTO [dbo].PartsSpare(name) VALUES (@name)";
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = nameNode;
                        dt.exCommand(cmd);
                        c = dt.getIdPartsSpare(command);
                        if (c > 0)
                        {
                            parentid = node.Key;
                            SqlCommand command1 = new SqlCommand();
                            command1.CommandText = @"INSERT INTO [dbo].Parents(partsid, parentid, count) VALUES (@id, @parentid, @count)";
                            command1.Parameters.Add("@id", SqlDbType.Int).Value = c;
                            command1.Parameters.Add("@parentid", SqlDbType.Int).Value = parentid;
                            command1.Parameters.Add("@count", SqlDbType.Int).Value = count;
                            dt.exCommand(command1);
                            newNode = new TreeNode(nameNode + "(" + count + " шт.)");
                            node.Value.Nodes.Add(newNode);
                        }
                    }
                }

            }
            SqlCommand commandSelect = new SqlCommand();
            command.CommandText = @"Select [dbo].Parents.id From [dbo].Parents Where ([dbo].Parents.partsid = @partsid) and ([dbo].Parents.parentid = @parentid)";
            command.Parameters.Add("@partsid", SqlDbType.Int).Value = c;
            command.Parameters.Add("@parentid", SqlDbType.Int).Value = parentid;
            c = dt.getIdPartsSpare(command);
            nodes.Add(c, newNode);

            
        }

        public int deleteNode(TreeNode node)
        {
            int status = 0;
            int selectKey = 0;
            foreach (var item in nodes)
            {
                if (item.Value.Text == node.Text)
                {
                    selectKey = item.Key;
                }

            }
            SqlCommand com1 = new SqlCommand();
            com1.CommandText = @"Select [dbo].Parents.partsid From [dbo].Parents WHERE [dbo].Parents.Id = @id";
            com1.Parameters.Add("@id", SqlDbType.Int).Value = selectKey;
            int id = dt.getIdPartsSpare(com1);

            if (id > 0)
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = @"DELETE FROM [dbo].Parents WHERE [dbo].Parents.Id = @id";
                com.Parameters.Add("@id", SqlDbType.Int).Value = selectKey;
                dt.exCommand(com);
                nodes.Remove(selectKey);
                status = 1;
            }

            SqlCommand com2 = new SqlCommand();
            com2.CommandText = @"Select count(*) From [dbo].Parents WHERE [dbo].Parents.partsid = @id";
            com2.Parameters.Add("@id", SqlDbType.Int).Value = id;
            int count = dt.getIdPartsSpare(com2);
            if (count <1)
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = @"DELETE FROM [dbo].PartsSpare WHERE [dbo].PartsSpare.Id = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                dt.exCommand(command);
            }
            return status;
        }

        public String renameItem(string newName, TreeNode node)
        {
            int selectKey = 0;
            String n = newName;
            foreach (var item in nodes)
            {
                if (item.Value.Text == node.Text)
                {
                    selectKey = item.Key;
                }

            }
            if (node.Text.Contains("("))
            {
                n = newName + node.Text.Substring(node.Text.IndexOf("("), node.Text.Length - node.Text.IndexOf("("));
            }
            if (checkName(newName) < 1)
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = @"UPDATE [dbo].PartsSpare SET [dbo].PartsSpare.name = @name FROM [dbo].Parents JOIN [dbo].PartsSpare ON [dbo].Parents.partsid = [dbo].PartsSpare.id WHERE [dbo].Parents.id = @id";
                com.Parameters.Add("@id", SqlDbType.Int).Value = selectKey;
                com.Parameters.Add("@name", SqlDbType.NVarChar).Value = newName;
                dt.exCommand(com);
                return n;
            }
            return "";
        }

        public int createParentsNode(String name)
        {
            //Database dt = new Database();
            int status = 0;
            int id = checkName(name);

            if (id < 1)
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = @"INSERT INTO [dbo].PartsSpare(name) VALUES (@name)";
                com.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                dt.exCommand(com);
                if (checkName(name) > 0)
                {
                    id = checkName(name);
                    com.CommandText = @"INSERT INTO [dbo].Parents(partsid) VALUES (@id)";
                    com.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    dt.exCommand(com);
                }
                status = 1;
            }
            else {
                MessageBox.Show("Агрегат уже существует");
            }
            
            return status;
        
        }

        public int checkName(String name) {
            SqlCommand com = new SqlCommand();
            com.CommandText = @"Select [dbo].PartsSpare.Id From [dbo].PartsSpare WHERE [dbo].PartsSpare.name = @name";
            com.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            return dt.getIdPartsSpare(com);
        }

        public int getIdParents(int partsid) {
            SqlCommand com = new SqlCommand();
            com.CommandText = @"Select [dbo].Parents.Id From [dbo].Parents WHERE [dbo].Parents.partsid = @partsid";
            com.Parameters.Add("@partsid", SqlDbType.Int).Value = partsid;
            return dt.getIdPartsSpare(com);
        }
    }
}
