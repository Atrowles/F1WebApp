//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "common/models", "common/SecurityContext"], function (c,model,auth) {

    var config = new c();
   // window.location = window.location.protocol + '//' + window.location.host;
    
    // console.log(new Date().getFullYear());
    //  console.log(raceId);

    var updateDriverResultVM = new Vue({
        el: '#updateDriverResults',
        data: {
            showLoading: false,
            firstError: false,
            showFastLap: true,
            showPoleLap: true,

            driverData: [],
            trackData: { trackId:0,trackName: "", pole: "", fastestLap: "", raceDate: "" },
            drivers: { id: "", name: "" },     
            updateData:  { trackId: 0, fastestLapDriver: 0, fastestLapTime: 0, poleTime: 0, poleDriver: 0, raceDate: "" },
        },
        methods: {        

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
                        console.log(data.result);
                        updateDriverResultVM.trackData.trackName = data.result.trackLocation;
                        updateDriverResultVM.trackData.pole = data.result.poleTime + " - " + data.result.poleDriver;
                        updateDriverResultVM.trackData.fastestLap = data.result.fastestLapTime + " - " + data.result.fastestLapDriver;
                        updateDriverResultVM.trackData.raceDate = data.result.raceDate;
                        updateDriverResultVM.trackData.trackId = data.result.trackId;
                        
                        //console.log(updateDriverResultVM.trackData);
                        updateDriverResultVM.showLoading = false;
                        if (data.result.poleDriver == "") {
                            updateDriverResultVM.showPoleLap = false;
                        }
                        if (data.result.fastestLapDriver == "") {
                            updateDriverResultVM.showFastLap = false;
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        updateDriverResultVM.showLoading = false;
                    });
            },
            "getAllDrivers": function getAllDrivers() {
                //this.showLoading = true;
                $.ajax({

                    url: config.getApiUrlForDriver() + '/GetDrivers',
                    contentType: "application/json",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                    type: "POST",
                    data: JSON.stringify(model),
                })
                    .done(function (data) {
                        auth.refresh();
                        var selectDrivers = Array.from(Object.keys(data.result), k => data.result[k]);
                        var allDriversSel = new Array();

                        //get the id and name of each driver out so we can build select
                        selectDrivers.forEach(function (item, index) {
                            allDriversSel.push({ id: item['id'], name: item['name'] });

                        });
                        //console.log(data);
                        updateDriverResultVM.drivers = allDriversSel;
                        console.log(updateDriverResultVM.drivers);

                       
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {

                        // alert(this.showLoading);
                        updateDriverResultVM.showLoading = false;
                    });
            },
            //pass in raceId
            "updateLapTimes": function updateLapTimes(id) {

                    //get 

                    var payLoad = {
                        id: parseInt(id),
                        fastestLapDriver: parseInt(this.updateData.fastestLapDriver),
                        trackId: parseInt(this.trackData.trackId),
                        fastestLapTime: parseFloat(this.updateData.fastestLapTime),
                        poleDriver: parseInt(this.updateData.poleDriver),
                        poleTime: parseFloat(this.updateData.poleTime),
//this bit is to ensure the date is converted as uk
                        raceDate: new Date(this.trackData.raceDate.split('/')[2], this.trackData.raceDate.split('/')[1] - 1, this.trackData.raceDate.split('/')[0]),

                };
                //console.log(this.trackData);
                
                  //  console.log(payLoad);
                    $.ajax({
                        url: config.getApiUrlForRaceResults(),
                        beforeSend: function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                        },
                        type: "PUT",
                        dataType: 'json',
                        contentType: "application/json; charset=utf-8",

                        data: JSON.stringify(payLoad),
                    })
                        .done(function (data) {
                            auth.refresh();
                              //go back to home page
                           window.location = window.location.protocol + '//' + window.location.host;
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(textStatus, errorThrown);
                        })
                        .always(function () {

                        });
                
            },
            "updateResult": function updateResult() {


                var payLoad = new Array();
                var hasFirstPos = false;
                this.firstError = false;
                $('#driver-results tr').each(function () {
                  
                    var driverId = $(this).find('select.sel-drivers > option:selected').val();
                    var position = $(this).find('input.position').val();
                    var gap = $(this).find('input.gap').val();
                    //console.log(driverId);
                    //console.log(position);
                    //console.log(gap);
                    //if all three are not undefined add to payLoad
                    if (position == 1) {
                      
                        hasFirstPos = true;  
                    }
                    if (typeof driverId !== 'undefined' && gap != "" && position != "") payLoad.push({ resultId: raceId, position: parseInt(position), driverId: parseInt(driverId), gap: parseFloat(gap) });
                }); 
                if (!hasFirstPos) this.firstError = true;
                console.log(payLoad);

                if (payLoad.length > 0 && hasFirstPos) {

                    this.showLoading = true;
                    console.log(JSON.stringify(payLoad));
                    $.ajax({
                        url: config.getApiUrlForDriverResults(),
                        beforeSend: function (request) {
                            request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                        },
                        type: "POST",
                        dataType: 'json',
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(payLoad),
                    })
                        .done(function (data) {
                            auth.refresh();
                            updateDriverResultVM.showLoading = false;
                            toastr.success('Added Driver Result', 'Success');
                            if (!updateDriverResultVM.showFastLap || !updateDriverResultVM.showPoleLap) {
                                updateDriverResultVM.updateLapTimes(raceId);
                            }
                          
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(textStatus, errorThrown);
                        })
                        .always(function () {
                            // alert(this.showLoading);
                            updateDriverResultVM.showLoading = false;
                        });

                }
            }
        }
    });


    if (typeof raceId !== 'undefined') {
        updateDriverResultVM.getTrackDetails(raceId);
       // updateDriverResultVM.updateLapTimes(raceId);
        updateDriverResultVM.getAllDrivers();
    }
});