using AceJobAgency.Model;
using AceJobAgency.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace AceJobAgency.Pages
{
    public class LoginModel : PageModel
    {
		private readonly GoogleCaptchaService _GoogleCaptchaService;
		[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<ApplicationUser> signInManager;

		private readonly IHttpContextAccessor contxt;
		protected void btnbtn_submit_Click(object sender, EventArgs e)
		{
			LModel.Email = HttpUtility.HtmlEncode(LModel.Email);
            LModel.Password = HttpUtility.HtmlEncode(LModel.Password);
        }
        public LoginModel(SignInManager<ApplicationUser> signInManager, 
			IHttpContextAccessor httpContextAccessor, GoogleCaptchaService googleCaptchaService)
		{
			this.signInManager = signInManager;
			_GoogleCaptchaService = googleCaptchaService;
			contxt = httpContextAccessor;
		}
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var _GoogleCaptcha = _GoogleCaptchaService.ResVer(LModel.Token);
			if (!_GoogleCaptcha.Result.success && _GoogleCaptcha.Result.score >= 0.5)
			{
				ModelState.AddModelError("", "You are not human");

			}
			if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, false);
				
				if (identityResult.Succeeded)
				{
	
					return RedirectToPage("Index");

				}
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}
	}
}