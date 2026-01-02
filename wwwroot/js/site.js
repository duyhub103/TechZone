

document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const searchResults = document.getElementById("searchResults");
    let debounceTimer;

    // Hàm định dạng tiền VNĐ
    const formatCurrency = (amount) => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(amount).format(amount).replace(/\s/g, '');
    };

    if (searchInput) {
        searchInput.addEventListener("input", function (e) {
            const query = e.target.value.trim();

            // Clear timer cũ để tránh gọi server liên tục (Debounce)
            clearTimeout(debounceTimer);

            if (query.length < 2) {
                searchResults.classList.add("hidden");
                searchResults.innerHTML = "";
                return;
            }

            // delay 400ms sau khi ngừng gõ
            debounceTimer = setTimeout(() => {
                fetch(`/Product/SearchLive?query=${encodeURIComponent(query)}`)
                    .then(response => response.json())
                    .then(data => {
                        if (!data || (data.suggestions.length === 0 && data.products.length === 0)) {
                            searchResults.innerHTML = `<div class="p-4 text-center text-gray-500 text-sm">Không tìm thấy kết quả</div>`;
                            searchResults.classList.remove("hidden");
                            return;
                        }

                        let html = "";

                        // Phần Gợi ý từ khóa
                        if (data.suggestions.length > 0) {
                            html += `<div class="bg-gray-50 px-4 py-2 text-xs font-semibold text-gray-500 uppercase tracking-wider">Có phải bạn muốn tìm</div>`;
                            data.suggestions.forEach(item => {
                                html += `
                                    <a href="/Product?value=${encodeURIComponent(item)}" class="block px-4 py-2.5 text-sm text-gray-700 hover:bg-neutral-light hover:text-primary transition-colors">
                                        <div class="flex items-center gap-2">
                                            <span class="material-symbols-outlined text-[18px] text-gray-400">search</span>
                                            ${item}
                                        </div>
                                    </a>`;
                            });
                        }

                        // Phần Sản phẩm gợi ý
                        if (data.products.length > 0) {
                            html += `<div class="bg-gray-50 px-4 py-2 text-xs font-semibold text-gray-500 uppercase tracking-wider border-t border-gray-100">Sản phẩm gợi ý</div>`;
                            data.products.forEach(p => {
                                html += `
                                    <a href="/Product/Detail/${p.id}" class="flex items-start gap-4 px-4 py-3 hover:bg-neutral-light transition-colors border-b border-gray-50 last:border-0">
                                        <img src="${p.image}" alt="${p.name}" class="w-12 h-12 object-cover rounded-md border border-gray-200">
                                        <div class="flex-1 min-w-0">
                                            <h4 class="text-sm font-medium text-gray-900 truncate">${p.name}</h4>
                                            <div class="flex items-center gap-2 mt-1">
                                                <span class="text-sm font-bold text-red-600">${formatCurrency(p.price)}</span>
                                                ${p.oldPrice ? `<span class="text-xs text-gray-400 line-through">${formatCurrency(p.oldPrice)}</span>` : ''}
                                            </div>
                                        </div>
                                    </a>`;
                            });
                        }

                        searchResults.innerHTML = html;
                        searchResults.classList.remove("hidden");
                    })
                    .catch(err => console.error(err));
            }, 400); // 400ms delay
        });

        // Click ra ngoài thì đóng popup
        document.addEventListener("click", function (e) {
            if (!searchInput.contains(e.target) && !searchResults.contains(e.target)) {
                searchResults.classList.add("hidden");
            }
        });
    }
});