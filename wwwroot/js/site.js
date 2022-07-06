async function chooseShop(sender) {
    document.getElementsByName("shopBtn").forEach(btn => btn.classList.remove("active"));
    sender.classList.add("active");
    let dishes;
    let resp = await fetch(sender.dataset.url);
    if (resp.ok) {
        dishes = await resp.json();
        let container = document.getElementById("dishContainer");
        container.innerHTML = "";
        for (let dish of dishes) {
            let el = document.createElement("div");
            el.classList.add("dish-card");

            let img = document.createElement("img");
            img.src = sender.dataset.imgUrl + "?imgName=" + dish.imgName;
            el.appendChild(img);

            let h6 = document.createElement("h6");
            h6.textContent = dish.name;
            el.appendChild(h6);

            let p = document.createElement("p");
            p.classList.add("text-muted");
            p.textContent = dish.description;
            el.appendChild(p);

            let line = document.createElement("div");
            line.classList.add("line");

            let price = document.createElement("span");
            price.classList.add("display-5");
            price.textContent = dish.price.toFixed(2) + "$";
            line.appendChild(price);

            let btn = document.createElement("button");
            btn.type = "button";
            btn.name = "dishBtn"
            btn.classList.add("btn");
            btn.classList.add("btn-outline-primary");
            btn.textContent = "Add to cart";
            btn.onclick = () => addToCart(btn);
            btn.dataset.id = dish.id;
            btn.dataset.shopId = dish.shopId;
            line.appendChild(btn);

            el.appendChild(line);

            let col = document.createElement("div");
            col.classList.add("col-4");
            col.classList.add("mb-2");
            col.appendChild(el);

            container.appendChild(col);
        }
        if (dishes.length == 0) {
            container.innerHTML = "<p class='text-center w-100 display-5'>Nothing found</p>";
        }
        updateBtns();
    } else {
        alert("Error");
        return;
    }
}

function addToCart(btn) {
    localStorage.setItem(btn.dataset.id, 1);
    if (localStorage.getItem(btn.dataset.shopId) === null)
        localStorage.setItem("shop", btn.dataset.shopId);
    updateBtns();
}

function removeFromCart(btn) {
    localStorage.removeItem(btn.dataset.id);
    if (localStorage.length == 1 && localStorage.getItem("shop") !== null) {
        localStorage.removeItem("shop");
    }
    updateBtns();
}

function updateBtns() {
    for (let btn of document.getElementsByName("dishBtn")) {
        if (localStorage.getItem(btn.dataset.id) !== null) {
            btn.textContent = "Remove";
            btn.onclick = () => removeFromCart(btn);
        } else {
            btn.textContent = "Add to cart";
            btn.onclick = () => addToCart(btn);
        }
    }
    if (localStorage.getItem("shop") === null) {
        document.getElementsByName("shopBtn").forEach(btn => btn.disabled = false);
    } else {
        for (let btn of document.getElementsByName("shopBtn")) {
            if (btn.dataset.id != localStorage.getItem("shop"))
                btn.disabled = true;
        }
    }
}

async function createOrderedDishCards() {
    let container = document.getElementById("dishContainer");
    if(localStorage.length>0)container.innerHTML = "";
    window.dishCards = [];
    for (let key of Object.keys(localStorage)) {
        if (key === "shop") continue;
        let card = new OrderedDishCard(key, localStorage.getItem(key), container);
        await card.create();
        card.onChangeQuantity = () => {
            localStorage.setItem(key, card.quantity);
            calculateTotalPrice();
        };
        window.dishCards.push(card);
    }
}
function calculateTotalPrice(){
    let totalPrice = window.dishCards.reduce((prev, curr)=>prev+curr.price*curr.quantity, 0);
    document.getElementById("totalPrice").textContent=totalPrice.toFixed(2);
}

class OrderedDishCard {
    constructor(dishId, quantity, container) {
        this.dishId = dishId;
        this.price = null;
        this.quantity = quantity;
        this.onChangeQuantity=(val)=>{};
        this.container = container;
    }
    async create() {
        let resp = await fetch(`/ShoppingCart/GetDish?dishId=${this.dishId}`);
        if (resp.ok) {
            let dish = await resp.json();
            this.price = dish.price;

            let row = document.createElement("div");
            row.classList.add("row");
            row.classList.add("ordered-dish-card");

            let img = document.createElement("img")
            img.classList.add("col-7");
            img.src="/Shop/Photo?imgName="+dish.imgName;
            row.appendChild(img);

            let col = document.createElement("div");
            col.classList.add("col");
            row.appendChild(col);

            let h6 = document.createElement("h6");
            h6.textContent = dish.name;
            col.appendChild(h6);

            let price = document.createElement("p");
            price.textContent = "Price: "+dish.price.toFixed(2)+"$";
            col.appendChild(price);

            let quantity = document.createElement("input");
            quantity.type = "number";
            quantity.value = this.quantity;
            quantity.min = 1;
            quantity.addEventListener("change",()=>this._changeQuantity(quantity));
            quantity.classList.add("form-control");
            col.appendChild(quantity);

            this.container.appendChild(row);
        } else {
            alert("Error");
        }
    }
    _changeQuantity(el){
        this.quantity = el.value;
        this.onChangeQuantity(this.quantity);
    }
    total(){
        return this.price*this.quantity;
    }
}

async function order(e){
    e.preventDefault();
    let formData = new FormData(document.getElementById("orderForm"));
    let i=0;
    for(let dish of window.dishCards){
        formData.append(`OrderedDishes[${i}].Key`, dish.dishId.toString());
        formData.append(`OrderedDishes[${i}].Value`, dish.quantity.toString());
        i++;
    }
    let resp = await fetch("/ShoppingCart/Order",{
        method: "post",
        body: formData
    });
    if(resp.ok){
        localStorage.clear();
        location = "/";
    }else{
        alert("Error");
    }
}