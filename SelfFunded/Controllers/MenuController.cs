using Microsoft.AspNetCore.Mvc;
using SelfFunded.Models;
using SelfFunded.DAL;
using System;
using System.Collections.Generic;

namespace SelfFunded.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuDal dals;

        public MenuController(MenuDal menuDal)
        {
            dals = menuDal;
        }

        [Route("api/Menu/GetMenu")]
        [HttpGet]
        public IActionResult GetMenu()
        {
            try
            {
                List<Menu> list = dals.getmenu();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred while retrieving menu: " + ex.Message);
            }
        }

        [Route("api/Menu/GetSubMenu")]
        [HttpGet]
        public IActionResult GetSubMenu(int id)
        {
            try
            {
                List<SubMenu> list = dals.getSubmenu(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred while retrieving submenu: " + ex.Message);
            }
        }
    }
}
