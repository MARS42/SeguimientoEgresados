﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function(){

    window.addEventListener('scroll', function() {

        const bannerLogos = document.getElementById('bannerLogos');
        const bannerLogosHeight = bannerLogos.offsetHeight;
        if (window.scrollY > bannerLogosHeight) {
            // add padding top to show content behind navbar
            navbar_height = document.getElementById('globalNavbar').offsetHeight;
            //bannerLogos.style.opacity = "0";
            document.body.style.paddingTop = navbar_height + 'px';

            bannerLogos.classList.remove("animate__fadeInDown");
            bannerLogos.classList.add("animate__fadeOut");
            
            document.getElementById('globalNavbar').classList.add('fixed-top');
        } else {
            // remove padding top from body
            document.body.style.paddingTop = '0';
            //bannerLogos.style.opacity = "1";
            bannerLogos.classList.remove("animate__fadeOut");
            bannerLogos.classList.add("animate__fadeInDown");
            document.getElementById('globalNavbar').classList.remove('fixed-top');
        }
    });
});

function resizeIframe(obj) {
    obj.style.height = obj.contentWindow.document.documentElement.scrollHeight + 'px';
}

function clickActionBtn(btn) {
    console.log("click " + btn)
    //btn.disabled = true;
    btn.style.pointerEvents = "none";
    btn.style.cursor = "default";
    btn.style.opacity = "0.7";
    btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
}