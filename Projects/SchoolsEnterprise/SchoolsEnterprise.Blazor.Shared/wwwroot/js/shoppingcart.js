function submitPayFastForm(form) {
    if (form && typeof form.submit === "function") {
        const formData = new FormData(form);
        const formObject = {};

        formData.forEach((value, key) => {
            if (Object.prototype.hasOwnProperty.call(formObject, key)) {
                if (!Array.isArray(formObject[key])) {
                    formObject[key] = [formObject[key]];
                }
                formObject[key].push(value);
            } else {
                formObject[key] = value;
            }
        });

        if (Object.keys(formObject).length === 0) {
            const elements = form && form.elements ? Array.from(form.elements) : [];
            const elementDiagnostics = elements.map((element) => ({
                tagName: element && element.tagName,
                type: element && element.type,
                name: element && element.name,
                value: element && element.value,
                disabled: element ? element.disabled : undefined,
            }));

            console.warn("PayFast form contains no fields at submission time.", {
                elementDiagnostics,
                markup: form && typeof form.innerHTML === "string" ? form.innerHTML.trim() : null,
            });
        }

        const formInfo = JSON.stringify(formObject, null, 2);
        console.log("Submitting PayFast form with data:", formObject);

        form.submit();
    }
}
