<template>
    <form @submit.prevent="onSubmit" class="flex gap-2">
        <input v-model="city" type="text" placeholder="Enter a city" 
            class="border rounded p-2 flex-1">
        <button type="submit" 
            class="bg-blue-500 text-white px-4 py-2 rounded">
            Search
        </button>
    </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits<{ (e: 'search', city: string): void}>()
const city = ref('')

function onSubmit() {
    if (!city.value.trim()) 
        return
    emit('search', city.value.trim())
}
</script>