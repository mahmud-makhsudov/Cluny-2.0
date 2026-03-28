(function () {
    const statusEl = document.getElementById('status-toast');
    if (!statusEl) return;

    const AUTO_HIDE_MS = 5000;
    let autoHideTimer = null;

    function hideToast() {
        if (!statusEl) return;
        statusEl.classList.remove('show');
        setTimeout(() => statusEl.classList.add('d-none'), 260);
        if (autoHideTimer) { clearTimeout(autoHideTimer); autoHideTimer = null; }
    }

    function showStatus(message, success) {
        if (!statusEl) return;
        const msgEl = statusEl.querySelector('.toast-message');
        if (msgEl) msgEl.textContent = message ?? '';

        statusEl.classList.remove('d-none', 'toast-info', 'toast-danger');
        statusEl.classList.add(success ? 'toast-info' : 'toast-danger');

        // trigger enter animation
        requestAnimationFrame(() => statusEl.classList.add('show'));

        if (autoHideTimer) clearTimeout(autoHideTimer);
        autoHideTimer = setTimeout(hideToast, AUTO_HIDE_MS);
    }

    const closeBtn = statusEl.querySelector('.toast-close');
    if (closeBtn) closeBtn.addEventListener('click', hideToast);

    // show server-side TempData message on initial load if present
    const initialMessage = statusEl.querySelector('.toast-message')?.textContent?.trim();
    const dataSuccess = statusEl.getAttribute('data-success');
    const initialSuccess = dataSuccess === 'true';
    if (initialMessage) {
        statusEl.classList.remove('d-none');
        showStatus(initialMessage, initialSuccess);
    }

    // expose for pages/components to call: showStatusToast(message, success)
    window.showStatusToast = showStatus;
})();