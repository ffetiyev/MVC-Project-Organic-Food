
let relatedCategories = document.querySelector(".related-products-categories");
if (relatedCategories && relatedCategories.firstElementChild) {
    relatedCategories.firstElementChild.classList.add("active");
}


let tabContent = document.querySelector(".tab-content");
if (tabContent && tabContent.firstElementChild) {
    tabContent.firstElementChild.classList.add("active");
}
//add product to basket

let addBasketBtn = document.querySelectorAll(".add-basket");

addBasketBtn.forEach((btn) => {
    btn.addEventListener("click", function () {

        let productId = this.getAttribute("data-id");
        fetch('http://localhost:62859/Home/AddProductToBasket?id=' + productId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.text()).then(res => {
            document.querySelector(".basket-count-show").innerText = res;

        });

    })
});


let deleteFromBasketBtns = document.querySelectorAll(".delete-basket");
 
deleteFromBasketBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
        let productId = this.parentNode.getAttribute("data-id");
        fetch('http://localhost:62859/Cart/Delete?id=' + productId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.text()).then(res => {

        });

    })
})
