
let customerProfileContainer = document.getElementById("customerProfileContainer");
let customerProfileDisplay = document.getElementById("customerProfileDisplay");

//function to open side section for profile settings
function openProfileSection() {
    customerProfileContainer.style.display = "block";
}

//function to close side section for profile settings
function closeProfileSection() {
    customerProfileContainer.style.display = "none";
}   


//Button to display container of account Info, Security, Password, 
let btnDisplayContainer = document.querySelectorAll(".btnDisplayContainer");
let customerInfoDisplayLayout = document.querySelectorAll(".customerInfoDisplayLayout");
btnDisplayContainer.forEach((btn, index) => {
    // index will be current button index
    btn.addEventListener("click", function (e) {
        for (let i = 0; i < customerInfoDisplayLayout.length; i++) {
            customerInfoDisplayLayout[i].style.display = "none";
        }
        
         customerInfoDisplayLayout[index].style.display = "block";
         customerInfoDisplayLayout[index].style.left = `${customerProfileDisplay.offsetWidth}px`;
    });
})

//to close display container of account info, security and password
let closecustomerInfoDisplayLayout = document.querySelectorAll(".closecustomerInfoDisplayLayout");
closecustomerInfoDisplayLayout.forEach((btn, index) => {
    // index will be current button index
    btn.addEventListener("click", function (e) {
        customerInfoDisplayLayout[index].style.display = "none";
    });
})

//check if phone number entered is numerical

let telCustomerChange = document.getElementById("telCustomerChange");
telCustomerChange.addEventListener('keydown', (ev) => {
    let key = ev.key;
    let result = false;
    for (let i = 0; i < 10; i++) {
        if (key == `${i}`) {
            result = true;
        }
    }
    if (key == "Backspace") {
        result = true;
    }
    if (result == false) {
        ev.preventDefault();
    }
});

//validate new password and confirm password
function validateConfirmPassword() {
    if (document.getElementById('newPassword').value ==
        document.getElementById('confirmPassword').value) {
        document.getElementById('wrongPasswordMessage').style.color = 'green';
        document.getElementById('wrongPasswordMessage').innerHTML = 'matching';
    } else {
        document.getElementById('wrongPasswordMessage').style.color = 'red';
        document.getElementById('wrongPasswordMessage').innerHTML = 'not matching';
        return false;
    }

}

//add card option for payments
let addCardContainer = document.getElementsByClassName("credit-card")[0];
function openCloseCardSection() {
    if (addCardContainer.style.display == "none" || addCardContainer.style.display == "") {
        addCardContainer.style.display = "block";
    }
    else {
        addCardContainer.style.display = "none";
    }
}

// Cart container display and hide function
let customerCartContainer = document.getElementById("customerCartContainer");

function openCartDisplay() {
    customerCartContainer.style.display = "block";
    //setInterval(() => {
    //    var cartHtml = window.localStorage.getItem('cartHtml');
    //    if (cartHtml !== null && cartHtml !== undefined) {
    //        StylistCartList.innerHTML = decodeURIComponent(cartHtml);
    //    }
    //},1000)
    //var cartHtml = window.localStorage.getItem('cartHtml');
    //if ( cartHtml !== null && cartHtml !== undefined) {
    //    StylistCartList.innerHTML = decodeURIComponent(cartHtml);
    //}
}

//function to close side section for profile settings
function closeCartDisplay() {
    customerCartContainer.style.display = "none";
}   

//add to cart btn
let addToCartBtn = document.querySelectorAll(".addToCartBtn");
let StylistCartList = document.getElementById("StylistCartList");
var cartArray = {};
var servicePriceArray = {};

//change cart number
var orderNumberLabel = document.getElementById("orderNumberLabel");
//window.localStorage.setItem('cart', JSON.stringify(cartArray));
//window.localStorage.setItem('servicePrice', JSON.stringify(servicePriceArray));
var taxes = 0;
var total = 0;
var paymentUrl = "../Paypal/PaymentWithPaypal";
addToCartBtn.forEach((btn, index) => {
    btn.addEventListener('click', function (e) {
        var cartArrayCheck = window.localStorage.getItem('cart');
        var servicePriceArrayCheck = window.localStorage.getItem('servicePrice');
        if (cartArrayCheck !== null && cartArrayCheck !== undefined) {
            cartArray = JSON.parse(cartArrayCheck);

        }
        if (servicePriceArrayCheck !== null && servicePriceArrayCheck !== undefined) {
            servicePriceArray = JSON.parse(servicePriceArrayCheck);
        }


        const myArray = btn.value.split(/,/);
        var stylistID = myArray[1];
        var serviceID = myArray[0];
        var stylistName = myArray[2];
        var serviceName = myArray[3];
        var servicePrice = myArray[4];
        //Making object to store stylistID and serviceID
        
        if (!cartArray[`id${stylistID}`]) {
            cartArray[`id${stylistID}`] = [];
            cartArray[`id${stylistID}`].push(serviceID);
            servicePriceArray[`id${stylistID}`] = servicePrice;
            taxes += parseFloat((0.12 * parseFloat(servicePrice)).toFixed(2));
            var numberOfServices = cartArray[`id${stylistID}`].length;
            //add new stylist in cart
            //<div style="font-size: 0.9rem;">Address: $<span>@Session["Address"].ToString()</span></div>
            StylistCartList.innerHTML += `<li id="list${stylistID}" style="display: flex; flex-direction: column;border-bottom: 1px solid #eeeeee; padding: 8px 16px;">
                            <div style="margin: 10px 0px;">
                                <h4 style="font-weight: bold;">
                                   ${stylistName}
                                </h4>
                                 
                            </div>
                            <div style="border-bottom: 1px solid #eeeeee;">
                                <div class="noOfServices" style="font-size: 0.9rem;">
                                    <span id="noOfService${stylistID}">${numberOfServices}</span>&nbsp;item
                                </div>
                            </div>
                            <div class="d-flex flex-column" id="ServiceList${stylistID}">
                                <div style="margin: 10px 0px;display: flex;justify-content: space-between;align-items: center;" class="servicesName">
                                    <div style="font-size: 1.1rem;">
                                        ${serviceName}
                                    </div>
                                    <button id="id${stylistID},${serviceID},${servicePrice}" class="removeServiceBtn">
                                       
                                    </button>
                                </div>
                            </div>

                            <div class="d-flex justify-content-between" style="padding: 10px 0px;padding-bottom: 0px; ">
                                <h6>Subtotal</h6>
                                <h6>$<span id="subTotal${stylistID}">${servicePriceArray[`id${stylistID}`]}</span></h6>
                            </div>
                            <div class="d-flex justify-content-between">
                                <h6>Taxes</h6>
                                <h6>$<span id="tax${stylistID}">${taxes}</span></h6>
                            </div><div class="d-flex justify-content-between" style="padding: 10px 0px;padding-top: 0px; ">
                                <h5>Total</h5>
                                <h5>$<span id="total${stylistID}">${parseFloat(servicePrice)+taxes}</span></h5>
                            </div>
                            <div>
                           <form action="${paymentUrl}" method="post">
                            <input type="text" name="serviceId" id="inputId${stylistID}" value="${serviceID}" hidden />
                            <input type="text" name="totalPrice" id="inputPrice${stylistID}" value="${parseFloat(servicePrice) + taxes}" hidden />
                            <input type="text" name="stylistId" id="stylist${stylistID}" value="${stylistID}" hidden />
                            <button type="submit" class="placeOrderBtn">Place Order</button>
                            </form>
                            </div>
                        </li>`;
            
            window.localStorage.setItem('cart', JSON.stringify(cartArray));
            //orderNumberLabel.innerText = `${Object.keys(cartArray).length}`;
            window.localStorage.setItem('servicePrice', JSON.stringify(servicePriceArray));
            window.localStorage.setItem('cartHtml', encodeURIComponent(StylistCartList.innerHTML));
            //<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 448 512"><path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" /></svg>
           

        } else {
            // If the key exists, push the value into the existing 
            cartArray = JSON.parse(window.localStorage.getItem('cart'));
            servicePriceArray = JSON.parse(window.localStorage.getItem('servicePrice'));
            StylistCartList.innerHTML = decodeURIComponent(window.localStorage.getItem('cartHtml'));

            if (cartArray[`id${stylistID}`].indexOf(`${serviceID}`) == -1) {
                cartArray[`id${stylistID}`].push(serviceID);
                servicePriceArray[`id${stylistID}`] = parseFloat(servicePriceArray[`id${stylistID}`]) + parseFloat(servicePrice);
                document.getElementById(`subTotal${stylistID}`).innerText = servicePriceArray[`id${stylistID}`];
                numberOfServices = cartArray[`id${stylistID}`].length;

                var updateNumberOfServices = document.getElementById(`noOfService${stylistID}`);
                updateNumberOfServices.innerText = `${numberOfServices}`;

                //update taxes
                document.getElementById(`tax${stylistID}`).innerText = (parseFloat(servicePriceArray[`id${stylistID}`]) * 0.12).toFixed(2);
                //update total
                document.getElementById(`total${stylistID}`).innerText = parseFloat((parseFloat(servicePriceArray[`id${stylistID}`]) * 0.12).toFixed(2)) + (parseFloat(servicePriceArray[`id${stylistID}`]));

                //add service id in input field
                document.getElementById(`inputId${stylistID}`).value = document.getElementById(`inputId${stylistID}`).value.concat(",",`${serviceID}`);
                
                //add total cost in input field
                document.getElementById(`inputPrice${stylistID}`).value = document.getElementById(`total${stylistID}`).innerText;

                var serviceList = document.getElementById(`ServiceList${stylistID}`);
                serviceList.innerHTML += `<div style="margin: 10px 0px;display: flex;justify-content: space-between;" class="servicesName">
                                    <div style="font-size: 1.1rem;">
                                        ${serviceName}
                                    </div>
                                    <button id="id${stylistID},${serviceID},${servicePrice}" class="removeServiceBtn">
                                         
                                    </button>
                                </div>`;
            }
            window.localStorage.removeItem('cartHtml');
            //<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 448 512"><path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" /></svg>
            window.localStorage.setItem('cartHtml', encodeURIComponent(StylistCartList.innerHTML));
            window.localStorage.setItem('cart', JSON.stringify(cartArray));
            //orderNumberLabel.innerText = `${Object.keys(cartArray).length}`;
            window.localStorage.setItem('servicePrice', JSON.stringify(servicePriceArray));
           
        }
        document.getElementById(`id${stylistID},${serviceID},${servicePrice}`).classList.add("removeServiceBtn");
        //give values to input fields
        orderNumberLabel.innerText = `${Object.keys(cartArray).length}`;
       
    })
});




window.addEventListener('load', function (event) {
    // Perform actions here
    // For example, you can call a function to handle page initialization
    let cartArray = JSON.parse(window.localStorage.getItem('cart'));
    orderNumberLabel.innerText = `${Object.keys(cartArray).length}`;
    var cartHtml = window.localStorage.getItem('cartHtml');
    if (cartHtml !== null && cartHtml !== undefined) {
        StylistCartList.innerHTML = decodeURIComponent(cartHtml);

        
    }
});


document.addEventListener('click', function (event) {
    // Check if the clicked element is the button you're interested in
    //console.log(event, event.target, event.target.classList.contains('removeServiceBtn'));
    if (event.target.classList.contains('removeServiceBtn')) {
        // Your button logic here
      
        var removeServiceBtn = document.querySelectorAll(".removeServiceBtn");
        let index = Array.from(removeServiceBtn).indexOf(event.target);
        //var servicesName = document.querySelectorAll(".servicesName");
        //removeServiceBtn.forEach((element, index) => {
        //    element.addEventListener('click', function (e) {
               
                cartArray = JSON.parse(localStorage.getItem('cart'));
                servicePriceArray = JSON.parse(localStorage.getItem('servicePrice'));

        //remove that service id from array
                    let elementId = event.target.id.split(/,/);
                //var elementId = element.id.split(/,/);
               
                const indexToRemove = cartArray[`${elementId[0]}`].indexOf(`${elementId[1]}`);
                let getStylistId = elementId[0].replace("id", "");
                if (indexToRemove !== -1) {
                  
                    if (cartArray[`${elementId[0]}`].length == 1) {
                        cartArray[`${elementId[0]}`] = null;
                        document.getElementById(`list${getStylistId}`).remove();
                       
                    }
                    else {
                        cartArray[`${elementId[0]}`].splice(indexToRemove, 1);
                       
                        //update number of services list
                        var updateItemNumber = document.getElementById(`noOfService${getStylistId}`);
                        let getUpdatedNumber = cartArray[`${elementId[0]}`].length;
                        updateItemNumber.innerText = `${getUpdatedNumber}`;
                        var subtractedNumber = parseFloat(servicePriceArray[`id${getStylistId}`]) - parseFloat(elementId[2])
                        servicePriceArray[`id${getStylistId}`] = parseFloat(servicePriceArray[`id${getStylistId}`]) - parseFloat(elementId[2]);

                        //update taxes and total when service removed
                        //document.getElementById(`subTotal${getStylistId}`).innerText = servicePriceArray[`id${getStylistId}`];
                        document.getElementById(`subTotal${getStylistId}`).innerText = subtractedNumber;

                        document.getElementById(`tax${getStylistId}`).innerText = parseFloat((parseFloat(servicePriceArray[`id${getStylistId}`]) * 0.12).toFixed(2));
                        document.getElementById(`total${getStylistId}`).innerText = parseFloat(servicePriceArray[`id${getStylistId}`]) + parseFloat(document.getElementById(`tax${getStylistId}`).innerText);

                        //remove service from input field
                        //console.log(elementId[1]);
                        
                        let pattern = new RegExp(`(^|,)${elementId[1]}(,|$)`, "g");
                        document.getElementById(`inputId${getStylistId}`).value = document.getElementById(`inputId${getStylistId}`).value.replace(pattern, "");
   
                        //update total in input field too
                        document.getElementById(`inputPrice${getStylistId}`).value = document.getElementById(`total${getStylistId}`).innerText;

                      
                        document.querySelectorAll(".servicesName")[index].remove();
                    }
                }
                //servicesName[index].remove();
                
                window.localStorage.removeItem('cartHtml');
        window.localStorage.setItem('cartHtml', encodeURIComponent(StylistCartList.innerHTML));
        orderNumberLabel.innerText = `${Object.keys(cartArray).length}`;
                window.localStorage.setItem('cart', JSON.stringify(cartArray));
                window.localStorage.setItem('servicePrice', JSON.stringify(servicePriceArray));
            //});

        //});
    }
});


//add new service
var addServiceDetailsContainer = document.getElementById("addServiceDetailsContainer");
var btnAddNewService = document.getElementById("btnAddNewService");
var btnNewServiceSubmit = document.getElementById("btnNewServiceSubmit");
btnAddNewService.addEventListener(('click'), () => {
    addServiceDetailsContainer.style.display = "block";
    btnAddNewService.style.display = "none";
});

function bringAddServiceBtn() {
    addServiceDetailsContainer.style.display = "none";
    btnAddNewService.style.display = "block";
};


//convert bg image to base 64;
var changeBgImage = document.getElementById("changeBgImage");
changeBgImage.onchange = function (e) {
    var certiFile = changeBgImage.files[0];
    var bgImageStore = document.getElementById("bgImageStore");
    var reader = new FileReader();
    reader.onload = function () {

        //var binaryImg = convertDataURIToBinary(reader.result);
        //var blob = new Blob([binaryImg], { type: certiFile.type });
        bgImageStore.value = reader.result;
        //console.log(reader.result);

    };
    reader.readAsDataURL(certiFile);
};

//validate bg size before submit
var bgSizeError = document.getElementById("bgSizeError");
function validateBgImageSize() {
    ev.preventDefault();
    if (changeBgImage.files[0].size > (5000000)) {
        bgSizeError.innerText = "File exceeds 5 MB";
        return false;
    }
}

//activate online offline mode for stylist
var flexSwitchCheckDefault = document.getElementById("flexSwitchCheckDefault");
var stylistSwitchForm = document.getElementById("stylistSwitchForm");
var switchStylistValue = document.getElementById("switchStylistValue");
flexSwitchCheckDefault.onchange = function (e) {
    if (flexSwitchCheckDefault.checked) {
        switchStylistValue.value = "1";
        stylistSwitchForm.submit();
    }
    else {
        switchStylistValue.value = "0";
        stylistSwitchForm.submit();
    }
}

