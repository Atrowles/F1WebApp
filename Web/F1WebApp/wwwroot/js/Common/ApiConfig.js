define([], function () {

  

    var returnedAddress = function () {
       // var apiAddress = 'https://localhost:44315/api/';
        
        var apiAddress = 'https://f1api.azurewebsites.net/api/';

        //malke sure client id and auth server are from same enviroment
        //I messed up and had one from dev and one frmom prod
        //also an account needs makking for the auth server in each envioment
        //thats why password reset on prod was returning null objcet ref error, cos account did not exist
        //i was trying to use the one from dev
        var authServer = "https://capitaauth.azurewebsites.net";
        var oAuthUrl = authServer + '/oauth/token';
        var claimsUrl = authServer + '/api/Auth/GetUserClaims';
        var registerUserUrl = authServer + '/api/Auth/RegisterUser';
        var requestPasswordResetUrl = authServer + '/api/Auth/RequestPasswordReset';
        var loginUrl = '/Home/Login';
 
        var client_id = "3afe1228563340b18862845de0f4a53b";


        this.getLoginUrl = function () {
            return loginUrl;
        };


        this.getOAuthUrl = function () {
            return oAuthUrl;
        };

        this.getClaimsUrl = function () {
            return claimsUrl;
        };

        this.getORegisterUserUrl = function () {
            return registerUserUrl;
        };

        this.getResetPasswordUrl = function () {
           return requestPasswordResetUrl;
        };

        this.getOAuthUrl = function () {
            return oAuthUrl;
        };

        this.getClientId = function () {
            return client_id;
        };

        this.getApiUrlForTrack = function () {
            return apiAddress + 'tracks';
        };
        this.getApiUrlForDriver = function () {
            return apiAddress + 'drivers';
        };
        this.getApiUrlForTeam = function () {
            return apiAddress + 'teams';
        };
        this.getApiUrlForRaceResults = function () {
            return apiAddress + 'raceresult';
        };
        this.getApiUrlForDriverResults = function () {
            return apiAddress + 'driverresult';
        };

    };

    return returnedAddress;
});