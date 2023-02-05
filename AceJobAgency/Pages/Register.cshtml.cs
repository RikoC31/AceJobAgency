using AceJobAgency.Model;
using AceJobAgency.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AceJobAgency.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public IFormFile? UploadFile { get; set; }
        private UserManager<ApplicationUser> userManager { get; }

        private SignInManager<ApplicationUser> signInManager { get; }

        private IWebHostEnvironment _environment;

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
        }
        public void OnGet()
        {
        }
        //Save data into the database
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
			{
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                var user = new ApplicationUser()
                {
        
                    UserName = RModel.Email,
                    Email = RModel.Email,
					FirstName = RModel.FirstName,
                    LastName= RModel.LastName,
                    Gender= RModel.Gender,
                    NRIC= protector.Protect(RModel.NRIC),
                    DateOfBirth= RModel.DateOfBirth,
                    WhoAmI = RModel.WhoamI
                };
                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                if(UploadFile != null)
                {
                    if(UploadFile.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }
                    if(UploadFile.ContentType == "application/pdf")
                    {
                        ModelState.AddModelError("Upload", "Only Allow PDF");
                        return Page();
                    }

                    var uploadsFolder = "FileUploads";
                    var resumeFile = Guid.NewGuid() + Path.GetExtension(UploadFile.FileName);

                    var resumePath = Path.Combine(_environment.ContentRootPath,"wwwroot", uploadsFolder, resumeFile);
                    using var fileStream = new FileStream(resumePath, FileMode.Create);

                    await UploadFile.CopyToAsync(fileStream);
                    RModel.Resume = string.Format("/{0}/{1}", uploadsFolder, resumeFile);
                }


            }
            return Page();
        }
    }
}
