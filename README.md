# PhoneNumberRegister

REST API for registering phone numbers. Accepts a number in any format, normalizes it to E.164 and saves it to PostgreSQL.

---

## Getting Started

```bash
# 1. Copy .env file and fill in your password
cp .env.sample .env

# 2. Run
docker-compose up --build
```

API: `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

---

## Endpoint

### `POST /check_number`

Checks if a phone number exists and adds it if not found.

**Headers:**
```
Content-Type: application/json
```

---

## Test Requests

### ✅ Successfully added — 201

```json
POST http://localhost:5000/check_number

{
  "phoneNumber": "+380991234567"
}
```

Response:
```json
{
  "message": "Phone number added successfully."
}
```

---

### ❌ Number already exists — 409

```json
POST http://localhost:5000/check_number

{
  "phoneNumber": "+380991234567"
}
```

Response:
```json
{
  "message": "Phone number already exists."
}
```

---

### ❌ Invalid number — 400

```json
POST http://localhost:5000/check_number

{
  "phoneNumber": "abc123"
}
```

Response:
```json
{
  "errors": ["Invalid phone number format."]
}
```

---

### ❌ Empty field — 400

```json
POST http://localhost:5000/check_number

{
  "phoneNumber": ""
}
```

Response:
```json
{
  "errors": ["Phone number is required."]
}
```

---

## Accepted Phone Number Formats

All formats are normalized to E.164 before saving.

| Input                  | Saved as        |
|------------------------|-----------------|
| `+1 (650) 253-0000`    | `+16502530000`  |
| `6502530000`           | `+16502530000`  |
| `(650) 253-0000`       | `+16502530000`  |
| `650-253-0000`         | `+16502530000`  |
| `+1 650 253 0000`      | `+16502530000`  |
| `1 650 253 0000`       | `+16502530000`  |