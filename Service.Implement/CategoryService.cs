using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Domain.ExceptionModel;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using Service.Abstract;
using Service.Implement.ObjectMapping;

namespace Service.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ITemplateCardRepository templateCardRepository;
        private readonly ISampleGreetingRepository sampleGreetingRepository;


        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        }).CreateMapper();

        public CategoryService(ICategoryRepository categoryRepository,
            ITemplateCardRepository templateCardRepository,
            ISampleGreetingRepository sampleGreetingRepository)
        {
            this.categoryRepository = categoryRepository;
            this.templateCardRepository = templateCardRepository;
            this.sampleGreetingRepository = sampleGreetingRepository;
        }

        public CategoryDTO Create(CategoryDTO dto)
        {
            ValidateObject(dto);

            dto.Id = Guid.NewGuid().ToString();
            Category category = mapper.Map<Category>(dto);

            categoryRepository.Create(category);
            categoryRepository.Save();

            return dto;
        }

        public void Delete(string id)
        {
            if (categoryRepository.FindById(id) == null)
                throw new CategoryNotFoundException(id);

            DeleteCategoryAndRelationShip(id);

            categoryRepository.Save();
        }

        public void PermanentlyDelete(string id)
        {
            if (categoryRepository.FindAll == null)
                throw new CategoryNotFoundException(id);

            Category category = categoryRepository
                .FindByConditionWithTracking(x => x.Id == id, x => x.InverseParent, x => x.TemplateCards)
                .FirstOrDefault()!;

            categoryRepository.Delete(category);
            categoryRepository.Save();
        }

        public List<CategoryDTO> GetAll()
        {
            List<Category> entityList = categoryRepository.FindAll();
            List<CategoryDTO> dtoList = new List<CategoryDTO>();
            foreach (Category entity in entityList)
            {
                CategoryDTO dto = mapper.Map<CategoryDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }


        public CategoryDTO GetById(string id)
        {
            Category entity = categoryRepository.FindById(id) ?? throw new CategoryNotFoundException(id);

            CategoryDTO dto = mapper.Map<CategoryDTO>(entity);

            return dto;
        }

        public List<CategoryDTO> GetByName(string name)
        {
            List<Category> entityList = categoryRepository.FindByCondition(x => x.Name!.Contains(name) & x.IsDeleted == false);
            List<CategoryDTO> dtoList = new List<CategoryDTO>();
            foreach (Category entity in entityList)
            {
                CategoryDTO dto = mapper.Map<CategoryDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public List<CategoryDTO> GetAllThemeCategory()
        {
            List<Category> entityList = categoryRepository.FindByCondition(c => c.ParentId == "01H5VCH7DFTDSPNRCC57ADVXPP" & c.IsDeleted == false);
            List<CategoryDTO> dtoList = new List<CategoryDTO>();
            foreach (Category entity in entityList)
            {
                CategoryDTO dto = mapper.Map<CategoryDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public CategoryDTO Update(CategoryDTO dto)
        {
            // check valid
            ValidateObject(dto);
            if (dto.Id == null)
                throw new ArgumentNullException("id");
            Category entity = categoryRepository.FindById(dto.Id)
                ?? throw new CategoryNotFoundException(dto.Id);

            //update entity
            entity.ParentId = dto.ParentId;
            entity.Name = dto.Name;
            entity.UpdateAt = DateTime.Now;

            categoryRepository.Update(entity);
            categoryRepository.Save();

            return dto;
        }
        public List<CategoryDTO> GetAllDeleted()
        {
            List<Category> entityList = categoryRepository.FindByCondition(x => x.IsDeleted == true);
            List<CategoryDTO> dtoList = new List<CategoryDTO>();
            foreach (Category entity in entityList)
            {
                CategoryDTO dto = mapper.Map<CategoryDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public CategoryDTO Restore(string id)
        {
            Category entity = categoryRepository.FindByCondition(x => x.Id == id).FirstOrDefault()
                ?? throw new CategoryNotFoundException(id);

            if (entity.IsDeleted == false)
                throw new BadRequestException($"Category {id} has not been deleted");

            entity.IsDeleted = false;

            categoryRepository.Update(entity);
            categoryRepository.Save();

            CategoryDTO dto = mapper.Map<CategoryDTO>(entity);
            return dto;
        }

        public PaginationData GetTemplateCardByCategoryId(string categoryId, int offset, int pageSize)
        {
            int total;

            if (categoryRepository.FindById(categoryId) == null)
                throw new CategoryNotFoundException(categoryId);

            IQueryable<TemplateCard> query = templateCardRepository
                .QueryAll(t => t.Categories)
                .Where(t => t.Categories.Any(c => c.Id == categoryId & c.IsDeleted == false));

            total = query.Count();
            if (pageSize == 0 && offset == 0)
            {
                pageSize = total;
            }
            List<TemplateCard> templateCardList = query.Skip(offset * pageSize)
                .Take(pageSize)
                .ToList();
            List<TemplateCardDTO> templateCardDTOList = new List<TemplateCardDTO>();
            foreach (TemplateCard entity in templateCardList)
            {
                TemplateCardDTO dto = mapper.Map<TemplateCardDTO>(entity);
                try // nếu entity.Data là null hoặc không đúng dạng json thì đặt là null
                {
                    dto.JsonData = JObject.Parse(entity.Data!);
                }
                catch { }

                foreach (var category in entity.Categories)
                {
                    dto.CategoryIdList.Add(category.Id);
                }

                templateCardDTOList.Add(dto);
            }
            return new PaginationData
            {
                Data = templateCardDTOList,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };
        }

        private void ValidateObject(CategoryDTO dto)
        {
            string parentId = dto.ParentId!;
            if (parentId != null)
            {
                if (categoryRepository.FindById(parentId) == null)
                    throw new CategoryNotFoundException(parentId);
                if (parentId == dto.Id)
                    throw new BadRequestException("parendId invalid");
            }
        }

        private void DeleteCategoryAndRelationShip(string id)
        {
            Category category = categoryRepository.FindByConditionWithTracking(x => x.Id == id, x => x.InverseParent, x => x.TemplateCards).FirstOrDefault()!;
            category.IsDeleted = true;

            ////xóa category con
            //foreach (var childCategory in category.InverseParent)
            //{
            //    DeleteCategoryAndRelationShip(childCategory.Id);
            //}

            ////xóa quan hệ với TemplateCard
            //foreach (var templateCard in category.TemplateCards)
            //{
            //    templateCard.Categories.Remove(category);
            //}
        }
        public PaginationData GetSampleGreetingsByCategoryId(string categoryId, int offset, int pageSize)
        {
            int total;

            if (categoryRepository.FindById(categoryId) == null)
            {
                throw new CategoryNotFoundException(categoryId);
            }

            IQueryable<SampleGreeting> query = sampleGreetingRepository
                .QueryAll()
                .Where(x => x.CategoryId == categoryId & x.IsDeleted == false);

            total = query.Count();
            if (pageSize == 0 && offset == 0)
            {
                pageSize = total;
            }
            List<SampleGreeting> sampleGreetings = query.Skip(offset * pageSize)
                .Take(pageSize)
                .ToList();
            List<SampleGreetingDTO> sampleGreetingDTOs = new List<SampleGreetingDTO>();

            foreach (SampleGreeting entity in sampleGreetings)
            {
                SampleGreetingDTO dto = mapper.Map<SampleGreetingDTO>(entity);

                sampleGreetingDTOs.Add(dto);
            }

            return new PaginationData
            {
                Data = sampleGreetingDTOs,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };
        }
    }
}
