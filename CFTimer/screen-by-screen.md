CrossFit Timer App — Screen-by-Screen UI Specification
1. App Navigation Structure
   Bottom Navigation (MVP)

Tabs:

Home
Workouts
History
Settings

Floating Action Button (optional):

➕ Create Workout
2. Splash Screen
   Purpose

Brand + fast load

UI
Center: Logo
Background: light gradient (white → soft gray)
Subtle animation (fade / scale)
Duration
1–2 seconds max
3. Onboarding (Optional MVP Lite)
   Screens (2–3 slides max)
   Screen 1
   Title: “Train Smarter”
   Subtitle: “Powerful CrossFit timers in one app”
   Illustration
   Screen 2
   Title: “Fast & Simple”
   Subtitle: “Start any workout in seconds”
   Screen 3
   Toggle options:
   Sound ON/OFF
   Vibration ON/OFF
   CTA
   “Get Started”
4. Home Screen
   Purpose

Fast entry point (Quick Start)

Layout
Header
Greeting (optional)
Small title: “Quick Start”
Section: Timer Modes (Grid)

Cards (2 columns):

EMOM
AMRAP
Tabata
Intervals
Countdown
Stopwatch

Each card:

Icon
Title
Light background
Subtle shadow
Tap → opens setup screen
Section: Recent Workouts

Horizontal scroll:

Card:

Workout name
Type (EMOM / AMRAP etc.)
Duration summary
“Start” button
Section: Favorites
Same layout as recent
Floating Button
➕ Create Workout
5. Timer Setup Screen (Generic)

This screen adapts based on selected timer type.

Layout
Header
Back button
Title: “EMOM Setup” (dynamic)
Optional: Save icon
Main Content (Scrollable)
Section: Parameters

Dynamic fields:

Example: EMOM
Total Rounds → stepper / input
Interval Duration → time picker
Prep Time → toggle + input
Example: Tabata
Work Time
Rest Time
Rounds
Sets (optional)
Section: Advanced Options

Collapsed by default

Sound toggle
Vibration toggle
Countdown before start
Rest between sets (if applicable)
Section: Save Workout
Input: Workout name
Button: “Save as Template”
Bottom Sticky Button
Start Workout (primary CTA)
6. Active Workout Screen (MOST IMPORTANT)
   Purpose

Core experience — must be extremely clean and readable

Layout
Fullscreen Mode (default)
Top Area
Workout name (small)
Current round / total rounds

Example:
Round 3 / 10

Center Area (MAIN FOCUS)
Large Timer
VERY LARGE font
Example: 12:45
Bold, centered
Phase Label
“WORK”
“REST”
“PREP”
Background Behavior

Changes based on state:

Work → soft green/blue
Rest → light green
Prep → warm yellow
Finish → neutral/light gray
Bottom Controls

Large buttons:

Pause / Resume
Reset
Skip (optional)
Secondary Info (small text)
Next phase: “Rest in 10s”
Total elapsed time
Interaction Behavior
Tap screen → show/hide controls
Long press → lock screen (prevent accidental taps)
Audio/Feedback
3-2-1 countdown
Beep on interval switch
Vibration on transitions
7. Workout Builder Screen
   Purpose

Create custom workouts

Layout
Header
Back
Title: “Create Workout”
Save button
Section: Basic Info
Workout Name (input)
Type selector (dropdown/cards)
Section: Structure
Simple Mode (MVP)
Just configure one timer
Advanced Mode (future)

Block list:

Card per block:

Type (EMOM / Rest / AMRAP)
Duration / rounds
Delete icon

Buttons:

➕ Add Block
Drag to reorder
Bottom
Save Workout
Start Workout
8. Workouts Screen (Library)
   Purpose

Manage saved workouts

Layout
Header
Title: “Workouts”
Search icon
Search Bar
Input: search workouts
Filters (chips)
All
EMOM
AMRAP
Tabata
Interval
Workout List

Card:

Name
Type
Summary (e.g. “10 rounds × 1 min”)
Favorite icon
Actions menu (⋯):
Edit
Duplicate
Delete

Tap card:

Open details
FAB
➕ Create Workout
9. Workout Details Screen
   Layout
   Header
   Back
   Title
   Content
   Workout name
   Type
   Parameters summary
   Buttons
   Start
   Edit
   Duplicate
10. History Screen
    Purpose

Track usage

Layout
Header
Title: “History”
List

Item:

Workout name
Date/time
Duration
Type

Optional:

Notes
Empty State
“No workouts yet”
CTA: “Start your first workout”
11. Settings Screen
    Layout
    Section: General
    Sound toggle
    Vibration toggle
    Countdown toggle
    Section: Display
    Keep screen awake
    Theme (Light / Dark future)
    Section: Timer Defaults
    Default prep time
    Default sound
    Section: About
    App version
    Privacy policy
    Contact/support
12. UI Component System
    Core Components
1. Timer Display
   Large font
   Monospaced digits
   Center aligned
2. Cards
   Rounded corners (12–16px)
   Soft shadow
   Light background
3. Buttons
   Primary
   Filled
   Accent color
   Secondary
   Outline / light fill
4. Inputs
   Minimal borders
   Clear labels
   Numeric/time input optimized
5. Chips (Filters)
   Rounded
   Selected state highlighted
13. Design Tokens
    Colors
    Background: #FFFFFF / #F7F7F7
    Primary: soft blue / green
    Success: green
    Warning: yellow/orange
    Danger: red (only for reset/stop)
    Typography
    Timer: very large (48–96px)
    Headers: bold
    Body: medium weight
    Spacing
    8pt grid system
14. Critical UX Rules

These are VERY important for this app:

1. Fast Start
   User must start workout in ≤3 taps
2. Readability
   Timer visible from distance (3–5 meters)
3. No Precision Required
   Large tap areas
   No small buttons
4. Feedback
   Every state change = sound + visual change
5. Reliability
   No UI glitches during active workout
15. Edge States
    Active Workout Edge Cases
    App goes to background → timer continues
    Screen locked → timer continues
    Phone call → resume correctly
    Empty States
    No workouts → show CTA
    No history → show guidance
    Error States
    Invalid input → inline validation
    Timer crash → safe fallback
16. Animation Guidelines
    Timer transitions: smooth (no jumps)
    Color transitions: soft fade
    Button press: subtle scale
    Countdown: animated numbers (optional)
17. Future UI Extensions
    Landscape mode (for gym screens)
    Tablet layout
    TV casting mode
    Coach dashboard