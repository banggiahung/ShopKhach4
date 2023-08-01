Admin_vue = new Vue({
    el: '#Admin_vue',
    data: {
        dataItems:[],
        ProductName: "",
        Description: "",
        Slug: "",
        Price: 0,
        CategoryID: 0,
        PrPath: null,
        imageFile: null,
        previewImage: null,
        uploadedImage: null,
    },
    mounted() {
        axios.get("/Products/GetAllProduct")
            .then((response) => {
                this.dataItems = response.data;
                return Promise.resolve();
            });
    },
    methods: {
        onFileChange(event) {
            this.imageFile = event.target.files[0];
            this.previewImage = URL.createObjectURL(this.imageFile);
            this.uploadedImage = null;
        },
        generateSlug() {
            // Chuyển đổi tên sản phẩm thành dạng slug
            this.Slug = this.ProductName
                .toLowerCase() // Chuyển tất cả thành chữ thường
                .replace(/\s+/g, '-') // Thay thế dấu cách bằng dấu '-'
                .replace(/[^a-z0-9\-]/g, '') // Loại bỏ các ký tự đặc biệt
                .replace(/\-{2,}/g, '-'); // Loại bỏ các dấu '-' liên tiếp

            // Đảm bảo slug không bắt đầu hoặc kết thúc bằng dấu '-'
            this.Slug = this.Slug.replace(/^\-+|\-+$/g, '');
        },
        async addProducts() {
            try {
                const formData = new FormData();
                formData.append('ProductName', this.ProductName);
                formData.append('Description', this.Description);
                formData.append('Slug', this.Slug);
                formData.append('Price', this.Price);
                formData.append('CategoryID', this.CategoryID); 
                formData.append('PrPath', this.$refs.PrPath.files[0]);

                await axios.post('/Products/AddProduct', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu thành công',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Đã có lỗi xảy ra',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            }
        },
        async addProductsSubCategory() {
            try {
                const formData = new FormData();
                formData.append('SubCategory_Name', this.SubCategory_Name);
                formData.append('CategoryId', 1);


                await axios.post('/Admin/Dashboard/AddCateSub', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu thành công',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Đã có lỗi xảy ra',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            }
        },
    },
});