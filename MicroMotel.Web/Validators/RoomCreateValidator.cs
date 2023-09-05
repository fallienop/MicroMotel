using FluentValidation;
using MicroMotel.Web.Models.Motel.Room;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Validators
{
    public class RoomCreateValidator:AbstractValidator<RoomCreateInput>
    {
        private readonly IMotelService _motelService;

        public RoomCreateValidator(IMotelService motelService)
        {
            _motelService = motelService;

            RuleFor(x => x).MustAsync(roomidnotexists).WithMessage("There is room with this number");
        }


        private async Task<bool> roomidnotexists(RoomCreateInput rci,CancellationToken token)
        {
            var rooms = await _motelService.GetPropertyWithRoomsAsync(rci.PropertyId);
            var roomnumbers=rooms.Select(x=>x.Number).ToList();

            return !roomnumbers.Any(x => x == rci.Number);
        }
    }
}
