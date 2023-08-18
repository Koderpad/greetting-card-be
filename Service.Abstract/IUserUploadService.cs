using Domain.DTO;

namespace Service.Abstract
{
    public interface IUserUploadService
    {
        List<UserUploadDTO> GetAll();

        UserUploadDTO GetById(string id);

        UserUploadDTO Create(UserUploadDTO dto);

        UserUploadDTO Update(UserUploadDTO dto);

        void Delete(string id);
    }
}
