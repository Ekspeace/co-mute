// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var password = document.getElementById('password');
var cPassword = document.getElementById('confirm-password'); 

cPassword.onkeyup = function () {
    if (password.value != cPassword.value) {
        password.style.borderColor = "Red";
        cPassword.style.borderColor = "Red";
        document.getElementById('sign-in').disabled = true;
    }
    else {
        password.style.borderColor = "Green";
        cPassword.style.borderColor = "Green";
        document.getElementById('sign-in').disabled = false;
    }
}
