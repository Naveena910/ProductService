using AutoMapper;
using Entities.Dtos.RequestDto;
using Entities.Dtos.ResponseDto;
using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class Mappers : Profile
    {
        public Mappers()
        {



            CreateMap<Product, ProductDto>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)));
            CreateMap<ProductForCreatingDto, Product>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image)));
            CreateMap<Product,ProductForCreatingDto>();
            CreateMap<Cart, CartForCreatingDto>();
            CreateMap<CartForCreatingDto, Cart>();
            CreateMap<Cart,CartDto>();
            CreateMap<WishListCreatingDto, WishList>();
         
            CreateMap<WishList, WishListDto>();
            



        }


    }
}
