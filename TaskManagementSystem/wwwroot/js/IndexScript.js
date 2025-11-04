function CreateAccount(){
    window.location.href = 'Home/CreateAccount';
}

async function SignIn(){
    const name = document.getElementById("name-Input").value;
    const email = document.getElementById("email-Input").value;
    const password = document.getElementById("password-Input").value;
    
    if(name.length < 3){
        alert("Please enter your name");
        return;
    }
    
    if(email.length < 3 || !email.includes("@")){
        alert("Please enter your email");
        return;
    }
    
    if(password.length < 3){
        alert("Please enter your password");
        return;
    }
    
    try
    {
        await fetch('Home/RegisterSignIn/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Name: name,
                Email: email,
                Password: password,
            })
        }.then((response) => response.json()).then((data) => {
            if (data.success === true) {
                const params = data.Data;
                window.location.href = `Home/SignIn?${params}`;
            } else {
                alert(data.message);
                const params = new URLSearchParams({
                    Message: data.error,
                    Type: 'RegisterError',
                    StatusCode: data.errorCode,
                    TimeStamp: new Date().toISOString()
                });
                window.location.href = `/Home/Error?${params.toString()}`;
            }
        }))          
    } 
    catch (e)
    {
        alert(e.message);
        return false;
    }
    return true;
}

function PrivacyPolicy(){
    
}