//let loginHair = document.getElementById("loginHair");
let loginImg = document.getElementById("loginImg");
let loginSection = document.getElementById("loginSection");
let btnLoginSection = document.querySelectorAll(".btnLoginSection");
var forgotContainerStylist = document.getElementById("forgotContainerStylist");
var forgotContainerCustomer = document.getElementById("forgotContainerCustomer");

//signup element access
var signUpSection = document.getElementById("signUpSection");
var btnSignUpStylist = document.getElementById("btnSignUpStylist");
var btnSignUpCustomer = document.getElementById("btnSignUpCustomer");
var signUpCustomer = document.getElementById("signUpCustomer");
var signUpStylist = document.getElementById("signUpStylist");

//event to bring in login page
//loginHair.addEventListener('click', (e) => {
//    //check if signup form is at display place or the barber image
//    if (signUpSection.classList.contains("animateSlideIn")) {
//        signUpSection.classList.add("animateSlideOut");
//    }
//    else {
//        loginImg.classList.add("animateSlideOut");
//    }
//    loginSection.classList.remove("animateSlideOut");
//    loginSection.classList.add("animateSlideIn");
    
    
//});

//set up the login form according to stylist or customerCartContainer
let loginHeading = document.getElementById("loginHeading");
let loginBlockStylist = document.getElementById("loginBlockStylist");
let loginBlockCustomer = document.getElementById("loginBlockCustomer");
btnLoginSection.forEach((btn, index) => {
    btn.addEventListener('click', () => { 
        //check if signup form is at display place or the barber image
        if (signUpSection.classList.contains("animateSlideIn")) {
            signUpSection.classList.add("animateSlideOut");
        }
        else {
            loginImg.classList.add("animateSlideOut");
        }
        forgotContainerStylist.style.display = "none";
        forgotContainerCustomer.style.display = "none";
        loginSection.classList.remove("animateSlideOut");
        loginSection.classList.add("animateSlideIn");

        if (index == 0) {
            loginBlockCustomer.style.display = "none";
            loginBlockStylist.style.display = "block";
                 
        }
        else if (index == 1) {
            loginBlockStylist.style.display = "none";
            loginBlockCustomer.style.display = "block";
        }
    });

})

//event to bring in signup page
//for stylist
btnSignUpStylist.addEventListener('click', (e) => {
    //check if login form is at display place or the barber image
    if (loginSection.classList.contains("animateSlideIn")) {
        loginSection.classList.add("animateSlideOut");
    }
    else {
        loginImg.classList.add("animateSlideOut");
    }
    signUpSection.classList.remove("animateSlideOut");
    signUpSection.classList.add("animateSlideIn");
    signUpCustomer.style.display = "none";
    signUpStylist.style.display = "block";


});

//for customer
btnSignUpCustomer.addEventListener('click', (e) => {
    //check if login form is at display place or the barber image
    if (loginSection.classList.contains("animateSlideIn") ) {
        loginSection.classList.add("animateSlideOut");
    }
    else {
        loginImg.classList.add("animateSlideOut");
    }
    signUpSection.classList.remove("animateSlideOut");
    signUpSection.classList.add("animateSlideIn");
    signUpStylist.style.display = "none";
    signUpCustomer.style.display = "block";
    


});

//function to show and hide password
function showHidePassword(passwordId) {
    let loginPassword = document.getElementById(`${passwordId}`);
    if (loginPassword.type == "password") {
        loginPassword.type = "text";
    } else {
        loginPassword.type = "password";
    }
}

//event listener to bring forgot password page
var forgotPswrdCustomer = document.getElementById("forgotPswrdCustomer");
var forgotPswrdStylist = document.getElementById("forgotPswrdStylist");
forgotPswrdCustomer.addEventListener('click', (e) => {
    loginBlockCustomer.style.display = "none";
    forgotContainerCustomer.style.display = "block";
})
forgotPswrdStylist.addEventListener('click', (e) => {
    loginBlockStylist.style.display = "none";
    forgotContainerStylist.style.display = "block";
})

//receiveOtp.addEventListener('click', (e) => {
//    receiveOtp.style.display = "none";
//    var timeout = setTimeout(() => {
//        resendOtp.setAttribute('style', 'display:block !important');
//        //resendOtp.style.display = "block !important";
//    }, 30000)
//})

//back icon from forgot pswrd to login
var backForgotIconStylist = document.getElementById("backForgotIconStylist");
var backForgotIconCustomer = document.getElementById("backForgotIconCustomer");
backForgotIconStylist.addEventListener('click', (e) => {
    loginBlockStylist.style.display = "block";
    forgotContainerStylist.style.display = "none";
})
backForgotIconCustomer.addEventListener('click', (e) => {
    loginBlockCustomer.style.display = "block";
    forgotContainerCustomer.style.display = "none";
})


//Validate forms

//stylist validation
function validateSignUpStylist() {
    let certificateFile = document.getElementById("certificateFile");
    let criminalCheckFile = document.getElementById("criminalCheckFile");
    let certificateError = document.getElementById("certificateError");
    let criminalCheckError = document.getElementById("criminalCheckError");
    let termsCheckboxStylist = document.getElementById("termsCheckboxStylist");

    ev.preventDefault();
    if (certificateFile.files[0].size > (5000000)) {
        certificateError.innerText = "File exceeds 5 MB";
        return false;
    }
    if (criminalCheckFile.files[0].size > 5000000) {
        criminalCheckError.innerText = "File exceeds 5 MB";
        return false;
    }
    if (!termsCheckboxStylist.checked) {
        return false;
    }
    
}

//convert files to blob urls
let certificateFile = document.getElementById("certificateFile");
let criminalCheckFile = document.getElementById("criminalCheckFile");
let certificateError = document.getElementById("certificateError");

certificateFile.onchange = function (e) {
    var certiFile = certificateFile.files[0];
    var stylistCertificate = document.getElementById("stylistCertificate");
    var reader = new FileReader();
    reader.onload = function () {
      
        //var binaryImg = convertDataURIToBinary(reader.result);
        //var blob = new Blob([binaryImg], { type: certiFile.type });
        stylistCertificate.value = reader.result;
        //console.log(reader.result);
        
       
    };
    reader.readAsDataURL(certiFile);
};

criminalCheckFile.onchange = function (e) {
    //if (criminalCheckFile.files[0].size > 5000000) {
    //    criminalCheckError.innerText = "File exceeds 5 MB";
    //    e.preventDefault();
    //}
    var criminalFile = criminalCheckFile.files[0];

    let stylistCriminalCheck = document.getElementById("stylistCriminalCheck");
    var reader = new FileReader();
    reader.onload = function () {
        stylistCriminalCheck.value = reader.result;
        //console.log(reader.result);
        //var blob = window.dataURLtoBlob(reader.result);
        //console.log(blob, new File([blob], "image.png", {
        //    type: "image/png"
        //}));
    };
    reader.readAsDataURL(criminalFile);
};


//customer validation
function validateSignUpCustomer() {
    let termsCheckboxCustomer = document.getElementById("termsCheckboxCustomer");
    if (termsCheckboxCustomer.checked) {
        return true;
    }
    else {
        return false;
    }
}

//phone number validation for numeric value
let phoneNumber = document.querySelectorAll(".phoneNumber");
phoneNumber.forEach((btn, index) => {
    btn.addEventListener('keydown', (ev) => {
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
})

