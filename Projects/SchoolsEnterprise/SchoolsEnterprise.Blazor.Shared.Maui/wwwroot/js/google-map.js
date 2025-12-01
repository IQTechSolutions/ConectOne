var resolveCallbacks = [];
var rejectCallbacks = [];

window.BlazorStore = {
    loadGoogleMaps: function (defaultView, apiKey, resolve, reject) {
        resolveCallbacks.push(resolve);
        rejectCallbacks.push(reject);

        if (defaultView['rz_map_init']) {
            return;
        }

        defaultView['rz_map_init'] = function () {
            for (var i = 0; i < resolveCallbacks.length; i++) {
                resolveCallbacks[i](defaultView.google);
            }
        };

        var document = defaultView.document;
        var script = document.createElement('script');

        script.src =
            'https://maps.googleapis.com/maps/api/js?' +
            (apiKey ? 'key=' + apiKey + '&' : '') +
            'callback=rz_map_init';

        script.async = true;
        script.defer = true;
        script.onerror = function (err) {
            for (var i = 0; i < rejectCallbacks.length; i++) {
                rejectCallbacks[i](err);
            }
        };

        document.body.appendChild(script);
    },
    createMap: function (wrapper, ref, id, apiKey, zoom, center, markers) {
        var api = function () {
            var defaultView = document.defaultView;

            return new Promise(function (resolve, reject) {
                if (defaultView.google && defaultView.google.maps) {
                    return resolve(defaultView.google);
                }

                BlazorStore.loadGoogleMaps(defaultView, apiKey, resolve, reject);
            });
        };

        api().then(function (google) {
            BlazorStore[id] = ref;
            BlazorStore[id].google = google;

            BlazorStore[id].instance = new google.maps.Map(wrapper, {
                center: center,
                zoom: zoom
            });

            BlazorStore[id].instance.addListener('click', function (e) {
                BlazorStore[id].invokeMethodAsync('GoogleMap.OnMapClick', {
                    Position: { Lat: e.latLng.lat(), Lng: e.latLng.lng() }
                });
            });

            BlazorStore.updateMap(id, zoom, center, markers);
        });
    },
    updateMap: function (id, zoom, center, markers) {
        if (BlazorStore[id] && BlazorStore[id].instance) {
            if (BlazorStore[id].instance.markers && BlazorStore[id].instance.markers.length) {
                for (var i = 0; i < BlazorStore[id].instance.markers.length; i++) {
                    BlazorStore[id].instance.markers[i].setMap(null);
                }
            }

            BlazorStore[id].instance.markers = [];

            markers.forEach(function (m) {
                var marker = new this.google.maps.Marker({
                    position: m.position,
                    title: m.title,
                    label: m.label
                });

                marker.addListener('click', function (e) {
                    BlazorStore[id].invokeMethodAsync('GoogleMap.OnMarkerClick', {
                        Title: marker.title,
                        Label: marker.label,
                        Position: marker.position
                    });
                });

                marker.setMap(BlazorStore[id].instance);

                BlazorStore[id].instance.markers.push(marker);
            });

            BlazorStore[id].instance.setZoom(zoom);

            BlazorStore[id].instance.setCenter(center);
        }
    },
    destroyMap: function (id) {
        if (BlazorStore[id].instance) {
            delete BlazorStore[id].instance;
        }
    },
};