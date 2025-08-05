using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Commands
{
	public record UpdateUserCommand : IRequest<IResponseWrapper>
	{
        public UpdateUserRequest  UpdateUserRequest { get; set; }
    }

	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand , IResponseWrapper>
	{
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
            var userId = await _userService.UpdateUserAsync(request.UpdateUserRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User update successfully");
		}
	}
}
