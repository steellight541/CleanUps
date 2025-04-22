
const apiUrl = "https://cleanups-api.mbuzinous.com/api/events"
var map = null; // Declare map variable in the global scope

async function loadMap() {
    // get location of the user 
    var loc = await getLocation();

    console.log(loc);

    var long = loc ? loc.longitude : 19.2593642;
    var lat = loc ? loc.latitude : 42.4304196;

    map = L.map('lf-map').setView([lat, long], 13);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    handleMarkers()

    await new Promise(resolve => {
        setTimeout(() => {
            resolve();
        }, 1000); // Wait for 1 second before setting the view
    });
    map.setView([lat, long], 13);

}

function handleMarkers() {
    $.ajax({
        url: apiUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            setMarkers(data);
        },
        error: function (error) {
            console.log("Error fetching markers:", error);
        }
    });
}

function setMarkers(markers) {
    markers.forEach(marker => {

        const startTime = new Date(marker.startTime);
        const endTime = new Date(marker.endTime);
        const startTimeString = startTime.toLocaleString('en-US', { timeZone: 'UTC' });
        const endTimeString = endTime.toLocaleString('en-US', { timeZone: 'UTC' });



        const popupContent = `
                <div class="card" style="width: 18rem;">
                    <div class="card-body">
                        <h5 class="card-title">${marker.title}</h5>
                        <p class="card-text">${marker.description || 'No description available'}</p>
                        <p class="card-text"><small class="text-muted">Start: ${startTimeString}</small></p>
                        <p class="card-text"><small class="text-muted">End: ${endTimeString}</small></p>
                        <p class="card-text"><small class="text-muted">Location: ${marker.location.latitude}, ${marker.location.longitude}</small></p>
                    </div>
                </div>
            `;

        L.marker([marker.location.longitude, marker.location.latitude]).addTo(map)
            .bindPopup(popupContent, {
                className: 'custom-popup',
                closeButton: true,
                maxWidth: 300,
                autoPan: true,
                autoPanPadding: [100, 100],
                autoPanPaddingBottomRight: [50, 50]
            });
    });
}

function getLocation() {
    return new Promise((resolve, reject) => {
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
                    resolve(null);
                }
            );
        } else {
            console.log("Geolocation is not supported by this browser.");
            resolve(null);
        }
    });
}

loadMap();