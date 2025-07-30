````md
# 🧾 Inventory Management System API

A scalable, production-ready **Inventory Management System** backend built with **.NET Core**, containerized with **Docker**, and deployed via **AWS ECS Fargate** using **GitHub Actions** for CI/CD.

This API enables businesses to manage product inventory, including product registration, stock updates, inventory tracking, and automated logging. Designed for extensibility and microservices integration.

---

## 🚀 Features

- 🔐 JWT-based authentication (optional extension)
- 📦 Product creation, update, deletion, and stock tracking
- 📊 Real-time inventory count per SKU
- 🧩 RESTful API design
- 🐳 Dockerized & cloud-native ready
- 🛠 Deployed with CI/CD (GitHub Actions → AWS ECS)

---

## 🧰 Tech Stack

| Layer         | Technology                     |
| ------------- | ------------------------------ |
| Backend       | .NET Core (C#)                 |
| Containerization | Docker, Docker Compose        |
| Cloud Infra   | AWS ECS (Fargate), AWS ECR      |
| CI/CD         | GitHub Actions                 |
| Logging       | AWS CloudWatch Logs            |

---

## 🖥 How to Run Locally

### ⚙️ Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker](https://www.docker.com/products/docker-desktop)

### 📦 Run with Docker

```bash
# Step 1: Build the Docker image
docker build -t inventory-api .

# Step 2: Run the container
docker run -d -p 8080:80 inventory-api
````

Visit: `http://localhost:8080`

---

### 🧩 Run with Docker Compose

You can also use Docker Compose for future scalability (e.g., attaching a database):

```yml
# docker-compose.yml
version: '3.8'

services:
  inventory-api:
    build: .
    ports:
      - "8080:80"
    environment:
      ASPNETCORE_ENVIRONMENT: Development
```

Then run:

```bash
docker-compose up --build
```

---

## ☁️ Deployment (CI/CD Pipeline)

This project uses GitHub Actions to deploy changes to AWS ECS automatically on every push to `main`.

### 📤 Pipeline Steps:

1. Checkout source code
2. Authenticate with AWS ECR
3. Build Docker image
4. Push to AWS ECR
5. Register new ECS Task Definition
6. Update ECS Service with latest task
7. Logs routed to CloudWatch (`/ecs/inventory-management-system-task-definition`)

---

## 🧪 Manual ECS Deployment (If Needed)

If the ECS service doesn’t auto-deploy, go to:

`ECS → Cluster → Services → Select service → Deploy → Force new deployment`

---

## 📄 CloudWatch Logging Setup

**Important**: Ensure this log group exists before deployment, or the task will fail.

```bash
aws logs create-log-group --log-group-name /ecs/inventory-management-system-task-definition
```

---

## 💡 Use Case

This backend serves as the core of an inventory management platform used by retail systems or internal ERP platforms to manage their stock levels, product catalog, and warehousing automation. It’s designed to be extended with:

* Order management microservice
* Role-based access controls

---


## 📁 Project Structure

```
├── .github/workflows/         # GitHub Actions workflow
├── container-definitions.json # ECS Task container config
├── Dockerfile                 # Docker image config
├── docker-compose.yml         # Local Docker multi-service runner
├── README.md
└── src/
    └── Inventory.API          # .NET Core API source code
```

---

## 🧠 Future Improvements
* Metrics + Prometheus exporter



## 📜 License

feel free to fork, build on it, or contribute.



## 👋 Author

Ruona Ethan 

Built with ❤️ in .NET and DevOps best practices.

