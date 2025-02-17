# **RabbitMQ Messaging System with .NET Core API and Worker Consumer**

## **Overview**
This project is a **.NET Core API** that follows **Clean Architecture** principles to provide a scalable and maintainable structure for handling message-based communication using **RabbitMQ**. The API exposes endpoints that publish messages to a **RabbitMQ queue**, while a separate **.NET Worker Service** acts as a consumer, processing the messages asynchronously. The system also integrates with **PostgreSQL** for persistent storage.

---

## **Tech Stack**
- **.NET Core API** (C#)
- **RabbitMQ** (as the message broker)
- **PostgreSQL** (as the database)
- **Entity Framework Core** (for ORM)
- **Worker Service** (as the background message consumer)
- **Docker & Docker Compose** (for local development and deployment)
- **Clean Architecture** (with layers: Application, Domain, Infrastructure, and Presentation)

---

## **Architecture Overview**
### **1. API Layer (Presentation)**
- Exposes RESTful endpoints for clients to send requests.
- Publishes messages to a **RabbitMQ exchange/queue** using the **RabbitMQ.Client** library.
- Stores relevant data in **PostgreSQL**.

### **2. Application Layer**
- Contains the use cases and business logic.
- Uses **Dependency Injection (DI)** to inject services.

### **3. Domain Layer**
- Defines **Entities, Value Objects, and Domain Events**.

### **4. Infrastructure Layer**
- Handles external dependencies such as **RabbitMQ, PostgreSQL, and EF Core**.
- Implements **Message Publisher** and **Consumer** logic.

### **5. Worker Service (Consumer)**
- Listens for messages from **RabbitMQ**.
- Processes the messages and performs database operations.
- Uses a **BackgroundService** in .NET to run continuously.

---

## **Workflow**
1. A client sends a request to the **.NET Core API**.
2. The API **publishes a message** to a **RabbitMQ queue**.
3. The **Worker Service** subscribes to the queue and **consumes messages**.
4. The worker processes the message and **stores data** in **PostgreSQL**.

---

## **Use Case Example**
**Scenario**: A user submits an order through the API.

1. The API exposes a `POST /api/orders` endpoint.
2. When an order is received, it is **stored in PostgreSQL** and a message (`OrderCreatedEvent`) is **published to RabbitMQ**.
3. The Worker Service **listens** for new `OrderCreatedEvent` messages and processes them (e.g., updating inventory, sending notifications, etc.).

---

## **Deployment**
- The project is **Dockerized**, allowing seamless deployment with **Docker Compose**.
- The RabbitMQ container is included for easy local testing.

---

## **Directory Structure**
