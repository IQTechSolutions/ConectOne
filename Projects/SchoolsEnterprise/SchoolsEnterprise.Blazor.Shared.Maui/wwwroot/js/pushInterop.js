// pushInterop.js  – register SW + return PushSubscription JSON

export async function getOrCreateSubscription(publicKeyBase64) {

    if (Notification.permission === 'default') {
        const permission = await Notification.requestPermission();
        if (permission !== 'granted') {
            console.warn('[pushInterop] user denied permission');
            return null;
        }
    } else if (Notification.permission !== 'granted') {
        console.warn('[pushInterop] permission =', Notification.permission);
        return null;                               // denied or not supported
    }

    console.log('[Interop] permission =', Notification.permission);

    if (!publicKeyBase64 )
        throw new Error('[pushInterop] VAPID publicKey is null/empty');

    console.log('[pushInterop] starting subscribe flow');


    if (!('serviceWorker' in navigator) || !('PushManager' in window))
        return null;

    const reg = await navigator.serviceWorker.register('/service-worker.js');
    const newKeyUint8 = urlBase64ToUint8Array(publicKeyBase64);

    let sub = await reg.pushManager.getSubscription();

    if (sub) {
        const curKey = sub.options.applicationServerKey    // may be null
            ? new Uint8Array(sub.options.applicationServerKey)
            : null;

        // Compare keys byte-wise; if mismatch, remove and recreate
        if (!curKey || curKey.length !== newKeyUint8.length ||
            curKey.some((b, i) => b !== newKeyUint8[i])) {
            console.warn('[pushInterop] VAPID key mismatch – re-subscribing');
            await sub.unsubscribe();
            sub = null;
        }
    }

    if (!sub) {
        sub = await reg.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: newKeyUint8
        });
    }

    console.log('[pushInterop] subscription is', sub);

    // 👉 Return the subscription (plain JSON) to .NET instead of POSTing here
    return sub.toJSON();
}

/* helper */
function urlBase64ToUint8Array(base64) {
    const pad = '='.repeat((4 - base64.length % 4) % 4);
    const data = (base64 + pad).replace(/\-/g, '+').replace(/_/g, '/');
    return Uint8Array.from(atob(data), c => c.charCodeAt(0));
}