window.initAfricaMap = () => {
    if (window.jQuery && typeof jQuery.fn.CSSMap === 'function') {
        const map = jQuery("#map-africa");
        if (map.length) {
            map.CSSMap({ size: 650 });
        }
    }
};
