function InitialiceTransactionForm(url) {
    $("#OperationTypeId").change(async function () {
        const valorSeleccionado = $(this).val();

        const respuesta = await fetch(url, {
            method: 'POST',
            body: valorSeleccionado,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const json = await respuesta.json();
        const options = json.map(category => `<option value=${category.value}>${category.text}</option>`);
        $("#CategoriaId").html(options);
    });
}