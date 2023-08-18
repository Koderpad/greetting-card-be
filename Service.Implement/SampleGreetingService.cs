using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Domain.ExceptionModel;
using Repository.Abstract;
using Service.Abstract;
using Service.Implement.ObjectMapping;

namespace Service.Implement
{
    public class SampleGreetingService : ISampleGreetingService
    {
        private readonly ISampleGreetingRepository sampleGreetingRepository;
        private readonly ICategoryRepository categoryRepository;

        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        }).CreateMapper();

        public SampleGreetingService(ISampleGreetingRepository sampleGreetingRepository,
            ICategoryRepository categoryRepository)
        {
            this.sampleGreetingRepository = sampleGreetingRepository;
            this.categoryRepository = categoryRepository;
        }
        public SampleGreetingDTO Create(SampleGreetingDTO dto)
        {
            ValidateObject(dto);

            dto.Id = Guid.NewGuid().ToString();
            SampleGreeting entity = mapper.Map<SampleGreeting>(dto);

            sampleGreetingRepository.Create(entity);
            sampleGreetingRepository.Save();

            return dto;
        }

        public void Delete(string id)
        {
            SampleGreeting entity = sampleGreetingRepository.FindById(id)
                ?? throw new SampleGreetingNotFoundException(id);

            entity.IsDeleted = true;
            sampleGreetingRepository.Update(entity);
            sampleGreetingRepository.Save();
        }

        public void PermanentlyDelete(string id)
        {
            SampleGreeting entity = sampleGreetingRepository.FindById(id)
                ?? throw new SampleGreetingNotFoundException(id);

            sampleGreetingRepository.Delete(entity);
            sampleGreetingRepository.Save();
        }

        public List<SampleGreetingDTO> GetAll()
        {
            List<SampleGreeting> entityList = sampleGreetingRepository.FindAll();
            List<SampleGreetingDTO> dtoList = new List<SampleGreetingDTO>();
            foreach (SampleGreeting entity in entityList)
            {
                SampleGreetingDTO dto = mapper.Map<SampleGreetingDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public SampleGreetingDTO GetById(string id)
        {
            SampleGreeting entity = sampleGreetingRepository.FindById(id)
                ?? throw new SampleGreetingNotFoundException(id);

            SampleGreetingDTO dto = mapper.Map<SampleGreetingDTO>(entity);
            return dto;
        }

        public SampleGreetingDTO Update(SampleGreetingDTO dto)
        {
            ValidateObject(dto);

            if (dto.Id == null)
                throw new ArgumentNullException("id");

            SampleGreeting entity = sampleGreetingRepository.FindById(dto.Id)
                ?? throw new SampleGreetingNotFoundException(dto.Id);

            entity.CategoryId = dto.CategoryId;
            entity.Data = dto.Data;
            entity.UpdateAt = DateTime.Now;

            sampleGreetingRepository.Update(entity);
            sampleGreetingRepository.Save();

            return dto;
        }

        List<SampleGreetingDTO> ISampleGreetingService.GetAllDeleted()
        {
            List<SampleGreeting> entityList = sampleGreetingRepository.FindByCondition(x => x.IsDeleted == true);
            List<SampleGreetingDTO> dtoList = new List<SampleGreetingDTO>();
            foreach (SampleGreeting entity in entityList)
            {
                SampleGreetingDTO dto = mapper.Map<SampleGreetingDTO>(entity);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        SampleGreetingDTO ISampleGreetingService.Restore(string id)
        {
            SampleGreeting entity = sampleGreetingRepository
                .FindByCondition(x => x.Id == id).FirstOrDefault()
                ?? throw new SampleGreetingNotFoundException(id);

            if (entity.IsDeleted == false)
                throw new BadRequestException($"Sample Greeting {id} has not been deleted");

            entity.IsDeleted = false;
            sampleGreetingRepository.Update(entity);
            sampleGreetingRepository.Save();
            SampleGreetingDTO dto = mapper.Map<SampleGreetingDTO>(entity);
            return dto;
        }

        private void ValidateObject(SampleGreetingDTO dto)
        {
            if (categoryRepository.FindById(dto.CategoryId) == null)
                throw new CategoryNotFoundException(dto.CategoryId);
        }

        public PaginationData Get(int offset, int pageSize)
        {
            int total;
            IQueryable<SampleGreeting> query = sampleGreetingRepository.QueryAll().Where(x => x.IsDeleted == false);

            total = query.Count();
            if (pageSize == 0 && offset == 0)
            {
                pageSize = total;
            }
            List<SampleGreeting> entityList = query
                .Skip(offset * pageSize)
                .Take(pageSize)
                .ToList();

            List<SampleGreetingDTO> dtoList = new List<SampleGreetingDTO>();
            foreach (SampleGreeting entity in entityList)
            {
                SampleGreetingDTO dto = mapper.Map<SampleGreetingDTO>(entity);

                dtoList.Add(dto);
            }
            return new PaginationData
            {
                Data = dtoList,
                pagable = new Pagable { offset = offset, pageSize = pageSize, total = total }
            };
        }
    }
}
