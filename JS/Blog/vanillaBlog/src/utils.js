export async function load(url, mapper) {
    let content;
    try {
        const response = await fetch(url);
        if(!response.ok) {
            throw new Error(`${url}: ${response.status} - ${response.statusText}`);
        }
        const data = await response.json();
        content = data.map(d => mapper(d));
    } catch (e) {
        content = [
            `<div class="error">${e.message} - Failed to load ${url}</div>`,
        ];
    }
    return content;
}

export function getInitials(name) {
    return name.split(' ').map(word => word[0].toUpperCase()).slice(0, 2).join('');
}