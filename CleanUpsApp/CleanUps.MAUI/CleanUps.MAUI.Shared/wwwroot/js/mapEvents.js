var map = null; // Declare map variable in the global scope
const apiUrl = "https://cleanups-api.mbuzinous.com/api/events";

window.mapEvents = {
    loadMap: async function () {
        // Check if map is already initialized
        if (map) {
            console.log("Map already initialized.");
            // Optionally, re-center or update markers if needed on resize/re-render
            // handleMarkers(); // Example: Refresh markers if needed
            return;
        }

        // get location of the user
        var loc = await getLocation();

        console.log(loc);

        var long = loc ? loc.longitude : 19.2593642;
        var lat = loc ? loc.latitude : 42.4304196;

        // Ensure the map container exists
        var mapContainer = document.getElementById('lf-map');
        if (!mapContainer) {
            console.error("Map container 'lf-map' not found.");
            return;
        }
        // Clear previous map instance if any (though the check above should prevent this)
        if (mapContainer._leaflet_id) {
            mapContainer._leaflet_id = null;
        }


        map = L.map('lf-map').setView([lat, long], 13);

        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);

        handleMarkers();

        // set view to user location
        if (loc) {
            map.setView([lat, long], 13);
        } else {
            console.log("User location not available, using default coordinates.");
        }
    },
    disposeMap: function () {
        if (map) {
            map.remove();
            map = null;
            console.log("Map disposed.");
        }
    }
};

function handleMarkers() {
    // Use fetch API instead of jQuery $.ajax for better Blazor integration if preferred,
    // but jQuery should still work if loaded correctly.
    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            setMarkers(data);
        })
        .catch(error => {
            console.log("Error fetching markers:", error);
        });
}

function setMarkers(markers) {
    if (!map) {
        console.error("Map object not initialized before setting markers.");
        return;
    }
    markers.forEach(marker => {
        // Leaflet uses Lat, Long order
        L.marker([marker.location.latitude, marker.location.longitude]).addTo(map)
            .bindPopup(marker.title); // Consider opening popup on click instead of immediately
        // .openPopup(); // Removed to avoid opening all popups at once
    });
}

function getLocation() {
    return new Promise((resolve) => { // Removed reject as we resolve with null on error
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                position => {
                    resolve({
                        latitude: position.coords.latitude,
                        longitude: position.coords.longitude
                    });
                },
                error => {
                    console.log("Error getting location:", error);
                    resolve(null); // Resolve with null when location fails
                }
            );
        } else {
            console.log("Geolocation is not supported by this browser.");
            resolve(null); // Resolve with null when geolocation is not supported
        }
    });
}