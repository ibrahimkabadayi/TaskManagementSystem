

async function openTaskModal(taskName, taskGroupName, sectionName) {
    
    const modal = document.getElementById('taskModalOverlay');
    if (!modal) return;
    let taskData;
    
    await fetch('/Task/GetTaskDetails',
        {
            method: 'GET',
            body: JSON.stringify({
                TaskTitle: taskName,
                TaskGroupName: taskGroupName,
                SectionName: sectionName
            })
        })
        .then((response) => {
        if (!response.ok) {
             alert(response.message);
        }
        return response.json;
    }).then((result) => {
            taskData = {
                title: result.title,
                description: result.description,
                createdByName: result.createdByName,
                createdByInitial: result.createdByInitial,
                createdByColor: result.createdByColor,
                assignedToName: result.assignedToName,
                assignedInitial: result.assignedInitial,
                assignedColor: result.assignedColor,
                createdDate: result.createdDate,
                dueDate: result.dueDate,
                priorityValue: result.priority
            }
    })
    

    const titleInput = document.getElementById('modalTaskTitle');
    titleInput.value = taskData.title || '';
    titleInput.style.height = 'auto';
    titleInput.style.height = titleInput.scrollHeight + 'px';

    document.getElementById('modalListName').innerText = taskData.listName;

    const createdAvatar = document.getElementById('modalCreatedByAvatar');
    const cInitial = taskData.createdByInitial; 

    createdAvatar.innerText = cInitial;
    createdAvatar.style.backgroundColor = taskData.createdByColor;
    createdAvatar.title = `Created By: ${taskData.createdByName}`;

    createdAvatar.onclick = function(event) {
        openUserProfile(event, cInitial);
    };


    const assignedAvatar = document.getElementById('modalAssignedToAvatar');

    if (taskData.assignedToName) {
        const aInitial = taskData.assignedInitial;

        assignedAvatar.style.display = 'flex';
        assignedAvatar.innerText = aInitial;

        assignedAvatar.style.backgroundColor = mockUserData[aInitial] ? mockUserData[aInitial].color : (taskData.assignedColor || '#0079bf');
        assignedAvatar.title = `Assigned: ${taskData.assignedToName}`;

        assignedAvatar.style.cursor = 'pointer';
        assignedAvatar.onclick = function(event) {
            openUserProfile(event, aInitial);
        };

    } else {
        assignedAvatar.style.display = 'none'; 
    }

    document.getElementById('modalCreatedDate').innerText = taskData.createdDate;
    document.getElementById('modalDueDate').innerText = taskData.dueDate;

    document.getElementById('modalTaskDesc').value = taskData.description;

    const prioritySelect = document.getElementById('modalPrioritySelect');
    if(taskData.priorityValue) {
        prioritySelect.value = taskData.priorityValue;
    } else {
        prioritySelect.value = 'medium';
    }

    modal.style.display = 'flex';
}

function closeTaskModal(event) {
    if (!event) {
        document.getElementById('taskModalOverlay').style.display = 'none';
        return;
    }
    if (event.target.id === 'taskModalOverlay') {
        document.getElementById('taskModalOverlay').style.display = 'none';
    }
}
function showAddCardForm(btnElement) {
    const footer = btnElement.parentElement;

    btnElement.style.display = 'none';

    const form = footer.querySelector('.add-card-form');
    form.style.display = 'block';

    const input = form.querySelector('textarea');
    input.focus();
}

function hideAddCardForm(btnElement) {
    const footer = btnElement.closest('.task-footer');

    const form = footer.querySelector('.add-card-form');
    form.style.display = 'none';

    form.querySelector('textarea').value = '';

    const addBtn = footer.querySelector('.add-task-btn');
    addBtn.style.display = 'flex';
}

async function saveNewCard(btnElement, userId) {
    const taskGroupName = btnElement.parentElement.querySelector('.task-group').children.item(0).textContent;
    const sectionName = document.getElementsByClassName('section-title').item(0).textContent;
    const footer = btnElement.closest('.task-footer');
    const input = footer.querySelector('textarea');
    const title = input.value.trim();

    if (title) {
        const newCard = document.createElement('div');
        
        await fetch('/Task/SaveTask', 
            {
                method: 'POST',
                body: JSON.stringify({
                    UserId: userId,
                    TaskTitle: taskGroupName,
                    TaskGroupName: taskGroupName,
                    SectionName: sectionName,
                })
            })
            .then((response) => {
                if (!response.ok) {
                    alert("Could not add another task please try again.");
                }
            });
        
        
        newCard.className = 'Task';
        newCard.onclick = function() {
            openTaskModal(title, taskGroupName, sectionName)
        };
        
        newCard.innerHTML = `
                <div class="Task-Title">${title}</div>
            `;

        makeCardDraggable(newCard);

        footer.parentElement.insertBefore(newCard, footer);

        input.value = '';
        input.focus();
    }
}

function handleEnterKey(event, inputElement, userId) {
    if (event.key === 'Enter' && !event.shiftKey) {
        event.preventDefault();
        const saveBtn = inputElement.parentElement.querySelector('.btn-add-card');
        saveNewCard(saveBtn, userId);
    }
}
function showAddListForm(btnElement) {
    const wrapper = btnElement.parentElement;
    wrapper.classList.add('active');
    btnElement.style.display = 'none';
    wrapper.querySelector('.add-list-form').style.display = 'block';
    wrapper.querySelector('input').focus();
}

function hideAddListForm(btnElement) {
    const wrapper = btnElement.closest('.add-list-wrapper');
    wrapper.classList.remove('active');
    wrapper.querySelector('.add-list-form').style.display = 'none';
    wrapper.querySelector('.add-list-btn-idle').style.display = 'flex';
    wrapper.querySelector('input').value = '';
}

async function saveNewList(btnElement, sectionId, userId) {
    const wrapper = btnElement.closest('.add-list-wrapper');
    const input = wrapper.querySelector('input');
    const listTitle = input.value.trim();

    if (listTitle) {
        
        await fetch('/TaskGroup/SaveNewTaskGroup/',
            {
                method: 'POST',
                body: JSON.stringify({
                    Name: listTitle,
                    SectionId: sectionId,
                    userId: userId
                })
            }).then(async (response) => {
                if (!response.ok) {
                    alert("Could not add another task group please try again.");
                    return false;
                }
        })
        
        const newListHTML = `
            <div class="task-group">
                <div class="task-group-header" style="display: flex; justify-content: space-between; align-items: center;">
                    <div class="task-group-title">${listTitle}</div>
                    <div class="task-group-menu" style="margin-left: auto; cursor: pointer;" onclick="openListMenu(event, this)">
                        <i class="fa-solid fa-ellipsis"></i>
                    </div>
                </div>
                
                <div class="task-footer">
                    <div class="add-task-btn" onclick="showAddCardForm(this)">
                        <i class="fa-solid fa-plus"></i> Kart ekle
                    </div>
                    <div class="add-card-form" style="display: none;">
                        <textarea class="card-composer-input" placeholder="Kart başlığı..." rows="3" onkeydown="handleEnterKey(event, this)"></textarea>
                        <div class="composer-controls">
                            <div class="composer-left">
                                <button class="btn-add-card" onclick="saveNewCard(this)">Ekle</button>
                                <button class="btn-close-composer" onclick="hideAddCardForm(this)"><i class="fa-solid fa-xmark"></i></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`;

        wrapper.insertAdjacentHTML('beforebegin', newListHTML);

        const newListElement = wrapper.previousElementSibling;
        if(newListElement) {
            makeColumnDroppable(newListElement);
        }

        input.value = '';
        hideAddListForm(btnElement);
    }
}

function handleListEnterKey(event, inputElement, sectionId, userId) {
    if (event.key === 'Enter') {
        const saveBtn = inputElement.parentElement.querySelector('.btn-add-card');
        saveNewList(saveBtn, sectionId, userId);
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const cards = document.querySelectorAll('.Task');
    const columns = document.querySelectorAll('.task-group');

    cards.forEach(card => makeCardDraggable(card));
    columns.forEach(column => makeColumnDroppable(column));
});

function makeCardDraggable(card) {
    card.setAttribute('draggable', 'true');

    card.addEventListener('dragstart', () => {
        card.classList.add('dragging');
    });

    card.addEventListener('dragend', () => {
        card.classList.remove('dragging');
    });
}

function makeColumnDroppable(column) {

    column.addEventListener('dragover', e => {
        e.preventDefault();
        const afterElement = getDragAfterElement(column, e.clientY);
        const draggable = document.querySelector('.dragging');
        
        if (!draggable) return;

        const taskListContent = column.querySelector('.task-list-content') || column;

        if (afterElement == null) {
            taskListContent.appendChild(draggable);
        } else {
            taskListContent.insertBefore(draggable, afterElement);
        }
    });
    column.addEventListener('drop', async e => {
        e.preventDefault();
        const draggable = document.querySelector('.dragging');
        if (!draggable) return;

        const taskListContent = column.querySelector('.task-list-content') || column;
        
        const currentTasksInOrder = [...taskListContent.querySelectorAll('.Task')];

        const newPositionIndex = currentTasksInOrder.indexOf(draggable);

        const taskId = draggable.id;
        const newTaskGroupId = column.id;
        
        try {
            const response = await fetch('/Task/ChangeTaskGroup', {
                method: 'PATCH', 
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    TaskId: taskId,
                    TaskGroupId: newTaskGroupId, 
                    NewPosition: newPositionIndex
                })
            });

            if (!response.ok) {
                alert("Could not change task group.");
                return false;
            }
        } catch (error) {
            alert(error.message);
            return false;
        }
    });
}

function getDragAfterElement(container, y) {
    const draggableElements = [...container.querySelectorAll('.Task:not(.dragging)')];

    return draggableElements.reduce((closest, child) => {
        const box = child.getBoundingClientRect();
        const offset = y - box.top - box.height / 2;

        if (offset < 0 && offset > closest.offset) {
            return { offset: offset, element: child };
        } else {
            return closest;
        }
    }, { offset: Number.NEGATIVE_INFINITY }).element;
}

function toggleBackgroundMenu(sectionId) {
    const menu = document.getElementById('backgroundMenu');

    if (!menu.classList.contains('open') && document.getElementById('photoGrid').children.length === 0) {
        initBackgroundOptions(sectionId);
    }

    menu.classList.toggle('open');
}

function initBackgroundOptions(sectionId) {

    const colors = ['#0079bf', '#d29034', '#519839', '#b04632', '#89609e', '#cd5a91', '#4bbf6b', '#00aecc'];
    const colorGrid = document.getElementById('colorGrid');

    colors.forEach(color => {
        const div = document.createElement('div');
        div.className = 'bg-option color-box';
        div.style.backgroundColor = color;
        div.onclick = () => setBackgroundColor(color);
        colorGrid.appendChild(div);
    });

    const photoGrid = document.getElementById('photoGrid');

    for (let i = 20; i <= 40; i++) {
        const thumbUrl = `https://picsum.photos/seed/${i * 100}/200/120`;
        const fullUrl = `https://picsum.photos/seed/${i * 100}/2560/1440`;

        const div = document.createElement('div');
        div.className = 'bg-option';
        div.style.backgroundImage = `url('${thumbUrl}')`;

        div.onclick = () => setBackgroundImage(fullUrl, sectionId);

        photoGrid.appendChild(div);
    }
}

async function setBackgroundImage(url, sectionId) {
    document.body.style.backgroundImage = `url('${url}')`;
    document.body.style.backgroundColor = ''; 
    
    await fetch('/Section/UpdateBackgroundUrl/', 
        {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                SectionId: sectionId,
                Url: url
            })
        }).then( response => {
            if (!response.ok) {
                alert(response.message);
                return false;
            }
    })
    
    document.querySelector('.top-bar').style.backgroundColor = 'rgba(0, 0, 0, 0.3)';
}

function setBackgroundColor(color) {
    document.body.style.backgroundImage = 'none'; 
    document.body.style.backgroundColor = color;
    
    document.querySelector('.top-bar').style.backgroundColor = 'rgba(0, 0, 0, 0.2)';
}

/* =========================================
LİSTE MENÜSÜ FONKSİYONLARI
========================================= */

let currentListElement = null;

function openListMenu(event, iconElement) {
    event.stopPropagation();
    const menu = document.getElementById('listActionMenu');

    currentListElement = iconElement.closest('.task-group');

    menu.style.display = 'flex';

    const rect = iconElement.getBoundingClientRect();
    menu.style.top = (rect.bottom + 5) + 'px';
    menu.style.left = (rect.left) + 'px';
}

function closeListMenu() {
    document.getElementById('listActionMenu').style.display = 'none';
}

document.addEventListener('click', (e) => {
    const menu = document.getElementById('listActionMenu');
    if (!menu.contains(e.target)) {
        closeListMenu();
    }
});

function actionEditTitle() {
    if (!currentListElement) return;

    const titleDiv = currentListElement.querySelector('.task-group-title');
    const taskGroupId = titleDiv.id;

    const textNode = titleDiv.childNodes[0];
    const currentText = textNode.nodeValue.trim();

    const input = document.createElement('input');
    input.type = 'text';
    input.className = 'edit-list-title-input';
    input.value = currentText;

    titleDiv.style.display = 'none';

    titleDiv.parentNode.insertBefore(input, titleDiv);

    input.focus();
    input.select();

     const saveAndClose = async () => {
         const newTitle = input.value.trim();

         if (newTitle) {
             textNode.nodeValue = newTitle + " ";
         }

         await fetch('/TaskGroup/ChangeTaskGroupName/', {
             method: 'PATCH',
             headers: {
                 'Content-Type': 'application/json'
             },
             body: JSON.stringify({
                 TaskGroupId: taskGroupId,
                 NewTaskGroupName: newTitle
             })
         }).then(response => {
             if (!response.ok) {
                alert("Could not change task group name.");
                return false;
             }
         })
         
         input.remove();
         titleDiv.style.display = 'block';
     };

    input.addEventListener('keydown', (e) => {
        if (e.key === 'Enter') {
            saveAndClose();
        }
    });

    input.addEventListener('blur', saveAndClose);

    closeListMenu();
}

// B. Yeni Task Ekle (Footer'daki butonu tetikler)
function actionAddTask() {
    if (!currentListElement) return;

    // Footer'daki "Add Task" butonunu bul ve tıkla
    const addTaskBtn = currentListElement.querySelector('.add-task-btn');
    if (addTaskBtn) {
        // Scroll ile en alta git ki form görünsün
        const listContent = currentListElement; // Scroll edilecek alan
        addTaskBtn.click();
        // Hafif bir gecikmeyle inputa odaklan
        setTimeout(() => listContent.scrollTop = listContent.scrollHeight, 100);
    }
    closeListMenu();
}

// C. Listeyi Sil
function actionDeleteList() {
    if (!currentListElement) return;

    if (confirm("Bu listeyi ve içindeki tüm kartları silmek istediğinize emin misiniz?")) {
        currentListElement.remove();
    }
    closeListMenu();
}

// D. Sıralama (Basit Algoritma)
function actionSortTasks(criteria) {
    if (!currentListElement) return;

    // Kartların olduğu container'ı bulmak lazım.
    // Senin HTML yapında kartlar direkt .task-group içinde, footer'dan önce.
    // O yüzden footer hariç tüm .Task elemanlarını alacağız.

    const tasks = Array.from(currentListElement.querySelectorAll('.Task'));
    const footer = currentListElement.querySelector('.task-footer');

    if (criteria === 'date') {
        // Tarih verisi olmadığı için DOM sırasını tersine çeviriyoruz (En yeni en üstte)
        tasks.reverse().forEach(task => {
            currentListElement.insertBefore(task, footer);
        });
    }
    else if (criteria === 'priority') {
        // İçinde "Yüksek" yazanları en üste al
        tasks.sort((a, b) => {
            const priorityA = a.innerText.toLowerCase().includes('yüksek') ? 1 : 0;
            const priorityB = b.innerText.toLowerCase().includes('yüksek') ? 1 : 0;
            return priorityB - priorityA; // 1 olan (yüksek) üste çıkar
        });

        tasks.forEach(task => {
            currentListElement.insertBefore(task, footer);
        });
    }

    closeListMenu();
}
/* =========================================
PAYLAŞIM MENÜSÜ FONKSİYONLARI
========================================= */

function openShareModal() {
    const modal = document.getElementById('shareModalOverlay');
    modal.style.display = 'flex'; // Overlay'i görünür yap
}

function closeShareModal(event) {
    const modal = document.getElementById('shareModalOverlay');

    // Eğer null gönderildiyse (X butonu) veya direkt overlay'e tıklandıysa kapat
    if (!event || event.target === modal) {
        modal.style.display = 'none';
    }
}
/* =========================================
KULLANICI PROFİL MANTIĞI
========================================= */

// 1. Sanal Kullanıcı Veritabanı
const mockUserData = {
    'MK': { fullName: 'Mehmet Kaya', email: 'mehmet.kaya@taskflow.com', role: 'Frontend Dev', color: '#df5c4e' },
    'CY': { fullName: 'Can Yılmaz', email: 'can.yilmaz@taskflow.com', role: 'Backend Lead', color: '#4bbf6b' },
    'İK': { fullName: 'İbrahim Kabadayı', email: 'ibrahim@taskflow.com', role: 'Project Manager', color: '#0079bf' }
};

let activeProfilePopup = null;

// 2. Profili Açan Fonksiyon
function openUserProfile(event, initials) {
    event.stopPropagation(); // Sayfa tıklamasını durdur

    const popup = document.getElementById('userProfileCard');
    const user = mockUserData[initials];

    if (!user) {
        console.error("Kullanıcı verisi bulunamadı: " + initials);
        return;
    }

    // A. Bilgileri Doldur
    document.getElementById('profileCardAvatar').innerText = initials;
    document.getElementById('profileCardAvatar').style.backgroundColor = user.color;
    document.getElementById('profileCardName').innerText = user.fullName;
    document.getElementById('profileCardEmail').innerText = user.email;
    document.getElementById('profileCardRole').innerText = user.role;

    // B. Taskları Tara ve Bul (DOM'dan okuma)
    const taskData = findTasksForUser(initials);

    document.getElementById('profileCardCount').innerText = taskData.count;

    // Task Listesini Render Et
    const listContainer = document.getElementById('profileCardTaskList');
    listContainer.innerHTML = ''; // Temizle

    if (taskData.tasks.length === 0) {
        listContainer.innerHTML = '<div class="empty-task-msg">Şu an aktif görevi yok.</div>';
    } else {
        taskData.tasks.forEach(taskTitle => {
            const div = document.createElement('div');
            div.className = 'mini-task-item';
            div.innerText = taskTitle;
            listContainer.appendChild(div);
        });
    }

    // C. Pozisyonlama (Tıklanan ikonun altına)
    popup.style.display = 'flex';

    const rect = event.currentTarget.getBoundingClientRect();
    // Popup'ı ikonun ortasına hizala ama ekran dışına taşmasın
    let leftPos = rect.left;
    if(leftPos + 320 > window.innerWidth) {
        leftPos = window.innerWidth - 340; // Sağa çok yakınsa sola çek
    }

    popup.style.top = (rect.bottom + 10) + 'px';
    popup.style.left = leftPos + 'px';
}

// 3. Kullanıcının Tasklarını Bulan Fonksiyon
function findTasksForUser(targetInitials) {
    const allTasks = document.querySelectorAll('.Task');
    let count = 0;
    let taskTitles = [];

    allTasks.forEach(task => {
        // Task içindeki avatarı bul
        const avatar = task.querySelector('.assigned-to-profile-icon');
        if (avatar) {
            // Avatarın içindeki metni (Örn: 'MK') al ve boşlukları temizle
            const assignedInitials = avatar.innerText.trim();

            if (assignedInitials === targetInitials) {
                count++;
                // Task başlığını al
                const title = task.querySelector('.Task-Title').innerText;
                taskTitles.push(title);
            }
        }
    });

    return { count: count, tasks: taskTitles };
}

// 4. Kapatma Fonksiyonu
function closeUserProfile() {
    document.getElementById('userProfileCard').style.display = 'none';
}

// Dışarı tıklayınca kapat
document.addEventListener('click', (e) => {
    const popup = document.getElementById('userProfileCard');
    if (!popup.contains(e.target)) {
        closeUserProfile();
    }
});
/* =========================================
FİLTRELEME FONKSİYONLARI
========================================= */

// 1. Menüyü Aç/Kapa
function toggleFilterMenu() {
    const menu = document.getElementById('filterMenuPopup');
    const btn = document.querySelector('.settings'); // Filtrele butonu class'ı

    if (menu.style.display === 'none') {
        menu.style.display = 'flex';
        // Butonun altına hizala
        const rect = btn.getBoundingClientRect();
        menu.style.top = (rect.bottom + 10) + 'px';
        menu.style.left = rect.left + 'px'; // Sola hizalı kalsın
    } else {
        menu.style.display = 'none';
    }
}

// 2. Filtreleri Uygula (Ana Motor)
function applyFilters() {
    const searchVal = document.getElementById('filterSearchInput').value.toLowerCase();

    // Seçili checkbox'ları bul
    const checkboxes = document.querySelectorAll('.filter-checkbox:checked');
    const selectedMembers = Array.from(checkboxes).map(cb => cb.value); // ['MK', 'CY'] gibi

    // Tüm kartları gez
    const allTasks = document.querySelectorAll('.Task');

    allTasks.forEach(task => {
        let isVisible = true;

        // A. Kelime Kontrolü
        const title = task.querySelector('.Task-Title').innerText.toLowerCase();
        if (searchVal && !title.includes(searchVal)) {
            isVisible = false;
        }

        // B. Üye Kontrolü (Eğer en az bir üye seçiliyse)
        if (isVisible && selectedMembers.length > 0) {
            const avatarDiv = task.querySelector('.assigned-to-profile-icon');

            if (avatarDiv) {
                // Kartın üstündeki harfleri al (Örn: CY)
                const assignedInitial = avatarDiv.innerText.trim();

                // Seçilenlerin içinde bu kişi var mı?
                if (!selectedMembers.includes(assignedInitial)) {
                    isVisible = false;
                }
            } else {
                // Kartta avatar yok (Atanmamış)
                // Eğer "NO_ASSIGN" seçili değilse gizle
                if (!selectedMembers.includes('NO_ASSIGN')) {
                    isVisible = false;
                }
            }
        }

        // Kararı uygula
        task.style.display = isVisible ? 'block' : 'none';
    });
}

// 3. Filtreleri Temizle
function clearAllFilters() {
    // Inputu temizle
    document.getElementById('filterSearchInput').value = '';

    // Checkboxları kaldır
    const checkboxes = document.querySelectorAll('.filter-checkbox');
    checkboxes.forEach(cb => cb.checked = false);

    // Filtre fonksiyonunu tekrar çağır (Hepsi görünür olacak)
    applyFilters();
}
