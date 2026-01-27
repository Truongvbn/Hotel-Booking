# Hotel Booking System "Ghost Aurora"

A modern, high-performance hotel booking application built with ASP.NET Core MVC, Entity Framework Core, and a custom "Pro Max" UI design inspired by Airbnb.

## 🌟 Features

- **Modern UI/UX**: "Ghost Aurora" design system featuring a clean, flat, and minimalistic aesthetic.
- **Hotel Search**: Advanced filtering, sticky search bars, and interactive horizontal cards.
- **Rich Details**: Immersive hotel detail pages with image grids, sticky booking widgets, and reviews.
- **Seamless Booking**: Streamlined checkout flow with "Request to Book" experience.
- **User Accounts**: Modern split-screen login and registration pages.
- **Responsive Design**: Fully responsive layout optimized for mobile, tablet, and desktop.

## 🛠 Tech Stack

- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server / Entity Framework Core
- **Frontend**: 
  - HTML5, Razor Views
  - Tailwind CSS (v3.4)
  - DaisyUI (v4.12)
  - Inter Font Family
- **Build Tools**: Node.js, NPM

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Node.js (for CSS build)

### Installation

1.  **Clone the repository**
    ```bash
    git clone <repository-url>
    cd HotelBooking
    ```

2.  **Install Frontend Dependencies**
    ```bash
    npm install
    npm run css:build
    ```

3.  **Configure Database**
    Update `appsettings.json` with your connection string if needed.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

4.  **Run the Application**
    ```bash
    dotnet run
    ```
    The application will act seed data automatically on the first run.

## 🎨 Design System

The "Ghost Aurora" theme is defined in `tailwind.config.js` and `input.css`.
- **Primary Color**: Rose (#FF385C)
- **Secondary Color**: Teal (#008489)
- **Typography**: Inter (Google Fonts)

## 📄 License

This project is licensed under the MIT License.
