
/* =========================================
GÜNCELLENMİŞ TASK MODAL AÇMA (Profil Kartı Destekli)
========================================= */

function openTaskModal(taskData, cardElement) {
    // 1. Tıklanan kartı global değişkene ata
    currentActiveCard = cardElement;

    const modal = document.getElementById('taskModalOverlay');
    if (!modal) return;

    // --- MEVCUT VERİLERİ DOLDURMA ---

    // 1. Başlık
    const titleInput = document.getElementById('modalTaskTitle');
    titleInput.value = taskData.title || '';
    // Yüksekliği ayarla
    titleInput.style.height = 'auto';
    titleInput.style.height = titleInput.scrollHeight + 'px';

    // 2. Liste Adı
    document.getElementById('modalListName').innerText = taskData.listName || "Yapılacaklar";

    // 3. OLUŞTURAN KİŞİ (BURASI GÜNCELLENDİ: onclick EKLENDİ)
    const createdAvatar = document.getElementById('modalCreatedByAvatar');
    const cInitial = taskData.createdByInitial || 'İK'; // Varsayılan İK olsun

    createdAvatar.innerText = cInitial;
    // MockData'da varsa rengini al, yoksa gri yap
    createdAvatar.style.backgroundColor = mockUserData[cInitial] ? mockUserData[cInitial].color : '#607d8b';
    createdAvatar.title = `Oluşturan: ${taskData.createdByName || 'Bilinmiyor'}`;

    // TIKLAMA ÖZELLİĞİ EKLENDİ
    // onclick eventini dinamik olarak bağlıyoruz
    createdAvatar.onclick = function(event) {
        openUserProfile(event, cInitial);
    };


    // 4. ATANAN KİŞİ (BURASI GÜNCELLENDİ: onclick EKLENDİ)
    const assignedAvatar = document.getElementById('modalAssignedToAvatar');

    if (taskData.assignedToName) {
        const aInitial = taskData.assignedInitial || 'CY';

        assignedAvatar.style.display = 'flex';
        assignedAvatar.innerText = aInitial;

        // MockData'dan veya parametreden renk al
        assignedAvatar.style.backgroundColor = mockUserData[aInitial] ? mockUserData[aInitial].color : (taskData.assignedColor || '#0079bf');
        assignedAvatar.title = `Atanan: ${taskData.assignedToName}`;

        // TIKLAMA ÖZELLİĞİ EKLENDİ
        assignedAvatar.style.cursor = 'pointer';
        assignedAvatar.onclick = function(event) {
            openUserProfile(event, aInitial);
        };

    } else {
        assignedAvatar.style.display = 'none'; // Atanan yoksa gizle
    }

    // 5. Tarihler
    document.getElementById('modalCreatedDate').innerText = taskData.createdDate || '12 Ara 2025';
    document.getElementById('modalDueDate').innerText = taskData.dueDate || '30 Ara 2025';

    // 6. Açıklama
    document.getElementById('modalTaskDesc').value = taskData.description || '';

    // 7. Öncelik
    const prioritySelect = document.getElementById('modalPrioritySelect');
    if(taskData.priorityValue) {
        prioritySelect.value = taskData.priorityValue;
    } else {
        prioritySelect.value = 'medium';
    }

    // Modalı Aç
    modal.style.display = 'flex';
}

function closeTaskModal(event) {
    // Eğer event null ise (X butonuna basıldıysa) direkt kapat
    if (!event) {
        document.getElementById('taskModalOverlay').style.display = 'none';
        return;
    }
    // Eğer tıklanan yer Overlay'in kendisiyse kapat (içerik değilse)
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

// 3. Yeni Kartı Kaydet (Mavi butona basınca)
function saveNewCard(btnElement) {
    const footer = btnElement.closest('.task-footer');
    const input = footer.querySelector('textarea');
    const title = input.value.trim();

    if (title) {
        // 1. Elementi DOM elemanı olarak oluştur (String değil)
        const newCard = document.createElement('div');
        newCard.className = 'Task';
        newCard.onclick = function() {
            openTaskModal({ title: title, createdByName: 'Ben', createdByInitial: 'B' });
        };

        // İçeriği doldur
        newCard.innerHTML = `
                <div class="Task-Title">${title}</div>
            `;

        // 2. Sürüklenebilir yap (Yazdığımız fonksiyonu çağırıyoruz)
        makeCardDraggable(newCard);

        // 3. Footer'dan hemen önceye ekle
        // footer.parentElement aslında .task-group dividir
        footer.parentElement.insertBefore(newCard, footer);

        input.value = '';
        input.focus();
    }
}

// 4. Enter Tuşuna Basınca Kaydet
function handleEnterKey(event, inputElement) {
    if (event.key === 'Enter' && !event.shiftKey) {
        event.preventDefault(); // Alt satıra geçmeyi engelle
        // Kaydet butonunu bul ve tıkla
        const saveBtn = inputElement.parentElement.querySelector('.btn-add-card');
        saveNewCard(saveBtn);
    }
}
// 1. Formu Göster
function showAddListForm(btnElement) {
    const wrapper = btnElement.parentElement;

    // Wrapper'ın rengini değiştirmek için class ekle
    wrapper.classList.add('active');

    // Butonu gizle, formu göster
    btnElement.style.display = 'none';
    wrapper.querySelector('.add-list-form').style.display = 'block';

    // Inputa odaklan
    wrapper.querySelector('input').focus();
}

// 2. Formu Gizle
function hideAddListForm(btnElement) {
    // closest ile en dıştaki wrapper'ı bul
    const wrapper = btnElement.closest('.add-list-wrapper');

    wrapper.classList.remove('active');

    // Formu gizle, butonu göster
    wrapper.querySelector('.add-list-form').style.display = 'none';
    wrapper.querySelector('.add-list-btn-idle').style.display = 'flex';

    // Inputu temizle
    wrapper.querySelector('input').value = '';
}

// 3. Yeni Listeyi Kaydet (EN ÖNEMLİ KISIM)
function saveNewList(btnElement) {
    const wrapper = btnElement.closest('.add-list-wrapper');
    const input = wrapper.querySelector('input');
    const listTitle = input.value.trim();

    if (listTitle) {
        // String olarak HTML'i hazırla (Senin mevcut yapın)
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

        // HTML'i ekle
        wrapper.insertAdjacentHTML('beforebegin', newListHTML);

        // EKLENEN KISIM: Yeni eklenen listeyi bul ve Droppable yap
        // Wrapper'dan bir önceki element bizim yeni listemizdir
        const newListElement = wrapper.previousElementSibling;
        if(newListElement) {
            makeColumnDroppable(newListElement);
        }

        input.value = '';
        hideAddListForm(btnElement);
    }
}

// 4. Enter Tuşu Kontrolü
function handleListEnterKey(event, inputElement) {
    if (event.key === 'Enter') {
        const saveBtn = inputElement.parentElement.querySelector('.btn-add-card');
        saveNewList(saveBtn);
    }
}

/* =========================================
DRAG & DROP MOTORU
========================================= */

// Sayfa yüklendiğinde mevcut kartları sürüklenebilir yap
document.addEventListener('DOMContentLoaded', () => {
    const cards = document.querySelectorAll('.Task');
    const columns = document.querySelectorAll('.task-group');

    cards.forEach(card => makeCardDraggable(card));
    columns.forEach(column => makeColumnDroppable(column));
});

// Bir kartı sürüklenebilir hale getiren fonksiyon
function makeCardDraggable(card) {
    card.setAttribute('draggable', 'true');

    card.addEventListener('dragstart', () => {
        card.classList.add('dragging');
    });

    card.addEventListener('dragend', () => {
        card.classList.remove('dragging');
    });
}

// Bir sütunu (Listeyi) kart kabul eder hale getiren fonksiyon
function makeColumnDroppable(column) {
    column.addEventListener('dragover', e => {
        e.preventDefault(); // Bırakmaya izin ver
        const afterElement = getDragAfterElement(column, e.clientY);
        const draggable = document.querySelector('.dragging');

        // Sadece Task class'ı olanları taşı, başlık veya footer'ı bozma
        if (!draggable) return;

        // Footer'ı bul (Ekleme butonu en altta kalsın diye)
        const footer = column.querySelector('.task-footer');

        if (afterElement == null) {
            // Eğer altında eleman yoksa, footer'ın hemen üstüne ekle
            column.insertBefore(draggable, footer);
        } else {
            // Altında eleman varsa, onun üstüne ekle
            column.insertBefore(draggable, afterElement);
        }
    });
}

// Fare pozisyonuna göre hangi elemanın altına geleceğini hesaplayan sihirli fonksiyon
function getDragAfterElement(container, y) {
    // Sürüklenen hariç diğer tüm kartları al
    const draggableElements = [...container.querySelectorAll('.Task:not(.dragging)')];

    return draggableElements.reduce((closest, child) => {
        const box = child.getBoundingClientRect();
        // Mouse'un elemanın ortasına olan uzaklığı
        const offset = y - box.top - box.height / 2;

        // offset < 0 demek mouse elemanın üstünde demek
        // En yakın negatif değeri arıyoruz (0'a en yakın olan üstteki elemandır)
        if (offset < 0 && offset > closest.offset) {
            return { offset: offset, element: child };
        } else {
            return closest;
        }
    }, { offset: Number.NEGATIVE_INFINITY }).element;
}

/* =========================================
ARKAPLAN DEĞİŞTİRİCİ MANTIĞI
========================================= */

// 1. Menüyü Aç/Kapa
function toggleBackgroundMenu() {
    const menu = document.getElementById('backgroundMenu');

    // Eğer menü ilk kez açılıyorsa ve içi boşsa doldur
    if (!menu.classList.contains('open') && document.getElementById('photoGrid').children.length === 0) {
        initBackgroundOptions();
    }

    menu.classList.toggle('open');
}

// 2. Renkleri ve Resimleri Oluştur (Sadece 1 kere çalışır)
function initBackgroundOptions() {

    // --- A. RENKLER ---
    const colors = ['#0079bf', '#d29034', '#519839', '#b04632', '#89609e', '#cd5a91', '#4bbf6b', '#00aecc'];
    const colorGrid = document.getElementById('colorGrid');

    colors.forEach(color => {
        const div = document.createElement('div');
        div.className = 'bg-option color-box';
        div.style.backgroundColor = color;
        div.onclick = () => setBackgroundColor(color);
        colorGrid.appendChild(div);
    });

    // --- B. FOTOĞRAFLAR (Sınırsız hissi için döngü) ---
    // Mockup için 'Lorem Picsum' kullanıyoruz. Gerçek projede veritabanından URL çekeceksin.
    const photoGrid = document.getElementById('photoGrid');

    // 20 tane rastgele yüksek kalite resim üretelim
    for (let i = 20; i <= 40; i++) {
        // Her seferinde farklı resim gelmesi için 'seed' kullanıyoruz
        const thumbUrl = `https://picsum.photos/seed/${i * 123}/200/120`; // Küçük hali (Hızlı yüklenir)
        const fullUrl = `https://picsum.photos/seed/${i * 123}/2560/1440`; // Büyük hali (Arkaplan olur)

        const div = document.createElement('div');
        div.className = 'bg-option';
        div.style.backgroundImage = `url('${thumbUrl}')`;

        // Tıklayınca ana arkaplanı değiştir
        div.onclick = () => setBackgroundImage(fullUrl);

        photoGrid.appendChild(div);
    }
}

// 3. Arkaplanı RESİM Yap
function setBackgroundImage(url) {
    document.body.style.backgroundImage = `url('${url}')`;
    document.body.style.backgroundColor = ''; // Rengi sıfırla
    // Navbar'ı biraz daha şeffaf yap ki resim belli olsun
    document.querySelector('.top-bar').style.backgroundColor = 'rgba(0, 0, 0, 0.3)';
}

// 4. Arkaplanı RENK Yap
function setBackgroundColor(color) {
    document.body.style.backgroundImage = 'none'; // Resmi kaldır
    document.body.style.backgroundColor = color;
    // Navbar rengini seçilen renge uyumlu hale getirmek için hafif koyulaştır
    document.querySelector('.top-bar').style.backgroundColor = 'rgba(0, 0, 0, 0.2)';
}

/* =========================================
LİSTE MENÜSÜ FONKSİYONLARI
========================================= */

let currentListElement = null; // Hangi liste üzerinde işlem yapıyoruz?

// 1. Menüyü Açma Fonksiyonu
function openListMenu(event, iconElement) {
    event.stopPropagation(); // Sayfa tıklamasını engelle
    const menu = document.getElementById('listActionMenu');

    // Tıklanan ikonun ait olduğu ana listeyi (.task-group) bul
    currentListElement = iconElement.closest('.task-group');

    // Menüyü görünür yap
    menu.style.display = 'flex';

    // Pozisyonu ayarla (Tıklanan ikonun hemen yanına/altına)
    const rect = iconElement.getBoundingClientRect();
    menu.style.top = (rect.bottom + 5) + 'px';
    menu.style.left = (rect.left) + 'px';
}

// 2. Menüyü Kapatma
function closeListMenu() {
    document.getElementById('listActionMenu').style.display = 'none';
}

// Sayfa boşluğuna tıklayınca menüyü kapat
document.addEventListener('click', (e) => {
    const menu = document.getElementById('listActionMenu');
    if (!menu.contains(e.target)) {
        closeListMenu();
    }
});

// --- İŞLEVLER ---

// A. Başlığı Değiştir
// A. Başlığı Değiştir (Modern, Inline Input Yöntemi)
function actionEditTitle() {
    if (!currentListElement) return;

    // Başlık divini bul
    const titleDiv = currentListElement.querySelector('.task-group-title');

    // Mevcut metni al (İçindeki ikonun HTML'ini bozmamak için sadece text node'u alıyoruz)
    // childNodes[0] genellikle "Yapılacaklar " yazısıdır.
    const textNode = titleDiv.childNodes[0];
    const currentText = textNode.nodeValue.trim();

    // 1. Geçici bir Input oluştur
    const input = document.createElement('input');
    input.type = 'text';
    input.className = 'edit-list-title-input';
    input.value = currentText;

    // 2. Görünümü Değiştir
    // Başlık divini geçici olarak gizle
    titleDiv.style.display = 'none';

    // Inputu başlık divinin hemen öncesine (yani tam yerine) ekle
    titleDiv.parentNode.insertBefore(input, titleDiv);

    // 3. Inputa odaklan ve içindeki metni seç (Hızlı düzenleme için)
    input.focus();
    input.select();

    // --- KAYDETME MANTIĞI ---
    const saveAndClose = () => {
        const newTitle = input.value.trim();

        if (newTitle) {
            // Sadece metin kısmını güncelle, ikona dokunma
            textNode.nodeValue = newTitle + " ";
        }

        // Temizlik: Inputu sil, başlığı geri getir
        input.remove();
        titleDiv.style.display = 'block'; // block veya flex yapına göre değişebilir, genelde block yeterli
    };

    // Enter tuşuna basınca kaydet
    input.addEventListener('keydown', (e) => {
        if (e.key === 'Enter') {
            saveAndClose();
        }
    });

    // Inputun dışına tıklayınca da kaydet (Kullanıcı dostu)
    input.addEventListener('blur', saveAndClose);

    // Menüyü kapat
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
