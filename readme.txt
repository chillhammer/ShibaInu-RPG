Team Members:

Andrew Hoyt, andrewhoyt@gatech.edu, ahoyt9
Brandon Shockley, bshockley3@gatech.edu, bshockley3
Joseph Gonzales-Dones
Rosemary Blair, rosieblair@gatech.edu, rblair8
Xueyu Wang, xueyuw@gatech.edu, xwang970

Gameplay Instructions:

Controls
- WASD to move
- Move mouse to pan camera
- Left click to attack
- Hold right click to lock on to faced enemy
- Space to jump

General Guide
- Traverse through the linear level
- Fight encountered enemies
- Unlock the gate to retrieve the sacred bone

Rubric Requirements:

3D Game Feel Game
- Clearly defined and achievable goal of retrieving sacred bone
- Success (getting the bone)/failure (dying) indicated with corresponding UI prompts
  - Win/die to see these
- Start menu
- Resettable through menu button upon winning or dying

Precursors to Fun Gameplay
- Goals communicated to player
  - Linear level giving clear indication of where to go
 - Instruction prompt detailing controls and goal
- Interesting choices for the player
  - Can avoid some enemies or choose to fight them
- Choice consequences
  - Fighting enemies could cause damage but rewards a health restoring pickup
  - Not fighting enemies gives no chance to restore health

3D Character with Real-Time Control
- Real-time control of the Shiba character
  - Uses root motion for ground translation
  - Uses non-root rotation + slight leaning animation for more responsive turning
- Camera follows player tightly
- Auditory feedback (TODO: Andrew is on this)
  - Animation events to trigger footsteps
  - Event-based system for triggering other sound effects
    - Hurt
    - Jump

3D World with Physics and Spatial Simulation
- Environment terrain made with Unity terrain tools
- Environment hazards placed within this terrain
- Simulated game world with physically interactable objects
  - Falling rocks in first jumping challenge (Triggered)
  - Falling tree after first jumping challenge (Triggered)
  - Swinging logs in second and third jumping challenges
  - Audio associated with environment triggers

Real-Time NPC Steering Behaviors / Artificial Intelligence
- Ninja enemy
  - Progress to the area with trees after the third jumping challenge to see it
  - Navmesh for environment navigation
  - Multiple behavior states
    - Idle when player out of range
    - Approach quickly when in range
    - Circle the player for a random duration of time
    - Run in and attack with a kick
    - Quickly retreat to a safe distance
  - Root motion for animation
- Slime enemy
  - Just faces player when in range and bounces up and down
  - Basic mecanim animation

Polish
- User interface
  - Start menu
  - In-game pause menu
    - Restart and quit options
- Environment acknowledgement of player
  - See triggered environment events above
- Environment hazards have sound effects
- Aesthetics
  - Consistent Shiba-Inu-related imagery
    - Shiba player
    - Ham health pickups
    - Ninja enemies (shares Japanese origin with Shiba)
    - Bone final goal
    - Some Shiba-themed text

Known Deficiencies/Bugs:
- Camera clips through terrain
- Not *all* interactions have corresponding sounds
- Ninja can sometimes be moved into the air, which it has no appropriate animations to respond to
- Health pickups (hams) sometimes float after dropping

External Resources:
Shiba: https://www.artstation.com/artwork/xzyvym
Ninja: https://sketchfab.com/3d-models/bigs2-ninja-game-ready-low-poly-547bdf45338d47b89edac73ad061c6dc
Gate: https://sketchfab.com/3d-models/metal-grate-bc71182bb3d546cf990d36c1f0cd09d1
Heart UI Icon: https://opengameart.org/content/health
Ninja Hit Sound: https://opengameart.org/content/37-hitspunches
Ninja Step Sound: https://opengameart.org/content/different-steps-on-wood-stone-leaves-gravel-and-mud
Ninja Whoosh Sound: https://opengameart.org/content/battle-sound-effects
Ambient Soundtrack: https://opengameart.org/content/creepy-forest-f
Skybox: https://assetstore.unity.com/packages/2d/textures-materials/sky/classic-skybox-24923
Environment: https://assetstore.unity.com/packages/3d/environments/nature-starter-kit-2-52977
TODO: Add the rest
Ham:
Ham Sound:
Spikey Bush:
Spikey Bush Sound:
Log Texture:
Rock Falling Sound:
Tree Falling Sound:

Team Work Division:

Andrew Hoyt
- Shiba player controls and animations

Brandon Shockley
- Ninja enemy AI and animation configuration
- Generic health system used by all agents
- Level design
- Sacred bone gate
- UI configuration and linkage to game state

Joseph Gonzales-Dones
- Slime enemy

Rosemary Blair
- Swinging log hazard
- Spikey bush hazard
- Health pickups

Xueyu Wang
- Environment sculpting
- Falling tree hazard
- Falling rocks hazard

Scenes of Interest:
TitleScreen
MainScene