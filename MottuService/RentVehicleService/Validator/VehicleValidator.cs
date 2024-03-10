public class VehicleValidator {

    public int[] validNumberOfDaysToRent = {7,15,30};
    public void ValidadeRentVehicleRequest(ICreateRentVehicleRequest request){
        if(!validNumberOfDaysToRent.Contains(request.numberDaysToRent)) {
            throw new InvalidRequestExeception("Sorry, no rental plans available for that number of days. Please choose between 7, 15, or 30 days.");
        }
    }
}