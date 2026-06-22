//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "common/models", "common/SecurityContext"], function (c, model,auth) {

    var config = new c();


    var editVM = new Vue({
        el: '#addEditRaceResult',
        data: {
            showLoading: false,
            raceDateError: false,
            locationError: false,
            isModelValid: true,
            drivers: { id: "", name: "" },
            tracks: { id: "", name: "" },
            raceResult: { trackId: 0, fastestLapDriver: 0, fastestLapTime: 0, poleTime: 0, poleDriver:0, raceDate:""  },
        },
        methods: {
            "resetValidation": function resetValidation() {
                this.raceDateError = false;
                this.locationError = false;
                this.isModelValid = true;
            },
            "validateRace": function validateDriver() {
                this.resetValidation();
                if (this.raceResult.raceDate=="") {
                    this.raceDateError = true;
                    this.isModelValid = false;
                }
                if (this.raceResult.trackId=="") {
                    this.locationError = true;
                    this.isModelValid = false;
                }
                //this.isModelValid = false;
                console.log(this.raceResult);

                if (this.isModelValid == true) { 

                    this.addRace();
                }
            },
            "getTracks": function getTracks() {
                //this.showLoading = true;
                
                $.ajax({

                    url: config.getApiUrlForTrack() + '/GetTracks',
                    contentType: "application/json",
                    type: "POST",
                    data: JSON.stringify(model),
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                })
                    .done(function (data) {
                        auth.refresh();
                        var selectTracks = Array.from(Object.keys(data.result), k => data.result[k]);
                        var allTracksSel = new Array();

                        //get the id and name of each team out so we can build select
                        selectTracks.forEach(function (item, index) {
                            allTracksSel.push({ id: item['id'], name: item['location'] });

                        });
                        // console.log(allTeamsSel[1]);
                        // console.log(allTeamsSel[1]);
                        editVM.tracks = allTracksSel;
                        //console.log(editVM.tracks);
                        console.log(data.result)

                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {

                    });


            },
            "getDrivers": function getDrivers() {
                //this.showLoading = true;
                $.ajax({

                    url: config.getApiUrlForDriver() + '/GetDrivers',
                    contentType: "application/json",
                    type: "POST",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    data: JSON.stringify(model),
                    dataType: 'json',
                })
                    .done(function (data) {
                        auth.refresh();
                        var selectDrivers = Array.from(Object.keys(data.result), k => data.result[k]);
                        var allDriversSel = new Array();

                        //get the id and name of each team out so we can build select
                        selectDrivers.forEach(function (item, index) {
                            allDriversSel.push({ id: item['id'], name: item['name'] });

                        });
                        // console.log(allTeamsSel[1]);
                        editVM.drivers = allDriversSel;
                        console.log(editVM.drivers);

                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {

                    });


            },
            "addRace": function addRace() {
                var data = {
                    trackId: this.raceResult.trackId,
                    fastestLapDriver: parseInt(this.raceResult.fastestLapDriver),
                    fastestLapTime: parseFloat(this.raceResult.fastestLapTime),
                    poleTime: parseFloat(this.raceResult.poleTime),
                    poleDriver: parseInt(this.raceResult.poleDriver),
                    raceDate: this.raceResult.raceDate

                };
                this.showLoading = true;
                console.log(JSON.stringify(data));
                $.ajax({
                    url: config.getApiUrlForRaceResults(),
                    type: "POST",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(data),
                })
                    .done(function (data) {
                        auth.refresh();
                        editVM.showLoading = false;
                        toastr.success('Added Race Result', 'Success');
                        editVM.resetValidation();
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        editVM.showLoading = false;
                    });

                //alert();
            },

        }
    });



    editVM.getDrivers();
    editVM.getTracks();
    //test if we are adding or editing by checking for the driver id    


});