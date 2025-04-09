using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Queries
{

	public record GetUserByIdQuery : IRequest<IResponseWrapper>
	{
        public string UserId { get; set; }
    }

	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, IResponseWrapper>
	{
        private readonly IUserService _userService;

        public GetUserByIdQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IResponseWrapper> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _userService.GetUserByIdAsync(request.UserId , cancellationToken);
            return await ResponseWrapper<UserDto>.SuccessAsync(data:user);
		}
	}
}
