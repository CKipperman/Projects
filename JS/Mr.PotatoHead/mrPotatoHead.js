(function () {
    'use strict';

    let dragging = false;
    let offset;
    let zIndex = 1;
    let parts = [];

    document.addEventListener('mousedown', e => {
        if (e.target.className === 'part') {
            e.preventDefault();
            dragging = e.target;
            offset = { x: e.offsetX, y: e.offsetY };
            dragging.style.zIndex = zIndex++;
        }
    });

    document.addEventListener('mousemove', e => {
        if (dragging) {
            dragging.style.left = `${e.pageX - offset.x}px`;
            dragging.style.top = `${e.pageY - offset.y}px`;
        }
    });

    document.addEventListener('mouseup', () => {
        savePartData(dragging);
        dragging = false;
    });

    function savePartData(part) {
        const data = {
            src: part.src,
            className: part.className,
            left: part.style.left,
            top: part.style.top,
            zIndex: part.style.zIndex
        };
        parts.push(data);
        localStorage.setItem('savedPart', JSON.stringify(parts));
    }

    function loadPartData() {
        const partsData = JSON.parse(localStorage.getItem('savedPart'));
        partsData.forEach(part => {
            const p = document.createElement('img');
            p.src = part.src;
            p.className = part.className;
            p.style.position = 'absolute';
            p.style.left = part.left;
            p.style.top = part.top;
            p.style.zIndex = part.zIndex;
            document.querySelector('#main').appendChild(p);
        });
    }
    loadPartData();
}());