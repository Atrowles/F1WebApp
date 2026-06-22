//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "Common/models", "common/SecurityContext"], function (c, model, auth) {

    var config = new c();

    auth.hasTokenExpired();

    //console.log(new Date().getFullYear());

    var raceResultVM = new Vue({
        el: '#raceCalBinding',
        data: {
          
            showLoading: false,
            raceData:[]
        },
        methods: {
            "showResult": function showResult(row) {
               // console.log(row.raceWinner);
                window.location = document.location.href + 'home/details/' + row.id;
            },
            "showUpdate": function showUpdate(row) {
                // console.log(row.raceWinner);
                window.location = document.location.href + 'home/update/' + row.id;
            },
            "isUpdate": function isUpdate(row) {
                
                if (row.raceWinner == "") return false;

                return true;
            },
            "getRaceCal": function getRaceCal() {
          
               

                this.showLoading = true;
                $.ajax({
                    url: config.getApiUrlForRaceResults() + '/' + '01-01-' + new Date().getFullYear() + '/RaceResult',
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    contentType: "application/json",
                    dataType: 'json',
                    
                })
                    .done(function (data) {
                        //refresh token
                        auth.refresh();
                        raceResultVM.raceData = data.result;
                        console.log(raceResultVM.raceData);
                        raceResultVM.showLoading = false;
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                       // alert(this.showLoading);
                        raceResultVM.showLoading = false;
                    });
            }
        }
    });


    raceResultVM.getRaceCal();
});