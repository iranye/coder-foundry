
$(document).ready(function () {
    console.log("READY!");

    $(function () {
        console.log("Changing the icon logo!");
        // Change default Swagger icon logo
        $("img .logo__img").attr("src", "~/Content/api-logo.jpg");
    });

});