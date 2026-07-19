function showToast(message, type = 'info') {
    const container = document.getElementById('toast-container');

    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;

    let iconClass = 'fa-info-circle';
    if (type === 'success') iconClass = 'fa-circle-check';
    if (type === 'error') iconClass = 'fa-circle-xmark';
    if (type === 'warning') iconClass = 'fa-triangle-exclamation';

    toast.innerHTML = `
            <div class="toast-content">
                <i class="fa-solid ${iconClass} toast-icon"></i>
                <span>${message}</span>
            </div>
            <i class="fa-solid fa-xmark toast-close" onclick="this.parentElement.remove()"></i>
        `;

    container.appendChild(toast);

    setTimeout(() => {
        toast.style.animation = 'slideOut 0.5s forwards';
        setTimeout(() => {
            toast.remove();
        }, 500);
    }, 4000);
}