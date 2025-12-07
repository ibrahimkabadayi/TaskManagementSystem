async function nextButtonClick(){
    const name = document.getElementById("name-Input").value;
    const email = document.getElementById("email-input").value;
    
    if(name === ""){
        alert("Please enter a name");
        return;
    }
    
    if(name.size < 3){
        alert("Please enter a valid name");
        return;
    }
    
    if(email === ""){
        alert("Please enter a email");
        return;
    }
    
    if(!email.contains("@") || email.charAt(0) === "@" || email.size() < 3) {
        alert("Please enter a valid email");
        return;
    }
    
    try{
        await fetch("/User/CreateAccountCheck/",{
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                Username: name,
                Email: email
            })
        }).then(response => response.json()).then(async data => {
            if (data.success) {
                await fetch('/Email/SendEmailVerificationCode', {
                    method: "POST",
                    headers: {"Content-Type": "application/json"},
                    body: JSON.stringify({
                        Email: email
                    })
                }).then(response => response.json()).then(async data => {
                    if (!data.success) {
                        alert(data.message);
                        return false;
                    }
                })
                
                const params = new URLSearchParams({
                    Email: email,
                    Name: name
                });
                window.location.href = `/User/EmailCodeVerification?${params.toString()}`;
            } else {
                alert(data.message);
                const params = new URLSearchParams({
                    Message: data.error,
                    Type: 'UserExists',
                    StatusCode: data.errorCode,
                    TimeStamp: new Date().toISOString()
                });
                window.location.href = `/Home/Error?${params.toString()}`;
            }
        })
    } catch (error) {
        const params = new URLSearchParams({
            Message: error.message,
            Type: error.type,
            StatusCode: error.errorCode,
            TimeStamp: new Date().toISOString()
        });
        window.location.href = `/Home/Error?${params.toString()}`;
    }
}