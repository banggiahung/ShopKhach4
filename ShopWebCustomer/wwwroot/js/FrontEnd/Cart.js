Details_View = new Vue({
    el: '#Details_View',
    data: {
        getCart: [],
        userId: null,
        checkUserId: false
    },
    mounted() {
        this.getApi();
    },
    methods: {
        async getApi() {
            try {
                const res = await axios.get('/Home/GetUserDataId');
                this.userId = res.data;
                if (this.userId != 0) {
                    this.checkUserId = true;
                }
            } catch (error) {
                console.log(error);
                this.checkUserId = false;
            }
        },
        AddToCartVue() {
            if (this.checkUserId == false) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Bạn chưa đăng nhập'
                });
                return;
            }
            const formData = new FormData();
            formData.append('ProductIds', $("#productId").val());
            formData.append('Product_Quantity', $("#quantity").val());

            axios.post("/Home/AddToCart", formData, {
                headers: {

                }
            }).then(response => {
                // Xử lý phản hồi thành công
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'success',
                    title: 'Đã thêm vào giỏ hành thành công'
                }).then(() => {
                    // Sau khi toast đóng, thực hiện reload trang
                    window.location.reload();
                });

            })
                .catch(error => {
                    // Xử lý lỗi
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'error',
                        title: 'Đã xảy ra lỗi'
                    });
                    console.error(error);
                });
        },
        GetCart() {
            if (this.userId != 0) {
                axios.get("/Home/GetCart")
                    .then(rs => {
                        this.getCart = rs.data;
                        console.log(" this.getCart", this.getCart);
                    }).catch(error => {
                        console.error(error);
                    });
            }
           

        }
    }
})