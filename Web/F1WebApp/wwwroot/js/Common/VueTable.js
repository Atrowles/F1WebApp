//this depends on the api config so lets bring that in so we can get api urls
define(["Common/ApiConfig", "common/models", "common/SecurityContext"], function (c, model, auth) {

  

    var config = new c();
    //better way to do it is below so follow methid of how security contect has been set up and login function is returned 
    //to access var in security context it must be returned from with in that files module
    //the reason I have to instatioate it is beacuse the are not return individually like they should be
    //so bottom line is do it the same way as security context

    // var username = 'andrew.trowles3@capita.co.uk';
    // var password = 'Jul2019!';
    //to update password use forgottern pasword link from datawarehouse ui

    //when first logging in uncomment the login call
    // auth.login('andrew.trowles3@capita.co.uk', 'Apr2020!');
    //new
    // auth.login('andrew.trowles3@capita.co.uk', 'Jul2020!');
    //s.refresh();
    console.log("logged in as: " + auth.loggedInAs());
    console.log("Roles: " + auth.userRoles());
    console.log("isAuthenticated: " + auth.isAuthenticated());
    console.log("Token : " + auth.getAccessToken());




    //config can be removed from here we dont need it

 


//    $("p").append("this proves jquery is working");

    var vTable = new Vue({
        el: '#vueTable',
        data: {
            showLoading: false,
            showGrid: false,
            isShownFirst: false,
            isShownLast: true,
            pages: new Array(),
            pageButtonsCount: 10,
            sortVal: "",
            searchVal: "",
            apiBaseUrl: "",
            currentPage: 1,
            totalRows: 0,
            elementsPerPage: 5,
            pageItems: {
                1: { id: 1, val: 5 },
                2: { id: 2, val: 10 },
                3: { id: 3, val: 25 },
            },
            ascending: false,
            sortColumn: '',
            rows: []
        },
        methods: {
            "deleteTeam": function deleteTeam(row) {
                this.showLoading = true;
                $.ajax({
                    url: config.getApiUrlForTeam() + '/' + row.id,
                    type: "Delete",
                })
                    .done(function (data) {
                        toastr.success('Deleted Team', 'Success');
                        vTable.getData(1);

                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        vTable.showLoading = false;
                    });
            },
            "deleteDriver": function deleteDriver(row) {
                this.showLoading = true;
                $.ajax({
                    url: config.getApiUrlForDriver() + '/' + row.id,
                    type: "Delete",
                })
                    .done(function (data) {
                        toastr.success('Deleted Driver', 'Success');
                        vTable.getData(1);

                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        vTable.showLoading = false;
                    });
            },
            "deleteTrack": function deleteTrack(row) {
                this.showLoading = true;
                $.ajax({
                    url: config.getApiUrlForTrack() + '/' + row.id,
                    type: "Delete",
                })
                    .done(function (data) {
                        toastr.success('Deleted Track', 'Success');
                        vTable.getData(1);

                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        vTable.showLoading = false;
                    });
            },
            "updateRow": function updateRow(row) {

                window.location = document.location.href + '/edit/' + row.id;

            },

            "prev_page": function prev_page() {
                if (this.currentPage != 1) this.currentPage = this.currentPage - 1;
                this.isShownFirst = (this.currentPage == 1 ? false : true);
                this.isShownLast = (this.currentPage == this.num_pages() ? false : true);
            },
            "next_page": function next_page() {
                if (this.currentPage != this.max_page) this.currentPage = this.currentPage + 1;
                this.isShownFirst = (this.currentPage == 1 ? false : true);
                this.isShownLast = (this.currentPage == this.num_pages() ? false : true);
            },
            "setCurrentPage": function setCurrentPage(page) {
                this.currentPage = page;

            },
            "setSearchVal": function setSearchVal(searchStr) {
                this.searchVal = searchStr;

            },
            "setSortVal": function setSortVal(sortStr) {
                this.sortVal = sortStr;

            },
            "setApiBaseUrl": function setApiBaseUrl(path) {
                this.apiBaseUrl = path;
                // alert(this.apiBaseUrl);
            },
            "updateTable": function updateTable(data) {
                //alert("updatetatb");                
                this.rows = data;
                this.totalItems = data.length;
            },
            "getData": function getData(page="") {
                if (page != "") this.currentPage = page;

                model.PageNo = this.currentPage;
                model.ItemsPerPage = this.elementsPerPage;
                
                if (this.searchVal != "") model.SearchStr = this.searchVal;
                if (this.SortStr != "") model.SortStr=this.sortVal;

               // dataUrl = this.apiBaseUrl + "?page=" + this.currentPage + "&itemsPerPage=" + this.elementsPerPage;
                //if (this.sortVal != "") dataUrl += " &sortString=" + this.sortVal;

                //console.log(model);

                this.showLoading = true;
                //before each call check access token is not null
                // if its null redirect to login page
                //refresh the token in the done bit
                $.ajax({
                    url: this.apiBaseUrl,
                    type: "POST",
                    beforeSend: function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + auth.getAccessToken());
                    },
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(model),
                })
                    .done(function (data) {

                        //refresh token
                        auth.refresh();
                        console.log(data);
                        toastr.success(data.totalRecords + ' row(s) returned', 'Success');
                        //  console.log(data.result);
                        vTable.totalRows = data.totalRecords;
                        // this.rows = data.result;
                        //console.log(this.currentPage);
                        // console.log(this.pagination_range);

                        //would need to check exists and destroy it

                        $('#pagination-demo').twbsPagination({
                            totalPages: vTable.num_pages(),
                            visiblePages: 6,
                            next: 'Next',
                            prev: 'Prev',
                            onPageClick: function (event, page) {
                                //fetch content and render here
                                vTable.getData(page);
                            }
                        });

                        if (data.result.length > 0) {
                            vTable.rows = data.result;
                            vTable.showGrid = true;
                            vTable.isShownFirst = (vTable.currentPage == 1 ? false : true);
                            vTable.isShownLast = (vTable.currentPage == vTable.num_pages() ? false : true);

                        }
                        else {
                            vTable.rows = [];
                            vTable.showGrid = false;
                        }
                        
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        toastr.error(textStatus, errorThrown);
                    })
                    .always(function () {
                        // alert(this.showLoading);
                        vTable.showLoading = false;
                    });

             
            },
            "sortTable": function sortTable(col) {
                if (this.sortColumn === col) {
                    this.ascending = !this.ascending;
                } else {
                    this.ascending = true;
                    this.sortColumn = col;
                }

                var ascending = this.ascending;
                this.currentPage = 1;
                this.setSortVal(col + " " + (ascending ? "ASC" : "DESC"));
                this.getData();

               // this.rows.sort(function (a, b) {
               //     if (a[col] > b[col]) {
               //         return ascending ? 1 : -1
               //     } else if (a[col] < b[col]) {
               //         return ascending ? -1 : 1
               //     }
               //     return 0;
                //})
            },
            "num_pages": function num_pages() {
                return Math.ceil(this.totalRows / this.elementsPerPage);
                
            },
            "max_page": function max_page() { //number of pages to show on pagination
                
                this.currentPage = this.num_pages();
                this.isShownLast = false;
                this.isShownFirst = true;
  
            },
            "get_rows": function get_rows() {               
                return this.rows;
            },
            "change_page": function change_page(page) {  
                console.log("chage page to");
                console.log(page);
                if (page == "...") return; //middle button clicked do nothing
                //check the page is a numeric value i.e. its not the ... button in between
                this.currentPage = page;
                this.getData();
               
                //alert(page == this.num_pages() ? false : true);
                //alert(this.isShownFirst);
                
               // this.isHiddenFirst = true;
               // console.log(page);
               // console.log(this.num_pages());
            }
        },
        computed: {

            "pagination_range": function pagination_range() {

                this.pages = [];

               

                if (this.currentPage >= 3) {
                    for (i = 1; i < 3; i++) {
                        this.pages.push(i);
                    }
                    //push the middle button
                    this.pages.push("...");

                }

                console.log("setting pages");
                console.log(this.pages);
                //if(this.currentPage>2) //  add page 1 and 2 and ... and do another for loop adding in pageButtons Count-2
                //else this.currentPage<3 do the stuff underneath and just count from 1 to pageButtonsCount

                //also check if currentPage + number pageCount button > max number of pages 
                //if it does take the amount its over by of the current page number and do the foor loop like that

                for (i = (this.currentPage == 1 ? 1 : this.currentPage - 1); i < (this.currentPage + this.pageButtonsCount); i++) {
                    this.pages.push(i);
                    //need to check when it hits the total pages
                    if(i==this.num_pages()) break;;
                    if ((this.currentPage == 1 && i == this.num_pages()) || (this.currentPage == 2 && i == (this.num_pages() - 1)) && (this.currentPage != this.num_pages() - 1) && this.currentPage != this.num_pages()) break;         
                }

                return  this.pages;
            },
            "columns": function columns() {
                if (this.rows.length == 0) {
                    return [];
                }
                return Object.keys(this.rows[0]);//this will return the column names from the data above
            }
        }
    });


    return vTable;
});

