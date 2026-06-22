//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "Common/VueTable"], function (c, v) {


    var config = new c();
    var vueTable = v;



    //on each call to an api if it fails request a refreshtoken
    // if that fails log the user out


    console.log(config.getApiUrlForTrack());
    vueTable.setApiBaseUrl(config.getApiUrlForTrack() + "/GetTracks");

    function getAllTracks() {

        vueTable.getData();


    }

    $('#btnRefresh').click(function () {
        if ($('#txtSearch').val() != "") {

            vueTable.setCurrentPage(1);
            vueTable.setSearchVal('Location.ToLower().Contains(\"' + $('#txtSearch').val().toLowerCase() + '\")');
            vueTable.getData();
        }
        else {

            vueTable.setSearchVal("");
            vueTable.getData();
        }
    });


    $(document).on('change', '#sel-track-items-per-page', function () {
        $('#pagination-demo').twbsPagination('destroy');
        vueTable.setCurrentPage(1);
        vueTable.getData();
    });


    getAllTracks();




  //for tbws call change_page(value) where value is page

});