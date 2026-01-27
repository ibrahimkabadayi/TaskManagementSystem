async function updateMemberRole(selectElement, projectUserId, userId, projectId) {
    const newRole = selectElement.value;
    const originalValue = selectElement.getAttribute('data-original') || newRole;

    try {
        const response = await fetch('/ProjectUser/UpdateRole', {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                ProjectUserId: projectUserId,
                NewRole: newRole,
                UserId: userId,
                ProjectId: projectId
            })
        });

        if (response.ok) {
            console.log("Role Updated: " + newRole);
            selectElement.setAttribute('data-original', newRole);

        } else {
            throw new Error("Update is unsuccessful. Please try again later.");
        }

    } catch (error) {
        console.error(error);
        alert("There was an error updating the role. Please try again later.");
        selectElement.value = originalValue;
    }
}

async function removeMember(projectUserId, userName, currentUserId, projectId) {
    if (!confirm(`Are you sure to remove ${userName} from the project?`)) {
        return;
    }

    try {
        const response = await fetch(`/ProjectUser/RemoveMember/${projectUserId + " " + currentUserId + " " + projectId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const row = document.getElementById(`row-${projectUserId}`);
            row.style.opacity = '0';
            setTimeout(() => row.remove(), 300);
        } else {
            alert("Member removal failed. Please try again later.");
        }

    } catch (error) {
        console.error(error);
        alert("There was an error removing the member. Please try again later. If the problem persists, please contact the site administrator.");
    }
}

function openShareModal() {
    const modal = document.getElementById('shareModalOverlay');
    if(modal) modal.style.display = 'flex';
    else alert("Sharing is not yet available. Please try again later. If the problem persists, please contact the site administrator.");
}
