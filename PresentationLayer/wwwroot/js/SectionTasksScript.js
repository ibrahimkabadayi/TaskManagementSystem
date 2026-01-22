let currentOpenedTaskId = 0;
async function openTaskModal(TaskId) {
    currentOpenedTaskId = TaskId;
    const modal = document.getElementById('taskModalOverlay');
    if (!modal) return;

    try {
        const response = await fetch(`/Task/GetTaskDetails/${TaskId}`, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) {
            alert("Could not get task details.");
            return;
        }

        const result = await response.json();

        const taskData = {
            title: result.title,
            description: result.description,
            listName: result.listName,
            createdByName: result.createdByName,
            createdByInitial: result.createdByInitial,
            createdColor: result.createdByColor,
            assignedToName: result.assignedToName,
            assignedInitial: result.assignedInitial,
            assignedColor: result.assignedColor,
            createdDate: result.createdDate,
            dueDate: result.dueDate,
            priorityValue: result.priority,
            state: result.state
        };

        const titleInput = document.getElementById('modalTaskTitle');
        titleInput.value = taskData.title || '';
        titleInput.style.height = 'auto';
        titleInput.style.height = titleInput.scrollHeight + 'px';

        const listNameEl = document.getElementById('modalListName');
        if (listNameEl) listNameEl.innerText = taskData.listName || '...';

        const createdAvatar = document.getElementById('modalCreatedByAvatar');
        const createdName = document.getElementById('modalCreatedByName');

        if (createdAvatar) {
            createdAvatar.innerText = taskData.createdByInitial || '?';
            createdAvatar.style.backgroundColor = taskData.createdColor || '#dfe1e6';

            createdAvatar.onclick = function(event) {
                if(typeof openUserProfile === 'function') openUserProfile(event, taskData.createdByInitial);
            };
        }
        if (createdName) {
            createdName.innerText = taskData.createdByName || 'Unknown';
        }

        const assignedAvatar = document.getElementById('modalAssignedToAvatar');
        const assignedName = document.getElementById('modalAssignedToName');
        const assignedWrapper = document.getElementById('modalAssignedWrapper');

        if (assignedAvatar && assignedName) {
            if (taskData.assignedToName) {
                assignedAvatar.innerText = taskData.assignedInitial || '';
                assignedAvatar.style.backgroundColor = taskData.assignedColor || '#0079bf';
                assignedAvatar.style.color = '#fff';
                assignedAvatar.classList.remove('unassigned-icon');

                assignedName.innerText = taskData.assignedToName;
                assignedName.style.color = '#172b4d';
                assignedName.style.fontStyle = 'normal';

                if (assignedWrapper) {
                    assignedWrapper.onclick = function(event) {
                        if(typeof openUserProfile === 'function') openUserProfile(event, taskData.assignedInitial);
                    };
                }
            } else {
                assignedAvatar.innerText = '+';
                assignedAvatar.style.backgroundColor = 'transparent';
                assignedAvatar.style.color = '#5e6c84';
                assignedAvatar.classList.add('unassigned-icon');

                assignedName.innerText = 'Kişi Ata';
                assignedName.style.color = '#5e6c84';
                assignedName.style.fontStyle = 'italic';

                if (assignedWrapper) assignedWrapper.onclick = null;
            }
        }

        const createdDateEl = document.getElementById('modalCreatedDate');
        if (createdDateEl) createdDateEl.innerText = taskData.createdDate || '-';

        const dueDateEl = document.getElementById('modalDueDate');
        if (dueDateEl) dueDateEl.innerText = taskData.dueDate || 'No date';

        const dueDateInput = document.getElementById('modalDueDateInput');
        if (dueDateInput) {
            const dateStr = taskData.dueDate || "";
            if (dateStr) {
                const months = {
                    "Jan": "01", "Feb": "02", "Mar": "03", "Apr": "04", "May": "05", "Jun": "06",
                    "Jul": "07", "Aug": "08", "Sep": "09", "Oct": "10", "Nov": "11", "Dec": "12"
                };
                const parts = dateStr.split(' ');

                if (parts.length === 3) {
                    const day = parts[0].padStart(2, '0');
                    const month = months[parts[1]];
                    const year = parts[2];
                    dueDateInput.value = `${year}-${month}-${day}`;
                }
            } else {
                dueDateInput.value = "";
            }
        }

        const descInput = document.getElementById('modalTaskDesc');
        if (descInput) descInput.value = taskData.description || '';

        const prioritySelect = document.getElementById('modalPrioritySelect');
        if (prioritySelect) {
            prioritySelect.value = (taskData.priorityValue || 'medium').toLowerCase();
        }

        modal.style.display = 'flex';

    } catch (error) {
        console.error(error);
    }
}

async function changeTaskPriority() {
    const prioritySelect = document.getElementById('modalPrioritySelect');
    const priority = await prioritySelect.value;
    
    await fetch('Task/ChangeTaskPriority', {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        },
        body: {
            TaskId: currentOpenedTaskId,
            Priority: priority
        }
    }).then((response) => {
        if (!response.ok) {
            alert("Fetch error for changing task priority");
        }
    })
}

async function deleteTask(userId) {
    const taskId = currentOpenedTaskId;

    const sectionTitle = document.querySelector('.section-title') || document.querySelector('.task-group-title');
    const projectId = sectionTitle ? sectionTitle.getAttribute('project-id') : 0;

    try {
        const response = await fetch('/Task/DeleteTask/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                TaskId: taskId,
                UserId: userId,
                ProjectId: projectId
            })
        });

        if (response.ok) {
            const taskCard = document.getElementById(taskId);

            if (taskCard) {
                taskCard.remove();
            } else {
                const taskCardByData = document.querySelector(`.Task[data-id="${taskId}"]`);
                if (taskCardByData) taskCardByData.remove();
            }

            closeTaskModal(null);

            console.log(`Task ${taskId} successfully deleted.`);

        } else {
            alert("There was a problem deleting task.!");
        }

    } catch (error) {
        console.error("Deleting error:", error);
        alert("Could not send api.");
    }
}
function closeTaskModal(event) {
    currentOpenedTaskId = 0;
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

async function saveNewCard(btnElement, userId, sectionId) {
    const taskGroup = btnElement.closest('.task-group');

    const taskGroupName = taskGroup.querySelector('.task-group-title').innerText.trim();

    const sectionElement = document.querySelector('.section-title');
    const sectionName = sectionElement ? sectionElement.textContent.trim() : "Default Section";

    const footer = btnElement.closest('.task-footer');
    const input = footer.querySelector('textarea');
    const title = input.value.trim();

    if (title) {
        const newCard = document.createElement('div');
        
        await fetch('/Task/SaveTask', 
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    UserId: userId,
                    TaskTitle: title,
                    TaskGroupName: taskGroupName,
                    SectionId: parseInt(sectionId),
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

        let newTaskGroupId;

        await fetch('/TaskGroup/SaveNewTaskGroup/',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Name: listTitle,
                    SectionId: sectionId,
                    UserId: userId
                })
            }).then(async (response) => {
                if (!response.ok) {
                    alert("Could not add another task group please try again.");
                    return false;
                }
                newTaskGroupId = response.id;
        })
        
        const newListHTML = `
            <div class="task-group" id="${newTaskGroupId}">
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
    const columns = document.querySelectorAll('.task-list-container');

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

async function makeColumnDroppable(column) {

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

let currentListElement = null;

function openListMenu(event, iconElement, taskGroupId) {
    event.stopPropagation();
    const menu = document.getElementById('listActionMenu');

    const taskGroup = iconElement.closest('.task-group');

    const deleteBtn = document.getElementById('btnDeleteTaskGroup');

    deleteBtn.setAttribute('onclick', `actionDeleteList(${taskGroupId})`);

    currentListElement = taskGroup;

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

async function actionEditTitle() {
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

function actionAddTask() {
    if (!currentListElement) return;

    const addTaskBtn = currentListElement.querySelector('.add-task-btn');
    if (addTaskBtn) {
        const listContent = currentListElement;
        addTaskBtn.click();
        setTimeout(() => listContent.scrollTop = listContent.scrollHeight, 100);
    }
    closeListMenu();
}

async function actionDeleteList(taskGroupId) {
    if (!currentListElement && !taskGroupId) return;

    if (confirm("Are you sure to delete this task group and every task in it?")) {

        await fetch(`/TaskGroup/DeleteTaskGroup/${taskGroupId}`, { 
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(response => {
            if (!response.ok) {
                alert("There was an error");
                return false;
            }

            if(currentListElement) {
                currentListElement.remove();
                closeListMenu();
            }
        });
    }
}

async function actionSortTasks(criteria) {
    if (!currentListElement) return;
    
    const tasks = Array.from(currentListElement.querySelectorAll('.Task'));
    const taskGroupId = currentListElement.id;
    const footer = currentListElement.querySelector('.task-footer');

    if (criteria === 'date') {
        try {
            const response = await fetch('/TaskGroup/GetTaskStartDates/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    TaskGroupId: taskGroupId
                })
            });

            if (!response.ok) return false;

            const dateData = await response.json();

            tasks.sort((a, b) => {
                const idA = a.getAttribute('data-id');
                const idB = b.getAttribute('data-id');

                const taskInfoA = dateData.find(x => x.id === idA);
                const taskInfoB = dateData.find(x => x.id === idB);

                const dateA = taskInfoA && taskInfoA.startDate ? new Date(taskInfoA.startDate) : new Date(8640000000000000);
                const dateB = taskInfoB && taskInfoB.startDate ? new Date(taskInfoB.startDate) : new Date(8640000000000000);

                return dateA - dateB;
            });

            tasks.forEach(task => {
                currentListElement.insertBefore(task, footer);
            });

        } catch (error) {
            console.error("Sorting error:", error);
        }
    }
    else if (criteria === 'priority') {
        try {
            const response = await fetch('/TaskGroup/GetTaskPriorities/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    TaskGroupId: taskGroupId
                })
            });

            if (!response.ok) return false;

            const priorityData = await response.json();

            tasks.sort((a, b) => {
                const idA = a.getAttribute('data-id');
                const idB = b.getAttribute('data-id');

                const infoA = priorityData.find(x => x.id === idA);
                const infoB = priorityData.find(x => x.id === idB);

                const valA = infoA ? infoA.priority : -1;
                const valB = infoB ? infoB.priority : -1;

                return valB - valA;
            });
    
            tasks.forEach(task => {
                currentListElement.insertBefore(task, footer);
            });
            
        } catch (error) {
            console.error("Sorting Error:", error);
        }
        
    }
    else if (criteria === 'state') {
        const stateWeights = {
            'done': 3,
            'inprogress': 2,
            'todo': 1
        };

        tasks.sort((a, b) => {
            const stateA = a.getAttribute('data-state') || 'todo';
            const stateB = b.getAttribute('data-state') || 'todo';

            const weightA = stateWeights[stateA] || 0;
            const weightB = stateWeights[stateB] || 0;

            return weightB - weightA;
        });

        tasks.forEach(task => {
            currentListElement.insertBefore(task, footer);
        });
    }

    closeListMenu();
}

function openShareModal() {
    const modal = document.getElementById('shareModalOverlay');
    modal.style.display = 'flex';
}

function closeShareModal(event) {
    const modal = document.getElementById('shareModalOverlay');

    if (!event || event.target === modal) {
        modal.style.display = 'none';
    }
}

let projectUsersData = {};
document.addEventListener("DOMContentLoaded", async () => {

    const sectionTitle = document.querySelector('.section-title');
    const projectId = sectionTitle ? sectionTitle.getAttribute('project-id') : 1;
    

    try {
        const response = await fetch(`/Section/GetProjectUsers/${projectId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const userList = await response.json();

            userList.forEach(user => {

                const names = user.fullName.split(' ');
                let initials = names[0][0];
                if (names.length > 1) {
                    initials += names[names.length - 1][0];
                }
                initials = initials.toUpperCase();

                projectUsersData[initials] = {
                    fullName: user.fullName,
                    email: user.email,
                    role: user.role,
                    color: user.profileColor || '#0079bf'
                };
            });

        } else {
            console.error("Kullanıcılar çekilemedi.");
        }

    } catch (error) {
        console.error("Fetch hatası:", error);
    }
});

let activeProfilePopup = null;

function openUserProfile(event, initials) {
    event.stopPropagation(); 
    closeUserProfile();

    const popup = document.getElementById('userProfileCard');

    const user = projectUsersData[initials];

    if (!user) {
        console.warn(`Kullanıcı verisi bulunamadı: ${initials}. Veriler henüz yüklenmemiş olabilir.`);
        return;
    }

    document.getElementById('profileCardAvatar').innerText = initials;
    document.getElementById('profileCardAvatar').style.backgroundColor = user.color || '#0079bf'; 
    document.getElementById('profileCardName').innerText = user.fullName;
    document.getElementById('profileCardEmail').innerText = user.email;
    document.getElementById('profileCardRole').innerText = user.role;

    const taskData = findTasksForUser(initials);

    document.getElementById('profileCardCount').innerText = taskData.count;

    const listContainer = document.getElementById('profileCardTaskList');
    listContainer.innerHTML = '';

    if (taskData.tasks.length === 0) {
        listContainer.innerHTML = '<div class="empty-task-msg" style="padding:10px; color:#5e6c84; font-size:13px; font-style:italic;">Şu an aktif görevi yok.</div>';
    } else {
        taskData.tasks.forEach(taskTitle => {
            const div = document.createElement('div');
            div.className = 'mini-task-item';
            div.style.padding = "4px 0";
            div.style.fontSize = "13px";
            div.style.borderBottom = "1px solid #eee";
            div.style.color = "#172b4d";

            div.innerText = taskTitle;
            listContainer.appendChild(div);
        });
    }

    popup.style.display = 'flex';

    const rect = event.currentTarget.getBoundingClientRect();

    const popupWidth = 320;

    let leftPos = rect.left;
    if (leftPos + popupWidth > window.innerWidth) {
        leftPos = window.innerWidth - popupWidth - 20;
    }

    popup.style.top = (rect.bottom + 8) + 'px';
    popup.style.left = leftPos + 'px';
}

function findTasksForUser(targetInitials) {
    const allTasks = document.querySelectorAll('.Task');
    let count = 0;
    let taskTitles = [];

    allTasks.forEach(task => {
        const avatar = task.querySelector('.assigned-to-profile-icon');

        if (avatar) {
            const assignedInitials = avatar.innerText.trim();

            if (assignedInitials === targetInitials) {
                count++;

                const titleEl = task.querySelector('.Task-Title');
                if (titleEl) {
                    taskTitles.push(titleEl.innerText.trim());
                }
            }
        }
    });

    return { count: count, tasks: taskTitles };
}

function closeUserProfile() {
    const popup = document.getElementById('userProfileCard');
    if (popup) {
        popup.style.display = 'none';
    }
}

document.addEventListener('click', (e) => {
    const popup = document.getElementById('userProfileCard');
    if (popup && popup.style.display === 'flex' && !popup.contains(e.target)) {
        closeUserProfile();
    }
});

function toggleFilterMenu() {
    const menu = document.getElementById('filterMenuPopup');
    const btn = document.querySelector('.settings');

    if (menu.style.display === 'none') {
        menu.style.display = 'flex';
        const rect = btn.getBoundingClientRect();
        menu.style.top = (rect.bottom + 10) + 'px';
        menu.style.left = rect.left + 'px';
    } else {
        menu.style.display = 'none';
    }
}

function applyFilters() {
    const searchVal = document.getElementById('filterSearchInput').value.toLowerCase();

    const checkboxes = document.querySelectorAll('.filter-checkbox:checked');
    const selectedMembers = Array.from(checkboxes).map(cb => cb.value); // ['MK', 'CY'] gibi

    const allTasks = document.querySelectorAll('.Task');

    allTasks.forEach(task => {
        let isVisible = true;

        const title = task.querySelector('.Task-Title').innerText.toLowerCase();
        if (searchVal && !title.includes(searchVal)) {
            isVisible = false;
        }

        if (isVisible && selectedMembers.length > 0) {
            const avatarDiv = task.querySelector('.assigned-to-profile-icon');

            if (avatarDiv) {
                const assignedInitial = avatarDiv.innerText.trim();

                if (!selectedMembers.includes(assignedInitial)) {
                    isVisible = false;
                }
            } else {
                if (!selectedMembers.includes('NO_ASSIGN')) {
                    isVisible = false;
                }
            }
        }

        task.style.display = isVisible ? 'block' : 'none';
    });
}

function clearAllFilters() {
    document.getElementById('filterSearchInput').value = '';

    const checkboxes = document.querySelectorAll('.filter-checkbox');
    checkboxes.forEach(cb => cb.checked = false);

    applyFilters();
}
