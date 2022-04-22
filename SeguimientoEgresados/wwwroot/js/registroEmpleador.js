const steps = document.getElementsByClassName("form-step");
let currentStep = 0;
let lastStep = 0;

document.querySelector('form').addEventListener('submit', evt => {
    fixHeight();
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