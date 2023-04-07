var requestManager = {

    ExecuteGet: {},
    ExecutePost: {},

    ExecuteGet: function (url, onSuccess) {
        $.ajax({
            type: "GET",
            url: url,
            success: onSuccess,
            error: function () {
                //alert("Lamentamos, aplicação falhou");
                spinner.RemoveSpinner();
            }
        });
    },

    ExecutePost: function (url, data, onSuccess) {
        $.ajax({
            type: "POST",
            url: url,
            data: {
                __RequestVerificationToken: GetAntiForgeryToken(),
                data
            },
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            success: onSuccess,
            error: function (e) {
                //alert("Lamentamos, aplicação falhou");
                spinner.RemoveSpinner();
            }
        });
    }
}