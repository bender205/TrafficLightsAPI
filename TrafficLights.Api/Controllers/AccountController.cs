using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrafficLights.Model.Entities;

namespace TrafficLights.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<UserIdentityEntity> _userManager;
        private readonly SignInManager<UserIdentityEntity> _signInManager;

        public AccountController(UserManager<UserIdentityEntity> userManager, SignInManager<UserIdentityEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
       
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserIdentityEntity model)
        {
            if (ModelState.IsValid)
            {
                UserIdentityEntity user = model;
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}