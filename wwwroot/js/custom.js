/* wwwroot/js/custom.js */

// Code cũ của bạn...

// Xử lý nút tăng giảm số lượng
document.addEventListener("DOMContentLoaded", function () {
    const btnMinus = document.querySelectorAll('.btn-outline-secondary:first-child'); // Chọn nút -
    const btnPlus = document.querySelectorAll('.btn-outline-secondary:last-child');   // Chọn nút +

    btnMinus.forEach(btn => {
        btn.addEventListener('click', function () {
            let input = this.nextElementSibling;
            let value = parseInt(input.value);
            if (value > 1) {
                input.value = value - 1;
            }
        });
    });

    btnPlus.forEach(btn => {
        btn.addEventListener('click', function () {
            let input = this.previousElementSibling;
            let value = parseInt(input.value);
            input.value = value + 1;
        });
    });
});