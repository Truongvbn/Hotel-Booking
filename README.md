# Hotel Booking System "Ghost Aurora"

A modern, high-performance hotel booking platform built with ASP.NET Core MVC, Entity Framework Core, and a custom "Pro Max" UI design inspired by Airbnb.

---

## 🌟 Features

### Customer Features
- **Hotel Search**: Advanced filtering by city, dates, guests with sticky search bars
- **Rich Hotel Details**: Immersive pages with image grids, reviews, and room selection
- **Seamless Booking**: Streamlined checkout flow with "Request to Book" experience
- **My Bookings**: View booking history, cancel bookings, write reviews
- **User Accounts**: Modern split-screen login and registration pages

### Hotel Owner Features
- **Dashboard**: View your hotel's key metrics (rooms, occupancy, bookings)
- **Room Management**: Manage room status (Available/Occupied/Maintenance)
- **Booking Management**: Confirm, check-in, check-out guests
- **Data Isolation**: Hotel owners can only see their own hotel data

### Platform Admin Features
- **Full Dashboard**: View platform-wide statistics
- **Hotel Management**: Create, edit, delete hotels
- **All Bookings**: View and manage bookings across all hotels
- **User Management**: Manage all users on the platform

---

## 🏗 Architecture

```
ghost-aurora/
├── DataAccess/                 # Data Access Layer
│   ├── Context/               # EF Core DbContext & Seed Data
│   ├── Models/                # Entity Models
│   └── Repositories/          # Repository Pattern Implementation
├── Services/                   # Business Logic Layer
│   └── Interfaces/            # Service Contracts
├── HotelBooking/              # Presentation Layer (ASP.NET MVC)
│   ├── Controllers/           # MVC Controllers
│   ├── ViewModels/            # View-specific Models
│   ├── Views/                 # Razor Views
│   └── wwwroot/               # Static Assets (CSS, JS, Images)
└── docs/                      # Documentation & Diagrams
```

See [docs/DIAGRAMS.md](docs/DIAGRAMS.md) for system diagrams including Use Case, ERD, and User Flows.

---

## 🛠 Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | ASP.NET Core 10.0 MVC |
| ORM | Entity Framework Core |
| Database | SQLite |
| Frontend | Razor Views, Tailwind CSS v3.4, DaisyUI v4.12 |
| Authentication | Cookie-based (Claims) |
| Build Tools | Node.js, NPM |

---

## 🔐 Role-Based Access Control

| Role | Description | Permissions |
|------|-------------|-------------|
| **Customer** | End users | Search, book rooms, view own bookings, write reviews |
| **HotelOwner** | Hotel managers | Manage their hotel's rooms and bookings only |
| **Admin** | Platform admins | Full access to all hotels, users, and system settings |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+ recommended, for CSS build)

### Installation & Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/Truongvbn/Hotel-Booking.git
   cd Hotel-Booking
   ```

2. **Build CSS (Frontend)**
   ```bash
   cd HotelBooking
   npm install
   npm run css:build
   ```

3. **Build the solution**
   ```bash
   cd ..
   dotnet build
   ```

4. **Run the Application**
   ```bash
   cd HotelBooking
   dotnet run
   ```
   
   The app will be available at: `https://localhost:5001` or `http://localhost:5000`

### First Run

On the first run, the application will automatically:
- Create the SQLite database (`hotel_booking.db`)
- Seed sample data (hotels, rooms, users)

### Development Mode (with CSS watch)

For development, run CSS in watch mode:
```bash
# Terminal 1: Watch CSS changes
cd HotelBooking
npm run css:watch

# Terminal 2: Run .NET app with hot reload
dotnet watch run
```

---

## 👤 Demo Accounts

| Role | Email | Password |
|------|-------|----------|
| Platform Admin | `admin@hotelbooking.com` | `Admin@123` |
| Customer | `customer@gmail.com` | `Customer@123` |
| Hotel Owner (Grand Luxury) | `owner.grandluxury@hotel.com` | `Owner@123` |
| Hotel Owner (Beach Paradise) | `owner.beachparadise@hotel.com` | `Owner@123` |
| Hotel Owner (Mountain View) | `owner.mountainview@hotel.com` | `Owner@123` |

---

## 🎨 Design System

The "Ghost Aurora" theme is defined in `tailwind.config.js` and `input.css`.
- **Primary Color**: Rose (#E31C5F)
- **Secondary Color**: Teal (#008489)
- **Typography**: Inter (Google Fonts)

---

## 📊 System Diagrams

Detailed diagrams are available in [docs/DIAGRAMS.md](docs/DIAGRAMS.md):
- Use Case Diagram
- Entity Relationship Diagram (ERD)
- User Flow (Booking Process)
- Admin Flow (Booking Management)
- System Architecture

---

## 📄 License

This project is licensed under the MIT License.
