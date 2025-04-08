# KirbStomp

This is our working branch for Sprint 4 functionality. 

All our project documentation can be found on our team Notion Webpage. This include tasks, meeting notes, and other project notes. Other documentations are in the Documentation folder.

### Project Documentation:
Notion Documentation and Task management: [KirbStomp Super Smash Notion](https://www.notion.so/Temp-Smash-Name-18b0992c88eb80bf8935dc57b3a3be01)

[Sprint 4 Specific Page](https://possible-aletopelta-198.notion.site/Sprint-4-1b90992c88eb8078b9c8d93d9757e40a?pvs=4)

**Please read Notion Documentation on [bugs with sound](https://possible-aletopelta-198.notion.site/Work-Around-Sound-Build-Errors-1cc0992c88eb8074a6f7e6e5d49031f8?pvs=4)** and do the following before you do dotnet run (if you are using Visual Studio Builds to run, then try to follow steps in Notion, but this works better)
````
git restore Content/
git clean -f -d
dotnet run
````

Note: All tasks are listed in Task Universe page and also in the bottom of Sprint 4 page

All Documentation files listed below are in the Documentation folder:
* KirbStomp Sprint Requirements.pdf - this list all the our game plan to meet each sprint requirement
* KirbStomp Sprint 4 Initial Planning.pdf - this lists our Sprint4 initial task planning
* KirbStomp Code Analysis.pdf - this contains our code metrics data obtained from Visual Studio's code analysis tool
* KirbStomp Sprint 4 Reflection.pdf - this documents our team sprint4 overall progress reflection

### Runtime Action (More details in Notion)
* 3 scenes - start scene, battle scene, and end scene
* Switch scene with key 1, 2, 3 - end scene should be automatic when one character used up all its lives
* 2 Player game - 3 chracters but link and megaman is using the same controller
* Items are randomly spawn

### Scenes
* D1 - Start Scene
* D2 - Battle Scene w/ 3 characters (does not reset automatically)
* D3 - End Scene
* D4 - Pause / Resume
* D5 - Reset
  
### Controller
Mario 1 Character:
* WASD - up, left, down, right
* Y - Regular/aerial attack
* T - Special attack
* Space - jump

Mario 2 Character:
* PL;' - up, left, down, right
* Down arrow - Regular/aerial attack
* Left arrow - Special attack
* Up arrow - jump

Camera:
* UHJK - up, left, down, right
* M - reset
* -/+ - zoom out/in (hold) 

### Code Reviews

* The code reviews are commented under a pull request
* Pull requests with code review is tagged CR in the closed pull request (Go to Pull request tabs -> select closed pull requests -> select CR label)

### Known Bugs
* Pause mechanism only work with Battle Scene
* Pause function is a little glitchy - you might need to wait at least a second or press the key a couple time to pause or resume
* Sometimes the arrows stop right before it hits the ground
* Please read notes in notion about music
