<template>   
    <v-container >     
        <v-row justify="center">       
            <v-col class="display-1 text-center">Cart Contents</v-col>     
        </v-row>     
        <v-row>       
            <v-col justify="center" align="center">         
                <v-img           
                :src="require('../assets/Cart.png')"           
                style="height:12vh;width:13vh;"           
                aspect-ratio="1"         
                />      
            </v-col>     
        </v-row>     
        <v-row style="margin:2vw;">{{status}}</v-row>
        <div v-if="cart.length > 0">
            <v-row>
                <v-col class="text-center">#</v-col>
                <v-col class="text-center">Name</v-col>
                <v-col class="text-center">Qty</v-col>
                <v-col class="text-center">Price</v-col>
                <v-col class="text-center">Ext</v-col>
            </v-row>
            <v-row v-for="cartItem in cart" :key="cartItem.id">             
                <v-col class="text-center">{{ cartItem.item.id }}</v-col>             
                <v-col class="text-center">{{ cartItem.item.productName }}</v-col> 
                <v-col class="text-center">{{ cartItem.qty }}</v-col>             
                <v-col class="text-center">{{'$' + cartItem.item.msrp.toFixed(2)}}</v-col>  
                <v-col class="text-center">{{'$' + (cartItem.item.msrp * cartItem.qty).toFixed(2)}}</v-col>                           
            </v-row>       
            <v-row>
                <v-col></v-col>   
                <v-col></v-col> 
                <v-col></v-col> 
                <v-col class="text-center">Sub Total: </v-col>             
                <v-col class="text-center">{{'$' + subtotal.toFixed(2)}}</v-col>                            
            </v-row>
            <v-row>   
                <v-col></v-col>   
                <v-col></v-col> 
                <v-col></v-col> 
                <v-col class="text-center">Tax: </v-col>             
                <v-col class="text-center">{{'$' + tax.toFixed(2)}}</v-col>                            
            </v-row>  
            <v-row>  
                <v-col></v-col>   
                <v-col></v-col> 
                <v-col></v-col>          
                <v-col class="text-center">Cart Total: </v-col>             
                <v-col class="text-center">{{'$' + carttotal.toFixed(2)}}</v-col>                            
            </v-row>          
            <v-row justify="center">
                <v-col>
                    <v-btn medium color="primary" class="text--secondary" @click="clearCart">Clear Cart</v-btn>
                </v-col>
                <v-col>
                    <v-btn medium color="primary" class="text--secondary" @click="addOrder">Add Order</v-btn>
                </v-col>     
            </v-row>
        </div>
    </v-container>    
</template>
<script> 
import fetcher from "../mixins/fetcher";
export default {   
    name: "CartDetails",   
    data() {     
        return {      
            totalQty:0,       
            subtotal: 0,
            tax: 0,
            carttotal: 0,
            cart: [],       
            status: ""     
        };   
    },   
    beforeMount: function() {     
        if (sessionStorage.getItem("cart")) {       
            this.cart = JSON.parse(sessionStorage.getItem("cart")); 
            this.cart.map(cartItem => { 
                this.totalQty += cartItem.qty;
                this.subtotal += cartItem.item.msrp * cartItem.qty;
                this.tax = this.subtotal * 0.13;
                this.carttotal = this.tax + this.subtotal;    
            });     
        } else {       
            this.cart = [];     
        }   
    },   
    mixins: [fetcher],
    methods: {     
        clearCart: function() {       
            sessionStorage.removeItem("cart");       
            this.cart = [];       
            this.status = "cart cleared";     
        },  
        addOrder: async function(){
            let customer = JSON.parse(sessionStorage.getItem("user"));      
            let cart = JSON.parse(sessionStorage.getItem("cart")); 
            try{
                this.status = "sending cart info to server"; 
                let cartHelper = { email: customer.email, selections: cart }; 
                let payload = await this.$_postdata("order", cartHelper); // in mixin 
                if (payload.indexOf("not") > 0) {           
                     this.status = payload;                             
                }else{
                    this.clearCart();
                    this.status = payload;
                }                
            }catch (err) {         
                console.log(err);         
                this.status = `Error add cart: ${err}`; 
            }
        } 
   }        
}; 
</script>