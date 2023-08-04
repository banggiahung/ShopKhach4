checkConfirm = new Vue({
    el: '#checkConfirm',
    data: {
        dataMain: [],

    },
    mounted() {
        axios.get("/Home/GetCart")
            .then((response) => {
                this.dataMain = response.data;
                console.log("this.dataMain", this.dataMain)
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
        ComfirmOrder(orderId) {
            try {
                // Gửi dữ liệu lên server
                axios.post(`/Home/ComfirmOrder?orderId=${orderId}`);
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
        }
    }
})