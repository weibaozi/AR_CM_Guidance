# How to run the project
Unity:
1. Download Unity Hub: Go to the Unity website and download Unity Hub. This application manages your Unity Editor installations and projects.
2. Install the Unity Editor: Through Unity Hub, you can install versions of the Unity Editor. Select a version that is `2022.3.xx`. 
3. Clone the Project: Clone the project using git clone with the following command `git clone git@github.com:weibaozi/AR_CM_Guidance.git`. 
4. Open an Existing Project: open the project through Unity Hub by navigating to the 'Projects' tab and clicking on 'Add'. Then, select the project folder.
5. Select the Right Scene: Select the easy scene inside the project. Double-click it to open the scene.
6. Play the Scene: Click on the play button to play the scene. 
Arduino:
1. open the file in the root folder called AR_CM.ino in the Arduino IDE
2. Change the wifi name and password to your own wifi name and password
3. Upload the code to the Arduino board
4. Connect the Ground and 5V pins to the Arduino board and connect the yellow wire to the A0 pin

# Code Structure
The project uses two IDEs: Unity and Arduino. Most of the code was done on Unity

## Unity Scripts

### MyUtils
The utils script contains useful functions:
- Save result
- Calculate the dihedral angle between two vectors
- Find the plane of the path

#### Public Variables
- State of the game
- Guiding Points
- Next guiding point
- etc...

### Accuracy
Functions to calculate accuracy by finding the distance between the tooltip and the line that connects the nearest two points.

### ArduinoCmControl
It uses the signal sent from the Arduino Nano to bend the tooltip.

### BarController
Find the next guiding point in the tooltip coordinate and set the x-axis value to the red arrow.

### VerticalBarController
It controls the vertical bar which represents the distance between the tooltip and the next guiding point.

### Guiding
Set the virtual guiding continuum manipulator at the initial of the game.

### NewRadar
It contains the radar that simply finds the local coordinates of the next guiding point in the manipulator tool tip’s coordinates. Uses the x and y coordinates to move the dot in the radar. There is also a constraint for the dot to stay on the radar. It also controls the rotation arrow. It calculates the angle between the norm of the path plane and the tooltip.

### Timer
Calculate the time to finish the task.

### Tooltip Controller
Detect collision between the tooltip and the guiding point. Set guiding point deactivated after the collision.

### UDPListener
Listener to receive the signal from Arduino Nano.

### UIInteract
Interaction for the in-game menu like changing levels, turning on and off different guiding systems, and saving the result.

## Arduino Scripts
AR_CM.ino: The script read the signal from A0 and broadcast it to the UDP port 50000.