using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Sale");
            }
            ViewData["HideHeader"] = true;
            return View();
        }
        public IActionResult Register()
        {
            ViewData["HideHeader"] = true;
            return View();
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
        public IActionResult AccessDenied()
        {
            ViewData["HideHeader"] = true;
            return View();
        }

        /// <summary>
        /// Registra um novo usuário e atribui uma role.
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        /// <param name="password">Senha do usuário.</param>
        /// <param name="role">Role (perfil) a ser atribuída.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));
                }
                await _userManager.AddToRoleAsync(user, model.Role);
                return RedirectToAction("Login");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            ViewData["HideHeader"] = true;
            return View(model);
        }

        /// <summary>
        /// Realiza o login de um usuário.
        /// </summary>
        /// <param name="username">E-mail do usuário.</param>
        /// <param name="password">Senha do usuário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Sale");
            
            ModelState.AddModelError(string.Empty, "Login Invalido.");
            ViewData["HideHeader"] = true;
            return View(model);
        }
    }
}
