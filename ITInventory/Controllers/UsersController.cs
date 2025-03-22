using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContentManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using GuardTour;

namespace ContentManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        db_Utility util = new db_Utility();

      

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {


                HttpContext.Session.Clear();
              //  string cnpwd = EncryptionHelper.Encrypt(loginModel.password);
                var ds = util.Fill("exec LoginValidate @username='" + loginModel.Username + "',@password='" + loginModel.Password + "' ", util.strElect);

                //  var userid = ds.Tables[0].Rows[0][0];
                string errmsg = ds.Tables[0].Rows[0][1].ToString();
                if (errmsg != "Incorrect Password")
                {
                    if (errmsg != "Invalid Username")
                    {
                        HttpContext.Session.SetString("UserId", ds.Tables[0].Rows[0]["UserId"].ToString());
                        HttpContext.Session.SetString("UserName", ds.Tables[0].Rows[0]["UserName"].ToString());
                        return RedirectToAction("BranchLogin", "Admin");
                    }
                    else
                        ViewBag.msg = errmsg;

                }
                else if (errmsg == "Incorrect Password")
                {
                    ViewBag.msg = errmsg;
                }


                
            }

            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();
            
            // Redirect to login page
            return RedirectToAction("Login", "Users");
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ////var users = await _context.Users
            //   // .FirstOrDefaultAsync(m => m.UserId == id);
            //if (users == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Name,Email,Password")] Users users)
        {
            if (ModelState.IsValid)
            {
              //  _context.Add(users);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var users = await _context.Users.FindAsync(id);
           
            return View();
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       /* public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Name,Email,Password")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }*/

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

         

            return View();
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return true;
        }
    }
}
