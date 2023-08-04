account = new Vue({
    el: '#account',
    data: {
        activeTab: 'Account',
        dataMain: [],
        email: "",
        phone: "",
        firstName: "",
        lastName: "",
        orderData: [],
        checkOrder: false,
        address: "",
        imageUser: "../Img/default.jpg"
    },
    computed: {
        formattedEmail() {
            return this.email.toLowerCase();
        },
    },
    mounted() {
        axios.get("/AccountUser/GetUser")
            .then((response) => {
                this.dataMain = response.data;
                this.email = this.dataMain.userName;
                this.phone = this.dataMain.phone;
                this.firstName = this.dataMain.firstName;
                this.lastName = this.dataMain.lastName;
                this.imageUser = this.dataMain.userImage;
                if (this.imageUser == "") {
                    this.imageUser = "../ Img /default.jpg";
                }
                this.address = this.dataMain.address;
                return Promise.resolve();
            })
            .catch(error => {
                console.log(error);
            });
        axios.get("/Home/GetCartNoFalse")
            .then((response) => {
                this.orderData = response.data;
                const check = this.orderData.length;
                console.log(check);
                if (check > 0) {
                    this.checkOrder = true;
                }
                return Promise.resolve();
            })
            .catch(error => {
                console.log(error);
            });
    },
    methods: {
      
        calculateTotalPrice() {
            let totalPrice = 0;
            this.dataMain.forEach(item => {
                const itemPrice = item.product_Price * item.product_Quantity;
                totalPrice += itemPrice;
            });
            return totalPrice;
        },
        formatCurrency(value) {
            return value.toLocaleString('en-US', {
                style: 'currency',
                currency: 'USD'
            });
        },
        handleImageClick() {
            // Khi click vào hình ảnh, kích hoạt sự kiện click của file input ẩn
            this.$refs.fileInput.click();
        },
        handleFileChange(event) {
            // Xử lý sự kiện thay đổi file, lấy hình ảnh đã chọn và hiển thị
            const file = event.target.files[0];
            if (file) {
                // Xử lý file ở đây (ví dụ: hiển thị hình ảnh mới)
                this.imageUser = URL.createObjectURL(file);
            }
        },
        async uploadFile() {
            try {
                // Khởi tạo formData
                const formData = new FormData();

                // Thêm ảnh đại diện vào formData
                formData.append('AnhDaiDien', this.$refs.fileInput.files[0]);

                // Gửi dữ liệu lên server
                await axios.post('/Home/Profile', formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                });

                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu ảnh thành công',
                    confirmButtonText: 'OK'
                });
            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Đã có lỗi xảy ra khi lưu ảnh',
                    confirmButtonText: 'OK'
                });
            }
        },
        async uploadProfile() {
            try {
                // Khởi tạo formData
                const formData = new FormData();
                
                // Thêm ảnh đại diện vào formData
                formData.append('Phone', this.phone);
                formData.append('FirstName', this.firstName);
                formData.append('LastName', this.lastName);

                // Gửi dữ liệu lên server
                await axios.post('/Home/ProfileInfo', formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                });

                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã cập nhật thành công',
                    confirmButtonText: 'OK'
                });
            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Đã có lỗi',
                    confirmButtonText: 'OK'
                });
            }
        },
    }
})