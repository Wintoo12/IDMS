using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace IDMS.Admin.Manage_Installation
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public int PackageID { get; set; }
    }

    public class DataAccess
    {
        private string connectionString = "Data Source=MARK\\SQLEXPRESS03;Initial Catalog=IDMS;Integrated Security=True";

        public List<Item> GetItems(int packageID)
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT componentID, componentName, componentQuantity, componentUnit, componentDescription FROM component where packageID = @packageID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@packageID", packageID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                        Unit = reader.GetString(3),
                        Description = reader.GetString(4),
                        PackageID = packageID
                    };
                    items.Add(item);
                }
            }

            return items;
        }

        public List<Item> GetPackageItems(int packageID)
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT freeItemID, ItemName, quantity, unit, itemDescription FROM freeItem where packageID = @packageID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@packageID", packageID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                        Unit = reader.GetString(3),
                        Description = reader.GetString(4),
                        PackageID = packageID
                    };
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
