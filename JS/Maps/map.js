/* global google */
(async function () {
    'use strict';

    const { Map } = await google.maps.importLibrary('maps');
    const { AdvancedMarkerElement } = await google.maps.importLibrary('marker');
    const { Geocoder } = await google.maps.importLibrary('geocoding');
    const bmg = { lat: 40.096044749672394, lng: -74.22197586384449 };

    const searchLbl = document.querySelector('.searchLabel');
    const searchInput = document.querySelector('.searchInput');
    const searchBtn = document.querySelector('.icon');
    const placesList = document.querySelector('#places');
    const searchType = document.querySelector('.searchType');

    const geonamesUsername = 'ckipperman';
    //const apiKey = 'YOUR KEY HERE';  // Replace with your actual key

    const map = new Map(document.querySelector('#map'), {
        center: bmg,
        zoom: 18,
        mapTypeId: google.maps.MapTypeId.SATELLITE,
        mapId: 'DEMO_MAP_ID'
    });

    function handleInputFocus() {
        searchLbl.classList.add('active');
    }

    function handleInputBlur() {
        searchLbl.classList.remove('active');
    }

    function showError(message) {
        placesList.innerHTML = '';
        const warningLi = document.createElement('li');
        warningLi.className = 'error';
        warningLi.innerHTML = `<div>${message}</div>`;
        placesList.appendChild(warningLi);
    }

    function createPlaceMarker(place) {
        const position = { lat: place.lat, lng: place.lng };

        let img;
        if (place.thumbnailImg) {
            img = document.createElement('img');
            img.src = place.thumbnailImg;
            img.alt = place.title;
            img.className = 'markerPic';
        }

        const marker = new AdvancedMarkerElement({
            map,
            position,
            title: place.title,
            content: img
        });

        const infoWindow = new google.maps.InfoWindow({
            content: place.title
        });

        marker.addListener('click', () => {
            infoWindow.setContent(`<div style="line-height: 1.5; text-align: center;">
                                <strong>${place.title}</strong><br>
                                ${place.summary}<br>
                                <a href="https://${place.wikipediaUrl}" target="_blank" style="color: blue; text-decoration: underline;">more info...</a>
                                </div>`);
            infoWindow.open({
                anchor: marker
            });
        });

        return marker;
    }

    function createPlaceListItem(place) {
        const li = document.createElement('li');
        li.innerHTML = `<div><span>${place.title}</span>
                    <img src=${place.thumbnailImg || 'media/default.jpg'}/></div>
                    <div class="summary">${place.summary}</div>`;

        li.addEventListener('click', () => {
            const position = { lat: place.lat, lng: place.lng };
            map.panTo(position);
            map.setZoom(5);
        });

        return li;
    }

    async function performPlaceSearch(query) {
        placesList.innerHTML = '';

        if (!query) {
            showError('Cannot have an empty search');
            return;
        }

        console.log(query);
        try {
            const response = await fetch(`http://api.geonames.org/wikipediaSearch?q=${query}&maxRows=10&username=${geonamesUsername}&type=json`);
            if (!response.ok) {
                throw new Error(`${response.status} - ${response.statusText}`);
            }
            const places = await response.json();
            console.log(places);

            const bounds = new google.maps.LatLngBounds();

            if (places.geonames.length === 0) {
                showError(`No results returned for ${query}`);
                return;
            }

            places.geonames.forEach(place => {
                const position = { lat: place.lat, lng: place.lng };
                bounds.extend(position);

                createPlaceMarker(place);
                placesList.appendChild(createPlaceListItem(place));
            });

            map.fitBounds(bounds);

        } catch (e) {
            console.error(e);
        }
    }

    async function performAddressSearch(query) {
        placesList.innerHTML = '';

        if (!query) {
            showError('Cannot have an empty search');
            return;
        }

        console.log(query);
        const geocoder = new google.maps.Geocoder();

        geocoder.geocode({ address: query }, (results, status) => {
            console.log({ results, status });

            const bounds = new google.maps.LatLngBounds();

            if (status === 'OK') {
                if (results.length === 0) {
                    showError(`No results returned for ${query}`);
                    return;
                }

                results.forEach(result => {
                    const position = result.geometry.location;
                    bounds.extend(position);

                    let img = document.createElement('img');
                    img.src = 'media/default.jpg';
                    img.alt = result.formatted_address;
                    img.className = 'markerPic';

                    const marker = new AdvancedMarkerElement({
                        map,
                        position,
                        title: result.formatted_address,
                        content: img
                    });

                    const infoWindow = new google.maps.InfoWindow({
                        content: result.formatted_address
                    });

                    marker.addListener('click', () => {
                        infoWindow.setContent(`<div style="line-height: 1.5; text-align: center;">
                        <strong>${result.formatted_address}</strong><br>
                        <em>Location details: ${result.formatted_address}</em><br>
                        <a href="https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(result.formatted_address)}" target="_blank" style="color: blue; text-decoration: underline;">more info...</a>
                        </div>`);
                        infoWindow.open({ anchor: marker });
                    });

                    const li = document.createElement('li');
                    li.innerHTML = `<div><span>${result.formatted_address}</span>
                            <img src="${img.src}"/></div>
                            <div class="summary"><em>Address: ${result.formatted_address}</em></div>`;

                    li.addEventListener('click', () => {
                        map.panTo(position);
                        map.setZoom(18);
                    });

                    placesList.appendChild(li);
                });

                map.fitBounds(bounds);
            } else {
                console.error('Geocoding failed:', status);
                let errorMsg = `Geocoding failed: ${status}`;
                if (status === 'REQUEST_DENIED') {
                    errorMsg += '. Check API key restrictions and enable Geocoding API in Google Cloud Console.';
                } else if (status === 'OVER_QUERY_LIMIT') {
                    errorMsg += '. Quota exceeded—wait or check billing.';
                }
                showError(errorMsg);
            }
        });
    }

    function setupEventListeners() {
        searchInput.addEventListener('focus', handleInputFocus);
        searchInput.addEventListener('blur', handleInputBlur);

        searchBtn.addEventListener('click', async () => {
            const query = searchInput.value.trim();
            placesList.innerHTML = '';

            if (!query) {
                showError('Cannot have an empty search');
                return;
            }

            console.log(`Searching "${query}" as ${searchType.value}`);

            if (searchType.value === 'places') {
                await performPlaceSearch(query);
            } else if (searchType.value === 'addresses') {
                await performAddressSearch(query);
            }
        });

        searchType.addEventListener('change', () => {
            placesList.innerHTML = '';
        });
    }

    setupEventListeners();
}());