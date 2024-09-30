var Site = {
    From: '',
    Destination: '',
    CheckIn: '',
    CheckOut: '',
    Rooms: 1,
    Adults: 1,
    Children: 0,
    PassengersCount: 0,
    FlightId: 0,
    HolteId: 0,
    CarId: 0
}

Site.getHotels = function() {
    var destination = Site.Destination;
    var fromDate = Site.CheckIn;
    var toDate = Site.CheckOut;

    $.ajax({
        url: 'https://localhost:44323/api/Hotel?destination=' + destination + '&fromDate=' + fromDate + '&toDate=' + toDate,
        type: 'GET',
        success: function(result) {

            if (result != undefined) {

                Site.HotelId = result.id;

                $("#foundHotelPhoto").attr("src", result.photoUrl);
                $("#foundHotelName").text(result.name);
                $("#foundHotelPrice").text("$" + result.price);
                $("#foundHotelAddress").text(result.address);
                $("#foundHotelDescription").text(result.description);
            }
        },
        error: function(err) {

        }
    });
}

Site.getCar = function() {
    var destination = Site.Destination;
    var fromDate = Site.CheckIn;
    var toDate = Site.CheckOut;
    var passengersCount = Site.PassengersCount;

    $.ajax({
        url: 'https://localhost:44321/api/Car?city=' + destination + '&departureDate=' + fromDate + '&returnDate=' + toDate + "&passengersCount=" + passengersCount,
        type: 'GET',
        success: function(result) {

            if (result != undefined) {

                Site.CarId = result.id;

                $("#foundCarPhoto").attr("src", result.carPhoto);
                $("#foundCarName").text(result.carName);
                $("#foundCarPrice").text("$" + result.carDescription);
                $("#foundCarAgency").text(result.agency);
                $("#foundCarPickUpLocation").text(result.pickUpLocation);
                $("#foundCarDeparturePickUp").text(result.departurePickUpTime);
                $("#foundCarReturnPickUp").text(result.returnPickUpTime);
                $("#foundCarMaxPassengers").text(result.maximumNumberOfPassengers);
            }
        },
        error: function(err) {

        }
    });
}

Site.getFlights = function() {
    var from = Site.From;
    var destination = Site.Destination;
    var returnDate = Site.CheckOut;
    var departure = Site.CheckIn;

    $.ajax({
        url: 'https://localhost:44314/api/Flight?from=' + from + '&destination=' + destination + "&departure=" + departure + "&returnDate=" + returnDate,
        type: 'GET',
        success: function(result) {
            console.log(result);

            if (result != undefined) {

                Site.FlightId = result.id;

                $("#foundFlightFrom").text(result.from);
                $("#foundFlightDestination").text(result.destination);

                $("#foundFlightFromReturn").text(result.from);
                $("#foundFlightDestinationReturn").text(result.destination);
                $("#foundFlightPrice").text("$ " + result.price);

                $("#departureTakeoffTime").text(result.departureTakeoffTime);
                $("#departureLandingTime").text(result.departureLandingTime);
                $("#returnTakeoffTime").text(result.returnTakeoffTime);
                $("#returnLandingTime").text(result.returnLandingTime);

                $("#foundFlightAirport").text(result.airportName);
                $("#foundFlightPlaneCompany").text(result.planeCompany);
            }
        },
        error: function(err) {

        }
    });
}

Site.book = function() {

    location.href = "/Home/BookingResult?flightId=" + Site.FlightId + "&hotelId=" + Site.HotelId + "&carId=" + Site.CarId;
}

Site.checkAvailability = function(e) {
    e.preventDefault();

    Site.From = $("#bookingFromInput").val();
    Site.Destination = $("#bookingDestinationInput").val();
    Site.CheckIn = $("#bookingCheckInInput").val();
    Site.CheckOut = $("#bookingCheckOutInput").val();
    Site.Children = $("#bookingChildrenInput").val();
    Site.Rooms = $("#bookingRoomsInput").val();
    Site.Adults = $("#bookingAdultsInput").val();

    Site.PassengersCount = parseInt(Site.Adults) + parseInt(Site.Children);

    Site.getHotels();
    Site.getFlights();
    Site.getCar();

    $("#bookingFromInputSelected").val(Site.From);
    $("#bookingDestinationInputSelected").val(Site.Destination);
    $("#bookingCheckInInputSelected").val(Site.CheckIn);
    $("#bookingCheckOutInputSelected").val(Site.CheckOut);

    $("#bookingChildrenInputSelected").val(Site.Children);
    $("#bookingRoomsInputSelected").val(Site.Rooms);
    $("#bookingAdultsInputSelected").val(Site.Adults);


    $("#startFrom").css("display", "none");
    $("#bookHotelForm").css("display", "block");

}