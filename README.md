# Loyalty Project 🎁

A simple **loyalty rewards system** built with **ASP.NET Core (.NET 8)** and **PostgreSQL**.  
It allows members to register, verify via OTP, earn loyalty points from purchases, and redeem coupons based on tiers.

---

## 🚀 Features

- Member registration and OTP verification  
- JWT-based authentication  
- Earn loyalty points from purchases  
- View points balance  
- Redeem coupons (500 / 1000 point tiers)  
- API documentation via **Swagger**  
- Ready-to-use **Postman collection**  
- Demo **web UI** served from `wwwroot/`

---

## 🛠️ Tech Stack
- **Frontend:** HTML, CSS, JS
- **Backend:** ASP.NET Core (.NET 8)  
- **Database:** PostgreSQL  
- **ORM:** Entity Framework Core (Npgsql)  
- **Auth:** JWT Bearer Tokens  
- **Docs:** Swagger / OpenAPI  

---

## 📦 Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)  
- [PostgreSQL 14+](https://www.postgresql.org/download/)  
- [Postman](https://www.postman.com/downloads/)  

---

## ⚙️ Setup & Installation
### 1. Clone the repo
```bash
git clone https://github.com/your-username/Loyalty-Project.git
cd Loyalty-Project
```

### 2. Create a PostgreSQL database
```bash
createdb loyaltydb
psql -U postgres -d loyaltydb -f Database/schema_postgres.sql
```

### 3. Configure app settings
Edit appsettings.json (or use environment variables):
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=loyaltydb;Username=postgres;Password=YOURPASS"
},
"Jwt": {
  "Issuer": "LoyaltyApi",
  "Audience": "LoyaltyApiUsers",
  "Key": "your-very-long-secret-key"
}
```

### 4. Restore, build, run
```bash
dotnet restore
dotnet build
dotnet run
```

#### App will start at:
```bash
Demo UI: http://localhost:5000/
```

### 🔑 API Flow (A → Z)
**1. Register member**
POST /api/member/register
```
{ "mobileNumber": "9999999999", "name": "Alice" }
➡️ Response includes an OTP (for demo).
```

**2. Verify OTP**
POST /api/member/verify
```
{ "mobileNumber": "9999999999", "otpCode": "123456" }
➡️ Response includes a JWT token.
```

**3. Authorise**
Use the JWT in all protected endpoints:
Authorization: Bearer <your-token>

**4. Add points**
POST /api/points/add
```
{ "memberId": 1, "purchaseAmount": 1200, "description": "Order #A1" }
```

**5. Get balance**
GET /api/points/1

**6. Redeem coupon**
POST /api/coupons/redeem
```
{ "memberId": 1, "tierPoints": 500 }
```

---

### 🧪 Testing Options
✔️ Swagger UI
```Go to http://localhost:5000/swagger```

✔️ Postman 
[Postman Collection URL](https://www.postman.com/anubhav-02/loyalty-api-collection/collection/w1gpez9/loyalty-api-collection)
**Or**
```
- Import LoyaltyApi.postman_collection.json into Postman.
- Set baseUrl = http://localhost:5000
```

---
#### Made By Anubhav Kushwaha

