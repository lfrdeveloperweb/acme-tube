using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.DataContracts.Responses;
using AutoMapper;
using MediatR;

namespace AcmeTube.Application.Services
{
    public abstract class AppServiceBase
    {
        protected AppServiceBase(
            ISender sender,
            IMapper mapper)
        {
            Sender = sender;
            Mapper = mapper;
        }

        protected ISender Sender { get; }
        protected IMapper Mapper { get; }

        protected Response<TResponseData> From<TModel, TResponseData>(CommandResult<TModel> result) => Response.From<TModel, TResponseData>(result, Mapper);
    }
}
