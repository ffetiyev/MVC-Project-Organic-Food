
document.querySelector(".related-products-categories").firstElementChild.classList.add("active");
document.querySelector(".tab-content").firstElementChild.classList.add("active");

//add product to basket

let addBasketBtn = document.querySelectorAll(".add-basket");

addBasketBtn.forEach((btn) => {
    btn.addEventListener("click", function () {

        let productId = this.getAttribute("data-id");
        fetch('http://localhost:5219/Home/AddProductToBasket?id=' + productId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.text()).then(res => {
            document.querySelector(".basket-count-show").inneText = res;
        });

    })
});


let deleteFromBasketBtns = document.querySelectorAll(".delete-from-basket");

deleteFromBasketBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
        alert("sdv")
        let productId = this.getAttribute("data-id");
        fetch('http://localhost:5219/Cart/Delete?id=' + productId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.text()).then(res => {

        });

    })
});
