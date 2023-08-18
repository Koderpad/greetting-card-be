using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Domain.ExceptionModel;
using Repository.Abstract;
using Service.Abstract;
using Service.Implement.ObjectMapping;

namespace Service.Implement
{
    public class UserUploadService : IUserUploadService
    {
        private readonly IUserUploadRepository userUploadRepository;
        private readonly IUserInfoRepository userInfoRepository;
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        }).CreateMapper();
        public UserUploadService(IUserUploadRepository userUploadRepository,
            IUserInfoRepository userInfoRepository)
        {
            this.userUploadRepository = userUploadRepository;
            this.userInfoRepository = userInfoRepository;
        }
        private void ValidateObject(UserUploadDTO dto)
        {
            if (userInfoRepository.FindById(dto.UserId) == null)
                throw new UserInfoNotFoundException(dto.UserId);
        }
        public UserUploadDTO Create(UserUploadDTO dto)
        {
            ValidateObject(dto);

            dto.Id = Guid.NewGuid().ToString();
            UserUpload userUpload = mapper.Map<UserUpload>(dto);

            userUploadRepository.Create(userUpload);
            userUploadRepository.Save();

            return dto;
        }

        public void Delete(string id)
        {
            UserUpload entity = userUploadRepository.FindById(id)
                                ?? throw new UserUploadNotFoundException(id);

            entity.IsDeleted = true;
            userUploadRepository.Update(entity);
            userUploadRepository.Save();        
        }

        public List<UserUploadDTO> GetAll()
        {
            List<UserUpload> entityList = userUploadRepository.FindAll();
            List<UserUploadDTO> dtoList = new List<UserUploadDTO>();
            foreach (UserUpload entity in entityList)
            {
                UserUploadDTO dto = mapper.Map<UserUploadDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public UserUploadDTO GetById(string id)
        {
            UserUpload entity = userUploadRepository.FindById(id)
            ?? throw new UserUploadNotFoundException(id);

            UserUploadDTO dto = mapper.Map<UserUploadDTO>(entity);
            return dto;
        }

        public UserUploadDTO Update(UserUploadDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
