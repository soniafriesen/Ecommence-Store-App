<template>
  <v-container>
    <v-row flex align-center>
      <v-col xs12 sm4 elevation-6>
        <v-card style="height:50vh;">
          <v-card-title class="justify-center display-1 deep-purple--text">Login</v-card-title>
          <v-card-text class="pt-4">
            <div>
              <v-form v-model="valid" ref="form">
                <v-text-field
                  color="primary"
                  label="Enter your e-mail address"
                  v-model="email"
                  :rules="[rules.required]"
                  required
                ></v-text-field>
                <v-text-field
                  color="primary"
                  label="Enter your password"
                  v-model="password"
                  min="4"
                  :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                  :rules="[rules.required, rules.min]"
                  :type="show1 ? 'text' : 'password'"
                  required
                  @click:append="show1 = !show1"
                ></v-text-field>
                <v-row justify="center">
                  <v-btn
                    @click="login"
                    :class=" { 'purple darken-4 white--text': valid, disabled: !valid }"
                    :disabled="valid===false"
                  >Login</v-btn>
                </v-row>
              </v-form>
            </div>
            <div class="text-center" style="margin-top:5vh;">{{status}}</div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template> 

<script> 
import fetcher from "../mixins/fetcher"; 
 
export default {   
    name: "Login",   
    data() {     
        return {       
            email: "",       
            password: "",       
            show1: false,       
            rules: {         
                required: value => !!value || "Required.",         
                min: v => v.length >= 4 || "Min 4 characters"       
            },       
            valid: false,       
            status: ""     
        };   
    },   
    methods: { 
    login: async function() {       
        await sessionStorage.removeItem("user"); 
        try 
        {         
            this.status = "logging into server";         
            let userHelper = {           
                firstname: "",           
                lastname: "",           
                email: this.email,           
                password: this.password         
            };         
            let payload = await this.$_postdata("login", userHelper); // in mixin         
            if (payload.token.indexOf("invalid") > 0) {           
                this.status = payload.token;         
            } 
            else {           
                await sessionStorage.setItem("user", JSON.stringify(payload));           
                this.$route.params.nextUrl             
                ? this.$router.push({ name: this.$route.params.nextUrl })             
                : this.$router.push({ name: "home" });         
            }       
        } catch (err) {         
            console.log("here" + err);         
            this.status = `Error has occured: ${err}`;      
            }     
        }   
    },   
    mixins: [fetcher] 
    }; 
</script>