window.AIChatInterop = {
    dispatchEvent: function (eventName) {
        const evt = new CustomEvent(eventName);
        window.dispatchEvent(evt);
    }
};

window.scrollToBottom = () => {
    const panel = document.querySelector('.chat-messages');
    if (panel) {
        panel.scrollTop = panel.scrollHeight;
    }
};

window.chatInterop = {
    saveSetting: function (key, value) {
        localStorage.setItem(key, value);
    },
    loadSetting: function (key) {
        return localStorage.getItem(key);
    },
    getBrowserLanguage: () => navigator.language || navigator.userLanguage
};
