import axios from 'axios'
import type { WeatherDto } from '../types'

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL
})

export async function getWeather(city: string, days = 3): Promise<WeatherDto> {
    const {data } = await api.get<WeatherDto>(`/weather/${encodeURIComponent(city)}`, { 
        params: { days } 
    })
    return data
}