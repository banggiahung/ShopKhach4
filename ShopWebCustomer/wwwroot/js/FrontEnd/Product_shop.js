product_shop = new Vue({
    el: '#product_shop',
    data: {
        dataItems: [],
        dataCate: [],
        itemsPerPage: 12, // Số sản phẩm trên mỗi trang
        currentPage: 1,
        searchKeyword: "",
    },
    computed: {
        filteredDataItems() {
            // Lọc danh sách sản phẩm dựa trên từ khóa tìm kiếm
            return this.dataItems.filter((product) =>
                product.productName.toLowerCase().includes(this.searchKeyword.toLowerCase())
            );
        },

        // Tổng số trang dựa trên số lượng sản phẩm và số sản phẩm trên mỗi trang
        totalPages() {
            return Math.ceil(this.filteredDataItems.length / this.itemsPerPage);
        },

        // Danh sách sản phẩm tương ứng với trang hiện tại
        paginatedDataItems() {
            const startIndex = (this.currentPage - 1) * this.itemsPerPage;
            const endIndex = startIndex + this.itemsPerPage;
            return this.filteredDataItems.slice(startIndex, endIndex);
        },

        
        visiblePages() {
            // Tính danh sách trang hiển thị trên phân trang
            const totalVisiblePages = 5; // Số lượng trang hiển thị tối đa
            const halfVisiblePages = Math.floor(totalVisiblePages / 2);
            let startPage = this.currentPage - halfVisiblePages;
            let endPage = this.currentPage + halfVisiblePages;

            if (startPage <= 0) {
                startPage = 1;
                endPage = Math.min(totalVisiblePages, this.totalPages);
            } else if (endPage > this.totalPages) {
                endPage = this.totalPages;
                startPage = Math.max(1, this.totalPages - totalVisiblePages + 1);
            }

            return Array.from({ length: endPage - startPage + 1 }, (_, index) => startPage + index);
        },
    },
    mounted() {
        axios.get("/Products/GetAllCategory")
            .then((response) => {
                this.dataCate = response.data;
                return Promise.resolve();
            });
        axios.get("/Products/GetAllProduct")
            .then((response) => {
                this.dataItems = response.data;
                return Promise.resolve();
            });
    },
    methods: {
        handleSearch() {
            this.currentPage = 1; 
        },
        getCategoryLink(categoryId) {
            axios.get(`/Home/GetProductsByCate?id=${categoryId}`)
                .then((response) => {
                    this.dataItems = response.data;
                });
            this.currentPage = 1;
        },
        formatCurrency(value) {
            const formatter = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
            });
            return formatter.format(value);
        },
        prevPage() {
            // Điều hướng đến trang trước đó
            if (this.currentPage > 1) {
                this.currentPage -= 1;
            }
        },
        nextPage() {
            // Điều hướng đến trang kế tiếp
            if (this.currentPage < this.totalPages) {
                this.currentPage += 1;
            }
        },
        gotoPage(pageNumber) {
            // Điều hướng đến trang có số thứ tự pageNumber
            this.currentPage = pageNumber;
        },
    },
})