<template>
  <form @submit.prevent="onSubmit" 
        class="bg-white shadow-md rounded-2xl p-6 space-y-4 max-w-lg">
    
    <!-- City -->
    <div class="grid grid-cols-3 gap-4 items-center">
      <label for="city" class="text-sm font-medium text-gray-700">
        City
      </label>
      <input 
        id="city"
        v-model="city" 
        type="text" 
        placeholder="Enter a city" 
        class="col-span-2 border border-gray-300 rounded-lg p-2 
               focus:ring-2 focus:ring-blue-400 focus:border-blue-400 outline-none"
      >
    </div>

    <!-- Forecast length -->
    <div class="grid grid-cols-3 gap-4 items-center">
      <label for="days" class="text-sm font-medium text-gray-700">
        Forecast length
      </label>
      <select 
        id="days"
        v-model.number="days" 
        class="col-span-2 border border-gray-300 rounded-lg p-2 
               focus:ring-2 focus:ring-blue-400 focus:border-blue-400 outline-none"
      >
        <option v-for="n in 7" :key="n" :value="n">
          {{ n }} day{{ n > 1 ? 's' : '' }}
        </option>
      </select>
    </div>

    <!-- Button -->
    <div class="flex justify-end">
      <button 
        type="submit" 
        class="bg-blue-500 hover:bg-blue-600 text-white font-semibold px-6 py-2 rounded-xl shadow-sm transition-colors"
      >
        Search
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits<{ 
  (e: 'search', city: string, days?: number): void 
}>()

const city = ref('')
const days = ref(3)

function onSubmit() {
  if (!city.value.trim()) 
    return
  emit('search', city.value.trim(), days.value)
}
</script>
