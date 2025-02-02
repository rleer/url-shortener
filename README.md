# URL Shortener

This is a simple URL Shortener built with **.NET 8** and **Entity Framework Core (EF Core)**. The application allows users to shorten URLs via a **single-page web interface** and stores the mappings in an **SQLite database**.

## Features
- **Shorten URLs** and generate unique short codes.
- **Redirect from short codes** to the original URLs.
- **Persistent storage** using SQLite.
- **Simple SPA frontend** using Pico.css.

## Requirements
- .NET 8 SDK ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- SQLite (installed with .NET dependencies, no manual installation required)

## Setup Instructions

### 1. Clone the Repository
```sh
git clone https://github.com/rleer/url-shortener.git
cd url-shortener
```

### 2. Install Dependencies
Ensure you have all required .NET dependencies:
```sh
dotnet restore
```

### 3. Apply Database Migrations
This will create an `urls.db` SQLite database and apply the necessary schema:
```sh
dotnet ef migrations add InitialCreate

dotnet ef database update
```

### 4. Run the Application
Start the server:
```sh
dotnet run
```

The application will be accessible at:
```
http://localhost:5062/
```

## API Endpoints
### **1. Shorten a URL**
**POST** `/shorten`
```json
{
  "url": "https://example.com"
}
```
**Response:**
```json
{
  "shortenedUrl": "http://localhost:5000/abc123"
}
```

### **2. Redirect to the Original URL**
**GET** `/{shortCode}`
- Redirects the user to the stored original URL.
- Returns `404 Not Found` if the code is not found in the database.

## Technologies Used
- **.NET 8** (Minimal API)
- **Entity Framework Core** (SQLite database)
- **Pico.css** (Lightweight CSS framework)
- **JavaScript Fetch API** (Frontend AJAX calls)

## Notes
- If you encounter issues with the database, try deleting `urls.db` and rerunning migrations:
  ```sh
  rm urls.db
  dotnet ef database update
  ```
- The frontend automatically updates the page with the shortened URL without refreshing.

## Future Enhancements
- Implement analytics (click tracking)
- Add user authentication for managing links
- Use a cloud database instead of SQLite

---
Now you can run and test the URL shortener! 🚀

