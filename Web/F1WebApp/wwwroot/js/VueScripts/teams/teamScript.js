//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "Common/VueTable"], function (c, v) {

    var config = new c();
    var vueTable = v;


    console.log(config.getApiUrlForTeam());
    vueTable.setApiBaseUrl(config.getApiUrlForTeam() + "/GetTeams");

    function getAllTeams() {
        
        vueTable.getData();

    }

    $('#btnRefresh').click(function () {
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

    $(document).on('change', '#sel-team-items-per-page', function () {
        $('#pagination-demo').twbsPagination('destroy');
        vueTable.setCurrentPage(1);
        vueTable.getData();
    });



    getAllTeams();
});