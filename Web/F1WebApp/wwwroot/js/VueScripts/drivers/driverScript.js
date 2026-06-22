//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "Common/VueTable"], function (c, v) {

    var config = new c();
    var vueTable = v;

    vueTable.setApiBaseUrl(config.getApiUrlForDriver() + "/GetDrivers");
    console.log(config.getApiUrlForTeam());


    function getAllDrivers() {
        
        vueTable.getData();

    }

    $('#btnRefresh').click(function () {filter
        if ($('#txtSearch').val() != "") {
           
            vueTable.setCurrentPage(1); 
            vueTable.setSearchVal('Name.ToLower().Contains(\"' + $('#txtSearch').val().toLowerCase() + '\")');
            vueTable.getData();
        }
        else {

            vueTable.setSearchVal("");
            vueTable.getData();
        }
    });

    $(document).on('change', '#sel-driver-items-per-page', function () {
        $('#pagination-demo').twbsPagination('destroy');
        vueTable.setCurrentPage(1);
        vueTable.getData();
    });



    getAllDrivers();
});