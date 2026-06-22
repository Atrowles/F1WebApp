define(["Common/ApiConfig"], function (c) {

  
    var config = new c();
    $('#loading').hide();

    //login process
    $("#login").click(function () {
      //  alert(config.getLoginUrl());
       // alert($("#email").val());
       // alert($("#password").val());
        if ($("#email").val() == "" || $("#password").val()=="") {
          //  alert("enter username and password");
            $("#login-error").show();
        }
        else {
            login($("#email").val(), $("#password").val());
        }
    });




    var securityContext = {

        hasTokenExpired: hasTokenExpired,
        logout:logout,
        login: login,
        refresh: refresh,
        getAccessToken: getAccessToken,
        isAuthenticated: isAuthenticated,
        loggedInAs: loggedInAs,
        userRoles: userRoles,
        getAccessToken: getAccessToken
  
    };

    return securityContext;

    

    function login(username, password) {
        $('#loading').show();
       
        var options = {

            url: config.getOAuthUrl(),

            data: 'grant_type=password&username=' + username + '&password=' + password + '&client_id=' + config.getClientId(),

            type: "POST",

            contentType: 'application/x-www-form-urlencoded',

            dataType: 'json'


        };

       // console.log(options);


        return $.ajax(options).then(querySuccess).fail(loginFailed);


        function querySuccess(data) {

          //  console.log(data);

            //data contains the access_token so we need to record this.

            save('access_token', data.access_token);

            save('loggedInAs', data.loggedInAs);

            save('userId', data.user_id);

            save('refresh_token', data.refresh_token);

            save('access_expires', data.expiryDate);

            //save roles here

            //this should be moved elsewhere 
           // var claimsUrl = "https://capitaauth.azurewebsites.net/api/Auth/GetUserClaims";


            var claimsoptions = {

                url: config.getClaimsUrl(),

                type: "Get",

                contentType: 'application/json',

                dataType: 'json',

                beforeSend: function (request) {

                    request.setRequestHeader("Authorization", 'Bearer ' + data.access_token)

                }

            };

           

            return $.ajax(claimsoptions).then(claimSuccess).fail(loginFailed);

            

            function claimSuccess(rolesdata) {
            
             //   console.log("reached");

//                console.log("here");
              //  console.log(rolesdata);

                save('userRoles', rolesdata);

                //save roles here

                save('loggedInAs', rolesdata[0]);
                $('#loading').hide();

                if (toastr)

                    toastr.info('User logged in successfully!');

               // console.log("redirecting to main page")
               location.href = '/';

            };

 



            var returnUrl = QueryString();

            location.href = '/';

            //console.log("redirecting to main page")

        };

    };

    function save(key, value) {
        var lsSupported = false;

        //check for localStorage (HTML5) support
        if (localStorage) {
            lsSupported = true;
        }

        if (value != null) {

            //if the value is an object convert it to json, otherwise just store it.
            if (typeof value === 'object') {
                value = JSON.stringify(value);
            }

            if (lsSupported) {
                localStorage.setItem(key, value);
            }
            else {
                createCookie(key, value, 30);
            }
        }
        else {
            //value is null so we need to remove the value from store if there is one.
            if (lsSupported) {
                localStorage.removeItem(key);
            }
            else {
                createCookie(key, '', -1);
            }
        }

    };

    function refresh() {

        console.log("in refresh for token: " + load('refresh_token'));

        var expiryDate = moment(load('access_expires'), 'DD/MM/YYYY h:mm:ss');
        var dateNow = moment(Date());

     
        console.log(expiryDate);

        if (dateNow.isSameOrAfter(expiryDate)) {

            var options = {

                url: config.getOAuthUrl(),

                data: 'grant_type=refresh_token&refresh_token=' + load('refresh_token') + '&client_id=' + config.getClientId(),

                type: "POST",

                contentType: 'application/x-www-form-urlencoded',

                dataType: 'json',

                cache: false


            };


            return $.ajax(options).then(querySuccess).fail(refreshFailed);


            function querySuccess(data) {
                //console.log("refresh result");
                //console.log(data);

                //data contains the access_token so we need to record this.

                save('access_token', data.access_token);

                save('refresh_token', data.refresh_token);

                save('access_expires', data.expiryDate);

                //save roles here

                var claimsoptions = {

                    url: config.getClaimsUrl(),

                    type: "Get",

                    contentType: 'application/json',

                    dataType: 'json',

                    beforeSend: function (request) {

                        request.setRequestHeader("Authorization", 'Bearer ' + data.access_token)

                    }

                };


                return $.ajax(claimsoptions).then(claimSuccess);



                function claimSuccess(rolesdata) {


                    save('userRoles', rolesdata);

                    // console.log(rolesdata);

                    //save roles here

                    if (toastr)
                        toastr.info('Token refreshed successfully!');

                };




            };

        }


    }

    function logout() {
        save('access_token', null);
        save('loggedInAs', null);
        save('userId', null);
        save('access_expires', null);

        location.href = config.getLoginUrl();;
    }

    function hasTokenExpired() {
        var expiryDate = moment(load('access_expires'), 'DD/MM/YYYY h:mm:ss');
        var dateNow = moment(Date());

        if (dateNow.isSameOrAfter(expiryDate)) {
            logout();
          
        }

    }

    function getAccessToken() {
 

        var token = load('access_token');
        if (token == null) {
            //redirect to login page
            location.href = config.getLoginUrl();
        }
        return token;
    }


    function load(key) {
        var lsSupported = false;
        var data = '';

        //check for localStorage (HTML5) support
        if (localStorage) {
            lsSupported = true;
        }

        if (lsSupported) {
            data = localStorage.getItem(key);
        }
        else {
            data = readCooke(key);
        }

        //try to parse the value in case it's an object, if it fails then it's just a plain value
        try {
            data = JSON.parse(data);
        }
        catch (e) {
            data = data; //not an object, so just return the value
        }

        return data;
    }

    function createCookie(key, value, exp) {
        var date = new Date();
        date.setTime(date.getTime() + (exp * 24 * 60 * 60 * 1000));
        var expires = '; expires=' + date.toGMTString();
        document.cookie = key + '=' + value + expires + ';path=/';
    }

    function readCooke(key) {
        var nameEQ = key + '=';
        var ca = document.cookie.split(';');
        for (var i = 0, max = ca.length; i < max; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    function isAuthenticated() {
        return load('access_token') != null;
    }

    function getUserId() {
        return load('userId');
    }

    function loggedInAs() {
        var name = load('loggedInAs');
        if (name != null) {
            return name;
        }
        else {
            location.href = config.getLoginUrl();
        }
    }

    function userRoles() {
        roles = load('userRoles');
        if (roles == null)
            return [];
        else
            return roles;
    }



    function getAccessToken() {
        var token =  load('access_token');
        if (token == null) {
            //redirect to login page
            location.href = config.getLoginUrl();
        }
        return token;
    }

    function refreshFailed(jqXHR, textStatus) {
        if (jqXHR.status == 400) {
            securityContext.logout();
        }
        else {
            var msg = jqXHR.statusText;
        }
    };




    function loginFailed(jqXHR, textStatus) {
        $('#loading').hide();

        if (typeof (jqXHR.responseJSON) != 'undefined') {
            if (jqXHR.responseJSON.error_description) {
                var msg = 'Error Logging in! ' + jqXHR.responseJSON.error_description;
            }
            else {
                var msg = 'Error Logging in! ' + textStatus;
            }
        }
        else {
            var msg = 'Error Logging in! ' + jqXHR.statusText;
        }

        
        $("#login-error").text(msg);
        $("#login-error").show();
    }



});