
document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const searchResults = document.getElementById("searchResults");
    let debounceTimer;

    // Hàm format tiền tệ (Client side)
    const formatVND = (amount) => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
            minimumFractionDigits: 0
        }).format(amount).replace(/\s/g, '');
    };

    if (searchInput) {
        searchInput.addEventListener("input", function (e) {
            const query = e.target.value.trim();
            clearTimeout(debounceTimer);

            if (query.length < 2) {
                searchResults.classList.add("hidden");
                searchResults.innerHTML = "";
                return;
            }

            debounceTimer = setTimeout(() => {
                fetch(`/Product/SearchLive?query=${encodeURIComponent(query)}`)
                    .then(response => response.json())
                    .then(data => {
                        // Backend giờ chỉ trả về { products: [...] }
                        if (!data || !data.products || data.products.length === 0) {
                            // Ẩn luôn nếu không có kết quả (hoặc hiện thông báo tùy bạn)
                            searchResults.classList.add("hidden");
                            return;
                        }

                        let html = `<div class="bg-gray-50 px-4 py-2 text-xs font-semibold text-gray-500 uppercase tracking-wider border-b border-gray-100">Sản phẩm gợi ý</div>`;

                        data.products.forEach(p => {
                            html += `
                                <a href="/Product/Detail/${p.id}" class="flex items-start gap-4 px-4 py-3 hover:bg-neutral-light transition-colors border-b border-gray-50 last:border-0">
                                    <img src="${p.image}" alt="${p.name}" class="w-12 h-12 object-cover rounded-md border border-gray-200">
                                    <div class="flex-1 min-w-0">
                                        <h4 class="text-sm font-medium text-gray-900 truncate">${p.name}</h4>
                                        <div class="flex items-center gap-2 mt-1">
                                            <span class="text-sm font-bold text-red-600">${formatVND(p.price)}</span>
                                        </div>
                                    </div>
                                </a>`;
                        });

                        searchResults.innerHTML = html;
                        searchResults.classList.remove("hidden");
                    })
                    .catch(err => console.error(err));
            }, 400);
        });

        // Click ra ngoài thì đóng
        document.addEventListener("click", function (e) {
            if (!searchInput.contains(e.target) && !searchResults.contains(e.target)) {
                searchResults.classList.add("hidden");
            }
        });
    }
});





//document.addEventListener("DOMContentLoaded", function () {
//    const searchInput = document.getElementById("searchInput");
//    const searchResults = document.getElementById("searchResults");
//    let debounceTimer;

//    // Hàm format tiền chuẩn VNĐ
//    const formatVND = (amount) => {
//        return new Intl.NumberFormat('vi-VN', {
//            style: 'currency',
//            currency: 'VND',
//            minimumFractionDigits: 0
//        }).format(amount).replace(/\s/g, '');
//    };

//    if (searchInput && searchResults) {
//        searchInput.addEventListener("input", function (e) {
//            const query = e.target.value.trim();
//            clearTimeout(debounceTimer);

//            // Nếu xóa trắng hoặc gõ ít hơn 2 ký tự thì ẩn popup
//            if (query.length < 2) {
//                searchResults.classList.add("hidden");
//                searchResults.innerHTML = "";
//                return;
//            }

//            // Debounce 400ms
//            debounceTimer = setTimeout(() => {
//                fetch(`/Product/SearchLive?query=${encodeURIComponent(query)}`)
//                    .then(response => response.json())
//                    .then(data => { // data bây giờ là mảng sản phẩm luôn [{}, {}, {}]

//                        if (!data || data.length === 0) {
//                            searchResults.innerHTML = `<div class="p-4 text-center text-gray-500 text-sm">Không tìm thấy sản phẩm nào.</div>`;
//                            searchResults.classList.remove("hidden");
//                            return;
//                        }

//                        let html = `<div class="bg-gray-50 px-4 py-2 text-xs font-semibold text-gray-500 uppercase tracking-wider">Sản phẩm gợi ý</div>`;

//                        // Duyệt mảng sản phẩm và vẽ HTML
//                        data.forEach(p => {
//                            html += `
//                                <a href="/Product/Detail/${p.id}" class="flex items-start gap-4 px-4 py-3 hover:bg-neutral-light transition-colors border-b border-gray-50 last:border-0">
//                                    <img src="${p.image}" alt="${p.name}" class="w-12 h-12 object-cover rounded-md border border-gray-200">
//                                    <div class="flex-1 min-w-0">
//                                        <h4 class="text-sm font-medium text-gray-900 truncate">${p.name}</h4>
//                                        <div class="flex items-center gap-2 mt-1">
//                                            <span class="text-sm font-bold text-red-600">${formatVND(p.price)}</span>
//                                            ${p.oldPrice ? `<span class="text-xs text-gray-400 line-through">${formatVND(p.oldPrice)}</span>` : ''}
//                                        </div>
//                                    </div>
//                                </a>`;
//                        });

//                        // Thêm nút "Xem tất cả" ở cuối
//                        html += `
//                            <a href="/Product?search=${encodeURIComponent(query)}" class="block text-center py-2 text-xs font-bold text-primary bg-gray-50 hover:bg-gray-100 border-t border-gray-100 transition-colors">
//                                Xem tất cả kết quả cho "${query}"
//                            </a>
//                        `;

//                        searchResults.innerHTML = html;
//                        searchResults.classList.remove("hidden");
//                    })
//                    .catch(err => console.error("Lỗi search:", err));
//            }, 400);
//        });

//        // Click ra ngoài thì ẩn
//        document.addEventListener("click", function (e) {
//            if (!searchInput.contains(e.target) && !searchResults.contains(e.target)) {
//                searchResults.classList.add("hidden");
//            }
//        });
//    }
//});