📄 Product Requirements Document (PRD)
Cross-Platform CrossFit Timer Mobile App
1. Overview
   Product Name (working title)

WOD Timer Pro (can be changed later)

Product Type

Cross-platform mobile application (iOS + Android)

Product Description

A modern, visually clean, light-themed CrossFit timer application designed for athletes and coaches. The app enables users to quickly configure and run workouts using common CrossFit timing formats (EMOM, AMRAP, Tabata, Intervals, etc.), while offering a premium and intuitive user experience.

2. Objectives
   Primary Objectives
   Provide a fast and reliable CrossFit timer tool
   Deliver a visually appealing, modern light UI
   Minimize friction in starting workouts
   Support the most commonly used CrossFit formats
   Enable saving and reusing workouts
   Secondary Objectives
   Lay foundation for future coaching and group features
   Enable scalability (wearables, tablets, TV mode)
   Provide data for workout tracking and analytics
3. Target Audience
   Primary Users
   CrossFit athletes
   Functional fitness enthusiasts
   Personal trainers
   CrossFit coaches
   Secondary Users
   Gym owners
   Group fitness instructors
   Home workout users (HIIT, interval training)
4. User Problems

The app aims to solve the following problems:

Existing timer apps are too generic or cluttered
Slow setup before workouts
Poor visibility during training
Lack of structured CrossFit formats
No easy way to reuse workouts
Weak audio/visual feedback during workouts
5. Key Features (MVP)
   5.1 Timer Modes

The app must support the following timer types:

1. Stopwatch
   Start / Pause / Resume / Reset
   Lap tracking (optional in MVP)
2. Countdown Timer
   Custom duration input
   End-of-timer alert
3. EMOM (Every Minute on the Minute)
   Total rounds configuration
   Interval duration
   Audio cue at each round start
4. AMRAP (As Many Rounds As Possible)
   Set total workout time
   Countdown timer
   Optional round tracking
5. Tabata
   Work interval
   Rest interval
   Number of rounds
   Optional sets (future)
6. Interval Timer
   Work / Rest configuration
   Number of rounds
   Optional warm-up
7. For Time
   Stopwatch with optional time cap
   Finish trigger
   5.2 Workout Builder

Users must be able to:

Create a workout
Select timer type
Configure parameters
Add workout name
Save as template
Edit existing templates
Delete templates
Duplicate workouts
5.3 Workout Library
List of saved workouts
Search functionality
Filter by type
Favorites
Recently used workouts
5.4 Active Workout Screen

Must include:

Large central timer display
Current round indicator
Current phase (Work / Rest / Prep)
Next phase preview (optional)
Color-coded state
Controls:
Pause
Resume
Reset
Skip interval (optional)
5.5 Audio & Feedback
Countdown before start (3-2-1)
Sound alerts:
Start
Interval switch
Final seconds
Workout complete
Vibration support
Sound on/off toggle
5.6 Workout History (Basic)
Workout name
Date/time
Duration
Optional notes (future)
5.7 Settings
Sound on/off
Vibration on/off
Default countdown time
Screen always on toggle
Light theme (default)
6. Out of Scope (MVP)
   User authentication
   Cloud sync
   Social features
   Sharing workouts
   Wearables integration
   Advanced analytics
   Coach/group mode
7. User Flows
   7.1 Quick Start Workout
   User opens app
   Selects timer type (e.g., EMOM)
   Adjusts parameters
   Presses “Start”
   Countdown begins
   Workout runs
   7.2 Create & Save Workout
   User opens “Create Workout”
   Selects timer type
   Configures settings
   Names workout
   Saves template
   7.3 Run Saved Workout
   User opens “Library”
   Selects saved workout
   Presses “Start”
8. UI/UX Requirements
   8.1 Design Principles
   Light, clean, minimal interface
   Large typography for timers
   High contrast states
   Touch-friendly controls
   Minimal setup friction
   8.2 Visual Style
   Light background (white/off-white)
   Rounded UI components
   Soft shadows
   Modern typography
   Accent color usage
   8.3 State Colors
   Work: strong accent color (e.g., green/blue)
   Rest: calm color (e.g., light green)
   Prep: yellow/orange
   Finished: neutral or success state
9. Non-Functional Requirements
   Performance
   Timer must be precise and stable
   Smooth animations (60 FPS target)
   Fast app launch (<2 seconds)
   Reliability
   Timer must continue accurately in background
   No timer drift
   Usability
   Readable from distance
   Large touch targets
   Works during intense physical activity
   Compatibility
   iOS (latest 2 major versions)
   Android (API level coverage ~90%)
   Offline Support
   App fully functional offline
10. Technical Requirements
    10.1 Platform

Cross-platform mobile:

Preferred: .NET MAUI or Flutter
10.2 Architecture
Clean architecture
Separation of concerns:
UI layer
Business logic (timer engine)
Data layer
10.3 Storage
Local storage (SQLite or equivalent)
Store workouts and history
10.4 Core Modules
Timer Engine (critical module)
Workout Builder
Workout Storage
Audio/Haptics Service
Settings Manager
11. Risks & Mitigation
    Risk	Mitigation
    Timer inaccuracy in background	Use native timers / foreground services
    Poor UX during workouts	Test in real gym conditions
    Overcomplicated builder	Keep MVP simple
    Battery drain	Optimize timer updates
    Platform inconsistencies	Test both iOS & Android early
12. Success Metrics
    MVP Success Criteria
    User can start a workout in <10 seconds
    Timer remains accurate throughout session
    No crashes during workout
    Positive usability feedback
    Users reuse saved workouts
    Metrics to Track
    Daily active users
    Workout starts per user
    Template usage rate
    Session duration
    Retention (Day 1 / Day 7)
13. Timeline (High-Level)
    Phase 1 — Discovery (1–2 weeks)
    Requirements finalization
    Wireframes
    Tech decisions
    Phase 2 — Design (2–3 weeks)
    UI design
    Prototype
    Design system
    Phase 3 — Development (6–10 weeks)
    Core timer engine
    UI screens
    Workout builder
    Storage
    Audio/haptics
    Phase 4 — QA (2–3 weeks)
    Functional testing
    Device testing
    Performance testing
    Phase 5 — Release (1–2 weeks)
    Store preparation
    Beta testing
    Launch
14. Future Roadmap
    Phase 2
    Cloud sync
    User accounts
    Advanced workout builder
    Sharing workouts
    Phase 3
    Coach mode (group workouts)
    TV/tablet display
    Voice guidance
    Wearable integration
15. Open Questions
    Should workouts support nested structures in MVP?
    Do we need voice cues from the beginning?
    How important is coach/group mode early?
    Which platform is priority: iOS or Android first?
    Do we support landscape mode in MVP?