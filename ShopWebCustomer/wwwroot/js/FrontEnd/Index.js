head_1 = new Vue({
    el: '#head_1',
    data: {
        dataItems: [],
        dataCate: [],
        selectedCategoryID: 0
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
        getCategoryLink(categoryId) {
            // Do something to generate the link based on the categoryId
            // Ví dụ: 
            // return '/categories/' + categoryId;
            // Hoặc sử dụng "#" nếu không cần tạo đường dẫn thực tế
            return categoryId === this.selectedCategoryID ? '#' : '#';
        },
        formatCurrency(value) {
            const formatter = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
            });
            return formatter.format(value);
        },
    },
})