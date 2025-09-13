
```markdown
# 📦 Inventory Management System API

The **Inventory Management System (IMS)** is a backend service built with **ASP.NET Core** and **PostgreSQL** to manage products, categories, suppliers, and stock levels.  
It provides a secure and scalable foundation for businesses to track inventory operations and is production-ready, leveraging **Docker**, **GitHub Actions CI/CD**, and deployment to **AWS ECS (Fargate)**.

---

## ✨ Features

### 🔑 Functional
- 👤 **Authentication** – JWT-based login and role support (Admin/User).
- 📦 **Product Management** – Create, update, delete, and fetch product records.
- 🏷️ **Category Management** – Organize products under categories.
- 🚚 **Supplier Management** – Manage supplier details linked to products.
- 📊 **Inventory Tracking** – Stock quantity updates, reorder thresholds.
- 📝 **Audit Logging** – Track user actions (create/update/delete) with IP & timestamp.

### ⚙️ Non-Functional
- ⚡ **Scalability** – Deployed on **ECS Fargate**, horizontally scalable.
- 🐳 **Portability** – Fully containerized with Docker & Docker Compose.
- 🔒 **Security** – Credentials & secrets managed in **AWS Secrets Manager**.
- 🧪 **Testability** – Unit tests with xUnit + EFCore InMemory.
- 📜 **Observability** – Integrated with **CloudWatch Logs** for monitoring.
- 🚀 **Automation** – CI/CD pipeline using GitHub Actions (build → test → deploy).

---

## 🛠 Tech Stack

| Layer             | Technology                               |
| ----------------- | ---------------------------------------- |
| **Backend**       | ASP.NET Core (.NET 8)                    |
| **Database**      | PostgreSQL                               |
| **Auth**          | JWT (JSON Web Tokens)                    |
| **Container**     | Docker, Docker Compose                   |
| **Infra**         | AWS ECS Fargate, AWS ECR, IAM            |
| **CI/CD**         | GitHub Actions                           |
| **Logs/Monitoring** | AWS CloudWatch Logs                    |
| **Testing**       | xUnit, FluentAssertions, EFCore.InMemory |

---

## 🏗 Architecture

```

+-------------------+        +--------------------+
\|   Frontend (TBD)  | <----> |  Inventory API     |
+-------------------+        |  (ASP.NET Core)    |
+--------------------+
\| Products           |
\| Categories         |
\| Suppliers          |
\| Users/Auth         |
+--------------------+
|
v
+----------------------+
\|   PostgreSQL DB      |
+----------------------+

+--------------------------------------------------------------+
\| Docker (local) / AWS ECS Fargate (production, auto-scaled)   |
+--------------------------------------------------------------+

````

- **ECS Service** runs tasks with new revisions auto-registered by pipeline.
- **ALB + HTTPS** fronts the API in production (via AWS Certificate Manager).
- **Logs** routed to `/ecs/inventory-api` in CloudWatch.

---

## ⚙️ Running the Project

### 1️⃣ Run Locally (without Docker)
```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run --project src/Inventory.API
````

API runs at: [http://localhost:5000](http://localhost:5000)

---

### 2️⃣ Run with Docker

```bash
# Build image
docker build -t inventory-api .

# Run container
docker run -d -p 8080:80 inventory-api
```

API runs at: [http://localhost:8080](http://localhost:8080)

---

### 3️⃣ Run with Docker Compose

For multi-service orchestration (API + DB):

```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "8080:80"
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      DB_HOST: db
      DB_NAME: inventory_db
      DB_USER: postgres
      DB_PASSWORD: postgres
    depends_on:
      - db

  db:
    image: postgres:15
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: inventory_db
    ports:
      - "5432:5432"
```

Run:

```bash
docker-compose up --build
```

---

## 🧪 Testing

Unit tests are written with **xUnit** + **FluentAssertions** and use **EFCore.InMemory** for test isolation.

```bash
# Run all tests
dotnet test
```

---

## 🔁 CI/CD Pipeline

Deployed with **GitHub Actions**:

1. Checkout code
2. Set up .NET SDK
3. Build & run tests
4. Authenticate with AWS ECR
5. Build Docker image
6. Push image to ECR (`:latest`)
7. Register ECS Task Definition (`container-definitions.json`)
8. Update ECS Service with `--force-new-deployment`

Logs available under CloudWatch log group:

```
/ecs/inventory-management-system-task-definition
```

---

## 📄 Future Improvements

* ✅ Add API documentation with Swagger/OpenAPI
* ✅ Integrate RDS PostgreSQL in AWS
* ✅ Add CI/CD stages for staging vs production
* ✅ Add autoscaling policies for ECS
* ✅ Frontend integration (React/Next.js) deployed to S3 + CloudFront

---

## 👤 Author

**Ruona Ethan Agadagba**
🔗 [meetruona.com](https://www.meetruona.com)
📧 [meetruona@gmail.com](mailto:meetruona@gmail.com)
💻 Software Engineer | DevOps | Cybersecurity Enthusiast

---

## 📜 License
free to use, modify, and distribute.

