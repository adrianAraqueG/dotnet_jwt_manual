using System;
using Microsoft.AspNetCore.Mvc;
using API.Dtos;
using Domain.Interfaces;
using API.Services;
using AutoMapper;

namespace API.Controllers;

public class UserController : ApiBaseController
{
    public readonly IUnitOfWork _unitOfWork;
    public readonly IMapper _mapper;
    public readonly IUserService _userService;
    public UserController(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService){
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
    }

    // Registrar Cliente (Guardando la password con hash)
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync (RegUserDto model){
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    // Login Cliente
    [HttpPost("auth")]
    public async Task<ActionResult> LoginAsync(LogUserDto model){
        var result = await _userService.LoginAsync(model);

        return Ok(result);
    }

    // Validar la vida del token
    [HttpPost("validate-token")]
    public async Task<ActionResult> ValidateTokenAsync(DataUserDto model)
    {
        var result = await _userService.ValidateTokenAsync(model);
        return Ok(result);
    }
    
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await _userService.Logout(Request.Headers.Authorization.ToString().Split("Bearer ")[1]);
        return Ok(new { Message = "Logout exitoso" });
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<DataUserDto>> RefreshToken()
    {
        var result = await _userService.RefreshUserToken(Request.Headers.Authorization.ToString().Split("Bearer ")[1]);
        if (result == null)
        {
            return BadRequest(new { Message = "Refresh token no válido o expirado" });
        }
        return Ok(result);
    }





    [HttpGet("list/basic")]
    public async Task<ActionResult<IEnumerable<BasicUserDto>>> GetUsersBasic(){
        var users = await _unitOfWork.Users.GetAllAsync();
        
        return _mapper.Map<List<BasicUserDto>>(users);
    }

    [HttpGet("list/all")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAll(){
        var users = await _unitOfWork.Users.GetAllUsers();
        
        return _mapper.Map<List<UserDto>>(users);
    }
}