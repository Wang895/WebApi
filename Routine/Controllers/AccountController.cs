using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Routine.Models;

namespace Routine.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController:ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentException(nameof(signInManager));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        [AllowAnonymous]
        [Route("/Register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var account = _mapper.Map<IdentityUser>(model);
                var result = await _userManager.CreateAsync(account, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(account, isPersistent: false);
                    Response.Headers.Add("IsLogin",_signInManager.IsSignedIn(User).ToString());
                    return CreatedAtRoute("GetCompanies", new { }, null);
                }

                return BadRequest(result.Errors);
            }
            return BadRequest();


        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginDto.Name, loginDto.Password,
                    isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok(loginDto);
                }
                ModelState.AddModelError("error","用户名/密码错误");
                return BadRequest(ModelState);
            }
            return BadRequest();
        }
    }
}
