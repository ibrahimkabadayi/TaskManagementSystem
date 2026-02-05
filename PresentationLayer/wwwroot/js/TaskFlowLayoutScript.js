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

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect()
    .build();

connection.on("ReceiveNotification", function (title, message) {
    console.log("🔔 Canlı Bildirim:", title, "-", message);

    showToastNotification(title, message);

    refreshNotificationBadge();
});

async function startSignalR() {
    try {
        await connection.start();
        console.log("✅ SignalR Bağlandı");
    } catch (err) {
        console.error("❌ SignalR Hatası:", err);
        setTimeout(startSignalR, 5000);
    }
}

startSignalR();

function refreshNotificationBadge() {
    loadNotifications();
}

function showToastNotification(title, message) {
    const toastHTML = `
        <div class="toast-notification">
            <div class="toast-icon"><i class="fa-solid fa-bell"></i></div>
            <div class="toast-content">
                <div class="toast-title">${title}</div>
                <div class="toast-message">${message}</div>
            </div>
            <button class="toast-close" onclick="this.parentElement.remove()">×</button>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', toastHTML);

    const toastElement = document.body.lastElementChild;
    setTimeout(() => {
        if(toastElement) {
            toastElement.style.opacity = '0';
            setTimeout(() => toastElement.remove(), 500); // Fade out efekti
        }
    }, 5000);
}

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

// HTML Olarak Listele (GÜNCELLENMİŞ VERSİYON)
// HTML Olarak Listele (GÜNCELLENMİŞ VERSİYON 2.0 - DAVET BUTONLU)
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
        listElement.innerHTML = `
            <li class="notification-empty">
                <i class="fa-regular fa-bell-slash"></i>
                <div>Hiç yeni bildirim yok.</div>
            </li>`;
        return;
    }

    // Her bir bildirimi listeye ekle
    data.forEach(item => {
        // Tarihi formatla
        const dateObj = new Date(item.createdDate);
        const timeStr = dateObj.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
        const dateStr = dateObj.toLocaleDateString('tr-TR', { day: 'numeric', month: 'short' });
        const formattedDate = `${dateStr}, ${timeStr}`;

        const li = document.createElement('li');
        li.className = 'notification-item';

        // --- KRİTİK DEĞİŞİKLİK BURADA ---

        // NotificationType.Invitation olduğunu varsayıyoruz (Enum değerin neyse onu yaz: örn 2 veya 3)
        // Backend'de Invitation = 2 ise buraya 2 yaz.
        const isInvitation = item.type === 4;

        if (isInvitation) {
            // --- YENİ MODERN TASARIM ---
            li.className = 'notification-item invite-type'; // Özel CSS sınıfı ekledik

            // Proje isminin baş harfini almak için basit bir mantık
            // Not: item.title veya message içinden proje ismini çekmek gerekebilir. 
            // Şimdilik ikon olarak 'P' veya davet ikonunu kullanıyoruz.

            li.innerHTML = `
        <div class="invite-card">
            <div class="invite-header">
                <div class="invite-project-avatar">
                    <i class="fa-solid fa-paper-plane"></i> </div>
                <div class="invite-text-group">
                    <div class="invite-title">Proje Daveti</div>
                    <div class="invite-subtitle">${item.message}</div>
                </div>
                <div style="font-size: 10px; color: #999; align-self: flex-start; margin-left: auto;">
                    ${timeStr}
                </div>
            </div>

            <div class="invite-actions">
                <button class="btn-invite-action btn-invite-decline" 
                        onclick="event.stopPropagation(); respondInvite(${item.relatedEntityId}, false, this)">
                    <i class="fa-solid fa-xmark"></i> Reddet
                </button>

                <button class="btn-invite-action btn-invite-accept" 
                        onclick="event.stopPropagation(); respondInvite(${item.relatedEntityId}, true, this)">
                    <i class="fa-solid fa-check"></i> Katıl
                </button>
            </div>
        </div>
    `;
        }
        else {
            // ... Diğer standart bildirim kodları aynen kalacak ...
            li.onclick = () => markAsRead(item.id, item.relatedTaskId);
            li.innerHTML = `
        <div class="notification-unread-indicator"></div>
        <div class="notification-content">
            <div class="notification-title">${item.title}</div>
            <div class="notification-message">${item.message}</div>
            <div class="notification-time">
                <i class="fa-regular fa-clock"></i> ${formattedDate}
            </div>
        </div>
    `;
        }

        listElement.appendChild(li);
    });
}

async function respondInvite(invitationId, isAccepted, btnElement) {
    const parentDiv = btnElement.parentElement;
    parentDiv.style.opacity = "0.5";
    parentDiv.style.pointerEvents = "none";

    try {
        const response = await fetch(`/Project/RespondInvitation?invitationId=${invitationId}&isAccepted=${isAccepted}`, {
            method: 'POST'
        });

        if (response.ok) {
            const listItem = btnElement.closest('li');
            listItem.remove();

            loadNotifications();

            alert(isAccepted ? "Projeye katıldınız! 🎉" : "Davet reddedildi.");

            if(isAccepted) {
                location.reload();
            }
        } else {
            alert("Bir hata oluştu.");
            parentDiv.style.opacity = "1";
            parentDiv.style.pointerEvents = "auto";
        }
    } catch (err) {
        console.error(err);
        alert("Bağlantı hatası.");
    }
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

async function sendInvitation(projectId) {
    const input = document.getElementById('share-email-input');
    const emailOrUsername = input.value;
    
    let role = "Developer";

    const roleElement = document.querySelector('.invite-role-select');

    if (roleElement) {
        role = roleElement.value;
    }

    if (!emailOrUsername) return alert("Lütfen bir e-posta veya kullanıcı adı girin.");

    const btn = document.querySelector('.btn-share-invite'); // Butonu bul
    btn.disabled = true;
    btn.innerText = "Gönderiliyor...";

    try {
        const response = await fetch('/Project/InviteUser', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ projectId, emailOrUsername, role })
        });

        const result = await response.json();

        if (response.ok) {
            alert("Davet başarıyla gönderildi! 🚀");
            input.value = "";
        } else {
            alert("Hata: " + result.message);
        }
    } catch (err) {
        console.error(err);
        alert("Bir hata oluştu.");
    } finally {
        btn.disabled = false;
        btn.innerText = "Share";
    }
}