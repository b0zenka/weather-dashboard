<template>
    <form @submit.prevent="onSubmit" class="flex gap-2">
        <input v-model="city" type="text" placeholder="Enter a city" 
            class="border rounded p-2 flex-1">
        <select v-model.number="days" class="border rounded p-2">
            <option value="1">1 day</option>
            <option value="2">2 days</option>
            <option value="3">3 days</option>
            <option value="4">4 days</option>
            <option value="5">5 days</option>
            <option value="6">6 days</option>
            <option value="7">7 days</option>
        </select>
        <button type="submit" 
            class="bg-blue-500 text-white px-4 py-2 rounded">
            Search
        </button>
    </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits<{ (e: 'search', city: string, days?: number): void}>()
const city = ref('')
const days = ref(3)

function onSubmit() {
    if (!city.value.trim()) 
        return
    emit('search', city.value.trim(), days.value)
}
</script>