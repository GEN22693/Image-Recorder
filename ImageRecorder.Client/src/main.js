import { createApp } from 'vue'
import App from './App.vue'
import './assets/main.css'

import PrimeVue from 'primevue/config';
import "primevue/resources/themes/lara-light-indigo/theme.css";     
import "primevue/resources/primevue.min.css";
import 'primeicons/primeicons.css';

import Button from "primevue/button"

const app = createApp(App);

app.use(PrimeVue);

app.component('Button', Button);

app.mount('#app')
