# Application server API documentation

## Introduction
This API serves as the backbone of a microservices architecture, enabling interactions between various services, actions, and reactions. It consists of the following key components :

- **Rust router :** Routes incoming requests to the appropriate service over a Docker network.
- **Action service :** Handles the registration and monitoring of actions.
- **Reaction service :** Processes actions and executes corresponding reactions.
- **C# Service:** Manages database-related operations and communicates with `Action service` through sockets to know whenever a new area is created and update the datas.
- **About endpoint :** Provides metadata about all available services, actions, and reactions.

## Getting Started
### Prerequisites
- Docker and Docker Compose installed.
- Appropriate environment variables configured for services.
- JSON Web Tokens (JWT) for authenticated routes.

### Base URL
The API uses the following base URL for all routes : http://localhost:80, please be aware that it may vary if you deploy it on an online infrastructure such as on a virtual private server or so.


### Authentication
Most routes require a **Bearer Token** for authentication. Include the token in the `X-User-Token` header.

---

## API Endpoints

### Authentication Routes
| Route                  | Method | Description                              |
|------------------------|--------|------------------------------------------|
| `/auth/login`          | POST   | Authenticate a user and obtain a token. |
| `/auth/register`       | POST   | Register a new user.                    |
| `/auth/userinformation`| GET    | Retrieve user information.              |
| `/auth/forget-password`| POST   | Request password reset via email.       |
| `/auth/reset-password` | POST   | Reset password for authenticated users. |
| `/auth/change-password`| PUT    | Change the password of the user.        |
| `/auth/change-username`| PUT    | Change the username of the user.        |
| `/auth/google-login`   | POST   | Login using Google OAuth.               |
| `/auth/discord-login`  | POST   | Login using Discord OAuth.              |

---

### Area Management Routes
| Route                  | Method | Description                                      |
|------------------------|--------|--------------------------------------------------|
| `/area`                | GET    | List all available areas.                       |
| `/area/update_area`    | POST   | Update the name or state of an area.            |
| `/area/delete_areas`   | DELETE | Delete specified areas.                         |
| `/area/services/{state}` | GET  | List services based on the state (true/false).  |
| `/area/subscribe_service` | POST| Subscribe to a service in an area.              |
| `/area/unsubscribe_service` | POST| Unsubscribe from a service.                    |

---

### Action and Reaction Routes
| Route                      | Method | Description                                |
|----------------------------|--------|--------------------------------------------|
| `/area/addactions`         | POST   | Add a new action for a service.           |
| `/area/update_action`      | PUT    | Update the trigger config of an action.   |
| `/area/addreactions`       | POST   | Add a new reaction for a service.         |
| `/area/update_reaction`    | PUT    | Update the execution config of a reaction.|
| `/area/services/{serviceName}/actions_reactions` | GET | List actions and reactions for a service. |
| `/area/services/{serviceName}/action` | GET | List actions for a specific service.      |
| `/area/services/{serviceName}/reaction` | GET | List reactions for a specific service.    |

---

### OAuth Routes
| Route                         | Method | Description                              |
|-------------------------------|--------|------------------------------------------|
| `/oauth/{provider}/authorize` | POST   | Start the OAuth process for a provider. |
| `/oauth/{provider}/access_token` | POST | Retrieve the OAuth access token.        |

**Supported providers :**
- Dropbox
- Spotify
- GitHub

---

### About Route
| Route        | Method | Description                                      |
|--------------|--------|--------------------------------------------------|
| `/about.json`| GET    | Retrieve metadata about services, actions, and reactions.|

---

## Data Models
### Authentication Request
```json
{
  "Email": "example@app.com",
  "Password": "password"
}
```

### Action Model
```json
{
  "ServiceId": "guid",
  "Name": "ActionName",
  "TriggerConfig": "some-configuration"
}
```

### Reaction Model
```json
{
  "ServiceId": "guid",
  "ActionId": "guid",
  "ExecutionConfig": "some-configuration"
}
```

## Support
If you encounter any issues or need assistance, please contact the development team or refer to the documentation for specific service configurations.