// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.addEventListener("scroll", function () {
    var header = this.document.querySelector("header");
    header.classList.toggle("stickly", this.window.scrollY > 0);
})