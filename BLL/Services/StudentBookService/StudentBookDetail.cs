using AutoMapper;
using BLL.StudentDto;

using DAL.Repository.BookDetails;
using Microsoft.Extensions.Caching.Memory;

namespace BLL.Services.StudentBookService
{
    public class StudentBookDetail : IStudentBookDetails
    {
        private readonly IStudentBookDetailRepositorys _studentBookDetails;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public StudentBookDetail(IStudentBookDetailRepositorys studentBookDetails, 
            IMapper mapper,
            IMemoryCache memoryCache
            ) 
        {
            _studentBookDetails =studentBookDetails;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<List<BllStudentBookDTO>> StudentBookDetails()
        {
            const string cacheKey = "StudentBookDetailsCacheKey";

            
            if (_memoryCache.TryGetValue(cacheKey, out List<BllStudentBookDTO> cachedData))
            {
                return cachedData;
            }

            try
            {
                var dalBookDetailsDtos = await _studentBookDetails.BookStudentDetails();
                var result = _mapper.Map<List<BllStudentBookDTO>>(dalBookDetailsDtos);

               
                _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); 

                return result;
            }
            catch (Exception ex)
            {
                
                throw new UserFriendlyException(ex.Message, ex);
            }
        }
    }
}
