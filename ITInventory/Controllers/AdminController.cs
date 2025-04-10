using GuardTour;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.ContentModel;

namespace ITInventory.Controllers
{
    public class AdminController : Controller
    {
        db_Utility util = new db_Utility();
        public IActionResult Profile()
        {
     
            return View();
        }

        public JsonResult ShowProfile()
        {
            var ds = util.Fill("exec UserCreation 'Select'", util.strElect);


            return Json(JsonConvert.SerializeObject(ds.Tables[0]));
        }

        public JsonResult SaveProfile(string UserId, string Usertype, string UserName,string Name,string Email,string Password)
        {
            var ds = util.Fill(@$"exec UserCreation 'Insert',@UserId='{UserId}',@Usertype='{Usertype}',@UserName='{UserName}',@Name='{Name}',@Email='{Email}',@Password='{Password}' ", util.strElect);

            return Json(JsonConvert.SerializeObject(ds.Tables[0]));
        }

        public JsonResult DeleteProfile(string UserId)
        {
            var ds = util.Fill(@$"exec UserCreation 'Delete',@UserId='{UserId}' ", util.strElect);

            return Json(JsonConvert.SerializeObject(ds.Tables[0]));
        }
    }
}
