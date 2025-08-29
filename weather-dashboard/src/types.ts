export interface HourPoint {
    time: string;
    temperature: number;
    precipitation: number;
    wind: number;
}

export interface DailyPoint {
    date: string;
    temperatureMin: number;
    temperatureMax: number;
}

export interface WeatherDto {
    city: string;
    coordinates: {
        latitude: number;
        longitude: number;
    };
    hourly: HourPoint[];
    daily: DailyPoint[];
}