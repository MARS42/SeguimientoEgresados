const steps = document.getElementsByClassName("form-step");
let currentStep = 0;
let lastStep = 0;

document.querySelector('form').addEventListener('submit', evt => {
    fixHeight();
});

var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
    return new bootstrap.Popover(popoverTriggerEl)
})

const cEmail = document.getElementById("cEmail");
cEmail.addEventListener('input', evt => verificarEmail(evt.target));
cEmail.addEventListener('change', evt => verificarEmailCompleto(evt.target));

const popoverEmail = new bootstrap.Popover(cEmail, {
    html: true,
    content: '<p class="text-danger m-0 p-0"><i class="fa-solid fa-circle-exclamation me-2"></i>Email no disponile</p>',
    trigger: 'manual'
});

for (let i = 0; i < steps.length; i++) {
    const step = steps[i];
    step.addEventListener("animationend", () => {
        if(step.classList.contains("animate__fadeOutRight") || step.classList.contains("animate__fadeOutLeft"))
            step.classList.add('invisible')
    });
}

const stepBtns = document.getElementsByClassName("stepBtn");

if(stepBtns.length > 0){

    console.log(stepBtns);

    for (let i = 0; i < stepBtns.length; i++) {
        const stepBtn = stepBtns[i];
        stepBtn.addEventListener("click", (event) => {
            if(stepBtn.dataset.stepmode === "prev")
                ActivateStep(parseInt(stepBtn.dataset.step) - 1);
            else
                ActivateStep(parseInt(stepBtn.dataset.step) + 1);
        });
    }
}

ActivateStep(0);

function ActivateStep(index) {
    if(index < 0 || index >= steps.length)
        return;

    for (let i = 0; i < steps.length; i++) {
        const step = steps[i];

        if(i == index){
            step.classList.remove("invisible");
            step.classList.remove("animate__fadeOutLeft");
            step.classList.remove("animate__fadeOutRight");
            step.classList.add("animate__fadeIn" + (lastStep < index ? "Right" : "Left"));
        }
        else{
            //step.classList.add("invisible");
            step.classList.remove("animate__fadeInLeft");
            step.classList.remove("animate__fadeInRight");
            step.classList.add("animate__fadeOut"+ (lastStep < index ? "Left" : "Right"));
        }
    }
    lastStep = index;
}

async function fixHeight(){
    await timer(300);
    resizeStacks();
    console.log("fixed");
}

function verificarEmail(campo){
    
    const email = campo.value;
    const url = campo.dataset.url;
    
    if(email === '' || email === null || email.length % 4 !== 0) {
        popoverEmail.hide();
        return;
    }
    
    fetch(`${url}?email=${email}`)
        .then(response => response.json())
        .then(json => {
            const available = json.disponible;
            console.log(available, json);
            notificarVerificacion(available, campo);
        });
}

function verificarEmailCompleto(campo){

    const email = campo.value;
    const url = campo.dataset.url;

    if(email === '' || email === null) {
        popoverEmail.hide();
        return;
    }
    
    fetch(`${url}?email=${email}`)
        .then(response => response.json())
        .then(json => {
            const available = json.disponible;
            console.log(available, json);
            notificarVerificacion(available, campo);

        });
}

function notificarVerificacion(available, campo){
    if(available) {
        campo.classList.remove('is-invalid');
        document.getElementById('emailError').innerText += '';
        popoverEmail.hide();
    }
    else {
        campo.classList.add('is-invalid');
        document.getElementById('emailError').innerText += 'Email no disponible';
        popoverEmail.show();
    }
}