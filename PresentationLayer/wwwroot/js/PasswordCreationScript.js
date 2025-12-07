async function ConfirmPassword(name, email){
    const password = document.getElementById("Password").value;
    const passwordAgain = document.getElementById("Password-again").value;
    
    if (password == null){
        alert("Password is required");
        return;
    }
    
    if (name == null || email == null){
        alert("No email or name has send fatal error.")
        return;
    }
    
    if(password !== passwordAgain){
        alert("Please enter the same password");
        return;
    }
    
    await fetch('Authenticate/Register',{
        method: 'POST',
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Name : name,
            Email : email,
            Password: password,
        })
    }).then(response => response.json()).then(data => {
        if (data.success) {
            window.location.href = 'Home/Home';
        }
        else{
            const params = new URLSearchParams({
                Message: "Authentication Failed",
                Type: "Authentication",
                StatusCode: -9999,
                TimeStamp: new Date().toISOString()
            });
            window.location.href = `/Home/Error?${params.toString()}`;
        }
    })
}

function HidePassword(element){
    const passwordInput = element.previousElementSibling;
    
    if(passwordInput.type === "password"){
        passwordInput.type = "text";
        element.src = "~/lib/Images/EyeOpened.ico";
    } else{
        passwordInput.type = "password";
        element.src = "~/lib/Images/EyeClosed.ico";
    }
}

function BackButtonClick(name, email){
    const params = new URLSearchParams({
        UserName : name,
        Email : email
    })
    
    window.location.href = `/User/CreateAccount?${params.toString()}`;
}