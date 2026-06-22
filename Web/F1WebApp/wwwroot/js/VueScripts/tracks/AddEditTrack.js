//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "common/SecurityContext"], function (c, auth) {

    var config = new c();


    var editVM = new Vue({
        el: '#addEditbinding',
        data: {
            showLoading:false,
            locationError: false,
            lengthError: false,
            fastestLapError: false,
            isModelValid:true,
            //this should be created as an array
            track: { id: "", location: "", fastestLap: 0.00, length:0.00 },          
        },
        methods: {
            "resetValidation": function resetValidation() {
                this.locationError = false;
                this.fastestLapError = false;
                this.lengthError = false;
                this.isModelValid = true;
            },
            "validateTrack": function validateTrack() {   
                this.resetValidation();
                if (this.track.location.search(/[^a-zA-Z]+/) > -1) {
                    this.locationError = true;
                    this.isModelValid = false;
                }
                if (!$.isNumeric(this.track.fastestLap)){
                    this.fastestLapError = true;
                    this.isModelValid = false;
                }
                if (!$.isNumeric(this.track.length)){
                    this.lengthError = true;
                    this.isModelValid = false;
                }

                console.log(this.isModelValid);
                if (this.isModelValid == true) {
                    if (typeof trackId !== 'undefined') {

                        this.updateTrack();
                    }
                    else {

                        this.addTrack();
                    }
                }
            },
            "updateTrack": function updateTrack() {
                var data = {
                    id: this.track.id,
                    location: this.track.location,
                    length: parseFloat(this.track.length),
                    fastestLap: parseFloat(this.track.fastestLap),
                };

                this.showLoading = true;
                
                console.log(JSON.stringify(data));
                $.ajax({
                    url: config.getApiUrlForTrack(),
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    type: "PUT",
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(data),
                })
                    .done(function (data) {
                        auth.refresh();
                        editVM.showLoading = false;
                        toastr.success('Updated Track', 'Success');
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
            "getTrack": function getTrack(id) {     
                this.showLoading = true;
                $.ajax({
                    
                    url: config.getApiUrlForTrack() + '/' + id,
                    contentType: "application/json",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                })
                    .done(function (data) {
                        auth.refresh();
                        editVM.track.id = data.result.id;
                        editVM.track.location = data.result.location;
                        editVM.track.fastestLap = data.result.fastestLap;
                        editVM.track.length = data.result.length;

                        console.log(data.result);
                       // this.trackId = data.result.id;
                       // this.location = data.result.location;
                       // this.fastestLap = data.result.fastestLap;
                       // this.length = data.result.length;
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        editVM.showLoading = false;
                    });


                    
            },
            "addTrack": function addTrack() {
                var data = {
                    location: this.track.location,
                    length: parseFloat(this.track.length),
                    fastestLap: parseFloat(this.track.fastestLap),
                };

                console.log(data);

                this.showLoading = true;
                console.log(JSON.stringify(data));
                $.ajax({
                    url: config.getApiUrlForTrack(),
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
                        toastr.success('Added Track', 'Success');
                        editVM.resetValidation();
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        editVM.showLoading = false;
                    });
            }
        }
    });

    if (typeof trackId !== 'undefined') editVM.getTrack(trackId);
});