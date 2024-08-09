# MVP 1

### Dashboard “ANYbotics WorkForce”

- **No login/registration, single user**
- Shows a list of the available agents alongside a summary. The agent summary includes:
  - Agent name
  - ID
  - Battery level
  - Status (unavailable / offline / idle / active)
- **Search** for an agent based on name and/or ID.
- By clicking on an agent, its details can be seen on a separate view. All information mentioned above is displayed, plus:
  - Current command being performed
  - ETA
  - Camera view (stub image)
  - Details from section **Agents “ANYmal”** (see below) are displayed
- **User can send a command** and see the commands history from the details view. Only one command at a time can be sent.
  - Command history includes command name, timestamp, and completion time.
- **Status history** shows the history of the statuses of the agent.

### Agents “ANYmal”

- **ID**
- **Name**
- **Status** (unavailable / offline / idle / active)
- **CurrentCommand** // the current command being executed
- **BatteryLevel**
- **Model** (ANYmal, ANYmal X)
- **FirmwareVersion**
- **FirmwareLastUpdated**
- **ManualModeOn**
- **Location** (x, y, z)
- **TrekkerVersion** // AI-based robot control software for ANYmal. Trekker now processes real-time input from ANYmal’s depth cameras, lidar, and terrain map. By intelligently walking with all of its senses, ANYmal can safely and efficiently move in even more challenging environments, such as climbing higher steps, dual-orientation stair climbing, and staying clear of high edges.
- **TrekkerLastUpdated**
- **Hardware** (each unit has a running, failed, anomaly_detected status)
  - Temperature sensor
  - Pressure sensor
  - Leg1Status, Leg2Status, Leg3Status, Leg4Status
  - GPS
  - Engine
  - Battery
  - Lidar scanner
  - WI-FI
  - 4G LTE
  - CPU_1, CPU_2
  - 6 depth cameras
  - 2 optical tele-operation cameras (20x)
  - Thermal camera (-40–550°C)
  - Pan-tilt-unit // high-range motion of the payload (+/- 90° vertical, +/- 165° horizontal)
  - Spotlight (maximal 3790lm supports visual inspections in the dark)
  - Ultrasonic microphone // record acoustic measurements in audible and ultrasonic frequencies (0—384kHz)

### Statuses 

- **Unavailable**: Battery level is 0; therefore, the agent cannot be awoken up.
- **Offline**: The agent is not reachable and cannot accept commands. The agent goes offline when the shutdown command is received (battery level is > 0 though!).
- **Idle**: The agent is reachable and can accept commands as it’s not busy executing any command.
- **Active**: The agent is reachable but cannot accept commands until the current command is completed.

When the agent is shut down, its status becomes offline. When it receives the wake-up command, the status is online.

- Every “x” amount of time battery level decreases. When it reaches 0, the agent goes unavailable. Before going unavailable, it sends a message to the main API.
- When the agent wakes up, it sends a signal to the main API. It’s ready to accept commands. Each command has an associated battery level cost that is subtracted at completion time. When performing the activity, the status is set to “active”.
- Upon completion, a message is sent to the main API.

### List of Available Commands

- `shutdown`
- `wakeup`
- `recharge`
- `thermal inspection`
- `combustible inspection`
- `gas inspection`
- `acoustic measure`
- `goto room “x”`
- `take image` (data includes timestamp and location)
- `zoom_in / zoom_out`
- `start_video_recording`
- `stop_video_recording`
- `take_3d_scan` (create_digital_twin)
- `update_firmware`
- `update_trekker`
- `run_diagnostic_check`
- `change_log_level`
- `define_region_interest`
- `set_manual_mode_on` // after that you can issue commands like: crawl, go_left, go_right, go_forward, go_backward
- `set_manual_mode_off` (previous commands cannot be executed)
- `set_temperature_ranges` (min.-max. settings by area and/or mission)
- `set_gas_level_thresholds` // setting preconfigured gas-level thresholds enables operators to take early precautionary measures to the presence of combustible and toxic gases. The gas sensor suite ensures wide coverage as a single robot can monitor large-scale sites through the scheduling of automated, routine mobile sensing.
  - **Combustible gases – LEL** (Lower Explosive Limit)
    - Ethylene C2H4
    - Methane CH4
    - Propane C3H8
  - **Toxic gases – ppm** (parts per million)
    - Ammonia NH3
    - Carbon Monoxide CO
    - Chlorine Cl2
    - Hydrogen H2
    - Hydrogen Sulphide H2S
    - Oxygen O2
    - Nitric Oxide NO
    - Nitrogen Dioxide NO2
    - Sulphur Dioxide SO2

### Failures and Anomalies Simulation

- Every “x” amount of time the agent detects equipment failures, then performs maintenance.
- Every “y” amount of time the agent identifies a thermal anomaly (room, equipment_id).
- Every “z” amount of time the agent identifies an acoustic anomaly (room, equipment_id).
- Every “k” amount of time the agent identifies gas/combustible (room, equipment_id).

**Comms** to be implemented using the gRPC protocol.

**Responsive design** using Angular Material framework.

# MVP 2

- **Fleet** (= display agents with different colors in the dashboard), search by fleet.
- Fleet-level commands (only affect agents of a given fleet).
- Global-level commands.
- Agent-level commands. Fleet ID becomes part of an agent. 
- Dashboard should allow a breakdown of agents based on fleet.

Introduce the concept of **“mission”**, each mission can be performed via a command. The UI also allows defining missions. A mission is a collection of commands. A mission can be scheduled.

# MVP 3

- Multiple users (admin user can add/remove/update user details. When the user is created, it can access the platform. No authentication for now. Single admin user).
- A user is assigned a fleet (this does not change) by the admin.
- There is an admin page reachable only by admin users. Admin API is protected (normal users receive a 403 when they try to execute admin endpoints).
- The admin user is not associated with any fleet and cannot perform actions.
- Users can be deactivated/activated. When deactivated, they can no longer log in to the platform. If logged in, they are forced to log out. Re-activated users can log in again.

# MVP 4

- Users can be assigned a different fleet.
- Authentication using JWT.
- Guest users.

# MVP 5

- Display 2D/3D position on a map.
- Introduce map component and paths.
- Inspection data and logs.