function CreateAccount() {
    window.location.href = '/Home/CreateAccount';
}

function ForgotPassword() {
    window.location.href = '/Home/PasswordCreation';
}

function PrivacyPolicy() {
    window.location.href = '/Home/Privacy';
}

async function SignIn() {
    const emailEl = document.getElementById("email-Input");
    const passwordEl = document.getElementById("password-Input");

    const email = emailEl.value.trim();
    const password = passwordEl.value;

    if (email.length < 3 || !email.includes("@")) {
        alert("Please enter a valid email address.");
        emailEl.focus();
        return;
    }

    if (password.length < 3) {
        alert("Please enter a valid password. Password must be at least 3 characters long.");
        passwordEl.focus();
        return;
    }

    const btn = document.querySelector('.btn-primary');
    const originalText = btn.innerText;
    btn.disabled = true;
    btn.innerText = "Loading...";

    try {
        const response = await fetch('/Authenticate/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Email: email,
                Password: password,
            })
        });

        const data = await response.json();

        if (data.success) {
            window.location.href = '/Home/Home';
        } else {
            btn.disabled = false;
            btn.innerText = originalText;

            if (data.message === "Wrong password" || data.message === "User not found") {
                alert("Email or password is incorrect..");
                return;
            }

            alert("Error: " + data.message);
        }
    }
    catch (e) {
        console.error("Login Error:", e);
        alert("There was an error logging in.");

        btn.disabled = false;
        btn.innerText = originalText;
    }
}

document.addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        SignIn();
    }
});