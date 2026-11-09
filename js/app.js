// AURAMODE UI & Interaction Helpers

function formatNaira(amount) {
  return 'N' + Number(amount).toLocaleString('en-US');
}

function showToast(message) {
  let toast = document.getElementById('toastNotification');
  if (!toast) {
    toast = document.createElement('div');
    toast.id = 'toastNotification';
    toast.className = 'toast-notification';
    document.body.appendChild(toast);
  }
  toast.innerText = message;
  toast.classList.add('show');
  setTimeout(() => {
    toast.classList.remove('show');
  }, 2600);
}

function updateHeaderBadges() {
  const cartBadge = document.querySelector('.cart-badge');
  const wishlistBadge = document.querySelector('.wishlist-badge');
  const userGreeting = document.querySelector('.user-greeting');

  if (cartBadge) {
    const count = store.getCartCount();
    cartBadge.textContent = count;
    cartBadge.style.display = count > 0 ? 'flex' : 'none';
  }

  if (wishlistBadge) {
    const count = store.getWishlistCount();
    wishlistBadge.textContent = count;
    wishlistBadge.style.display = count > 0 ? 'flex' : 'none';
  }

  if (userGreeting) {
    if (store.user && store.user.loggedIn) {
      userGreeting.innerHTML = `Hi, ${store.user.name.split(' ')[0]} <i class="fas fa-user-circle"></i>`;
      userGreeting.href = 'account.html';
    } else {
      userGreeting.innerHTML = `Sign In <i class="far fa-user"></i>`;
      userGreeting.href = 'login.html';
    }
  }
}

// Mobile drawer toggle
function toggleMobileMenu() {
  const drawer = document.getElementById('mobileDrawer');
  const overlay = document.getElementById('drawerOverlay');
  if (drawer) drawer.classList.toggle('open');
  if (overlay) overlay.classList.toggle('open');
  // Prevent body scroll when drawer is open
  document.body.style.overflow = (drawer && drawer.classList.contains('open')) ? 'hidden' : '';
}

// Floating WhatsApp Concierge Helper
function initWhatsAppConcierge() {
  if (document.querySelector('.whatsapp-floating-btn')) return;
  if (window.location.pathname.toLowerCase().includes('admin.html')) return;

  const btn = document.createElement('a');
  btn.href = 'https://wa.me/2349024462630?text=' + encodeURIComponent('Hi AURAMODE, I would like to inquire about sizing, order status, or custom dispatch.');
  btn.className = 'whatsapp-floating-btn';
  btn.target = '_blank';
  btn.rel = 'noopener noreferrer';
  btn.setAttribute('aria-label', 'Chat with AURAMODE Concierge on WhatsApp');
  btn.innerHTML = `
    <span class="whatsapp-status-dot"></span>
    <i class="fab fa-whatsapp"></i>
    <span class="whatsapp-label">Concierge</span>
  `;
  document.body.appendChild(btn);
}

// Setup common listeners
document.addEventListener('DOMContentLoaded', () => {
  updateHeaderBadges();
  initWhatsAppConcierge();

  window.addEventListener('auramode_store_updated', () => {
    updateHeaderBadges();
  });

  // Accordion toggle behavior
  document.querySelectorAll('.accordion-header').forEach(header => {
    header.addEventListener('click', () => {
      const parent = header.closest('.accordion-item');
      if (parent) {
        parent.classList.toggle('open');
      }
    });
  });

  // Search input redirection or filter
  const searchInputs = document.querySelectorAll('.header-search-input');
  searchInputs.forEach(input => {
    input.addEventListener('keypress', (e) => {
      if (e.key === 'Enter' && input.value.trim()) {
        window.location.href = `shop.html?search=${encodeURIComponent(input.value.trim())}`;
      }
    });
  });

  // Newsletter form submission
  document.querySelectorAll('.newsletter-form').forEach(form => {
    form.addEventListener('submit', (e) => {
      e.preventDefault();
      const input = form.querySelector('.newsletter-input');
      if (input && input.value) {
        showToast('Thank you for joining the AURAMODE newsletter.');
        input.value = '';
      }
    });
  });
});

