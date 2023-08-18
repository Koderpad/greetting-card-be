using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Domain.ExceptionModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using Service.Abstract;
using Service.Implement.ObjectMapping;
using Service.Implement.Utilities;
using System.Text;

namespace Service.Implement
{
    public class UserCardService : IUserCardService
    {
        private readonly IUserCardRepository userCardRepository;
        private readonly IUserInfoRepository userInfoRepository;
        private readonly ITemplateCardRepository templateCardRepository;
        private readonly IConfiguration configuration;

        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        }).CreateMapper();

        public UserCardService(IUserCardRepository userCardRepository,
            IUserInfoRepository userInfoRepository,
            ITemplateCardRepository templateCardRepository,
            IConfiguration configuration)
        {
            this.userCardRepository = userCardRepository;
            this.userInfoRepository = userInfoRepository;
            this.templateCardRepository = templateCardRepository;
            this.configuration = configuration;
        }
        public UserCardDTO Create(UserCardDTO dto, IFormFile images, List<FileUploadModel> items)
        {
            ValidateObject(dto);

            // lưu ảnh background
            string imageUrl = Utils.SaveImageToExternalApi(images).Result;
            dto.ImageLink = imageUrl;


            // lưu ảnh cho từng item trong jsondata
            foreach (var fileUploadModel in items)
            {
                if (!dto.JsonData!.ContainsKey("items"))
                    throw new Exception("json");

                JArray itemsArray = (JArray)dto.JsonData["items"]!;

                foreach (JObject jobject in itemsArray)
                {
                    string itemId = jobject["id"]!.ToString();

                    if (itemId == fileUploadModel.ItemId)
                    {
                        string src = Utils.SaveImageToExternalApi(fileUploadModel.FileDetails!).Result;
                        jobject["src"] = src;
                    }
                }
            }

            dto.Id = Guid.NewGuid().ToString();
            UserCard entity = mapper.Map<UserCard>(dto);
            entity.Data = dto.JsonData?.ToString();

            userCardRepository.Create(entity);
            userCardRepository.Save();

            return dto;
        }

        public void Delete(string id)
        {
            UserCard entity = userCardRepository.FindById(id)
                ?? throw new UserCardNotFoundException(id);

            entity.IsDeleted = true;
            userCardRepository.Update(entity);
            userCardRepository.Save();
        }

        public void PermanentlyDelete(string id)
        {
            UserCard entity = userCardRepository.FindById(id)
                ?? throw new UserCardNotFoundException(id);

            userCardRepository.Delete(entity);
            userCardRepository.Save();
        }

        public List<UserCardDTO> GetAll()
        {
            List<UserCard> entityList = userCardRepository.FindAll();
            List<UserCardDTO> dtoList = new List<UserCardDTO>();
            foreach (UserCard entity in entityList)
            {
                UserCardDTO dto = mapper.Map<UserCardDTO>(entity);
                try // nếu entity.Data là null hoặc không đúng dạng json thì đặt là null
                {
                    dto.JsonData = JObject.Parse(entity.Data!);
                }
                catch { }
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public UserCardDTO GetById(string id)
        {
            UserCard entity = userCardRepository.FindById(id)
                ?? throw new TemplateCardNotFoundException(id);

            UserCardDTO dto = mapper.Map<UserCardDTO>(entity);
            try // nếu entity.Data là null hoặc không đúng dạng json thì đặt là null
            {
                dto.JsonData = JObject.Parse(entity.Data!);
            }
            catch { }
            return dto;
        }

        public List<UserCardDTO> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<UserCardDTO> GetByUserInfoId(string id)
        {
            //check user id exist
            if (userInfoRepository.FindById(id) == null)
                throw new UserInfoNotFoundException(id);

            List<UserCard> entityList = userCardRepository.FindByCondition(x => x.UserId == id);
            List<UserCardDTO> dtoList = new List<UserCardDTO>();
            foreach (UserCard entity in entityList)
            {
                UserCardDTO dto = mapper.Map<UserCardDTO>(entity);
                try // nếu entity.Data là null hoặc không đúng dạng json thì đặt là null
                {
                    dto.JsonData = JObject.Parse(entity.Data!);
                }
                catch { }
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public UserCardDTO Update(UserCardDTO dto, IFormFile images, List<FileUploadModel> items)
        {
            if (dto.Id == null)
                throw new ArgumentNullException("id");
            ValidateObject(dto);

            string imageUrl = Utils.SaveImageToExternalApi(images).Result;
            dto.ImageLink = imageUrl;

            foreach (var fileUploadModel in items)
            {
                if (!dto.JsonData!.ContainsKey("items"))
                    throw new Exception("json");

                JArray itemsArray = (JArray)dto.JsonData["items"]!;
                foreach (JObject jobject in itemsArray)
                {
                    string itemId = jobject["id"]!.ToString();

                    if (itemId == fileUploadModel.ItemId)
                    {
                        string src = Utils.SaveImageToExternalApi(fileUploadModel.FileDetails!).Result;
                        jobject["src"] = src;
                    }
                }
            }

            UserCard entity = userCardRepository.FindById(dto.Id);

            UserCardDTO tempDto = mapper.Map<UserCardDTO>(entity);

            JArray tempItems = (JArray)dto.JsonData?["items"]!;

            var imageSources = new List<string>();

            foreach (JObject item in tempItems)
            {
                if (item["type"]!.ToString() == "image")
                {
                    string src = (string)item["src"]!;
                    imageSources.Add(src);
                }
            }

            foreach (string src in imageSources)
            {
                callDeleteImageApi(src);
            }

            entity = mapper.Map<UserCard>(dto);
            entity.Data = dto.JsonData?.ToString();

            userCardRepository.Update(entity);
            userCardRepository.Save();

            return dto;
        }

        private void ValidateObject(UserCardDTO dto)
        {
            if (userInfoRepository.FindById(dto.UserId) == null)
                throw new UserInfoNotFoundException(dto.UserId);
            if (dto.TemplateCardId != null)
            {
                if (templateCardRepository.FindById(dto.TemplateCardId) == null)
                    throw new TemplateCardNotFoundException(dto.TemplateCardId);
            }
        }

        public PaginationData Get(int offset, int pageSize)
        {
            int total;
            IQueryable<UserCard> query = userCardRepository.QueryAll().Where(x => x.IsDeleted == false);

            total = query.Count();
            if (pageSize == 0 && offset == 0)
            {
                pageSize = total;
            }
            List<UserCard> entityList = query
                .Skip(offset * pageSize)
                .Take(pageSize)
                .ToList();

            List<UserCardDTO> dtoList = new List<UserCardDTO>();
            foreach (UserCard entity in entityList)
            {
                UserCardDTO dto = mapper.Map<UserCardDTO>(entity);

                dtoList.Add(dto);
            }
            return new PaginationData
            {
                Data = dtoList,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };
        }

        private void callDeleteImageApi(string imageSource)
        {
            var httpClient = new HttpClient();
            var deleteImage = new
            {
                url = imageSource
            };

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(deleteImage), Encoding.UTF8,
                "application/json");

            var appSetting = configuration.GetSection("AppSettings");

            var request = new HttpRequestMessage(HttpMethod.Delete,
                appSetting["ImageApi"]);
            request.Content = stringContent;

            httpClient.Send(request);
        }
    }
}
