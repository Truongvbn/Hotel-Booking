# Hotel Booking - 3-Layer Architecture

## System Architecture Overview

```mermaid
graph TB
    subgraph "CLIENT"
        Browser["🌐 Browser"]
    end

    subgraph "PRESENTATION LAYER - HotelBooking Project"
        subgraph "Controllers"
            HC["HomeController"]
            UC["UserController"]
            HoC["HotelController"]
            BC["BookingController"]
            AC["AdminController"]
            HOC["HotelOwnerController"]
        end
        
        subgraph "Views"
            V1["User Views<br/>Login, Register, Profile"]
            V2["Hotel Views<br/>Search, Details"]
            V3["Booking Views<br/>Create, Confirm, MyBookings"]
            V4["Admin Views<br/>Dashboard, Hotels, Rooms"]
            V5["HotelOwner Views<br/>Dashboard, Manage"]
        end
        
        subgraph "ViewModels"
            VM["LoginViewModel<br/>RegisterViewModel<br/>SearchViewModel<br/>BookingViewModel<br/>AdminViewModel"]
        end
    end

    subgraph "BUSINESS LOGIC LAYER - Services Project"
        subgraph "Service Interfaces"
            IUS["IUserService"]
            IHS["IHotelService"]
            IRS["IRoomService"]
            IBS["IBookingService"]
        end
        
        subgraph "Service Implementations"
            US["UserService"]
            HS["HotelService"]
            RS["RoomService"]
            BS["BookingService"]
        end
    end

    subgraph "DATA ACCESS LAYER - DataAccess Project"
        subgraph "Repository Interfaces"
            IUR["IUserRepository"]
            IHR["IHotelRepository"]
            IRR["IRoomRepository"]
            IRTR["IRoomTypeRepository"]
            IBR["IBookingRepository"]
        end
        
        subgraph "Repository Implementations"
            UR["UserRepository"]
            HR["HotelRepository"]
            RR["RoomRepository"]
            RTR["RoomTypeRepository"]
            BR["BookingRepository"]
        end
        
        subgraph "Entity Models"
            M["User | Hotel | Room<br/>RoomType | Booking<br/>Payment | Review | Amenity"]
        end
        
        CTX["HotelBookingContext<br/>DbContext"]
    end

    subgraph "DATABASE"
        DB[("PostgreSQL<br/>Supabase")]
    end

    Browser --> HC & UC & HoC & BC & AC & HOC
    HC & UC & HoC & BC & AC & HOC --> V1 & V2 & V3 & V4 & V5
    
    UC --> IUS
    HoC --> IHS
    BC --> IBS
    AC --> IHS & IRS & IBS
    HOC --> IHS & IRS & IBS
    
    IUS --> US
    IHS --> HS
    IRS --> RS
    IBS --> BS
    
    US --> IUR
    HS --> IHR & IRTR
    RS --> IRR & IRTR
    BS --> IBR & IRR
    
    IUR --> UR
    IHR --> HR
    IRR --> RR
    IRTR --> RTR
    IBR --> BR
    
    UR & HR & RR & RTR & BR --> CTX
    CTX --> M
    CTX --> DB
```

---

## Layer Details

### 1️⃣ Presentation Layer (`HotelBooking/`)

| Component | Files | Responsibility |
|-----------|-------|----------------|
| **Controllers** | 6 files | Handle HTTP requests, route to services |
| **Views** | 31 .cshtml files | Razor templates for UI rendering |
| **ViewModels** | 6 files | Data transfer objects for views |
| **Validation** | DateValidationAttributes.cs | Custom validation attributes |

### 2️⃣ Business Logic Layer (`Services/`)

| Interface | Implementation | Responsibility |
|-----------|----------------|----------------|
| `IUserService` | `UserService` | Auth, registration, profile |
| `IHotelService` | `HotelService` | Hotel CRUD, search, availability |
| `IRoomService` | `RoomService` | Room management, status |
| `IBookingService` | `BookingService` | Booking lifecycle, payments |

### 3️⃣ Data Access Layer (`DataAccess/`)

| Component | Files | Responsibility |
|-----------|-------|----------------|
| **Models** | 8 entity classes | Database schema representation |
| **Repositories** | 5 interfaces + 5 implementations | Data access abstraction |
| **Context** | HotelBookingContext | EF Core DbContext, Fluent API |
| **Migrations** | SQL scripts | Database versioning |

---

## Dependency Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                        │
│  Controllers → ViewModels → Views                           │
│         ↓ (depends on)                                       │
├─────────────────────────────────────────────────────────────┤
│                  BUSINESS LOGIC LAYER                        │
│  IServices ← Services (implements)                          │
│         ↓ (depends on)                                       │
├─────────────────────────────────────────────────────────────┤
│                   DATA ACCESS LAYER                          │
│  IRepositories ← Repositories → DbContext → Database        │
└─────────────────────────────────────────────────────────────┘
```

---

## Dependency Injection (Program.cs)

```csharp
// Data Access Layer
builder.Services.AddDbContext<HotelBookingContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Business Logic Layer
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
```

---

## Key Design Patterns

| Pattern | Implementation |
|---------|----------------|
| **Repository Pattern** | `IRepository<T>` base + specific repos |
| **Dependency Injection** | Constructor injection via DI container |
| **Interface Segregation** | Separate interfaces per domain |
| **MVC Pattern** | ASP.NET Core MVC architecture |
