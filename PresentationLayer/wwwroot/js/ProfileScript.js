function switchTab(tabName) {
    document.querySelectorAll('.content-panel').forEach(panel => {
        panel.classList.remove('active');
    });

    document.querySelectorAll('.menu-btn').forEach(btn => {
        btn.classList.remove('active');
    });

    document.getElementById('tab-' + tabName).classList.add('active');

    const btns = document.querySelectorAll('.menu-btn');
    if(tabName === 'settings') btns[0].classList.add('active');
    if(tabName === 'projects') btns[1].classList.add('active');
    if(tabName === 'security') btns[2].classList.add('active');
}

async function updateUserProfile() {
    const userId = document.getElementById('profileId').value;
    const name = document.getElementById('profileName').value;
    const color = document.getElementById('profileColor').value;

    if (!name) {
        showToast("Please enter your name.", "warning");
        return;
    }

    try {
        const response = await fetch('/Account/UpdateProfile', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                UserId: userId,
                NewUserName: name,
                NewProfileColor: color
            })
        });

        if (response.ok) {
            window.location.reload();
        } else {
            showToast("There was an error updating your profile. Please try again later.", "error");
        }

    } catch (error) {
        showToast("Server error. Please try again later.", "error");
    }
}

async function changePassword() {
    const userId = document.getElementById('profileId').value;
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (!currentPassword || !newPassword || !confirmPassword) {
        showToast("Please fill in all fields.", "warning");
        return;
    }

    if (newPassword !== confirmPassword) {
        showToast("Passwords do not match.", "warning");
        return;
    }

    if (newPassword.length < 6) {
        showToast("Password must be at least 6 characters long.", "warning");
        return;
    }

    try {
        const response = await fetch('/Account/ChangePassword', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                UserId: userId,
                CurrentPassword: currentPassword,
                NewPassword: newPassword
            })
        });

        const result = await response.json();

        if (response.ok) {
            showToast("Password changed successfully.", "success");
            document.getElementById('currentPassword').value = '';
            document.getElementById('newPassword').value = '';
            document.getElementById('confirmPassword').value = '';
        } else {
            showToast(result.message, "error");
        }

    } catch (error) {
        showToast("Server error. Please try again later.", "error");
    }
}