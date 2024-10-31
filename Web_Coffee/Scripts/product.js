function filterProducts() {
    const searchQuery = $('#searchBox').val().toLowerCase();
    const categoryFilter = $('#categoryFilter').val();
    const priceFilter = $('#priceFilter').val();

    $('.product-card').each(function () {
        const productName = $(this).find('h3').text().toLowerCase();
        const productCategory = $(this).data('category').toString();
        const productPrice = parseFloat($(this).data('price'));

        let show = true;

        // Lọc theo từ khóa tìm kiếm
        if (searchQuery && !productName.includes(searchQuery)) {
            show = false;
        }

        // Lọc theo loại sản phẩm
        if (categoryFilter && productCategory !== categoryFilter) {
            show = false;
        }

        // Lọc theo mức giá
        if (priceFilter) {
            const [min, max] = priceFilter.split('-').map(Number);
            if (max && (productPrice < min || productPrice > max) || (!max && productPrice < min)) {
                show = false;
            }
        }

        $(this).toggle(show);
    });
}

// Thêm sự kiện khi nhấn nút "Thêm vào giỏ"
$(document).on('click', '.add-to-cart-btn', function () {
    alert("Sản phẩm đã được thêm vào giỏ!");
});
// Chuyển hướng đến trang ShowToCart khi nhấn vào nút "Thêm vào giỏ"
$(document).on('click', '.add-to-cart-btn', function () {
    const url = $(this).data('url');
});


            