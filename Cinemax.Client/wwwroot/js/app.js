window.bootstrapModal = {
    hide: function (id) {
        const modalElement = document.getElementById(id);

        if (!modalElement) {
            return;
        }

        const modal = bootstrap.Modal.getInstance(modalElement)
            || new bootstrap.Modal(modalElement);

        modal.hide();
    }
};

window.multiSelectChips = {
    registerOutsideClick: function (element, dotNetHelper) {
        const handler = function (event) {
            if (!element.contains(event.target)) {
                dotNetHelper.invokeMethodAsync('CloseDropdown');
            }
        };

        document.addEventListener('mousedown', handler);

        return {
            dispose: function () {
                document.removeEventListener('mousedown', handler);
            }
        };
    }
};