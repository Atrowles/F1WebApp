//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "common/SecurityContext"], function (c, auth) {

    var config = new c();

    //add below to register use of vee validate
    //team name is example of use
    //this is the better method to use rather than what I have wrote
    //I could also check if the errors var of vee validate is populated and block the submit buton
    //use this to do it http://vee-validate.logaretm.com/v2/guide/components/validation-observer.html#scoped-slot-data
    Vue.use(VeeValidate);
    var editVM = new Vue({
        el: '#addEditTeamBinding',
        data: {
            picked: "",
            showLoading: false,
            nameError: false,
            engineError: false,
            currentPointsError: false,
            isModelValid: true,

            //this should be created as an array
            team: { id: "", name: "", engine: "", customerEngine:false, currentPoints: 0 },
        },
        methods: {
            "resetValidation": function resetValidation() {
                this.nameError = false;               
                this.engineError = false;
                this.currentPointsError = false;
                this.isModelValid = true;
            },
            "validateTeam": function validateTeam() {
            
                this.resetValidation();
                if (this.team.name.search(/[^a-zA-Z]+/) > -1) {
                    this.nameError = true;
                    this.isModelValid = false;
                }
                if (this.team.engine.search(/[^a-zA-Z]+/) > -1) {
                    this.engineError = true;
                    this.isModelValid = false;
                }
                if (!$.isNumeric(this.team.currentPoints)) {
                    this.currentPointsError = true;
                    this.isModelValid = false;
                }

                console.log(this.isModelValid);
                if (this.isModelValid == true) {
                    if (typeof teamId !== 'undefined') {

                        this.updateTeam();
                    }
                    else {

                        this.addTeam();
                    }
                }
            },
            "updateTeam": function updateTeam() {

                //need to convert string values in ints
                var data = {
                    id: this.team.id,
                    name: this.team.name,
                    engine: this.team.engine,
                    customerEngine:(this.picked =="Yes" ? true : false),
                    currentPoints: parseInt(this.team.currentPoints),               

                };
                this.showLoading = true;
                //this.driver.teamID = parseInt(this.driver.teamID);
                // alert(this.driver.teamID);
                console.log(JSON.stringify(data));
                $.ajax({
                    url: config.getApiUrlForTeam(),
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
            "getTeam": function getTeam(id) {
                
                this.showLoading = true;
                
                $.ajax({
                    
                    url: config.getApiUrlForTeam() + '/' + id,
                    contentType: "application/json",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                })
                    .done(function (data) {
                        auth.refresh();
                        editVM.team.id = data.result.id;
                        editVM.team.name = data.result.name;
                        editVM.team.engine = data.result.engine;
                        editVM.team.customerEngine = data.result.customerEngine;
                        editVM.team.currentPoints = data.result.currentPoints;

                        editVM.picked = (editVM.team.customerEngine ? "Yes" : "No");


                        console.log(data.result);
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        editVM.showLoading = false;
                    });

            },
            "addTeam": function addTeam() {
                
                console.log(this.team);

                var data = {
                    name: this.team.name,
                    engine: this.team.engine,
                    customerEngine: (this.picked == "Yes" ? true : false),
                    currentPoints: parseInt(this.team.currentPoints),

                };

                //console.log(data);

                this.showLoading = true;
               // console.log(JSON.stringify(data1));
                $.ajax({
                    url: config.getApiUrlForTeam(),
                    type: "POST",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                   data: JSON.stringify(data)
                })
                    .done(function (data) {
                        auth.refresh();
                        editVM.showLoading = false;
                        toastr.success('Added Team', 'Success');
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

    console.log(errors);

    if (typeof teamId !== 'undefined') editVM.getTeam(teamId);
});