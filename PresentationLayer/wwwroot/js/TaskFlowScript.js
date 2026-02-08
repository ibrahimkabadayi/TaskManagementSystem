function NewWorkspaceClick() {
    const formHtml = `
    <div class="form-group">
        <label>Workspace Name <span style="color:red">*</span></label>
        <input type="text" id="wsName" class="form-input" placeholder="e.g., TaskFlow Web Project, Marketing Team...">
        <div id="wsNameError" style="color:red; font-size:12px; display:none; margin-top:4px;">⚠️ This field is required</div>
    </div>

    <div class="form-group">
        <label>Description (Optional)</label>
        <textarea id="wsDesc" class="form-textarea" placeholder="What will your team use this workspace for?"></textarea>
    </div>
`;

    showModal("Create New Workspace", formHtml, async () => {
        const nameInput = document.getElementById('wsName');
        const descInput = document.getElementById('wsDesc');

        if (!nameInput.value.trim()) {
            nameInput.style.borderColor = "red";
            document.getElementById('wsNameError').style.display = "block";
            return;
        }

        const payload = {
            Name: nameInput.value,
            Description: descInput.value
        };

        console.log("Creating Workspace:", payload);

        try {
            const response = await fetch('/Section/CreateProject/', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(payload)
            });
            
            
            if (response.ok) {
                window.location.reload();
            }else{
                console.log("Error:", response.statusText);   
            }

        } catch (error) {

        }

        closeModal();
    });
}

function closeModal() {
    document.getElementById('dynamicModal').style.display = 'none';
    const sideModal = document.getElementById('sideModal');
    if(sideModal) sideModal.style.display = 'none';
}
function ShowAllBackgrounds(event) {
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

const originalCloseModal = window.closeModal;
window.closeModal = function() {
    document.getElementById('dynamicModal').style.display = 'none';
    closeSideModal();
}

function selectBackground(val, type, element) {
    selectedBackground = val;

    updatePreview(val, type);

    document.querySelectorAll('.bg-option-top, .bg-option-bot').forEach(b => b.classList.remove('selected'));
    if(element) element.classList.add('selected');

    closeSideModal();
}

function selectFromSidePanel(val, type, element) {
    selectedBackground = val;

    updatePreview(val, type);

    document.querySelectorAll('.side-photo-item').forEach(b => b.classList.remove('selected'));
    element.classList.add('selected');
}

function updatePreview(val, type) {
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
}

function closeSideModal() {
    document.getElementById('sideModal').style.display = 'none';
}

const originalClose = window.closeModal;
window.closeModal = function() {
    document.getElementById('dynamicModal').style.display = 'none';
    closeSideModal();
}

function generateImageCookies() {
    let images = [];
    for (let i = 20; i <= 50; i++) {
        images.push({
            id: i,
            thumbUrl: `https://picsum.photos/seed/${i * 100}/200/120`,
            fullUrl: `https://picsum.photos/seed/${i * 100}/1920/1080`
        });
    }
    return images;
}

let generatedImages = generateImageCookies();
let selectedBackground = "#0079bf";

function NewSectionClick() {
    const template = document.getElementById('createSectionTemplate');
    if (!template) {
        return;
    }

    const formHtml = template.innerHTML;

    showModal("Create Section", formHtml, async () => {
        const nameInput = document.getElementById('newBoardName');
        const workspaceSelect = document.getElementById('workspaceSelect');

        if (!nameInput.value.trim()) {
            nameInput.style.borderColor = "red";
            document.getElementById('inputError').style.display = "block";
            return;
        }

        try {
            const payload = {
                Name: nameInput.value,
                ImageUrl: selectedBackground,
                ProjectId: workspaceSelect.value
            };

            console.log("Data Sent:", payload);

            const response = await fetch('/Section/CreateSection/', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if(response.ok) {
                window.location.reload();
            } else {
                console.error("An error occurred");
            }

        } catch (error) {
            console.error("Error:", error);
        }
        closeModal();
    });

    const imageContainer = document.getElementById('modalTopImages');
    if (imageContainer && typeof generatedImages !== 'undefined') {
        const topImages = generatedImages.slice(0, 4);

        const imagesHtml = topImages.map(img => `
            <button class="bg-option-top" 
                    onclick="selectBackground('${img.fullUrl}', 'image', this)"
                    style="background-image: url('${img.thumbUrl}')">
            </button>
        `).join('');

        imageContainer.innerHTML = imagesHtml;
    }

    selectedBackground = "#0079bf";
}

function MembersButtonClick(projectId){
    window.location.href = `/Section/SectionUsers?projectId=${projectId}`;
}

function getTodayString() {
    const d = new Date();
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

function SettingsButtonClick(id, currentName, currentDesc, start, end) {
    const template = document.getElementById('settingsTemplate');
    if (!template) return;

    const formHtml = template.innerHTML;

    showModal("Workspace Settings", formHtml, async () => {
        const wsId = document.getElementById('editWsId').value;
        const newName = document.getElementById('editWsName').value;
        const newDesc = document.getElementById('editWsDesc').value;
        const newStart = document.getElementById('editWsStart').value;
        const newEnd = document.getElementById('editWsEnd').value;

        if (!newName.trim()) {
            alert("Name cannot be empty!");
            return;
        }

        if (newEnd && newStart && newEnd < newStart) {
            alert("Error: End date cannot be earlier than the start date!");
            return;
        }

        try {
            const response = await fetch('/Section/UpdateProject', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    ProjectId: wsId,
                    Name: newName,
                    Description: newDesc,
                    StartDate: newStart,
                    EndDate: newEnd ? newEnd : null
                })
            });

            if (response.ok) {
                window.location.reload();
            } else {
                alert("An error occurred during the update.");
            }
        } catch (error) {
            console.error("Error:", error);
        }
        closeModal();
    });

    setTimeout(() => {
        document.getElementById('editWsId').value = id;
        document.getElementById('editWsName').value = currentName;
        document.getElementById('editWsDesc').value = (currentDesc && currentDesc !== 'null') ? currentDesc : '';
        document.getElementById('editWsStart').value = start;
        document.getElementById('editWsEnd').value = end;

        const startInput = document.getElementById('editWsStart');
        const endInput = document.getElementById('editWsEnd');
        endInput.min = getTodayString();

        startInput.addEventListener('change', function() {
            endInput.min = this.value;

            if (endInput.value && endInput.value < this.value) {
                endInput.value = this.value;
            }
        });

    }, 0);
}

async function DeleteWorkspace() {
    const wsId = document.getElementById('editWsId').value;

    if (!confirm("Are you sure you want to delete this workspace and ALL data within it? This action cannot be undone!")) {
        return;
    }

    try {
        const response = await fetch('/Section/DeleteProject/' + wsId, {
            method: 'DELETE',
        });

        if (response.ok) {
            window.location.reload();
        } else {
            alert("Delete operation failed.");
        }

    } catch (error) {
        console.error("Error:", error);
        alert("An error occurred.");
    }
}

document.addEventListener('keydown', (e) => {
    if (e.key === "Escape") closeModal();
});

function showModal(title, htmlContent, onConfirm) {
    const modal = document.getElementById('dynamicModal');
    document.getElementById('modalTitle').innerText = title;
    document.getElementById('modalBody').innerHTML = htmlContent;

    const confirmBtn = document.getElementById('modalConfirmBtn');

    const newBtn = confirmBtn.cloneNode(true);
    confirmBtn.parentNode.replaceChild(newBtn, confirmBtn);

    newBtn.addEventListener('click', () => {
        onConfirm();
    });

    modal.style.display = 'flex';
}

document.addEventListener('keydown', (e) => {
    if (e.key === "Escape") closeModal();
});
