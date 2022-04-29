// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const timer = ms => new Promise(res => setTimeout(res, ms));

var x = window.matchMedia("(max-width: 765px)")
x.addEventListener('change', ajustarImagenInicio());

containersHeaderSize();
ResizeMainContainers(headerHeight())

document.addEventListener("DOMContentLoaded", function(){
    
    window.addEventListener('scroll', function() {

        const bannerLogos = document.getElementById('bannerLogos');
        if(bannerLogos == null)
            return;
        
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

async function clickActionBtn(btn) {

    await timer(300);
    
    const errors1 = document.getElementsByClassName("input-validation-error");
    const errors2 = document.getElementsByClassName("field-validation-error");
    if(errors1.length > 0 || errors2.length > 0)
        return;
    
    //console.log("click " + btn)
    //btn.disabled = true;
    
    btn.style.pointerEvents = "none";
    btn.style.cursor = "default";
    btn.style.opacity = "0.7";
    btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
}

function headerHeight(){
    return document.getElementById("header").offsetHeight;
}

function containersHeaderSize(){
    const fullHeaderHeight = headerHeight();
    
    window.addEventListener('resize', () => ResizeMainContainers(fullHeaderHeight));
}

function ResizeMainContainers(h){
    const containers = document.getElementsByClassName("main-container");
    for(let i = 0; i < containers.length; i++){
        const c = containers[i];
        console.log(window.innerHeight - h);
        c.style.height = `${ window.innerHeight - h}px`;
    }
}

function ajustarImagenInicio(){
    return;
    const imag = document.getElementById("img-inicio");
    if(imag === null)
        return;
    console.log("imag",imag.clientHeight);
    document.getElementsByClassName("cont-img")[0].style.height = `${imag.clientHeight}px`;
}