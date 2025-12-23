function SectionClick(element){
    
}

function NewSectionClick(){
    const formHtml = `
        <label style="display:block; margin-bottom:5px;">Section Name:</label>
        <input type="text" id="newSectionName" class="form-control" placeholder="Example: To-Do" style="width:100%; padding:8px;">
    `;
    
    showModal("Create new section.", formHtml, () => {
        const name = document.getElementById('newSectionName').value;
        if(name) {
            console.log("Creating new pane: " + name);
            
        } else {
            alert("Please enter a name!");
        }
    });
}

function BoardsButtonClick(){
    
}

function MembersButtonClick(){
    
}

function SettingsButtonClick(){
    
}

function showModal(title, htmlContent, onConfirm) {
    const modal = document.getElementById('dynamicModal');
    
    document.getElementById('modalTitle').innerText = title;
    document.getElementById('modalBody').innerHTML = htmlContent;
    
    const confirmBtn = document.getElementById('modalConfirmBtn');
    
    const newBtn = confirmBtn.cloneNode(true);
    confirmBtn.parentNode.replaceChild(newBtn, confirmBtn);
    
    newBtn.addEventListener('click', () => {
        onConfirm(); 
        closeModal(); 
    });
    
    modal.style.display = 'flex';
}

function closeModal() {
    document.getElementById('dynamicModal').style.display = 'none';
}

document.addEventListener('keydown', (e) => {
    if (e.key === "Escape") closeModal();
});