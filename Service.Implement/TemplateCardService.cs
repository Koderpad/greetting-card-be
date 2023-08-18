using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Domain.ExceptionModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using Service.Abstract;
using Service.Implement.ObjectMapping;
using System.Text;

namespace Service.Implement
{
    public class TemplateCardService : ITemplateCardService
    {
        private readonly ITemplateCardRepository templateCardRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITagRepository tagRepository;
        private readonly IConfiguration configuration;

        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        }).CreateMapper();

        public TemplateCardService(ITemplateCardRepository templateCardRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            IConfiguration configuration)
        {
            this.templateCardRepository = templateCardRepository;
            this.categoryRepository = categoryRepository;
            this.tagRepository = tagRepository;
            this.configuration = configuration;
        }
        public TemplateCardDTO Create(TemplateCardDTO dto)
        {
            dto.Id = Guid.NewGuid().ToString();
            TemplateCard entity = mapper.Map<TemplateCard>(dto);
            entity.Data = dto.JsonData?.ToString();

            dto.CategoryIdList.ForEach(categoryId =>
            {
                Category category = categoryRepository.FindById(categoryId)
                    ?? throw new CategoryNotFoundException(categoryId);
                entity.Categories.Add(category);
                //attach để không tạo mới category
                categoryRepository.Attach(category);
            });

            dto.TagIdList.ForEach(TagId =>
            {
                Tag tag = tagRepository.FindById(TagId)
                    ?? throw new NotFoundException($"Tag '{TagId}' Not Found");
                entity.Tags.Add(tag);
                tagRepository.Attach(tag);
            });

            templateCardRepository.Create(entity);
            templateCardRepository.Save();

            return dto;
        }

        public void Delete(string id)
        {
            TemplateCard entity = templateCardRepository.FindById(id)
                ?? throw new TemplateCardNotFoundException(id);

            entity.IsDeleted = true;
            templateCardRepository.Update(entity);
            templateCardRepository.Save();
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

        public void PermanentlyDelete(string id)
        {
            TemplateCard entity = templateCardRepository.FindById(id)
                ?? throw new TemplateCardNotFoundException(id);

            TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);

            JArray items = (JArray)dto.JsonData?["items"]!;

            var imageSources = new List<string>();

            foreach (JObject item in items)
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

            templateCardRepository.Delete(entity);
            templateCardRepository.Save();
        }

        public List<TemplateCardDTO> GetAll() //không dùng, thay thế bởi hàm Get()
        {
            List<TemplateCard> entityList = templateCardRepository.FindAll(x => x.Categories);
            List<TemplateCardDTO> dtoList = new List<TemplateCardDTO>();
            foreach (TemplateCard entity in entityList)
            {
                TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public TemplateCardDTO GetById(string id)
        {
            TemplateCard entity = templateCardRepository.FindById(id, x => x.Categories, x => x.Tags)
                ?? throw new TemplateCardNotFoundException(id);

            TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
            return dto;
        }

        public List<TemplateCardDTO> GetByName(string name) //không dùng, thay thế bởi hàm Get()
        {
            List<TemplateCard> entityList = templateCardRepository
                .FindByCondition(x => x.Name!.Contains(name) & x.IsDeleted == false, x => x.Categories);
            List<TemplateCardDTO> dtoList = new List<TemplateCardDTO>();
            foreach (TemplateCard entity in entityList)
            {
                TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public TemplateCardDTO Update(TemplateCardDTO dto)
        {
            if (dto.Id == null)
                throw new ArgumentNullException("id");

            TemplateCard entity = templateCardRepository
                .FindByConditionWithTracking(x => x.Id == dto.Id, x => x.Categories)
                .FirstOrDefault()
                ?? throw new TemplateCardNotFoundException(dto.Id);

            entity.Name = dto.Name;
            entity.Width = dto.Width;
            entity.Height = dto.Height;
            entity.ImageLink = dto.Image;
            entity.Data = dto.JsonData?.ToString();
            entity.Categories.Clear();
            entity.Tags.Clear();

            dto.CategoryIdList.ForEach(categoryId =>
            {
                Category category = categoryRepository
                    .FindByConditionWithTracking(x => x.Id == categoryId)
                    .FirstOrDefault()
                    ?? throw new CategoryNotFoundException(categoryId);
                entity.Categories.Add(category);
            });

            dto.TagIdList.ForEach(TagId =>
            {
                Tag tag = tagRepository
                .FindByConditionWithTracking(x => x.Id == TagId)
                .FirstOrDefault()
                ?? throw new NotFoundException($"Tag '{TagId}' Not Found");
                entity.Tags.Add(tag);
            });

            templateCardRepository.Save();

            return dto;
        }

        List<TemplateCardDTO> ITemplateCardService.GetAllDeleted()
        {
            List<TemplateCard> entityList = templateCardRepository
                .FindByCondition(x => x.IsDeleted == true, x => x.Categories, x => x.Tags);
            List<TemplateCardDTO> dtoList = new List<TemplateCardDTO>();
            foreach (TemplateCard entity in entityList)
            {
                TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        TemplateCardDTO ITemplateCardService.Restore(string id)
        {
            TemplateCard entity = templateCardRepository
                .FindByCondition(x => x.Id == id)
                .FirstOrDefault()
                ?? throw new TemplateCardNotFoundException(id);

            if (entity.IsDeleted == false)
                throw new BadRequestException($"Category {id} has not been deleted");

            entity.IsDeleted = false;
            templateCardRepository.Update(entity);
            templateCardRepository.Save();

            TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
            try // nếu entity.Data là null hoặc không đúng dạng json thì đặt là null
            {
                dto.JsonData = JObject.Parse(entity.Data!);
            }
            catch { }

            return dto;
        }


        public PaginationData Get(int offset, int pageSize)
        {
            int total;
            IQueryable<TemplateCard> query = templateCardRepository.QueryAll(x => x.Categories, x => x.Tags).Where(x => x.IsDeleted == false);

            total = query.Count();
            if (pageSize == 0 && offset == 0)
            {
                pageSize = total;
            }
            List<TemplateCard> entityList = query
                .Skip(offset * pageSize)
                .Take(pageSize)
                .ToList();

            List<TemplateCardDTO> dtoList = new List<TemplateCardDTO>();
            foreach (TemplateCard entity in entityList)
            {
                TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
                dto.Image = entity.ImageLink!;
                dtoList.Add(dto);
            }
            return new PaginationData
            {
                Data = dtoList,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };
        }
        public List<TemplateCardDTO> GetByTag(string tagName)
        {
            List<TemplateCard> entityList = templateCardRepository
                .FindByCondition(x => x.Tags.Any(t => t.Name == tagName) && x.IsDeleted == false, x => x.Categories, x => x.Tags);

            List<TemplateCardDTO> dtoList = new List<TemplateCardDTO>();
            foreach (TemplateCard entity in entityList)
            {
                TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
                try
                {
                    dto.JsonData = JObject.Parse(entity.Data!);
                }
                catch { }

                foreach (var category in entity.Categories)
                {
                    dto.CategoryIdList.Add(category.Id);
                }
                dtoList.Add(dto);
            }
            return dtoList;
        }

    }
}
