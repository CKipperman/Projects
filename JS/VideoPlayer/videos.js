(function () {
    'use strict';

    const vidList = document.querySelector('#vidList');
    const vidPlayer = document.querySelector('#vidPlayer');

    async function fetchVideos() {
        try {
            const response = await fetch('videos.json');
            if (!response.ok) {
                throw new Error(`${response.status} - ${response.statusText}`);
            }
            const results = await response.json();

            results.forEach(video => {
                const li = document.createElement('li');
                const thumbnail = video.image || 'media/default.png';
                li.innerHTML = `<img src="${thumbnail}" alt="${video.title}">
                                <div>${video.title}</div>`;

                li.addEventListener('click', () => {
                    vidPlayer.src = video.url;
                    vidPlayer.poster = video.image || 'media/default.png';
                    vidPlayer.load();
                    vidPlayer.play();
                });
                vidList.appendChild(li);
            });

        } catch (e) {
            console.error('oops', e)
        }
    }

    fetchVideos();
}());