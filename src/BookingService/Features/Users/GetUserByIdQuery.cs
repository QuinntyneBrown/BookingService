using System.Threading;
using MediatR;
using BookingService.Data;
using BookingService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Features.Users
{
    public class GetUserByIdQuery
    {
        public class GetUserByIdRequest : IRequest<GetUserByIdResponse> { 
            public int Id { get; set; }
			public int? TenantId { get; set; }
        }

        public class GetUserByIdResponse
        {
            public UserApiModel User { get; set; } 
        }

        public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
        {
            public GetUserByIdHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
            {                
                return new GetUserByIdResponse()
                {
                    User = UserApiModel.FromUser(await _context.Users.SingleAsync(x=>x.Id == request.Id && x.TenantId == request.TenantId))
                };
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
