....................
To Run The Solution
....................
1)Open Project solution in Visual studio/VS code editor:

2)Set Planday.Schedule.Api as startup project and run it 

  It will open swagger page with below methods:

a)getAllShifts-This is build to show all the created shifts , this method is created for data verification during testing from Swagger.
b)getShiftById-This accepts shift id as input paramenter and returns shift details along with employee email id.
c)createOpenShift-This accepts shift model as input paramenter and creates a shift without assigning any employee id to it.
d)assignShiftToEmployee-This accepts shiftId , employeeId as input parameter and assign shift to employee based on certain conditions.
....................
To Run Test Cases
....................
3)Set Planday.Schedule.Test as startup project and run it.
4)Go to Test ->Test Explorer->Run All Tests
