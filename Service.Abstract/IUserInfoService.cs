using Domain.DTO;
using Domain.Entity;

namespace Service.Abstract
{
    public interface IUserInfoService
    {
        List<UserInfoDTO> GetAll();

        UserInfoDTO GetById(string id);

        List<UserInfoDTO> GetByUsername(string username);

        List<UserInfoDTO> GetByName(string name);

        UserInfoDTO Create(UserInfoDTO dto);

        UserInfoDTO Update(UserInfoDTO dto);

        void Delete(string id);
        UserInfo ValidateUser(LoginDTO dto);

        UserInfoDTO RegisterUser(UserInfoDTO dto);
        PaginationData Get(int offset, int pageSize);
        PaginationData GetUserCardsByUserId(string userId, int offset, int pageSize);
        ResetpasswordDTO ResetPassword(string email);

        void ChangePassword(ChangePasswordDTO dto);
        PaginationData GetUserUploadByUserId(string userId, int offset, int pageSize);
    }
}
