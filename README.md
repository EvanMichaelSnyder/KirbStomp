# KirbStomp

This is our working branch for Sprint 3 functionality. 

All our project documentation can be found on our team Notion Webpage. This include tasks, meeting notes, and other project notes. Other documentations are in the Documentation folder.

### Project Documentation:
Notion Documentation and Task management: [KirbStomp Super Smash Notion](https://www.notion.so/Temp-Smash-Name-18b0992c88eb80bf8935dc57b3a3be01)

The [Sprint 3 Specific Page](https://possible-aletopelta-198.notion.site/Sprint-3-19d0992c88eb80e4b850cd00e3a972f2?pvs=4) includes sample collision demo video with hitboxes.

Note: All tasks are listed in Task Universe page

All Documentation files listed below are in the Documentation folder:
* KirbStomp Sprint Requirements.pdf - this list all the our game plan to meet each sprint requirement
* KirbStomp Sprint 3 Initial Planning.pdf - this lists our Sprint3 initial task planning. NOTE: this was done on time, but there was a merge issue that messed up our branch (Please check this [pull request](https://github.com/StaticYolt/KirbStomp/pull/86))
* KirbStomp Code Analysis.pdf - this contains our code metrics data obtained from Visual Studio's code analysis tool
* KirbStomp Sprint 3 Reflection.pdf - this documents our team sprint3 overall progress reflection

### Runtime Action
* Character, platform block, items have collision with each other
* Both Mario can attack each other and result in damage collision
* Arrow and fireball hurt Mario
* Hitting the arrow will sprawn arrows
* Collision with hamburger will remove it from the screen

### Scenes
* D1 - Test Scene w/o items or attack collision but w/ platform collision
* D2 - Battle Scene w/ all items and collision
* D3 - Reset Current Scene to initial conditions
  
### Controller
Mario 1 Character:
* WASD - up, left, down, right
* Y - Regular/aerial attack
* T - Special attack
* Space - jump

Mario 2 Character:
* PL;' - up, left, down, right
* Down - Regular/aerial attack
* Left - Special attack
* Right Shift - jump

Items:
* Fireball - 0 key, move mouse to control placement
* Arrow - 9 key, move mouse to control placement

Camera:
* UHJK - up, left, down, right
* M - reset
* -/+ - zoom out/in (hold) 

### Code Reviews

* The code reviews are commented under a pull request
* Pull requests with code review is tagged CR in the closed pull request (Go to Pull request tabs -> select closed pull requests -> select CR label)
* NOTE: most code reviews for the team were done verbally when the team met, so the timestamp on the pull request comment may not accurately represent the date the CR was done. We will make sure to document them on the right date for the next sprint (based on the feedback we got back from Sprint 2).

### Known Bugs
* Attack hitboxes are too precise, so collision damage only occurs when characters are very close
* Collision does not work if Key 1 is pressed
