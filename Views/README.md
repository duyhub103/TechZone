🎨 TechZone UI/UX Documentation

Tài liệu này mô tả cấu trúc giao diện (Front-end) và các màn hình chính của ứng dụng TechZone.

📂 Cấu trúc thư mục Views

Thư mục Views chứa toàn bộ các file giao diện .cshtml (Razor Views) được chia theo Controller.

Views/
├── Account/            # Các trang Đăng nhập, Đăng ký
├── Cart/               # Giỏ hàng
├── Checkout/           # Trang Thanh toán
├── Home/               # Trang chủ, About, Contact
├── Product/            # Danh sách sản phẩm, Chi tiết, Tìm kiếm
├── Shared/             # Các thành phần dùng chung (Layout, Header, Footer)
│   ├── _Layout.cshtml  # Layout chính
│   ├── _Header.cshtml  # Thanh điều hướng (Navigation)
│   └── _Footer.cshtml  # Chân trang
└── demo-images/        #


📸 Demo Screenshots

1. Trang chủ (Home Page)

Giao diện chính hiển thị Banner, Sản phẩm nổi bật và Danh mục.

2. Đăng nhập & Tài khoản (Login)

Màn hình đăng nhập dành cho thành viên.

3. Chi tiết sản phẩm (Product Detail)

Hiển thị thông tin chi tiết, thông số kỹ thuật và đánh giá.

4. Giỏ hàng (Shopping Cart)

Quản lý các sản phẩm đã chọn mua.

5. Thanh toán (Checkout)

Quy trình nhập thông tin giao hàng và đặt hàng.

🧩 Components (Partial Views)

Các thành phần nhỏ được tái sử dụng:

_ProductItem.cshtml: Card hiển thị thông tin tóm tắt của 1 sản phẩm.

_ReviewList.cshtml: Danh sách đánh giá load bằng AJAX.

_CartSummary.cshtml: Widget giỏ hàng nhỏ trên Header.