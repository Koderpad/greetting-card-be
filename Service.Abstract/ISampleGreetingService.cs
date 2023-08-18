using Domain.DTO;

namespace Service.Abstract
{
    public interface ISampleGreetingService
    {
        List<SampleGreetingDTO> GetAll();

        SampleGreetingDTO GetById(string id);

        SampleGreetingDTO Create(SampleGreetingDTO dto);

        SampleGreetingDTO Update(SampleGreetingDTO dto);

        void Delete(string id);
        List<SampleGreetingDTO> GetAllDeleted();

        SampleGreetingDTO Restore(string id);
        void PermanentlyDelete(string id);
        PaginationData Get(int offset, int pageSize);
    }
}
