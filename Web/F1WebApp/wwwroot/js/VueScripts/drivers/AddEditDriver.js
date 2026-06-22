//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "common/models", "common/SecurityContext"], function (c, model, auth) {

    var config = new c();


    var editVM = new Vue({
        el: '#addEditDriverBinding',
        data: {
            showLoading: false,
            nameError : false,
            teamIdError : false,
            polesError : false,
            raceWinsError : false,
            championshipsError : false,
            currentPointsError : false,
            isModelValid: true,
            teams: { id: "", name: "" },           
            driver: { id: "", name: "",  teamId: 0, poles: 0, raceWins: 0, championships: 0, currentPoints: 0, raceDate:"" },
        },
        methods: {
            "resetValidation": function resetValidation() {
                this.nameError = false;
                this.teamIdError = false;
                this.polesError = false;
                this.raceWinsError = false;
                this.championshipsError = false;
                this.currentPointsError = false;
                this.isModelValid = true;
            },
            "validateDriver": function validateDriver() {
                this.resetValidation();
                if (this.driver.name.search(/[^a-zA-Z]+/) > -1) {
                    this.nameError = true;
                    this.isModelValid = false;
                }
                if (!$.isNumeric(this.driver.currentPoints)) {
                    this.currentPointsError = true;
                    this.isModelValid = false;
                }
                if (!$.isNumeric(this.driver.championships)) {
                    this.championshipsError = true;
                    this.isModelValid = false;
                }
                if (!$.isNumeric(this.driver.raceWins)) {
                    this.raceWinsError = true;
                    this.isModelValid = false;
                }

                if (!$.isNumeric(this.driver.poles)) {
                    this.polesError = true;
                    this.isModelValid = false;
                }

                console.log(this.isModelValid);
                if (this.isModelValid == true) {
                    if (typeof driverId !== 'undefined') {

                        this.updateDriver();
                    }
                    else {

                        this.addDriver();
                    }
                }
            },
            "updateDriver": function updateDriver() {

     
                //need to convert string values in ints
                var data = {
                    id: this.driver.id,
                    name: this.driver.name,
                    //this should come from the selected drop box
                    teamId : parseInt(this.driver.teamId),
                    championships : parseInt(this.driver.championships),
                    poles : parseInt(this.driver.poles),
                    currentPoints : parseInt(this.driver.currentPoints),
                    raceWins : parseInt(this.driver.raceWins),
                   
                };
                this.showLoading = true;
                //this.driver.teamID = parseInt(this.driver.teamID);
               // alert(this.driver.teamID);
                //console.log(JSON.stringify(this.driver));
                $.ajax({
                    url: config.getApiUrlForDriver(),
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
            "getDriver": function getDriver(id) {
               
                this.showLoading = true;
                $.ajax({
                    
                    url: config.getApiUrlForDriver() + '/' + id,
                    contentType: "application/json",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                })
                    .done(function (data) {
                        //toastr.success( + ' row(s) returned', 'Success');
                        auth.refresh();
                        editVM.driver.id = data.result.id;
                        editVM.driver.name = data.result.name;
                        editVM.driver.teamId = data.result.teamId;
                        editVM.driver.teamName = data.result.teamName;
                        editVM.driver.championships = data.result.championships;
                        editVM.driver.poles = data.result.poles;
                        editVM.driver.currentPoints = data.result.currentPoints;
                        editVM.driver.raceWins = data.result.raceWins;
                        // console.log(data.result);
                       
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        editVM.showLoading = false;
                    });

            },
            "getTeams": function getDriver() {
                //this.showLoading = true;
                $.ajax({

                    url: config.getApiUrlForTeam() + '/GetTeams',
                    contentType: "application/json",
                    type: "POST",
                    data: JSON.stringify(model),
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                })
                    .done(function (data) {
                        var selectTeams = Array.from(Object.keys(data.result), k => data.result[k]);
                        var allTeamsSel = new Array();
                        auth.refesh();
                        //get the id and name of each team out so we can build select
                        selectTeams.forEach(function (item, index) {
                            allTeamsSel.push({ id: item['id'], name: item['name'] });

                        });
                        // console.log(allTeamsSel[1]);
                        editVM.teams = allTeamsSel;
                        //console.log(this.teams);
                       
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                      
                        // alert(this.showLoading);
                        editVM.showLoading = false;
                    });


            },
            "addDriver": function addDriver() {
                var data = {
                    name: this.driver.name,
                    teamId: parseInt(this.driver.teamId),
                    championships: parseInt(this.driver.championships),
                    poles: parseInt(this.driver.poles),
                    currentPoints: parseInt(this.driver.currentPoints),
                    raceWins: parseInt(this.driver.raceWins),

                };
                this.showLoading = true;
                console.log(JSON.stringify(data));
                $.ajax({
                    url: config.getApiUrlForDriver(),
                    type: "POST",
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    data: JSON.stringify(data),
                })
                    .done(function (data) {
                        auth.refresh();
                        editVM.showLoading = false;
                        toastr.success('Added Driver', 'Success');
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

    

    editVM.getTeams();
    //test if we are adding or editing by checking for the driver id
    if (typeof driverId !== 'undefined') editVM.getDriver(driverId);

   
});