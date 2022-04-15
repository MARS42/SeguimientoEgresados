window.addEventListener('DOMContentLoaded', (event) => {
    const vista = document.getElementById('vista');

    let lastItem = document.getElementById("inicio");
    const firtsUrl = lastItem.dataset.url;
    fetch(firtsUrl)
        .then(res => res.text())
        .then(body => {
            vista.innerHTML = body;
        });

    const links = document.getElementsByClassName('list-group-item');

    for (let i = 0; i < links.length; i++) {
        links[i].addEventListener('click', (event) => {

            if (event.target === lastItem)
                return;

            event.target.classList.add('active');
            lastItem.classList.remove('active');
            lastItem = event.target;

            const url = event.target.dataset.url;

            fetch(url)
                .then(res => res.text())
                .then(body => {
                    vista.innerHTML = body;
                    updateView();
                });
        });
    }
});

let order = "";

function updateView() {
    //const cont = document.getElementById('contenido');
    const contenidosAcargar = document.getElementsByClassName("to-load");

    for (let i = 0; i < contenidosAcargar.length; i++) {
        const dataUrl = contenidosAcargar[i].dataset.url;

        contenidosAcargar[i].innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
        fetch(dataUrl)
            .then(res => res.text())
            .then(body => {
                contenidosAcargar[i].innerHTML = body;

                const buscador = document.getElementById("busqueda");

                order = "";

                if (buscador != null) {

                    buscador.addEventListener('input', (event) => actualizarTabla(buscador.dataset.tabla, 1, order));
                }
            });
    }

    // const dataUrl = cont.dataset.url;
    //
    // cont.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
    // fetch(dataUrl)
    // .then(res => res.text())
    // .then(body => {
    //     cont.innerHTML = body;
    //    
    //     document.getElementById("busqueda").addEventListener('input', (event) => actualizar(1, order));
    // });
}

function actualizarTabla(idTabla, pagina = 1, ordenTabla = null) {

    //const orden = "@ViewData["OrdenActual"]";
    const orden = ordenTabla === null ? order : ordenTabla;
    order = orden;
    const busqueda = document.getElementById("busqueda").value;
    const filtroActual = "@ViewData["
    Busqueda
    "]";

    const cont = document.getElementById(idTabla);
    const action = cont.dataset.url;
    const url = action + "?" + `ordenTabla=${orden}&busqueda=${busqueda}&filtroActual=${filtroActual}&pagina=${pagina}`;

    cont.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
    fetch(url)
        .then(res => res.text())
        .then(body => {
            cont.innerHTML = body;
        });
}