using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Commands
{

	public class CreateUserCommand : IRequest<IResponseWrapper>
	{
        public CreateUserRequest  CreateUserRequests { get; set; }
    }

	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand , IResponseWrapper>
	{
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
           _userService = userService;
        }
        public async Task<IResponseWrapper> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
            var userId = await _userService.CreateUserAsync(request.CreateUserRequests);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "USer Created Successfully");
		}
	}
}
