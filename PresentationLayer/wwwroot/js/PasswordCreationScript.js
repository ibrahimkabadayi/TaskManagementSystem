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
        alert("Lütfen bir şifre belirleyin.");
        passwordInput.focus();
        return;
    }

    if (password.length < 6) {
        alert("Şifreniz en az 6 karakter olmalıdır.");
        passwordInput.focus();
        return;
    }

    if (password !== passwordAgain) {
        alert("Şifreler uyuşmuyor, lütfen kontrol edin.");
        passwordAgainInput.focus();
        return;
    }

    const btn = document.querySelector('.btn-finish');
    btn.innerText = "Hesap Oluşturuluyor...";
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
            alert("Hesabınız başarıyla oluşturuldu! Giriş yapabilirsiniz.");
            window.location.href = '/Home/SignIn'; // Direkt Home yerine SignIn'e atmak daha güvenlidir
        } else {
            alert("Kayıt Başarısız: " + (data.message || "Bilinmeyen hata"));

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
                btn.innerText = "Kaydı Tamamla";
            }
        }
    } catch (e) {
        console.error(e);
        alert("Sunucu hatası oluştu.");
        btn.disabled = false;
        btn.innerText = "Kaydı Tamamla";
    }
}

function BackButtonClick(name, email) {
    const params = new URLSearchParams({
        UserName: name,
        Email: email
    });
    // Önceki adım olan Kod Doğrulama yerine E-posta girmeye mi yoksa Kod'a mı döneceği size kalmış
    // Genelde "Geri" butonu bir önceki sayfaya (CodeVerification) döner.
    window.location.href = `/User/EmailCodeVerification?${params.toString()}`;
}