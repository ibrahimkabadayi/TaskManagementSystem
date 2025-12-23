const allBackgrounds = {
    photos: [
        "https://images.unsplash.com/photo-1470071459604-3b5ec3a7fe05?w=200&q=80",
        "https://images.unsplash.com/photo-1441974231531-c6227db76b6e?w=200&q=80",
        "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=200&q=80",
        "https://images.unsplash.com/photo-1472214103451-9374bd1c798e?w=200&q=80",
        "https://images.unsplash.com/photo-1501854140884-074cf2b2c3af?w=200&q=80",
        "https://images.unsplash.com/photo-1505144808419-1957a94ca61e?w=200&q=80",
        "https://images.unsplash.com/photo-1465146344425-f00d5f5c8f07?w=200&q=80",
        "https://images.unsplash.com/photo-1433086966358-54859d0ed716?w=200&q=80"
    ],
    colors: [
        "#0079bf", "#d29034", "#519839", "#b04632",
        "#89609e", "#cd5a91", "#4bbf6b", "#00aecc",
        "#838c91", "#172b4d", "#ff9f1a", "#eb5a46"
    ]
};

function AccountClick(event) {
    event.stopPropagation();

    const menu = document.getElementById('accountMenu');

    if (menu.classList.contains('show')) {
        menu.classList.remove('show');
        return;
    }

    document.querySelectorAll('.popup-menu').forEach(m => m.classList.remove('show'));

    const button = event.currentTarget;
    const rect = button.getBoundingClientRect();

    const menuWidth = 300;

    menu.style.top = (rect.bottom + window.scrollY + 10) + 'px';

    menu.style.left = (rect.right + window.scrollX - menuWidth) + 'px';

    menu.classList.add('show');
}

document.addEventListener('click', function(event) {
    const menu = document.getElementById('accountMenu');
    if (menu && !menu.contains(event.target)) {
        menu.classList.remove('show');
    }
});