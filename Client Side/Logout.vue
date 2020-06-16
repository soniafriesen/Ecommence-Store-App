<template>
  <div class="purple--text title" align="center" style="margin-top:20vh;">Goodbye!</div>
</template> 
<script>
    export default {
        data() {
            return {};
        },
        created: function() {
            sessionStorage.clear();
        }
    };
</script>