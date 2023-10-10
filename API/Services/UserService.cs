using Domain.Interfaces;
using System.Linq;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using API.Helpers;

namespace API.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TokenService _tokenService;
    public UserService(IUnitOfWork unitOfWork){
        _unitOfWork = unitOfWork;
        _tokenService = new TokenService("secretKey");
    }

    
}