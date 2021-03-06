﻿
$(document).ready(function () {
    console.log("READY!");
    $("#logo").html("&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ");
    var baseUrl = getBaseUrl();
    console.log("baseUrl: " + baseUrl);

    var homeUrl = getBaseUrl() + "/Home/Index";
    console.log("homeUrl: " + homeUrl);
    $("#logo").attr("href", homeUrl);

    $("input[name='baseUrl'], input[name='apiKey'] ").css("display", "none");

    $("link[type='image/png']").attr("href", "/favicon.ico");

    $("title").text("Penny Saver - API");

    $("#explore").text("Rehydrate");

    $(function () {
        console.log("Changing the icon logo!");
        // Change default Swagger icon logo
        var $img = $("img .logo__img");
        $img.attr({
            "src": "~/Content/api-logo.jpg"
        });
    });

    function getBaseUrl() {
        return window.location.origin;
    }
});