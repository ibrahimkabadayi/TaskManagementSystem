
function SectionClick(element){

}

function ShowAllBackgrounds(event){
    event.stopPropagation();
    event.preventDefault();

    const sideModal = document.getElementById('sideModal');
    const sideBody = document.getElementById('sideModalBody');

    let html = `
        <div class="side-cat-title">🖼️ Fotoğraflar</div>
        <div class="side-grid-photos">
            ${allBackgrounds.photos.map(url =>
        `<div class="side-item" onclick="selectFromSidePanel('${url}', 'image')" style="background-image: url('${url}')"></div>`
    ).join('')}
        </div>

        <div class="side-cat-title">🎨 Renkler</div>
        <div class="side-grid-colors">
            ${allBackgrounds.colors.map(color =>
        `<div class="side-item" onclick="selectFromSidePanel('${color}', 'color')" style="background-color: ${color}"></div>`
    ).join('')}
        </div>
    `;

    sideBody.innerHTML = html;

    const mainModalContent = document.querySelector('.modal-content');

    if (mainModalContent) {
        const rect = mainModalContent.getBoundingClientRect();

        sideModal.style.top = rect.top + 'px';
        sideModal.style.left = (rect.right + 10) + 'px';

        if (rect.right + 310 > window.innerWidth) {
            sideModal.style.left = (rect.right - 310) + 'px';
        }
    }

    sideModal.style.display = 'block';
}

function closeSideModal() {
    document.getElementById('sideModal').style.display = 'none';
}

function selectFromSidePanel(val, type) {
    if (type === 'image') {
        selectedBackground = `url('${val}')`;
    } else {
        selectedBackground = val;
    }

    const previewBox = document.getElementById('boardPreview');
    if (previewBox) {
        if (type === 'image') {
            previewBox.style.backgroundImage = `url('${val}')`;
            previewBox.style.backgroundColor = 'transparent';
        } else {
            previewBox.style.backgroundImage = 'none';
            previewBox.style.backgroundColor = val;
        }
    }

    document.querySelectorAll('.bg-option-top, .bg-option-bot').forEach(b => b.classList.remove('selected'));
}

const originalCloseModal = window.closeModal;
window.closeModal = function() {
    document.getElementById('dynamicModal').style.display = 'none';
    closeSideModal();
}

let selectedBackground = "#0079bf";
function NewSectionClick() {
    const formHtml = `
        <div class="create-board-container">
            <div id="boardPreview" class="board-preview-box">
                <div class="preview-illustration">
                    <div class="preview-col"></div>
                    <div class="preview-col"></div>
                    <div class="preview-col"></div>
                </div>
            </div>

            <div>
                <div class="bg-picker-title">Arkaplan</div>
                <div class="bg-grid-top">
                    <button class="bg-option-top" data-type="image" data-val="url('https://images.unsplash.com/photo-1470071459604-3b5ec3a7fe05?w=100&q=80')" style="background-image: url('https://images.unsplash.com/photo-1470071459604-3b5ec3a7fe05?w=100&q=80')"></button>
                    <button class="bg-option-top" data-type="image" data-val="url('https://images.unsplash.com/photo-1441974231531-c6227db76b6e?w=100&q=80')" style="background-image: url('https://images.unsplash.com/photo-1441974231531-c6227db76b6e?w=100&q=80')"></button>
                    <button class="bg-option-top" data-type="image" data-val="url('https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=100&q=80')" style="background-image: url('https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=100&q=80')"></button>
                    <button class="bg-option-top" data-type="image" data-val="url('https://images.unsplash.com/photo-1472214103451-9374bd1c798e?w=100&q=80')" style="background-image: url('https://images.unsplash.com/photo-1472214103451-9374bd1c798e?w=100&q=80')"></button>
                </div>
                <div class="bg-grid-bot">
                <button class="bg-option selected" data-type="color" data-val="#0079bf" style="background-color: #0079bf;"></button>
                    <button class="bg-option-bot" data-type="color" data-val="#d29034" style="background-color: #d29034;"></button>
                    <button class="bg-option-bot" data-type="color" data-val="#519839" style="background-color: #519839;"></button>
                    <button class="bg-option-bot" data-type="color" data-val="#b04632" style="background-color: #b04632;"></button>
                    <button class="bg-option-bot" data-type="color" data-val="#98167b" style="background-color: #519839;"></button>
                    <button class="bg-option-bot" data-type="color" data-val="#519839" style="background-color: #e4d5d5;"></button>
                    <button class="bg-option-bot" data-type="color" data-val="#519839" style="background-color: #eab815;"></button>
                    <button class="bg-option-bot" onclick="ShowAllBackgrounds(event)">...</button>
                </div>       
            </div>

            <div class="form-group">
                <label>Pano Başlığı <span style="color:red">*</span></label>
                <input type="text" id="newBoardName" class="form-input" placeholder="Örn: Yaz Stajı Planı">
                <div id="inputError" style="color:red; font-size:12px; display:none; margin-top:5px;">⚠️ Pano başlığı gerekli</div>
            </div>

            <div class="form-group">
                <label>Çalışma Alanı</label>
                <select id="workspaceSelect" class="form-select">
                    <option value="1">Term Project</option>
                    <option value="2">Kişisel İşler</option>
                </select>
            </div>

             <div class="form-group">
                <label>Görünürlük</label>
                <select class="form-select">
                    <option>🔒 Özel</option>
                    <option>💼 Çalışma Alanı</option>
                    <option>🌎 Herkese Açık</option>
                </select>
            </div>
        </div>
    `;

    showModal("Pano Oluştur", formHtml, () => {
        const nameInput = document.getElementById('newBoardName');

        if (!nameInput.value.trim()) {
            nameInput.style.borderColor = "red";
            document.getElementById('inputError').style.display = "block";
            return;
        }

        const payload = {
            name: nameInput.value,
            background: selectedBackground,
            workspaceId: document.getElementById('workspaceSelect').value
        };

        console.log("Creating Board with:", payload);
        // Burada AJAX isteği atılacak... createBoard(payload);
        closeModal();
    });

    const bgButtons = document.querySelectorAll('.bg-option-top, .bg-option-bot');
    const previewBox = document.getElementById('boardPreview');

    bgButtons.forEach(btn => {
        btn.addEventListener('click', function() {

            bgButtons.forEach(b => b.classList.remove('selected'));
            this.classList.add('selected');

            const val = this.getAttribute('data-val');
            selectedBackground = val;


            if (this.getAttribute('data-type') === 'image') {
                previewBox.style.backgroundImage = val;
                previewBox.style.backgroundColor = 'transparent';
            } else {
                previewBox.style.backgroundImage = 'none';
                previewBox.style.backgroundColor = val;
            }
        });
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
