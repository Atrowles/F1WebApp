//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "common/SecurityContext"], function (c, auth) {

    var config = new c();
    
 
   // console.log(new Date().getFullYear());
  //  console.log(raceId);

    var driverResultVM = new Vue({
        el: '#driverResultsBinding',
        data: {
           
            showLoading: false,
            driverData: [],
            trackData: {trackName:"", pole:"", fastestLap:"", raceDate:""}
        },
        methods: {
            "getRaceResult": function getRaceResult(id) {
                this.showLoading = true;
                $.ajax({

                    url: config.getApiUrlForDriverResults() + '/' + id,
                    contentType: "application/json",
                    dataType: 'json',
                })
                    .done(function (data) {
                        //toastr.success( + ' row(s) returned', 'Success');
                        driverResultVM.driverData = data.result;
                        //console.log(driverResultVM.driverData);

                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        driverResultVM.showLoading = false;
                    });
            },
            "getTrackDetails": function getTrackDetails(id) {
                this.showLoading = true;
                $.ajax({
                    url: config.getApiUrlForRaceResults() + '/' + id,
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    contentType: "application/json",
                    dataType: 'json',

                })
                    .done(function (data) {
                        auth.refresh();
                        //console.log(data.result);
                        driverResultVM.trackData.trackName = data.result.trackLocation;
                        driverResultVM.trackData.pole = data.result.poleTime + " - " + data.result.poleDriver;
                        driverResultVM.trackData.fastestLap = data.result.fastestLapTime + " - " + data.result.fastestLapDriver;
                        driverResultVM.trackData.raceDate = data.result.raceDate;
                        console.log(driverResultVM.trackData);
                        driverResultVM.showLoading = false;
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        driverResultVM.showLoading = false;
                    });
            }

        }
    });

  
    if (typeof raceId !== 'undefined') {
        driverResultVM.getRaceResult(raceId);
        driverResultVM.getTrackDetails(raceId);
    }
});