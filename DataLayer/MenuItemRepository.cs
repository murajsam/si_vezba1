using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public class MenuItemRepository
    {
        private SqlConnection _conn = new SqlConnection(Constants.connectionString);
        private SqlCommand _command;
        private SqlDataReader _reader;

        public List<MenuItem> GetAllMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            using (_command = new SqlCommand("SELECT * FROM [Menu Items]", _conn))
            {
                try
                {
                    _conn.Open();

                    _reader = _command.ExecuteReader();

                    while (_reader.Read())
                    {
                        MenuItem menuItem = new MenuItem
                        {
                            menuItemId = Convert.ToInt32(GetValueFromReader(_reader, "Id")),
                            title = GetValueFromReader(_reader, "title"),
                            description = GetValueFromReader(_reader, "description"),
                            price = Convert.ToDouble(GetValueFromReader(_reader, "price"))
                        };
                        menuItems.Add(menuItem);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    if (_reader != null)
                        _reader.Close();

                    _conn.Close();
                }
            }

            return menuItems;
        }

        public bool InsertMenuItem(MenuItem menuItem)
        {
            using (_command = new SqlCommand("INSERT INTO [Menu Items] (title, description, price) VALUES (@title, @description, @price)", _conn))
            {
                try
                {
                    _command.Parameters.AddWithValue("@title", menuItem.title);
                    _command.Parameters.AddWithValue("@description", menuItem.description);
                    _command.Parameters.AddWithValue("@price", menuItem.price);

                    _conn.Open();
                    _command.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Unutar tabele Providers - {ex.Message}");
                    return false;
                }
                finally
                {
                    _conn.Close();
                }
            }
        }

        public bool RemoveMenuItem(MenuItem menuItem)
        {
            using (_command = new SqlCommand("DELETE FROM [Menu Items] WHERE Id = @itemId", _conn))
            {
                try
                {
                    _command.Parameters.AddWithValue("@itemId", menuItem.menuItemId);

                    _conn.Open();
                    int rowsAffected = _command.ExecuteNonQuery();

                    // Check if a row was affected, indicating a successful deletion
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Unable to remove menu item - {ex.Message}");
                    return false;
                }
                finally
                {
                    _conn.Close();
                }
            }
        }


        private string GetValueFromReader(SqlDataReader reader, string fieldName)
        {
            return (reader[fieldName].ToString());
        }
    }
}
