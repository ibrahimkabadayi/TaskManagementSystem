async function VerifyEmailCode(email){
    const userEnteredCode = document.getElementById("code-Input").value;
    
    if(!userEnteredCode){
        alert("Wrong code entered please try again or click Send Again button for another code.")
        return false;
    }

    await fetch('/Email/VerifyCode', {
        method: 'POST',
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Email: email,
            Code: userEnteredCode
        })
    }).then(response => response.json()).then(data => {
        if(data.success) {
            window.location.href = '/Account/PasswordCreation';
        } else {
            alert('Incorrect Code! Please try again or click Send Again button for another code.');
        }
    });
}

async function SendEmailCodeAgain(email){
    await fetch('/Email/SendEmailVerificationCode', {
        method: 'POST',
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify({
            Email: email
        })
    }).then(response => response.json()).then(data => {
        if(data.success){
            alert("Email sent successfully.");
        } else{
            alert("Error: Could not send email verification code.");
            return false;
        }
    })
}