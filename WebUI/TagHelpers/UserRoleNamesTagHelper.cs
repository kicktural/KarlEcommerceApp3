﻿using Core.Entities.Concreate.EntityFremawork;
using Entities.Concreate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace WebUI.TagHelpers
{
	public class UserRoleNamesTagHelper : TagHelper
	{
		public string UserId { get; set; } = null!;

		private readonly UserManager<User> _userManager;

		public UserRoleNamesTagHelper(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			var user = await _userManager.FindByIdAsync(UserId);

			var userRoles = await _userManager.GetRolesAsync(user!);

			var stringBuilder = new StringBuilder();

			userRoles.ToList().ForEach(x =>
			{
				stringBuilder.Append(@$"<span class=""badge bg-secondary"">{x.ToLower()}</span>");
			});

			output.Content.SetHtmlContent(stringBuilder.ToString());
		}
	}
}
