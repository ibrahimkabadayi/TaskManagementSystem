let currentOpenedTaskId = 0;
async function openTaskModal(taskId) {
    currentOpenedTaskId = taskId;
    const modal = document.getElementById('taskModalOverlay');
    if (!modal) return;

    try {
        const response = await fetch(`/Task/GetTaskDetails/${taskId}`, {
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
            finishedByName: result.finishedByName,
            finishedByInitial: result.finishedByInitial,
            finishedByColor: result.finishedByColor,
            createdDate: result.createdDate,
            dueDate: result.dueDate,
            priorityValue: result.priority,
            state: result.state
        };

        const titleInput = document.getElementById('modalTaskTitle');
        titleInput.value = taskData.title || '';

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
                        openUserSelectionMenu(event);
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

                if (assignedWrapper) {
                    assignedWrapper.onclick = function(event) {
                        openUserSelectionMenu(event);
                    };
                }
            }
        }

        const createdDateEl = document.getElementById('modalCreatedDate');
        if (createdDateEl) createdDateEl.innerText = taskData.createdDate || '-';

        const dueDateEl = document.getElementById('modalDueDate');
        if (dueDateEl) dueDateEl.innerText = taskData.dueDate || 'No date';

        const dueDateInput = document.getElementById('modalDueDateInput');
        if (dueDateInput) {
            dueDateInput.value = "";

            const dateStr = taskData.dueDate;

            if (dateStr) {
                const dateObj = new Date(dateStr);

                if (!isNaN(dateObj.getTime())) {
                    const yyyy = dateObj.getFullYear();
                    const mm = String(dateObj.getMonth() + 1).padStart(2, '0');
                    const dd = String(dateObj.getDate()).padStart(2, '0');

                    dueDateInput.value = `${yyyy}-${mm}-${dd}`;
                }
            }

            const today = new Date();
            const t_yyyy = today.getFullYear();
            const t_mm = String(today.getMonth() + 1).padStart(2, '0');
            const t_dd = String(today.getDate()).padStart(2, '0');
            dueDateInput.min = `${t_yyyy}-${t_mm}-${t_dd}`;
        }

        const descInput = document.getElementById('modalTaskDesc');
        if (descInput) descInput.value = taskData.description || '';

        const prioritySelect = document.getElementById('modalPrioritySelect');
        if (prioritySelect) {
            prioritySelect.value = (taskData.priorityValue || 'medium').toLowerCase();
        }

        const stateSelect = document.getElementById('modalStateSelect');
        if (stateSelect) {
            let stateValue = taskData.state;

            if (typeof stateValue === 'string') {
                stateValue = stateValue.toLowerCase();
            }
            else if (typeof stateValue === 'number') {
                if (stateValue === 0) stateValue = 'todo';
                else if (stateValue === 1) stateValue = 'inprogress';
                else if (stateValue === 2) stateValue = 'done';
            }

            stateSelect.value = stateValue;
        }

        modal.style.display = 'flex';

    } catch (error) {
        console.error(error);
    }
}

function closeTaskModal(event) {
    const userSelectionPopup = document.getElementById('userSelectionPopup');
    if (userSelectionPopup) {
        userSelectionPopup.remove();
    }
    currentOpenedTaskId = 0;
    if (!event) {
        document.getElementById('taskModalOverlay').style.display = 'none';
        return;
    }
    if (event.target.id === 'taskModalOverlay') {
        document.getElementById('taskModalOverlay').style.display = 'none';
    }
}

async function saveTaskTitle(inputElement) {
    const newTitle = inputElement.value.trim();
    const taskId = currentOpenedTaskId;

    if (!newTitle) {
        alert("Task title can't be empty!");
        return;
    }

    try {
        const response = await fetch('/Task/UpdateTitle', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                TaskId: taskId,
                Title: newTitle,
                UserId: document.querySelector('.section-title').getAttribute('current-user-id'),
                ProjectId: document.querySelector('.section-title').getAttribute('project-id'),
            })
        });

        if (!response.ok) {
            console.error("Title could not be updated!.");
            return;
        }

        const taskCard = document.querySelector(`.Task[data-id="${taskId}"]`);
        if (taskCard) {
            const titleDiv = taskCard.querySelector('.Task-Title');
            if (titleDiv) {
                titleDiv.innerText = newTitle;
            }
        }

        console.log("Title Updated.");

    } catch (error) {
        console.error("Fetch error:", error);
    }
}

function openUserSelectionMenu(event) {
    event.stopPropagation();

    if (document.getElementById('userSelectionPopup')) {
        document.getElementById('userSelectionPopup').remove();
    }

    const menu = document.createElement('div');
    menu.id = 'userSelectionPopup';
    menu.className = 'list-menu-popup';
    menu.style.display = 'flex';
    menu.style.flexDirection = 'column';
    menu.style.zIndex = '25000';

    const header = document.createElement('div');
    header.className = 'list-menu-header';
    header.innerHTML = '<span class="list-menu-title">Kişi Seç</span>';
    menu.appendChild(header);

    const content = document.createElement('div');
    content.className = 'list-menu-content';

    Object.values(projectUsersData).forEach(user => {

        const item = document.createElement('button');
        item.className = 'list-menu-item';
        item.style.display = 'flex';
        item.style.alignItems = 'center';
        item.style.gap = '10px';

        item.innerHTML = `
            <div class="user-avatar-sm" style="background-color:${user.color}; width:24px; height:24px; font-size:10px;">${user.initials}</div>
            <span>${user.fullName}</span>
        `;

        item.onclick = async function() {
            await assignUserToTask(user.email);
            menu.remove();
        };

        content.appendChild(item);
    });

    menu.appendChild(content);
    document.body.appendChild(menu);

    const rect = event.currentTarget.getBoundingClientRect();
    menu.style.position = 'fixed';
    menu.style.top = (rect.bottom + 5) + 'px';
    menu.style.left = rect.left + 'px';

    document.addEventListener('click', function closeMenu(e) {
        if (!menu.contains(e.target) && e.target !== event.currentTarget) {
            menu.remove();
            document.removeEventListener('click', closeMenu);
        }
    });
}

async function assignUserToTask(userEmail) {
    const selectedUserData = projectUsersData[userEmail];
    const selectedUserInitials = selectedUserData ? selectedUserData.initials : '';

    const taskId = currentOpenedTaskId;
    const sectionTitle = document.querySelector('.section-title') || document.querySelector('.task-group-title');
    const projectId = sectionTitle ? sectionTitle.getAttribute('project-id') : 0;

    const userId = sectionTitle ? sectionTitle.getAttribute('current-user-id') : 0;

    await fetch('/Task/AssignUser', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            UserEmail: userEmail,
            TaskId: taskId,
            ProjectId: projectId,
            UserId: userId,
        })
    }).then((response) => {
        if (!response.ok) {
            alert("There was a problem assigning user to task. Please try again.");
        } else {

            if (selectedUserData) {
                const assignedAvatar = document.getElementById('modalAssignedToAvatar');
                const assignedName = document.getElementById('modalAssignedToName');

                if (assignedAvatar && assignedName) {
                    assignedAvatar.innerText = selectedUserInitials;
                    assignedAvatar.style.backgroundColor = selectedUserData.color;
                    assignedAvatar.style.color = '#fff';
                    assignedAvatar.classList.remove('unassigned-icon');

                    assignedName.innerText = selectedUserData.fullName;
                    assignedName.style.color = '#172b4d';
                    assignedName.style.fontStyle = 'normal';
                }

                const taskCard = document.getElementById(taskId);
                if (taskCard) {
                    let cardIcon = taskCard.querySelector('.assigned-to-profile-icon');

                    if (!cardIcon) {
                        cardIcon = document.createElement('div');
                        cardIcon.className = 'assigned-to-profile-icon';

                        const titleEl = taskCard.querySelector('.Task-Title');
                        if(titleEl) titleEl.after(cardIcon);
                        else taskCard.appendChild(cardIcon);
                    }

                    cardIcon.innerText = selectedUserInitials;
                    cardIcon.style.backgroundColor = selectedUserData.color;
                    cardIcon.style.display = 'flex';
                }
            }

            const menu = document.getElementById('userSelectionPopup');
            if (menu) menu.remove();
        }
    })
}

async function changeTaskDueDate(userId, projectId){
    const dateSelect = document.getElementById('modalDueDateInput');
    const dueDate = dateSelect.value;
    
    await fetch('/Task/UpdateDueDate', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            TaskId: currentOpenedTaskId,
            DueDate: dueDate,
            ProjectId: projectId,
            UserId: userId,
        })
    }).then((response) => {
        if (!response.ok) {
            alert(response.message);
        }
    })
}

async function changeTaskPriority() {
    const prioritySelect = document.getElementById('modalPrioritySelect');
    const priority = prioritySelect.value;

    let priorityVal = 0;
    if(priority === 'high') priorityVal = 2;
    else if(priority === 'medium') priorityVal = 1;
    else priorityVal = 0;

    await fetch('/Task/UpdatePriority', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            TaskId: currentOpenedTaskId,
            Priority: priority
        })
    }).then((response) => {
        if (!response.ok) {
            alert("Fetch error for changing task priority");
        } else {
            const taskCard = document.getElementById(currentOpenedTaskId) || document.querySelector(`.Task[data-id="${currentOpenedTaskId}"]`);
            if(taskCard) {
                taskCard.setAttribute('data-priority', priorityVal); 
            }
        }
    })
}

async function changeTaskState(userId, projectId) {
    const stateSelect = document.getElementById('modalStateSelect');
    const state = stateSelect.value;

    await fetch('/Task/UpdateTaskState', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            TaskId: currentOpenedTaskId,
            State: state,
            UserId: userId,
            ProjectId: projectId
        })
    }).then((response) => {
        if (!response.ok) {
            alert("Fetch error for changing task state");
        } else {
            const taskCard = document.getElementById(currentOpenedTaskId) || document.querySelector(`.Task[data-id="${currentOpenedTaskId}"]`);
            if(taskCard) {
                taskCard.setAttribute('data-state', state);
            }
        }
    })
}

async function saveTaskDescription(userId, projectId){
    const taskId = currentOpenedTaskId;
    
    const taskDescriptionInput = document.getElementById('modalTaskDesc');
    const description = taskDescriptionInput.value.trim();

    await fetch('/Task/UpdateDescription', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            TaskId: taskId,
            UserId: userId,
            ProjectId: projectId,
            Description: description,
        })
    }).then((response) => {
        if (!response.ok) {
            alert("Fetch error for changing task priority");
        }

        const taskCard = document.getElementById(taskId);

        if (taskCard) {
            const existingIcon = taskCard.querySelector('.task-desc-button');

            if (description && !existingIcon) {
                const iconDiv = document.createElement('div');
                iconDiv.className = 'task-desc-button';
                iconDiv.innerHTML = '<i class="fa-solid fa-align-left"></i>';
                taskCard.appendChild(iconDiv);
            }
            else if (!description && existingIcon) {
                existingIcon.remove();
            }
        }
        closeTaskModal(null);
    });
}

async function deleteTask(userId) {
    const taskId = currentOpenedTaskId;

    const sectionTitle = document.querySelector('.section-title') || document.querySelector('.task-group-title');
    const projectId = sectionTitle ? sectionTitle.getAttribute('project-id') : 0;

    try {
        const response = await fetch('/Task/DeleteTask', {
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

    const footer = btnElement.closest('.task-footer');
    const input = footer.querySelector('textarea');
    const title = input.value.trim();

    if (title) {
        const newCard = document.createElement('div');
        
        let taskId;
        
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
            .then(async (response) => {
                if (!response.ok) {
                    alert("Could not add another task please try again.");
                }
                const data = await response.json();
                
                taskId = data.id;
            });
        
        
        newCard.className = 'Task';

        newCard.id = taskId;
        newCard.setAttribute('data-id', taskId);
        
        newCard.onclick = function() {
            openTaskModal(taskId);
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

    if (!listTitle) return;

    try {
        const response = await fetch('/TaskGroup/SaveNewTaskGroup/', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                Name: listTitle,
                SectionId: sectionId,
                UserId: userId
            })
        });

        if (!response.ok) {
            alert("List could not be saved. Please try again.");
            return;
        }

        const data = await response.json();


        const newTaskGroupId = data.id;
        
        const newListHTML = `
            <div class="task-group" id="${newTaskGroupId}">
                <div class="task-group-title" id="${newTaskGroupId}">
                    ${listTitle} 
                    <i class="fa-solid fa-ellipsis" style="float:right; font-size:12px; cursor: pointer;" 
                       onclick="openListMenu(event, this, ${newTaskGroupId})"></i>
                </div>
                
                <div class="task-list-container" id="container-${newTaskGroupId}">
                </div>
                
                <div class="task-footer">
                    <div class="add-task-btn" onclick="showAddCardForm(this)">
                        <i class="fa-solid fa-plus"></i> Add Task
                    </div>

                    <div class="add-card-form" style="display: none;">
                        <textarea class="card-composer-input" placeholder="Enter a task title..." rows="3" 
                                  onkeydown="handleEnterKey(event, this, ${userId})"></textarea>

                        <div class="composer-controls">
                            <div class="composer-left">
                                <button class="btn-add-card" 
                                        onclick="saveNewCard(this, '${userId}', '${newTaskGroupId}')">Add Task</button>
                            </div>

                            <div class="composer-right">
                                <button class="btn-close-composer" onclick="hideAddCardForm(this)" 
                                        style="background:none; border:none; cursor:pointer; font-size:16px; color:#6b778c;">
                                    <i class="fa-solid fa-xmark"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`;

        wrapper.insertAdjacentHTML('beforebegin', newListHTML);

        const newTaskGroupDiv = document.getElementById(newTaskGroupId);
        if (newTaskGroupDiv) {
            const newContainer = newTaskGroupDiv.querySelector('.task-list-container');
            if(newContainer) {
                makeColumnDroppable(newContainer);
            }
        }

        input.value = '';
        hideAddListForm(btnElement);

    } catch (error) {
        console.error("List saving error:", error);
        alert("There was a problem saving list. Please try again.");
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
        const newTaskGroupId = column.id.replace('container-', '');
        
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

    const taskContainer = currentListElement.querySelector('.task-list-container');
    if (!taskContainer) return;

    if (criteria === 'date') {
        try {
            const response = await fetch('/TaskGroup/GetTaskStartDates', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ TaskGroupId: taskGroupId })
            });

            if (!response.ok) return false;
            const dateData = await response.json();

            tasks.sort((a, b) => {
                const idA = a.getAttribute('data-id');
                const idB = b.getAttribute('data-id');

                const taskInfoA = dateData.find(x => x.id == idA);
                const taskInfoB = dateData.find(x => x.id == idB);

                const dateA = taskInfoA && taskInfoA.startDate ? new Date(taskInfoA.startDate) : new Date(0);
                const dateB = taskInfoB && taskInfoB.startDate ? new Date(taskInfoB.startDate) : new Date(0);

                return dateB - dateA;
            });

        } catch (error) {
            console.error("Sorting error:", error);
            return;
        }
    }
    else if (criteria === 'priority') {
        try {
            const response = await fetch('/TaskGroup/GetTaskPriorities', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ TaskGroupId: taskGroupId })
            });

            if (!response.ok) return false;
            const priorityData = await response.json();

            tasks.sort((a, b) => {
                const idA = a.getAttribute('data-id');
                const idB = b.getAttribute('data-id');

                const infoA = priorityData.find(x => x.id == idA);
                const infoB = priorityData.find(x => x.id == idB);

                const valA = infoA ? infoA.priority : -1;
                const valB = infoB ? infoB.priority : -1;

                return valB - valA;
            });

        } catch (error) {
            console.error("Sorting Error:", error);
            return;
        }
    }
    else if (criteria === 'state') {
        const stateWeights = {
            'done': 3,
            'inprogress': 2,
            'todo': 1,
            '2': 3, 
            '1': 2,
            '0': 1
        };

        tasks.sort((a, b) => {
            let stateA = (a.getAttribute('data-state') || '').toString().toLowerCase();
            let stateB = (b.getAttribute('data-state') || '').toString().toLowerCase();

            if (!stateA) stateA = 'todo';
            if (!stateB) stateB = 'todo';

            const weightA = stateWeights[stateA] || 0;
            const weightB = stateWeights[stateB] || 0;

            return weightB - weightA;
        });
    }
    tasks.forEach(task => {
        taskContainer.appendChild(task);
    });

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
            headers: { 'Content-Type': 'application/json' }
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

                projectUsersData[user.email] = {
                    initials: initials,
                    fullName: user.fullName,
                    email: user.email,
                    role: user.role,
                    color: user.profileColor || '#0079bf',
                    isActive: (user.isActive === "True" || user.isActive === true)
                };
            });

        } else {
            console.error("Could not get project users.");
        }

    } catch (error) {
        console.error("Fetch Error:", error);
    }
});

let activeProfilePopup = null;

function openUserProfile(event, identifier) {
    event.stopPropagation();
    closeUserProfile();

    let user = projectUsersData[identifier];

    if (!user) {
        user = Object.values(projectUsersData).find(u => u.initials === identifier);
    }

    if (!user) {
        console.warn(`Could not find user data: ${identifier}`);
        return;
    }

    document.getElementById('profileCardAvatar').innerText = user.initials;
    document.getElementById('profileCardAvatar').style.backgroundColor = user.color || '#0079bf';
    document.getElementById('profileCardName').innerText = user.fullName;
    document.getElementById('profileCardEmail').innerText = user.email;
    document.getElementById('profileCardRole').innerText = user.role;

    const statusEl = document.getElementById('profileCardStatus');

    if (statusEl) {
        const isActive = (String(user.isActive).toLowerCase() === 'true');

        if (isActive) {
            statusEl.innerText = "Online";
            statusEl.style.color = "#4bbf6b";
        } else {
            statusEl.innerText = "Offline";
            statusEl.style.color = "#6b778c";
        }
    } else {
        console.warn("Error could not find element.");
    }

    const taskData = findTasksForUser(user.initials);

    document.getElementById('profileCardCount').innerText = taskData.count;

    const listContainer = document.getElementById('profileCardTaskList');
    listContainer.innerHTML = '';

    if (taskData.tasks.length === 0) {
        listContainer.innerHTML = '<div class="empty-task-msg" style="padding:10px; color:#5e6c84; font-size:13px; font-style:italic;">There are no active tasks.</div>';
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

    const popup = document.getElementById('userProfileCard');
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
    const selectedMembers = Array.from(checkboxes).map(cb => cb.value);

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
