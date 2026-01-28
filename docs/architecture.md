# Hotel Booking - 3-Layer Architecture

## Request/Response Flow

```mermaid
flowchart TB
    subgraph CLIENT
        Browser["🌐 Browser"]
    end

    subgraph LAYER1["1️⃣ PRESENTATION LAYER"]
        Controllers["Controllers<br/>UserController, HotelController,<br/>BookingController, AdminController"]
        Views["Views (31 files)<br/>Razor Templates"]
        ViewModels["ViewModels<br/>LoginVM, SearchVM, BookingVM"]
    end

    subgraph LAYER2["2️⃣ BUSINESS LOGIC LAYER"]
        IServices["Interfaces<br/>IUserService, IHotelService,<br/>IRoomService, IBookingService"]
        Services["Implementations<br/>UserService, HotelService,<br/>RoomService, BookingService"]
    end

    subgraph LAYER3["3️⃣ DATA ACCESS LAYER"]
        IRepos["Interfaces<br/>IUserRepository, IHotelRepository,<br/>IRoomRepository, IBookingRepository"]
        Repos["Implementations<br/>UserRepository, HotelRepository,<br/>RoomRepository, BookingRepository"]
        Context["HotelBookingContext<br/>EF Core DbContext"]
        Models["Entity Models<br/>User, Hotel, Room, Booking"]
    end

    subgraph DB["DATABASE"]
        PostgreSQL[("PostgreSQL<br/>Supabase")]
    end

    %% Request Flow (down) - Blue
    Browser -->|"① HTTP Request"| Controllers
    Controllers -->|"② Call Interface"| IServices
    IServices -->|"DI Inject"| Services
    Services -->|"③ Call Interface"| IRepos
    IRepos -->|"DI Inject"| Repos
    Repos -->|"④ Query"| Context
    Context -->|"⑤ SQL"| PostgreSQL

    %% Response Flow (up) - Green
    PostgreSQL -.->|"⑥ Data"| Context
    Context -.->|"⑦ Entity"| Repos
    Repos -.->|"⑧ Entity"| Services
    Services -.->|"⑨ DTO/Entity"| Controllers
    Controllers -->|"Bind"| ViewModels
    ViewModels -->|"Render"| Views
    Views -.->|"⑩ HTML"| Browser

    %% Styling
    style LAYER1 fill:#e3f2fd,stroke:#1976d2
    style LAYER2 fill:#e8f5e9,stroke:#388e3c
    style LAYER3 fill:#fff3e0,stroke:#f57c00
    style DB fill:#fce4ec,stroke:#c2185b
```

---

## Detailed Flow (2-Way)

```
┌─────────────────────────────────────────────────────────────────┐
│  BROWSER                                                         │
│  ① Request: POST /Login {email, password}                       │
│  ⑩ Response: HTML + Cookie                                      │
└─────────────────────────┬───────────────────────────────────────┘
                          │ ↓ Request    ↑ Response
┌─────────────────────────▼───────────────────────────────────────┐
│  1️⃣ PRESENTATION LAYER                                          │
│  ② Controller gọi IUserService.AuthenticateAsync()              │
│  ⑨ Nhận User entity → Tạo cookie → Render View                  │
└─────────────────────────┬───────────────────────────────────────┘
                          │ ↓ Request    ↑ Response
┌─────────────────────────▼───────────────────────────────────────┐
│  2️⃣ BUSINESS LOGIC LAYER                                        │
│  ③ Service gọi IUserRepository.GetByEmailAsync()                │
│  ⑧ Verify password → Return User hoặc null                     │
└─────────────────────────┬───────────────────────────────────────┘
                          │ ↓ Request    ↑ Response
┌─────────────────────────▼───────────────────────────────────────┐
│  3️⃣ DATA ACCESS LAYER                                           │
│  ④ Repository gọi DbContext.Users.FirstOrDefaultAsync()        │
│  ⑦ Return User entity                                           │
└─────────────────────────┬───────────────────────────────────────┘
                          │ ↓ SQL        ↑ Data
┌─────────────────────────▼───────────────────────────────────────┐
│  DATABASE                                                        │
│  ⑤ SELECT * FROM "Users" WHERE "Email" = @email                 │
│  ⑥ Return row data                                               │
└─────────────────────────────────────────────────────────────────┘
```

---

## Layer Responsibilities

| Layer | Request (↓) | Response (↑) |
|-------|-------------|--------------|
| **Presentation** | Nhận HTTP request, validate input | Return View/JSON/Redirect |
| **Business Logic** | Thực thi business rules | Return DTO/Entity/Exception |
| **Data Access** | Query database | Return Entity/Collection |
