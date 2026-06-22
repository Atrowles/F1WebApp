//this depends on the api config so lets bring that in so we can get api urls
define(["js/Common/ApiConfig"], function (c) {

    var config = new c();

    var vm = new Vue({
        el: '#databinding',
        data: {
            num1: 100,
            num2: 200,
            total: '',
            styleobj: {
                width: "100px",
                height: "100px",
                backgroundColor: "red"
            }
        },
        methods: {
            changebgcolor: function () {
                this.styleobj.backgroundColor = "green";
            },
            originalcolor: function () {
                this.styleobj.backgroundColor = "red";
            }
        }
    });
});