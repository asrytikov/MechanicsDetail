using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MechanicsDetails
{
    class Database
    {
        SqlConnection connect = new SqlConnection(@"Data Source=VDSWIN2K12;Initial Catalog=sparepartsbase;Integrated Security=True");

        public List<Nodes> executeCommand(String command) {
            List<Nodes> nodes = new List<Nodes>();
            

            try
            {
                connect.Open();
                using (SqlCommand sql = connect.CreateCommand()) {
                    SqlCommand com = new SqlCommand(command, connect);
                    SqlDataReader read = com.ExecuteReader();
                    while (read.Read()) {
                        Nodes node = new Nodes();
                        node.Id = read.GetInt32(0);
                        node.PartsId = read.GetInt32(1);
                        node.Name = read.GetString(2);
                        node.Count = read.IsDBNull(3) ? null : (int?)read.GetInt32(3);
                        node.ParentId = read.IsDBNull(4) ? null : (int?)read.GetInt32(4);
                        nodes.Add(node);
                    }
                }
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally {
                connect.Close();
            }

            return nodes;
        }

        public void exCommand(SqlCommand com)
        {
            
            try
            {
                using (SqlCommand sql = connect.CreateCommand())
                {
                    connect.Open();
                    com.Connection = connect;
                    SqlDataReader read = com.ExecuteReader();
                    read.Close();
                    connect.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                connect.Close();
            }
        }

        public int getIdPartsSpare(SqlCommand com) {
            int id = 0;
            try
            {
                using (SqlCommand sql = connect.CreateCommand())
                {
                    connect.Open();
                    com.Connection = connect;
                    SqlDataReader read = com.ExecuteReader();

                    while (read.Read())
                    {
                        
                       id = (int)read.GetValue(0);


                    }
                    read.Close();
                    connect.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                connect.Close();
            }
            return id;
            
        }





    }
}
