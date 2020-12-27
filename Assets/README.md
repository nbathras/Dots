# Dots

## Project Setup

Project is setup to use the Universal Render Pipeline.  This can be installed and configured by following the steps listed bellow:

1. Window -> Package Manager -> Search: Universal RP -> Install
2. Assets -> Create -> Rendering -> Universal Render Pipeline -> Pipeline Asset (Forward Renderer)
3. Delete the file `UniversalRenderPipelineAsset_Renderer` which was just created
4. Assets -> Create -> Rendering -> Universal Render Pipeline -> 2D Renderer (Experimental)
5. Click on `UniversalRenderPipelineAsset` and drag the newly created `New 2D Renderer Data` into the Renderer List
6. Edit -> Project Settings -> Graphics -> the dot in the far right of Scriptable Renderer Pipeline Settings -> Select `UniversalRenderPipelineAsset`

The project also uses an exteneral packaged call **TextMeshPro**.  This can be installed and configured by following the steps listed bellow:
1. Window -> Package Manager -> Search: Universal RP -> Install
2. Window -> TextMeshPro -> Import TMP Essential Resources


## File Structure

```
Dots
│   .gitignore (all unity files that should not be comitted) 
│
└───Assets
│   │   README.md (you are here)
│   │
│   └───Prefabs
│   │   │   pfCell.prefab (reference stored in Board in order to instantiate new objects)
│   │   │   pfDot.prefab (reference stored in Board in order to instantiate new objects)
│   │   │   pfEdge.prefab (reference stored in Board in order to instantiate new objects)
│   │
│   └───Rendering
│   │   │   2DRenderer.asset (URP renderer)
│   │   │   UniversalRenderPipelineAsset.asset (Scriptable Renderer Pipeline)
│   │
│   └───Scenes
│   │   │   boardScene.unity (Main Scene where everything is loaded)
│   │
│   └───Scripts (contains all scripts used by GameObjects)
│   │   │   ...
│   │
│   └───TextMesh Pro (Essential resources used by Text UI elements)
│   │   │   ...
│   │
│   └───Textures
│       │   Arrow.png (used in scoreUI)
│       │   White_1x1.png (used for all other gameobjects)
│       │   ...
│   
└───Packages
│   │   manifest.json (Unity Package Manager reads the project manifest so that it can compute a list of which packages to retrieve and load)
│   │   packages-lock.json (A lock file contains the results of the Package Manager’s dependency resolution for a project)
└───ProjectSettings (stores all project settings including audio, editor, graphics, etc )
    │   ...
```


## Code Structure

### **Board**

A board is a 2d-array of the BoardEntity (abstract class). The array is of size (# of cells wide * 2 + 1) by (# of cells tall * 2 + 1).  A BoardEntity can be one of three things:

<u>Cell (cCc)</u>

Contains the four edges attached to the cell and the four dots attached to the cell (currently no reason to store the dots, but does not cause any preformance issues, so included in case of future use).

Tracks which player captured the edge.  It is set to `null` when it is uncaptured.

<u>Dot (\*)</u>

Contains no code, but must be added to dot prefab because BoardEntity is an abstract class.

<u>Edge (---)</u>

Contains a default, highlight, and click color that can be set in the inspector.

Tracks which player captured the edge.  It is set to `null` when it is uncaptured.

And it also contains the two cells that are attached to the edge.

<u>Example Board Array:</u>
```
\* --- \* --- \* --- \*

| cCc  | cCc | cCc |

\* --- \* --- \* --- \*

| cCc  | cCc | cCc |

\* --- \* --- \* --- \*
```

<u>Board Creation:</u>

The board is created in a single pass.  Dots are on all even columns and rows.  Cells are on all odd columns and rows.  And edges are every where else.  Cells get there associated edge references and Edges get there associated cell references on a single Attach pass that preforms in O(n).

Note: Edges orientation (vertical or horizontal) is set at prefab Instantiation time depending on place in 2d array.

### **UI**

There are three main UI's.  The UI is communicated with through the unity Event System.  The listeners are set during in the Start()
method of each of the 

**MainMenu**: Used to input width and height of the board as well as the names of the players

**Score**: Displays the player name as well as there score and points to whichever players turn it is.  Only displayed when the game is being played.

**Winner**: Once the game is over, it displays the player with the highest score overtop of the completed board.

### GameManager

Stores all logic including game setup, next turn, scoring, game over, and restart.  All of these cases also emit a Unity Event which can be subscribed to by other componenets.  The associated events are listed in order as described previously: `OnGameSetupEvent`, `OnNextTurnEvent`, `OnAddScoreEvent`, `OnGameOverEvent`, and `OnGameRestartEvent`.

## Development Process

Unity Version: 2019.4.9f1

Editor: VsCode ([Configure with Unity Guide](https://code.visualstudio.com/docs/other/unity))

Version Controll: [GitHub/nbathras/dots](https://github.com/nbathras/Dots) (used through the vscode Source Control application and GitBash Windows Application)

Systems: 
* OS: Windows 10
* GPU: GTX 1080
* RAM: 16 GB

