using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VillaVista.Models;
using VillaVista.Models.ViewModels;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(SignInManager<ApplicationUser> signInManager,
                             UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid registration request.");
        }

        // Ensure role exists
        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            return BadRequest("Invalid role selected.");
        }

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return BadRequest("Email is already in use.");
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Fullname = model.FullName // Ensure this matches the property in ApplicationUser model
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(errors);
        }

        // Assign role
        await _userManager.AddToRoleAsync(user, model.Role);

        return Json(new { success = true, message = "Registration successful!", redirectUrl = Url.Action("Index", "Home") });
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest("Invalid login request.");
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (!result.Succeeded)
        {
            return Unauthorized("Invalid email or password.");
        }

        // ✅ Check user role and redirect accordingly
        var roles = await _userManager.GetRolesAsync(user);
        string redirectUrl = Url.Action("Index", "Home"); // Default redirect

        if (roles.Contains("Admin"))
        {
            redirectUrl = Url.Action("Admin", "Dashboard");
        }
        else if (roles.Contains("Staff"))
        {
            redirectUrl = Url.Action("Staff", "Dashboard");
        }
        else if (roles.Contains("Homeowner"))
        {
            redirectUrl = Url.Action("Homeowner", "Dashboard");
        }

        return Json(new { success = true, redirectUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }
}
