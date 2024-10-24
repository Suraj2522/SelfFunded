using SelfFunded.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Security.AccessControl;

namespace SelfFunded.DAL
{
    public class MenuDal
    {
        private readonly string conString;
        CommonDal commondal;
        public MenuDal(IConfiguration configuration)
        {
            conString = configuration.GetConnectionString("adoConnectionstring");
        }

        public List<Menu> getmenu()
        {
            List<Menu> menu = new List<Menu>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetMenu]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    menu.Add(new Menu
                    {
                        menuName = dr["menuName"].ToString(),
                        menuIcon = dr["menuIcon"].ToString(),
                        routerLink = dr["routerLink"].ToString(),
                        menuId = Convert.ToInt32(dr["menuId"]),
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("getmenu", "MenuController", ex.Message, "MenuDal");

                Console.WriteLine($"Error in getmenu method: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return menu;
        }

        public List<SubMenu> getSubmenu(int id)
        {
            List<SubMenu> menu = new List<SubMenu>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("SP_GetSubMenu", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@MenuId", id);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    menu.Add(new SubMenu
                    {
                        subMenuId = Convert.ToInt32(dr["subMenuId"]),
                        subMenuName = dr["SubMenuName"].ToString(),
                        menuId = Convert.ToInt32(dr["MenuId"]),
                        routerLink = dr["RouterLink"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("getSubmenu", "MenuController", ex.Message, "MenuDal");
                Console.WriteLine($"Error in getSubmenu method: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return menu;
        }
    }
}
