<template>
    <div>
        <LineChart v-if="chartData" :data="chartData" :options="options"/>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Chart as ChartJS, Title, Tooltip, Legend, LineElement, PointElement, LinearScale, CategoryScale, plugins } from 'chart.js'
import { Line as LineChart } from 'vue-chartjs'
import type { WeatherDto } from '../types'

ChartJS.register(Title, Tooltip, Legend, LineElement, PointElement, CategoryScale, LinearScale)

const props = defineProps<{ data: WeatherDto }>()

const chartData = computed(() => ({
    labels: props.data.hourly.map(h => h.time.slice(11, 16)), //hour
    datasets: [
        {
            label: 'Temperature (°C)',
            data: props.data.hourly.map(h => h.temperature),
            // borderColor: 'rgb(255, 99, 132)',
            // backgroundColor: 'rgba(255, 99, 132, 0.5)',
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.2
        }
    ]
}))

const options = {
    responsive: true,
    plugins: {
        legend: { display: true },
        title: {
            display: true,
            text: 'Hourly forecast'
        }
    }
}
</script>