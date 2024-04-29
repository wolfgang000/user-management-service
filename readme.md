# Start the project
```
docker compose up
```

# Run migrations
```
make migrate
```

# Run Tests
```
make test
```

## URLs

Backend: `http://localhost:5154/`

DB: `postgres://test_user:test_password@localhost:8004/postgres_dev`

# Docs

## Create a New User
```
curl --location 'http://localhost:5154/api/users/' \
--header 'Content-Type: application/json' \
--data '{  
    "name": "Mike T",
    "birthdate": "1999-10-01"
}'
```

## Update User State
```
curl --location --request PATCH 'http://localhost:5154/api/users/1' \
--header 'Content-Type: application/json' \
--data '{
    "active": false
}'
```

## Delete User
```
curl --location --request DELETE 'http://localhost:5154/api/users/1'
```

## List All Active Users
```
curl --location 'http://localhost:5154/api/users/'
```

## Swagger page
```
http://localhost:5154/swagger/index.html
```