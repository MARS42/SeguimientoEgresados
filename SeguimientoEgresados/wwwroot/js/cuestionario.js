document.addEventListener("DOMContentLoaded", function() {
    
    const footer = document.querySelector("footer");
    const btn_cuestionario = document.querySelector("#btn-cuestionario");
    
    window.addEventListener('scroll', () => {
        const bounds = footer.getBoundingClientRect();
        const visibleFoot = (window.innerHeight - bounds.top);
        if(visibleFoot >= 1){
            btn_cuestionario.classList.remove('position-fixed', 'start-50', 'translate-middle');
            btn_cuestionario.classList.add('fixed-bottom');
            btn_cuestionario.style.paddingBottom = visibleFoot + 20 + 'px';
        }
        else
        {
            btn_cuestionario.classList.remove('fixed-bottom');
            btn_cuestionario.style.paddingBottom= '0';
            btn_cuestionario.classList.add('position-fixed', 'start-50', 'translate-middle');
        }
    });
});