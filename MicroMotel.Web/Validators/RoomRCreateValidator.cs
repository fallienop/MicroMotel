using FluentValidation;
using MicroMotel.Web.Models.Reservation.RoomR;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Validators
{
    public class RoomRCreateValidator:AbstractValidator<RoomRCreateInput>
    {
        private readonly IReservationService _reservationservice;
        public RoomRCreateValidator(IReservationService reservationService)
        {
            _reservationservice = reservationService;
            RuleFor(x => x.ReservStart).Must(minimumtoday).WithMessage("cannot be less than today");
            RuleFor(x=>x).Must(endbiggerthanstart).WithMessage("bruhhhh");
            RuleFor(x=>x).Must(diff3hour).WithMessage("minimum 2 hour");
            RuleFor(x => x).MustAsync(overlappingcontrol).WithMessage("this room is already reserved for this time interval");
            
        }

        private bool minimumtoday(DateTime reservstart)
        {
            return reservstart > DateTime.Now;
        }

        private bool endbiggerthanstart(RoomRCreateInput rrci)
        {
            return rrci.ReservStart<rrci.ReservEnd;
        }
        private bool diff3hour (RoomRCreateInput rrci)
        {
            return rrci.ReservStart.AddHours(2) < rrci.ReservEnd;
        }

        private async Task<bool> overlappingcontrol (RoomRCreateInput rrci,CancellationToken cancellationToken)
        {
            var reservs=await _reservationservice.GetAllByRoomId(rrci.RoomId);
            return !reservs.Any(existingres => rrci.ReservStart < existingres.ReservStart && rrci.ReservEnd > existingres.ReservStart);
        }

        
    }
}
