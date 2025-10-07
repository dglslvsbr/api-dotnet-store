using AutoMapper;
using StoreAPI.DTOs;
using StoreAPI.Entities.Authentication;
using StoreAPI.Entities.Models;

namespace StoreAPI.Mappings;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        // Clients
        CreateMap<Client, CreateClientDTO>().ReverseMap();
        CreateMap<Client, LoginDTO>().ReverseMap();
        CreateMap<Client, ShowClientDTO>().ReverseMap();

        // Roles
        CreateMap<Role, CreateRoleDTO>().ReverseMap();

        // Categorys
        CreateMap<Category, CreateCategoryDTO>().ReverseMap();
        CreateMap<Category, ShowCategoryDTO>().ReverseMap();

        // Products
        CreateMap<Product, CreateProductDTO>().ReverseMap();
        CreateMap<Product, ShowProductDTO>().ReverseMap();

        // Orders
        CreateMap<Order, CreateOrderDTO>().ReverseMap();
        CreateMap<Order, ShowOrderDTO>().ReverseMap();
    }
}