using AutoMapper;
using BCrypt.Net;
using Domain.DTO;
using Domain.Entity;
using Domain.ExceptionModel;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using Service.Abstract;
using Service.Implement.ObjectMapping;
using System.Text;
using System.Text.RegularExpressions;

namespace Service.Implement
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IUserInfoRepository userInfoRepository;
        private readonly IUserCardRepository userCardRepository;
        private readonly IUserUploadRepository userUploadRepository;
        private readonly ITemplateCardRepository templateCardRepository;



        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        }).CreateMapper();

        public UserInfoService(IUserInfoRepository userInfoRepository, IUserCardRepository userCardRepository, ITemplateCardRepository templateCardRepository, IUserUploadRepository userUploadRepository)
        {
            this.userInfoRepository = userInfoRepository;
            this.userCardRepository = userCardRepository; this.templateCardRepository = templateCardRepository;
            this.userUploadRepository = userUploadRepository;
        }

        public UserInfoDTO Create(UserInfoDTO dto)
        {
            if (dto.Email == null)
            {
                throw new BadRequestException("Email field cannot be empty");
            }

            if (dto.Password == null)
            {
                throw new BadRequestException("Password field cannot be empty");
            }

            if (dto.FullName == null)
            {
                throw new BadRequestException("FullName field cannot be empty");
            }

            if (dto.IsAdmin == null)
            {
                throw new BadRequestException("Please choose a role");
            }

            dto.Id = Guid.NewGuid().ToString();

            UserInfo entity = mapper.Map<UserInfo>(dto);
            userInfoRepository.Create(entity);
            userInfoRepository.Save();

            return dto;
        }

        public void Delete(string id)
        {
            UserInfo user = userInfoRepository.FindById(id);
            if (user == null)
            {
                throw new UserInfoNotFoundException(id);
            }
            user.IsDeleted = true;
            userInfoRepository.Update(user);
            userInfoRepository.Save();
        }


        public List<UserInfoDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserInfoDTO GetById(string id)
        {
            UserInfo entity = userInfoRepository.FindById(id)
                ?? throw new UserInfoNotFoundException(id);

            UserInfoDTO dto = new UserInfoDTO
            {
                Id = entity.Id,
                Email = entity.Email,
                FullName = entity.FullName,
                IsAdmin = entity.IsAdmin
            };

            return dto;
        }

        public List<UserInfoDTO> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<UserInfoDTO> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public UserInfoDTO Update(UserInfoDTO dto)
        {
            if (dto.Id == null)
                throw new ArgumentNullException("id");

            if (dto.Email == null)
                throw new ArgumentNullException("email");

            if (dto.FullName == null)
                throw new ArgumentNullException("fullName");

            UserInfo entity = userInfoRepository.FindById(dto.Id)
                ?? throw new NotFoundException($"User with id = {dto.Id} is not found");

            if (!IsValidEmail(dto.Email!))
            {
                throw new BadRequestException("Email format is not valid");
            }

            var user = userInfoRepository.FindByCondition(u => u.Email == dto.Email).FirstOrDefault();
            if (user != null && user.Email != entity.Email)
            {
                throw new BadRequestException("Email existed");
            }

            entity.Email = dto.Email;
            entity.FullName = dto.FullName;
            entity.UpdateAt = DateTime.Now;

            userInfoRepository.Update(entity);
            userInfoRepository.Save();
            return dto;
        }

        public UserInfo ValidateUser(LoginDTO dto)
        {
            var user = userInfoRepository.FindByCondition(u => u.Email == dto.Email)
            .FirstOrDefault();

            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            if (!VerifyPassword(dto.Password!, user.Password!))
            {
                throw new InvalidCredentialsException();
            }

            return user;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA256);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, HashType.SHA256);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public UserInfoDTO RegisterUser(UserInfoDTO dto)
        {
            if (dto.Email == null)
            {
                throw new BadRequestException("Email field cannot be empty");
            }

            if (!IsValidEmail(dto.Email))
            {
                throw new BadRequestException("Email format is not valid");
            }

            var user = userInfoRepository.FindByCondition(u => u.Email == dto.Email).FirstOrDefault();
            if (user != null)
            {
                throw new BadRequestException("Email existed");
            }

            if (dto.Password == null)
            {
                throw new BadRequestException("Password field cannot be empty");
            }

            if (dto.FullName == null)
            {
                throw new BadRequestException("FullName field cannot be empty");
            }

            dto.Id = Guid.NewGuid().ToString();
            dto.IsAdmin = false;
            dto.Password = HashPassword(dto.Password);

            UserInfo entity = mapper.Map<UserInfo>(dto);
            userInfoRepository.Create(entity);
            userInfoRepository.Save();
            dto.Password = "";

            return dto;
        }

        public PaginationData Get(int offset, int pageSize)
        {
            int total;
            IQueryable<UserInfo> query = userInfoRepository.QueryAll().Where(x => x.IsDeleted == false);

            total = query.Count();
            List<UserInfo> entityList = query
                .Skip(offset * pageSize)
                .Take(pageSize)
                .ToList();

            List<UserViewDTO> dtoList = new List<UserViewDTO>();
            foreach (UserInfo entity in entityList)
            {
                //UserViewDTO dto = mapper.Map<UserViewDTO>(entity);

                UserViewDTO dto = new UserViewDTO
                {
                    Id = entity.Id,
                    Email = entity.Email,
                    FullName = entity.FullName,
                    IsAdmin = entity.IsAdmin,
                };

                dtoList.Add(dto);
            }
            return new PaginationData
            {
                Data = dtoList,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };
        }


        public PaginationData GetUserCardsByUserId(string userId, int offset, int pageSize)
        {
            UserInfo userInfo = userInfoRepository.FindById(userId);

            if (userInfo == null)
            {
                throw new UserInfoNotFoundException(userId);
            }

            int total;
            IQueryable<UserCard> query = userCardRepository.QueryAll().Where(uc => uc.UserId == userId & uc.IsDeleted == false);

            total = query.Count();
            if (pageSize == 0 && offset == 0)
            {
                pageSize = total;
            }
            List<UserCard> userCards = query
                .Skip(offset * pageSize)
                .Take(pageSize)
                .ToList();

            List<UserCardDTO> userCardDTOs = new List<UserCardDTO>();
            foreach (UserCard userCard in userCards)
            {
                UserCardDTO dto = mapper.Map<UserCardDTO>(userCard);
                try
                {
                    dto.JsonData = JObject.Parse(userCard.Data!);
                }
                catch { }
                userCardDTOs.Add(dto);
            }

            return new PaginationData
            {
                Data = userCardDTOs,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };
        }

        public void ChangePassword(ChangePasswordDTO dto)
        {
            if (dto.Id == null)
                throw new ArgumentNullException("id");

            if (dto.OldPassword == null)
                throw new ArgumentNullException("oldPassword");

            if (dto.NewPassword == null)
                throw new ArgumentNullException("newPassword");

            var entity = userInfoRepository.FindById(dto.Id)
                ?? throw new NotFoundException($"User with id = {dto.Id} is not found");

            if (!VerifyPassword(dto.OldPassword, entity.Password!))
                throw new BadRequestException("Old Password is not correct");

            entity.Password = HashPassword(dto.NewPassword);
            entity.UpdateAt = DateTime.Now;
            userInfoRepository.Update(entity);
            userInfoRepository.Save();

            //UserInfoDTO response = mapper.Map<UserInfoDTO>(entity);
            //return response;
        }
        public ResetpasswordDTO ResetPassword(string email)
        {
            if (!IsValidEmail(email))
            {
                throw new BadRequestException("Invalid email format");
            }

            var user = userInfoRepository.FindByCondition(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                throw new UserInfoNotFoundException("User not found");
            }

            string newPassword = GenerateRandomPassword();

            string hashedPassword = HashPassword(newPassword);

            user.Password = hashedPassword;
            userInfoRepository.Update(user);
            userInfoRepository.Save();
            ResetpasswordDTO resetpasswordDTO = new ResetpasswordDTO
            {
                Email = user.Email,
                NewPassword = newPassword
            };

            return resetpasswordDTO;
        }
        private string GenerateRandomPassword()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder passwordBuilder = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(characters.Length);
                passwordBuilder.Append(characters[index]);
            }

            string newPassword = passwordBuilder.ToString();
            return newPassword;
        }

        public PaginationData GetUserUploadByUserId(string userId, int offset, int pageSize)
        {
            UserInfo userInfo = userInfoRepository.FindById(userId)
                ?? throw new UserInfoNotFoundException(userId);

            IQueryable<UserUpload> query = userUploadRepository
                .QueryAll()
                .Where(uu => uu.UserId == userId && uu.IsDeleted == false);

            int total = query.Count();

            List<UserUpload> userUploads = query
                .Skip((offset) * pageSize)
                .Take(pageSize)
                .ToList();

            List<UserUploadDTO> userUploadDTOs = mapper.Map<List<UserUploadDTO>>(userUploads);

            var paginationData = new PaginationData
            {
                Data = userUploadDTOs,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };

            return paginationData;
        }
    }
}
