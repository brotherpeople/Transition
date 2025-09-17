# Smartphone Tutorial Game for Seniors

## Overview
A Unity-based educational game designed to teach elderly users essential smartphone gestures through interactive step-by-step tutorials. Each level introduces a fundamental touch interaction, making smartphone technology more accessible and less intimidating for seniors.

## How to install
You can easily download *FirstTouch.apk* and install it. Don't worry I didn't put any virus in it. 

## Game Levels
The game progresses through seven core smartphone gestures:
- **Level 1: Click** - Basic tap interaction
- **Level 2: Drag & Drop** - Moving objects to target areas  
- **Level 3: Swipe** - Directional swipe gestures
- **Level 4: Collect** - Multi-target interaction through dragging
- **Level 5: Long Press** - Extended touch with visual feedback
- **Level 6: Double Tap** - Quick consecutive taps with timing validation
- more levels will be added
  
## Technical Implementation
Built with Unity's UI system and C# scripting, the game uses a modular approach with dedicated managers for game progression, level transitions, and user interface animations. The interaction system supports multiple gesture types through a unified drag handler that switches behavior based on level requirements.

## Core Scripts
- **GameManager**: Handles overall game state and progression
- **LevelManager**: Controls level-specific interactions and transitions
- **SimpleDrag**: Multi-purpose gesture handler supporting drag, swipe, collect, and pinch zoom
- **SimplePress**: Long press detection with animated feedback
- **SimpleDoubleTap**: Double tap recognition with timing constraints
