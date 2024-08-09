# ANYbotics
Job listings
- https://jobs.lever.co/anybotics/9359a84a-22b2-4f85-a2dd-37ada28cce32
- https://jobs.lever.co/anybotics/1e3dbed0-3189-4950-a1bf-4f094c464e78

## Decision logs
I have created 2 files, one for each job listing. I then asked ChatGPT to propose an interview project for both.
I have defined milestones for the dashboard, the API and the agents.
I have created some chat-gpt prompts for various tasks.
I have setup the front-end application using prompt #4

Finish it by visiting: https://cloud.nx.app/connect/CgEFov5gbo

Angular material not installed correctly: https://stackoverflow.com/questions/66498407/add-angular-material-to-nx-workspace

npx nx g @angular/material:ng-add --project=anybotics-workforce
npx nx g ngrx-root-store --project=anybotics-workforce // alternatively ngrx-feature-store, https://andrewevans.dev/blog/2020-06-22-ngrx-feature-state-vs-root-state/
https://medium.com/@danilrabizo/nx-angular-monorepo-and-shared-ngrx-store-f34bb3bb2bb5

npx nx add @nx/cypress // https://nx.dev/nx-api/cypress, followed by the next command
npx nx g @nx/cypress:configuration --project=anybotics-workforce

because I was getting: npx nx e2e anybotics-workforce --verbose
   NX   Cannot find configuration for task anybotics-workforce:e2e

to run e2e tests you can simply do: npx nx e2e

TODO
I have setup the console application representing a bot using prompt
I have setup the back-end API using prompt

## Front-end

To showcase your skills during the interview for the Frontend Engineer position at ANYbotics, you should consider creating a project that aligns with their tech stack and highlights your expertise in Angular, user experience design, and real-time data handling. Here’s a suggestion for a project:

### Project: **Robotic Fleet Management Dashboard**

**Objective:** Create a web-based dashboard for managing and monitoring a fleet of mobile robots, demonstrating real-time data visualization, seamless user interactions, and integration with backend services.

**Key Features:**

1. **Dashboard Overview:**
   - Display a summary of all robots, including their status (active, idle, offline), location, battery levels, and recent activities.

2. **Robot Details Page:**
   - Provide detailed information for each robot, such as current tasks, health metrics, sensor data, and visual feed if applicable.

3. **Real-time Data Updates:**
   - Implement real-time updates for robot status and location using WebSockets or another real-time data protocol.

4. **Interactive Map:**
   - Integrate a map component (e.g., using Google Maps or Leaflet) to visualize the robots' locations and their paths.

5. **Task Assignment:**
   - Allow users to assign tasks to robots and track their progress.

6. **User Authentication:**
   - Implement a simple user authentication system to demonstrate secure access to the dashboard.

7. **Data Visualization:**
   - Use charts and graphs to visualize historical data and performance metrics of the robots.

8. **Responsive Design:**
   - Ensure the dashboard is fully responsive and works seamlessly on various devices (desktops, tablets, smartphones).

**Technical Stack:**

- **Frontend:** Angular v17, Angular Material, SCSS, TypeScript, RxJS
- **Testing:** Jest, Cypress
- **State Management:** NgRx or another state management library
- **Data Visualization:** D3.js or Chart.js
- **Real-time Communication:** WebSockets or Firebase
- **Mapping:** Google Maps API, Leaflet, or Babylon.js for 3D visualization
- **Backend:** A simple Node.js/Express server to simulate API responses (if needed)
- **CI/CD:** Set up a basic CI/CD pipeline using GitLab CI

**Presentation:**

- **Demo:** Prepare a live demo of your project, showcasing different features and interactions.
- **Code Walkthrough:** Be ready to walk through your code, explaining your design choices, how you structured the application, and how you handled challenges.
- **Documentation:** Provide comprehensive documentation, including a README file with setup instructions, a summary of the project, and a list of features.
- **Testing:** Highlight your unit and integration tests, demonstrating your commitment to code quality.

**Additional Tips:**

- **UX Focus:** Emphasize the user experience by showing how intuitive and seamless the interface is.
- **Performance:** Showcase any optimizations you’ve made for performance, especially for real-time data handling.
- **Best Practices:** Highlight your adherence to software engineering best practices, such as code modularity, maintainability, and use of design patterns.

By presenting this project, you can effectively demonstrate your skills in Angular, your ability to create complex, user-friendly interfaces, and your understanding of real-time data integration—all of which are highly relevant to the position at ANYbotics.

## Back-end

To showcase your skills effectively during the interview for the Senior Backend Engineer position at ANYbotics, consider creating a project that aligns with their focus on robotics, scalable backend systems, and seamless integration with customer systems. Here’s a comprehensive project idea:

### Project: Robotic Inspection Dashboard with Real-Time Data Processing

**Objective:**
Develop a web-based dashboard that integrates with a robotic system (simulated or real) to perform real-time data processing and inspection tasks. This project should demonstrate your ability to create scalable backend systems, APIs, and a user-friendly interface.

**Key Components:**

1. **Backend System:**
   - **API Development:** Build RESTful or gRPC APIs for interaction between the robot and the dashboard.
   - **Data Processing:** Implement real-time data processing from the robot (e.g., sensor data, image processing).
   - **Database Management:** Use a scalable database (e.g., PostgreSQL, MongoDB) to store inspection data and logs.

2. **Robot Simulation:**
   - **Simulated Environment:** Use a robot simulation platform (e.g., Gazebo, ROS) to mimic the robot’s inspection tasks.
   - **Sensor Integration:** Simulate sensor data (e.g., temperature, pressure, images) that the robot would collect during inspections.

3. **Web Dashboard:**
   - **User Interface:** Create a web-based dashboard using a frontend framework (e.g., React, Angular) to visualize real-time data and inspection results.
   - **User Authentication:** Implement user authentication and role-based access control.
   - **Interactive Maps:** Integrate maps to show the robot’s location and inspection path within the facility.

4. **Cloud Integration:**
   - **Cloud Services:** Integrate with cloud services (e.g., AWS, Azure) for scalable data storage and processing.
   - **Deployment:** Deploy the backend and frontend on cloud platforms to demonstrate scalability.

5. **Documentation and Testing:**
   - **Code Documentation:** Provide comprehensive documentation for your code and APIs.
   - **Automated Testing:** Implement unit and integration tests to ensure the reliability of the system.
   - **Simulation Testing:** Demonstrate the system using the robot simulation in different scenarios.

**Technologies to Use:**
- **Backend:** Python, Node.js, or C++ with frameworks like Flask, Express.js, or FastAPI.
- **APIs:** RESTful or gRPC APIs.
- **Database:** PostgreSQL, MongoDB.
- **Frontend:** React, Angular, or Vue.js.
- **Robot Simulation:** ROS (Robot Operating System), Gazebo.
- **Cloud Services:** AWS, Azure, or Google Cloud for deployment and storage.

**Presentation:**
- **Demo:** Prepare a live demo or a recorded video demonstrating the robot performing inspection tasks, data being processed in real-time, and results displayed on the dashboard.
- **Architecture Diagram:** Present an architecture diagram showing the interaction between different components.
- **Challenges and Solutions:** Discuss the challenges you faced during development and the solutions you implemented.
- **Future Enhancements:** Suggest potential improvements and additional features that could be added to the project.

This project will not only demonstrate your technical skills in backend development, API design, and cloud integration but also show your ability to work on complex systems relevant to ANYbotics’ focus on robotic solutions.