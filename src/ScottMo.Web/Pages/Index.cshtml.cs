using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

public class IndexModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public IndexModel(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public IActionResult OnPostLogout()
    {
        if (_signInManager.IsSignedIn(User))
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
        }
        return RedirectToPage();
    }
}
