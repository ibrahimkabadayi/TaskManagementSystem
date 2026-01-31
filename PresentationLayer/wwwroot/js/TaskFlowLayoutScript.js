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

// Menüyü Aç/Kapa
function toggleNotificationMenu() {
    const menu = document.getElementById('notification-dropdown');

    if (menu.style.display === 'none') {
        menu.style.display = 'block';
        loadNotifications(); // Menü açılınca verileri çek
    } else {
        menu.style.display = 'none';
    }
}

// Verileri Sunucudan Çek
async function loadNotifications() {
    const listElement = document.getElementById('notification-list');

    try {
        const response = await fetch('/Notification/GetUnreadNotifications');

        if (!response.ok) {
            console.error("Bildirimler alınamadı.");
            return;
        }

        const notifications = await response.json();
        renderNotifications(notifications);

    } catch (error) {
        console.error("Hata:", error);
        listElement.innerHTML = '<li style="padding:10px; text-align:center;">Hata oluştu.</li>';
    }
}

// HTML Olarak Listele
function renderNotifications(data) {
    const listElement = document.getElementById('notification-list');
    const badgeElement = document.getElementById('notification-badge');

    listElement.innerHTML = ''; // Listeyi temizle

    // Rozeti güncelle
    if (data.length > 0) {
        badgeElement.innerText = data.length;
        badgeElement.style.display = 'inline-block';
    } else {
        badgeElement.style.display = 'none';
        listElement.innerHTML = '<li style="padding:15px; text-align:center; color:#777;">Hiç yeni bildirim yok 🎉</li>';
        return;
    }

    // Her bir bildirimi listeye ekle
    data.forEach(item => {
        // Tarihi formatla (Örn: 10 dk önce)
        const date = new Date(item.createdDate).toLocaleString('tr-TR');

        const li = document.createElement('li');
        li.style.borderBottom = '1px solid #eee';
        li.style.padding = '10px';
        li.style.cursor = 'pointer';
        li.style.transition = 'background 0.2s';

        // Hover efekti (JS ile basitçe)
        li.onmouseover = () => li.style.background = '#f1f1f1';
        li.onmouseout = () => li.style.background = 'white';

        // Tıklayınca okundu işaretle
        li.onclick = () => markAsRead(item.id, item.relatedTaskId);

        li.innerHTML = `
            <div style="font-weight: bold; font-size: 14px; color: #333;">${item.title}</div>
            <div style="font-size: 13px; color: #666; margin-top: 2px;">${item.message}</div>
            <div style="font-size: 11px; color: #aaa; margin-top: 5px; text-align: right;">${date}</div>
        `;

        listElement.appendChild(li);
    });
}

async function markAsRead(notifId, relatedTaskId) {
    await fetch(`/Notification/MarkAsRead?notificationId=${notifId}`, { method: 'POST' });

    loadNotifications();
}

document.addEventListener('click', function(event) {
    const container = document.querySelector('.notification-container');
    const menu = document.getElementById('notification-dropdown');
    if (!container.contains(event.target)) {
        menu.style.display = 'none';
    }
});