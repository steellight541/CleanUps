# 🧼 CleanUps App – Zero Waste Montenegro

## 🌍 Project Goals

The CleanUps App was developed in partnership with [Zero Waste Montenegro](https://cleanupmontenegro.me/en/), an environmental NGO focused on eliminating waste pollution and encouraging community action across Montenegro. This app provides a digital platform to organize, track, and visualize local cleanup efforts.

### Mission Alignment

Zero Waste Montenegro promotes:
- Zero waste methodologies and circular economy practices
- Guidance for municipalities to transition to sustainable systems
- Community engagement and behavioral change
- Alternatives to single-use products and landfill disposal

The CleanUps App aligns with this by giving volunteers and organizers the tools to coordinate, participate in, and track the impact of cleanup events.

---

## 🎯 What We Planned to Achieve

### Core Objectives

Based on client input, user research, and technical constraints, we chose to focus on delivering the following MVP features:

1. **🗓 Event Tracking & Attendance**
   - Users can view upcoming cleanups and register their attendance post-event.
   - Participation data is collected and stored.

2. **📍 Event History with Map**
   - Past events are shown on an interactive map.
   - Each event includes trash metrics, number of participants, and images.

3. **📦 Trash Collection Data**
   - Participants can enter trash weight, categorize it (plastic, glass, etc.), and upload images.

4. **📊 Personal Stats**
   - Users can view their own cleanup history, such as events attended and trash collected.

5. **📲 Push Notifications** *(Planned – see below)*

---

### 💡 Initial Vision vs Realistic Scope

The initial app vision included ambitious features like:
- Voting-based trash challenges (e.g., “weirdest trash”)
- Point systems and leaderboards
- Reward-based gamification

However, during development, we intentionally **de-scoped** these due to complexity and time constraints. Our focus shifted to:
- Building a functional and scalable foundation
- Delivering core value to users and the organization
- Ensuring that the app could be expanded in future iterations

---

## 🛠️ Technologies & Tools

To ensure performance, maintainability, and cross-platform compatibility, we selected:

- **.NET MAUI Blazor Hybrid** – cross-platform mobile + desktop
- **Microsoft SQL Server (Azure-hosted)** – cloud-hosted relational database
- **Entity Framework Core** – ORM for clean database interactions
- **Swagger/OpenAPI** – interactive API documentation and testing
- **Figma** – prototyping and user flow design
- **Jira** – user story planning and sprint management
- **GitHub** – version control, branches, collaboration

🔗 Useful Links:
- [🖌 Figma Design](https://www.figma.com/design/XeXlInrOn2fUAKroFSUgFS/App-Montenegro-Cleanup)
- [📋 User Stories (Jira)](https://bipcleanup.atlassian.net/jira/software/projects/SCRUM/boards/1/backlog)
- [🔗 Swagger API Docs](http://cleanups-api.mbuzinous.com/swagger/index.html)

---

## 📌 Features Not Yet Implemented

Despite our structured development approach, we couldn’t realize all planned features in time.

### 🔔 Push Notifications

While part of our MVP plans, push notifications were not implemented due to time constraints and the need to prioritize core functionality. However:

> ✅ Our architecture supports notification integration via .NET MAUI’s native capabilities and backend triggers—so this feature can be added in the next iteration with minimal restructuring.

---

### 🚫 Other Postponed Features

These features were originally scoped but deprioritized:

- **Gamification & Trash Challenges**
  - Submitting “weirdest trash” photos
  - Community voting and challenge scoring
  - Leaderboards and rewards

- **Event-Based Media Feeds**
  - Social updates, commenting, or post-event highlights

- **Real-Time Attendance Verification**
  - Role-call or check-in systems during events

Each was deferred to keep the MVP focused and achievable within the 10-week development window.

---

## ✅ Next Steps

- Finalize frontend integration (map, filters, profile page)
- Continue testing and refinement
- Deliver presentation in Madrid
- Hand over documentation and deployment notes
- Plan future feature expansion (notifications, gamification)

---

## 🙌 Team & Acknowledgments

This app was developed as part of an international student collaboration in Spring 2025. Huge thanks to:
- [Zero Waste Montenegro](https://www.instagram.com/zero.waste.montenegro/)
- Jan Willem (mentor)
- Mirza (client contact)
- The student team: Afrika, Bryan, Kevin, Nigel, Gorm

---
