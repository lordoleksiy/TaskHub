using AutoMapper;
using TaskHub.Common.DTO.Reponse;
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
        
        protected static ApiResponse CreateErrorResponse(string message, IEnumerable<string>? erorrs = null)
        {
            return new ApiResponse
            {
                Status = Status.Error,
                Message = message,
                Errors = erorrs
            };
        }

        protected static ApiResponse CreateSucсessfullResponse(Object? data = null, string? message = null)
        {
            return new ApiResponse
            {
                Status = Status.Success,
                Message = message ?? nameof(Status.Success),
                Data = data
            };
        } 
    }
}
