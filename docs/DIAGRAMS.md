# System Design & Diagrams

This document contains the system architecture diagrams for the Hotel Booking application. All diagrams are written in Mermaid format and will render directly on GitHub.

---

## 1. Use Case Diagram

```mermaid
flowchart TB
    subgraph Actors
        Guest((Guest))
        Admin((Admin))
    end

    subgraph "Hotel Booking System"
        UC1[Search Hotels]
        UC2[View Hotel Details]
        UC3[Book Room]
        UC4[Manage Booking]
        UC5[Write Review]
        UC6[Login / Register]
        
        UC7[Manage Hotels]
        UC8[Manage Rooms]
        UC9[Manage Bookings]
        UC10[View Dashboard]
    end

    Guest --> UC1
    Guest --> UC2
    Guest --> UC3
    Guest --> UC4
    Guest --> UC5
    Guest --> UC6

    Admin --> UC6
    Admin --> UC7
    Admin --> UC8
    Admin --> UC9
    Admin --> UC10
```

---

## 2. Entity Relationship Diagram (ERD)

```mermaid
erDiagram
    User ||--o{ Booking : "makes"
    User ||--o{ Review : "writes"
    Hotel ||--o{ RoomType : "has"
    Hotel ||--o{ Review : "receives"
    RoomType ||--o{ Room : "contains"
    Room ||--o{ Booking : "is_reserved_in"
    Booking ||--o| Payment : "generates"
    
    User {
        int UserId PK
        string Email
        string PasswordHash
        string FirstName
        string LastName
        string PhoneNumber
        string Role "Customer/Admin/Staff"
        bool IsActive
        datetime CreatedDate
    }

    Hotel {
        int HotelId PK
        string Name
        string City
        string Country
        string Address
        string Description
        decimal StarRating
        string ImageUrl
        bool IsActive
    }

    RoomType {
        int RoomTypeId PK
        int HotelId FK
        string Name
        string Description
        decimal BasePrice
        int Capacity
        string ImageUrl
    }

    Room {
        int RoomId PK
        int RoomTypeId FK
        string RoomNumber
        int Floor
        string Status "Available/Occupied/Maintenance"
    }

    Booking {
        int BookingId PK
        int UserId FK
        int RoomId FK
        datetime CheckInDate
        datetime CheckOutDate
        int GuestCount
        decimal TotalPrice
        string Status "Pending/Confirmed/CheckedIn/CheckedOut/Cancelled"
        string SpecialRequests
        datetime CreatedDate
    }

    Review {
        int ReviewId PK
        int HotelId FK
        int UserId FK
        int BookingId FK
        int Rating
        string Comment
        datetime CreatedDate
    }

    Payment {
        int PaymentId PK
        int BookingId FK
        decimal Amount
        string Method
        string Status
        datetime PaymentDate
    }

    Amenity {
        int AmenityId PK
        string Name
        string Icon
    }

    RoomAmenity {
        int RoomTypeId FK
        int AmenityId FK
    }
```

---

## 3. User Flow: Booking Process

```mermaid
sequenceDiagram
    participant Guest
    participant Frontend
    participant Backend
    participant Database

    Guest->>Frontend: Enter search criteria (City, Dates, Guests)
    Frontend->>Backend: GET /Hotel/Search
    Backend->>Database: Query available hotels & rooms
    Database-->>Backend: Return matching hotels
    Backend-->>Frontend: Display search results
    Frontend-->>Guest: Show hotel cards

    Guest->>Frontend: Click on a hotel
    Frontend->>Backend: GET /Hotel/Details/{id}
    Backend->>Database: Get hotel info, rooms, reviews
    Database-->>Backend: Return hotel data
    Backend-->>Frontend: Render hotel details page
    Frontend-->>Guest: Show hotel page with room options

    Guest->>Frontend: Click "Reserve" on a room
    Frontend->>Backend: Check if user is logged in
    
    alt Not Logged In
        Backend-->>Frontend: Redirect to login
        Guest->>Frontend: Login/Register
        Frontend->>Backend: POST /User/Login
        Backend-->>Frontend: Auth success, redirect back
    end

    Frontend->>Backend: GET /Booking/Create?roomId=X
    Backend->>Database: Get room details & pricing
    Database-->>Backend: Room info
    Backend-->>Frontend: Show booking form
    Frontend-->>Guest: Display booking summary

    Guest->>Frontend: Confirm booking
    Frontend->>Backend: POST /Booking/Create
    Backend->>Database: Check availability again
    Database-->>Backend: Room available
    Backend->>Database: INSERT Booking (Status: Pending)
    Database-->>Backend: BookingId
    Backend-->>Frontend: Redirect to confirmation
    Frontend-->>Guest: Show confirmation & receipt
```

---

## 4. Admin Flow: Booking Management

```mermaid
flowchart LR
    subgraph "Booking Lifecycle"
        A[Pending] --> B[Confirmed]
        B --> C[Checked In]
        C --> D[Checked Out]
        
        A --> E[Cancelled]
        B --> E
    end

    subgraph "Admin Actions"
        AdminConfirm[Confirm Booking] --> B
        AdminCheckIn[Check In Guest] --> C
        AdminCheckOut[Check Out Guest] --> D
        AdminCancel[Cancel Booking] --> E
    end

    subgraph "Room Status Changes"
        C --> RoomOccupied[Room: Occupied]
        D --> RoomAvailable[Room: Available]
        E --> RoomAvailable
    end
```

---

## 5. System Architecture

```mermaid
flowchart TD
    subgraph "Presentation Layer"
        Views[Razor Views]
        CSS[Tailwind CSS + DaisyUI]
    end

    subgraph "Application Layer"
        Controllers[ASP.NET MVC Controllers]
        ViewModels[ViewModels]
    end

    subgraph "Business Layer"
        Services[Services]
        UserService[UserService]
        HotelService[HotelService]
        BookingService[BookingService]
        RoomService[RoomService]
    end

    subgraph "Data Access Layer"
        Repositories[Repositories]
        DbContext[HotelDbContext]
    end

    subgraph "Database"
        SQLite[(SQLite Database)]
    end

    Views --> Controllers
    CSS --> Views
    Controllers --> ViewModels
    Controllers --> Services
    Services --> Repositories
    Repositories --> DbContext
    DbContext --> SQLite
```

---

## Technology Stack

| Layer | Technology |
|-------|------------|
| Frontend | Razor Views, Tailwind CSS, DaisyUI |
| Backend | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core |
| Database | SQLite |
| Authentication | Cookie-based (Claims) |
| Architecture | Clean Architecture (3-Layer) |
