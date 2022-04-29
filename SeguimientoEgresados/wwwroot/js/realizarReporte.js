
let modalReporte;

function realizarReporte(element){
    const url = element.dataset.url;
    const surl = element.dataset.surl;
    const area = element.dataset.area;
    const cont = document.getElementById("contModalReporte");
    
    fetch(`${url}?area=${area}`).then(res => res.text()).then(html => {
        cont.innerHTML = html;

        document.getElementById("reportForm").addEventListener('submit', evt => {
            evt.preventDefault();
            enviarReporte(evt.target, area, surl);
        });
        
        modalReporte = new bootstrap.Modal(document.getElementById('modalReporte'));
        modalReporte.show();
    });
}

function enviarReporte(formulario, area, url){

    const btns = document.querySelectorAll("#reportForm > div > button");

    for(let i = 0; i < btns.length; i++)
        btns[i].disabled = true;
    
    const datos = Object.fromEntries(new FormData(formulario));

    fetch(`${url}?titulo=${datos.titulo}&descripcion=${datos.descripcion}&area=${area}`, { method: 'post' })
        .then(res => res.ok)
        .then(ok => {
            modalReporte.toggle();
        });
    
    return false;
}