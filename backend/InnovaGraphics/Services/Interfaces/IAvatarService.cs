using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnovaGraphics.Services.Interfaces
{
    public interface IAvatarService: IBaseService<Avatar>
    {
        Task<Response> CreateAsync(CreateShopItemDto shopItemDto);

    }
}
