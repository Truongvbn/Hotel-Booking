/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Views/**/*.cshtml",
        "./wwwroot/**/*.js"
    ],
    theme: {
        extend: {
            fontFamily: {
                sans: ['Inter', 'sans-serif'],
            },
            boxShadow: {
                'soft': '0 4px 6px -1px rgba(0, 0, 0, 0.05), 0 2px 4px -1px rgba(0, 0, 0, 0.03)',
                'hover': '0 10px 15px -3px rgba(0, 0, 0, 0.08), 0 4px 6px -2px rgba(0, 0, 0, 0.04)',
            }
        },
    },
    plugins: [require("daisyui")],
    daisyui: {
        themes: [
            {
                ghost: {
                    "primary": "#E31C5F", // Slightly deeper Airbnb Red
                    "primary-content": "#ffffff",
                    "secondary": "#008489",
                    "accent": "#FFB400",
                    "neutral": "#222222",
                    "base-100": "#ffffff",
                    "base-200": "#f7f7f7",
                    "base-300": "#dddddd",
                    "info": "#3b82f6",
                    "success": "#10b981",
                    "warning": "#f59e0b",
                    "error": "#ef4444",
                    "--rounded-box": "1rem",
                    "--rounded-btn": "0.5rem",
                    "--rounded-badge": "1.9rem",
                    "--animation-btn": "0.2s",
                },
            },
            "light",
        ],
        darkTheme: "light", // Force light mode for now as per "Airbnb" clean aesthetic, or keep it light default.
    },
}
