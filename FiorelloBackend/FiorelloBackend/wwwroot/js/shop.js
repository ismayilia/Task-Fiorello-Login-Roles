$(document).ready(function () {

    $(document).on("click", ".show-more", function () {
        let parent = $(".parent-elem");
        let skipCount = $(parent).children().length;
        let producsCount = $(parent).attr("data-count")

        $.ajax({
            url: `shop/loadmore?skipCount=${skipCount}`,
            type: "Get",
            success: function (res) {
                $(parent).append(res);
                skipCount = $(parent).children().length;

                if (skipCount>= producsCount) {
                    $(".show-more button").addClass("d-none")
                    $(".show-less button").removeClass("d-none")
                }
            }
        })
    })

    $(document).on("click", ".show-less", function () {
        let parent = $(".parent-elem");
        
        

        $.ajax({
            url: "shop/loadless",
            type: "Get",
            success: function (res) {
                $(parent).html(" ")
                $(parent).append(res);

                $(".show-more button").removeClass("d-none")
                $(".show-less button").addClass("d-none")
            }
        })
    })




})