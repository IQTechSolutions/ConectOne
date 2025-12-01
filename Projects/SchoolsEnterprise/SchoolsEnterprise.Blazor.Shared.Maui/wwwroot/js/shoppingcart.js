function submitPayFastForm(form) {
    if (form && typeof form.submit === "function") {
        form.submit();
    }
}

function submitPayGateForm(form) {
    if (form && typeof form.submit === "function") {
        form.submit();
    }
}
