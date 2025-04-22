using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Enums.Entity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace ECOM.Infrastructure.Implementations.Notifications.SignalR
{
	public class NotificationHub(
		[FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork) : Hub
	{
		private readonly IUnitOfWork _unitOfWork = mainUnitOfWork;

		public override async Task OnConnectedAsync()
		{
			var userId = Context.UserIdentifier;

			if (Guid.TryParse(userId, out var guid))
			{
				var user = await _unitOfWork.Repository<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == guid);
				if (user != null)
				{
					user.ConnectionStatus = (int)ConnectionStatusEnum.Online;
					_unitOfWork.Repository<ApplicationUser>().Update(user);
					await _unitOfWork.SaveChangesAsync();
				}
			}

			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var userId = Context.UserIdentifier;

			if (Guid.TryParse(userId, out var guid))
			{
				var user = await _unitOfWork.Repository<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == guid);
				if (user != null)
				{
					user.ConnectionStatus = (int)ConnectionStatusEnum.Offline;
					user.LastSeenAt_Utc = DateTime.UtcNow;
					_unitOfWork.Repository<ApplicationUser>().Update(user);
					await _unitOfWork.SaveChangesAsync();
				}
			}

			await base.OnDisconnectedAsync(exception);
		}
	}
}
