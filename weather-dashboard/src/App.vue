<script setup lang="ts">
import { ref } from 'vue'
import { getWeather } from './services/api';
import type { WeatherDto } from './types';
import WeatherChart from './components/WeatherChart.vue';
import WeatherForm from './components/WeatherForm.vue';

const weather = ref<WeatherDto | null>(null)
const loading = ref(false)
const error = ref<string | null>(null)

async function fetchWeather(city: string, days = 3) {
  weather.value = null
  error.value = null
  loading.value = true

  try {
    weather.value = await getWeather(city, days)
  } catch (e) {
    error.value = (e as Error).message ?? 'Weather download error'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="p-6 max-w-2xl mx-auto">
    <h1 class="text-2xl font-bold mb-4">Weather Dashboard</h1>
    
    <WeatherForm @search="fetchWeather" />
    
    <div v-if="loading" class="mt-4 text-gray-600">Loading...</div>
    <div v-if="error" class="mt-4 text-red-500">{{ error }}</div>
    
    <div v-if="weather?.coordinates" class="mt-6">
      <p class="mb-2 font-semibold">
        Results for: {{ weather.city }} ({{ weather.coordinates.latitude.toFixed(2) }}, {{ weather.coordinates.longitude.toFixed(2) }})
      </p>
      <WeatherChart :data="weather" />
      <div class="mt-4">
        <h2 class="font-bold">Daily forecasts:</h2>
        <ul>
          <li v-for="d in weather.daily" :key="d.date">
            {{ d.date }}: min {{ d.temperatureMin }}°C, max {{ d.temperatureMax }}°C
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<style>
body { font-family: sans-serif; }
</style>
