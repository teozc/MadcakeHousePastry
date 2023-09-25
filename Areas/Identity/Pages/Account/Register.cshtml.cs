﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MadcakeHousePastry.Areas.Identity.Data;

namespace MadcakeHousePastry.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<MadcakeHousePastryUser> _signInManager;
		private readonly UserManager<MadcakeHousePastryUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;
		private readonly RoleManager<IdentityRole> _roleManager;

		public SelectList RoleSelectList = new SelectList(
			new List<SelectListItem>
			{
				new SelectListItem { Selected =true, Text = "Select Role", Value = ""},
				new SelectListItem { Selected =true, Text = "Admin", Value = "Admin"},
				new SelectListItem { Selected =true, Text = "Customer", Value = "Customer"},
			}, "Value", "Text", 1);

		public RegisterModel(
			UserManager<MadcakeHousePastryUser> userManager,
			SignInManager<MadcakeHousePastryUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
			_roleManager = roleManager;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel  // model use for form design
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			[Required(ErrorMessage = "You must key in the full name before proceed!")]
			[Display(Name = "Your Full Name")]
			[StringLength(50, ErrorMessage = "Name must be in between of 10 - 50 chars", MinimumLength = 10)]
			[RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Should start with Capital Letter")]
			public string CustomerFullName { get; set; }

			[Required(ErrorMessage = "User must key in any age before proceed!")]
			[Display(Name = "Your Age")]
			[Range(21, 100, ErrorMessage = "Only adult is allowed to register this website!")]
			public int CustomerAge { get; set; }

			[Display(Name = "User Role")]
			public string userRole { get; set; }

		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)  // if form is validated, only then proceed
			{
				var user = new MadcakeHousePastryUser   // Register data through here
				{
					// Table column : input column
					UserName = Input.Email,
					Email = Input.Email,
					CustomerFullName = Input.CustomerFullName,
					CustomerAge = Input.CustomerAge,
					EmailConfirmed = true,
					userRole = Input.userRole
				};
				var result = await _userManager.CreateAsync(user, Input.Password);
				if (result.Succeeded)
				{
					//_logger.LogInformation("User created a new account with password.");

					//var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					//code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					//var callbackUrl = Url.Page(
					//    "/Account/ConfirmEmail",
					//    pageHandler: null,
					//    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
					//    protocol: Request.Scheme);

					//await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
					//    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					bool roleresult = await _roleManager.RoleExistsAsync("Admin");
					if (!roleresult)
					{
						await _roleManager.CreateAsync(new IdentityRole("Admin"));
					}
					roleresult = await _roleManager.RoleExistsAsync("Customer");
					if (!roleresult)
					{
						await _roleManager.CreateAsync(new IdentityRole("Customer"));
					}
					await _userManager.AddToRoleAsync(user, Input.userRole); //store userid and roleid in ASPNETUSERROLES table

					if (_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						//return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
						return RedirectToPage("login");
					}
					else
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
