using AutoMapper;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Bll.Services.Abstract
{
    public abstract class BaseService
    {
        private protected readonly IUnitOfWork _unitOfWork;
        private protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
