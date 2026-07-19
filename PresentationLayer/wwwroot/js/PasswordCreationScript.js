function TogglePassword(inputId, iconElement) {
    const input = document.getElementById(inputId);

    if (input.type === "password") {
        input.type = "text";
        iconElement.classList.remove("fa-eye");
        iconElement.classList.add("fa-eye-slash");
    } else {
        input.type = "password";
        iconElement.classList.remove("fa-eye-slash");
        iconElement.classList.add("fa-eye");
    }
}

async function ConfirmPassword(name, email) {
    const passwordInput = document.getElementById("password-input");
    const passwordAgainInput = document.getElementById("password-again-input");

    const password = passwordInput.value;
    const passwordAgain = passwordAgainInput.value;

    if (!password) {
        showToast("Please enter your password.", "warning");
        passwordInput.focus();
        return;
    }

    if (password.length < 6) {
        showToast("Password must be at least 6 characters long.", "warning");
        passwordInput.focus();
        return;
    }

    if (password !== passwordAgain) {
        showToast("Passwords do not match.", "warning");
        passwordAgainInput.focus();
        return;
    }

    const btn = document.querySelector('.btn-finish');
    btn.innerText = "Creating Account...";
    btn.disabled = true;

    try {
        const response = await fetch('/Authenticate/Register', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                Name: name,
                Email: email,
                Password: password,
            })
        });

        const data = await response.json();

        if (data.success) {
            showToast("Registration Successful!", "success");
            window.location.href = '/Home/SignIn';
        } else {
            showToast("Server Error", "error");

            if (data.errorCode === -9999) {
                const params = new URLSearchParams({
                    Message: "Authentication Failed",
                    Type: "Authentication",
                    StatusCode: -9999,
                    TimeStamp: new Date().toISOString()
                });
                window.location.href = `/Home/Error?${params.toString()}`;
            } else {
                btn.disabled = false;
                btn.innerText = "Complete Registration";
            }
        }
    } catch (e) {
        console.error(e);
        showToast("Server Error", "error");
        btn.disabled = false;
        btn.innerText = "Complete Registration";
    }
}

function BackButtonClick(name, email) {
    const params = new URLSearchParams({
        UserName: name,
        Email: email
    });
    window.location.href = `/User/EmailCodeVerification?${params.toString()}`;
}