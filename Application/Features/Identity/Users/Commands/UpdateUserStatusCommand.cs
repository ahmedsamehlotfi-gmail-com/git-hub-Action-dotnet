using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Commands
{

	public record UpdateUserStatusCommand : IRequest<IResponseWrapper>
	{
        public ChangeUserStatusRequest  changeUserStatus { get; set; }
    }

	public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand , IResponseWrapper>
	{
        private readonly IUserService _userService;

        public UpdateUserStatusCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IResponseWrapper> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
		{
            var userId = await _userService.ActivateOrDeactivateAsync(request.changeUserStatus.UserId, request.changeUserStatus.Activation);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: request.changeUserStatus.Activation ?
                "User activation successfully" : "User de-activated successfully");
		}
	}
}
