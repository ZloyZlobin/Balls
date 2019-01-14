Implement an application using Unity3D (version 4+) in C#,

in which there will be generation of objects and their movement by predetermined 

trajectory towards the opposite side of the screen. First generated 

object starts its movement from the bottom of the screen and moves upward. The second object 

is generated at the top of the screen after a short period of time and moves downwards. And 

so on alternately objects are created from the bottom/top. The object must have 

three-dimensional representation of a sphere and a specific trajectory. For 

implementing the proposed 3 types of trajectories:

a) linear;

b) by the circumference;

b) sine.

A linear trajectory is the minimum necessary and sufficient for 

successful acceptance test task. Add the implementation of the circular movement 

and sinusoid is optional, increases the complexity of test tasks and 

increases its value.

The speed of all objects is single and uniform. The motion of objects in addition to 

the main trajectory must be the angle of deflection (i.e., the entire trajectory, regardless 

its type, must have a slope). The angle of deviation of the trajectory from vertical 

is calculated during the generation of the object, but must be generated 

so that the object reached the opposite side of the screen without going beyond its limits. When 

generating object based on the motion trajectory of the object (which is selected 

randomly) and speed (which should be set the same for all objects), 

need to find a “spawn point” and the angle of trajectory of the object so that 

he should not collide with any object on the playing field and easily reached 

the opposite edge of the screen. In this case, if a collision does occur, 

you should visualize it (paint red the colliding objects). The frequency of generation of the objects increases with

time. The initial speed setting of generation facilities and the increase of speed 

must be specified (for example on the prefab) and is easily edited before running 

app.

If you cannot find a position and angle for a smooth launch 

object, you should skip the spawn and display a counter for the total number 

passed spawns. Also you should display the total count 

running and colliding objects.

The application has not logical conclusion. Balls should generate more often with 

time, but it is necessary to provide a button to restart the process from the beginning.