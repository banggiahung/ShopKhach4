Cart_checkout = new Vue({
    el: '#Cart_checkout',
    data: {
        dataMain: [],
        isCheckedItems: [],
        itemCount:0
    },
    mounted() {
        axios.get("/Home/GetCart")
            .then((response) => {
                this.dataMain = response.data;
                this.itemCount = this.dataMain.length;
                return Promise.resolve();
            })
            .catch(error => {
                console.log(error);
            });
    },
    methods: {
        formatCurrency(value) {
            return value.toLocaleString('en-US', {
                style: 'currency',
                currency: 'USD'
            });
        },
        toggleItemChecked(index) {
            this.isCheckedItems[index] = !this.isCheckedItems[index];
        },
        deleteOrdersAndOrderDetails(index) {
            const orderDetailIds = this.dataMain[index].orderDetailIds;
            const orderIds = this.dataMain[index].orderIds;
            const request = {
                orderDetailIds: orderDetailIds,
                orderIds: orderIds
            };
            console.log(request)
            axios.post("/Customer/Home/DeleteOrdersAndOrderDetails", request)
                .then(response => {
                    console.log("thành công");
                    window.location.reload();
                })
                .catch(error => {
                    console.log("Lỗi ", error);
                });
        },
        calculateTotalPrice() {
            let totalPrice = 0;
            this.dataMain.forEach(item => {
                const itemPrice = item.product_Price * item.product_Quantity;
                totalPrice += itemPrice;
            });
            return this.formatCurrency(totalPrice);
        },
    },
    computed: {
        totalPrices() {
            return this.dataMain.map((item, index) => {
                const basePrice = item.product_Price * item.product_Quantity;
                return this.isCheckedItems[index] ? basePrice + 39  :  basePrice;
            });
        }
    },
   
})