using FluentValidation;
using MicroMotel.Web.Models.Reservation.MealR;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Validators
{
    public class MealRCreateValidator:AbstractValidator<MealRCreateInput>
    {
        private readonly IReservationService _reservationservice;

        public MealRCreateValidator(IReservationService reservationservice)
        {
            _reservationservice = reservationservice;
            RuleFor(x => x).MustAsync(isroomridtimevalid).WithMessage("Meal reservation is not in room reservation interval");
        }

        private async  Task<bool> isroomridtimevalid(MealRCreateInput mci ,CancellationToken token)
        {
            var roomr =await _reservationservice.GetRoomRById(mci.RoomRId);
            return roomr.ReservStart<=mci.ReservationDate&&mci.ReservationDate.AddHours(1)<roomr.ReservEnd;
        }
    }
}
