
$(document).ready(function () {
    console.log("READY!");
    $("#logo").html("&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ");
    $("#logo").attr("href", "#");

    $("input[name='baseUrl'], input[name='apiKey'] ").css("display", "none");

    $("link[type='image/png']").attr("href", "/favicon.ico");

    $("title").text("Penny Saver - API");

    $("#explore").text("Rehydrate");

    $(function () {
        console.log("Changing the icon logo!");
        // Change default Swagger icon logo
        $("img .logo__img").attr({
            "src": "~/Content/api-logo.jpg",
            "height": "20px"
        });
    });

});