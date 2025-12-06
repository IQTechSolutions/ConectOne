window.ScrollToBottom = (elementName) => {
    const element = document.getElementById(elementName);
    if (!element) {
        return;
    }

    element.scrollTop = element.scrollHeight - element.clientHeight;
};

window.scrollToTop = () => {
    if (typeof window === "undefined") {
        return;
    }

    window.scrollTo({ top: 0, behavior: "smooth" });
};

window.scrollToElement = (elementId) => {
    if (typeof window === "undefined" || !elementId) {
        return;
    }

    const maxAttempts = 5;
    const startScroll = () => {
        let attempts = 0;

        const tryScroll = () => {
            const element = document.getElementById(elementId);
            if (!element) {
                if (attempts++ < maxAttempts) {
                    setTimeout(tryScroll, 100);
                }
                return;
            }

            const executeScroll = () => element.scrollIntoView({ behavior: "smooth", block: "start" });

            if (typeof window.requestAnimationFrame === "function") {
                requestAnimationFrame(() => requestAnimationFrame(executeScroll));
            } else {
                setTimeout(executeScroll, 0);
            }

            if (attempts++ < maxAttempts) {
                setTimeout(() => {
                    const rect = element.getBoundingClientRect();
                    if (rect.top > 4) {
                        tryScroll();
                    }
                }, 140);
            }
        };

        tryScroll();
    };

    if (document.readyState === "complete") {
        startScroll();
    } else {
        const onLoad = () => {
            window.removeEventListener("load", onLoad);
            startScroll();
        };

        window.addEventListener("load", onLoad);
    }
};
