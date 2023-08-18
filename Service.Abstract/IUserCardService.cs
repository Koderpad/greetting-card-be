using Domain.DTO;
using Microsoft.AspNetCore.Http;

namespace Service.Abstract
{
    public interface IUserCardService
    {
        List<UserCardDTO> GetAll();

        UserCardDTO GetById(string id);

        List<UserCardDTO> GetByName(string name);

        List<UserCardDTO> GetByUserInfoId(string id);

        UserCardDTO Create(UserCardDTO dto, IFormFile image, List<FileUploadModel> item);

        UserCardDTO Update(UserCardDTO dto, IFormFile images, List<FileUploadModel> items);

        void Delete(string id);
        void PermanentlyDelete(string id);
        PaginationData Get(int offset, int pageSize);
    }
}
