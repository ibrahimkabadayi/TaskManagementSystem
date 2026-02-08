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
    console.log("🔔 Live Notification:", title, "-", message);

    showToastNotification(title, message);

    refreshNotificationBadge();
});

async function startSignalR() {
    try {
        await connection.start();
    } catch (err) {
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
            setTimeout(() => toastElement.remove(), 500);
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

function toggleNotificationMenu() {
    const menu = document.getElementById('notification-dropdown');

    if (menu.style.display === 'none') {
        menu.style.display = 'block';
        loadNotifications();
    } else {
        menu.style.display = 'none';
    }
}

async function loadNotifications() {
    const listElement = document.getElementById('notification-list');

    try {
        const response = await fetch('/Notification/GetUnreadNotifications');

        if (!response.ok) {
            return;
        }

        const notifications = await response.json();
        renderNotifications(notifications);

    } catch (error) {
        listElement.innerHTML = '<li style="padding:10px; text-align:center;">Hata oluştu.</li>';
    }
}

function renderNotifications(data) {
    const listElement = document.getElementById('notification-list');
    const badgeElement = document.getElementById('notification-badge');

    listElement.innerHTML = ''; 

    if (data.length > 0) {
        badgeElement.innerText = data.length;
        badgeElement.style.display = 'inline-block';
    } else {
        badgeElement.style.display = 'none';
        listElement.innerHTML = `
            <li class="notification-empty">
                <i class="fa-regular fa-bell-slash"></i>
                <div>There are no new notifications.</div>
            </li>`;
        return;
    }

    data.forEach(item => {
        const dateObj = new Date(item.createdDate);
        const timeStr = dateObj.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
        const dateStr = dateObj.toLocaleDateString('tr-TR', { day: 'numeric', month: 'short' });
        const formattedDate = `${dateStr}, ${timeStr}`;

        const li = document.createElement('li');
        li.className = 'notification-item';

        const isInvitation = item.type === 4;

        if (isInvitation) {
            li.className = 'notification-item invite-type';

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

            alert(isAccepted ? "You joined the project! 🎉" : "Invitation declined.");

            if(isAccepted) {
                location.reload();
            }
        } else {
            alert("An error occurred.");
            parentDiv.style.opacity = "1";
            parentDiv.style.pointerEvents = "auto";
        }
    } catch (err) {
        console.error(err);
        alert("Connection error.");
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

    if (!emailOrUsername) return alert("Please enter an email or username.");

    const btn = document.querySelector('.btn-share-invite');
    btn.disabled = true;
    btn.innerText = "Sending...";

    try {
        const response = await fetch('/Project/InviteUser', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ projectId, emailOrUsername, role })
        });

        const result = await response.json();

        if (response.ok) {
            alert("Invitation sent successfully! 🚀");
            input.value = "";
        } else {
            alert("Error: " + result.message);
        }
    } catch (err) {
        console.error(err);
        alert("An error occurred.");
    } finally {
        btn.disabled = false;
        btn.innerText = "Share";
    }
}