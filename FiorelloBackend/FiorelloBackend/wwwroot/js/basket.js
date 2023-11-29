$(document).ready(function () {

    $(document).on("click", "#products .price a", function (e) {

        e.preventDefault();

        let id = parseInt($(this).closest(".product-item").attr("data-productId"));
        let count = $(".count-Basket").text();
        
        $.ajax({
            url: `home/addbasket?id=${id}`,
            type: "Post",
            success: function (res) {
                
                count++;
                
                $(".count-Basket").text(count);
                
            }
        })

          

    })

    $(document).on("click", ".delete-basket-item", function (e) {
        let id = parseInt($(this).attr("data-id"));

        $.ajax({
            url: `cart/delete?id=${id}`,
            type: "Post",
            success: function (res) {
                Swal.fire({
                    text: "Deleted",
                    icon: 'warning',
                }).then((result) => {
                    if (result.isConfirmed) {

                        $(".count-Basket").text(res.count);
                        $(e.target).closest("tr").remove();
                        $(".grand-total h1 span").text(res.grandTotal);

                        if (res.count === 0) {
                            $(".alert-basket-empty").removeClass("d-none");
                            $(".basket-table").addClass("d-none");
                        }

                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }
                })
            }

        })

    })

    $(document).on("click", ".basket-table .fa-plus", function (e) {

        let id = parseInt($(this).attr("data-id"))
        let count = $(".count-Basket").text();
        $.ajax({

            url: `cart/plusicon?id=${id}`,
            type: "Post",
            success: function (res) {

                $(e.target).prev().text(res.countItem)
                $(".grand-total h1 span").text(res.basketGrandTotal);
                $(e.target).parent().next().next().children().text(res.productGrandTotal)
                count++;

                $(".count-Basket").text(count);
            }
        })

    })


    $(document).on("click", ".basket-table .fa-minus", function (e) {

        let id = parseInt($(this).attr("data-id"))
        let count = $(".count-Basket").text();
        let a = 0;
        
        $.ajax({

            url: `cart/minusicon?id=${id}`,
            type: "Post",
            success: function (res) {
                
                $(e.target).next().text(res.countItem)
                $(".grand-total h1 span").text(res.basketGrandTotal);
                $(e.target).parent().next().next().children().text(res.productGrandTotal)
                $(".count-Basket").text(res.countBasket)

                //if (count <= res.countItem) {
                //    count--;
                //    $(".count-Basket").text(count)
                //}

            }
        })

    })


    


})