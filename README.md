# CSD3D Extension - Exit plans, Baked shadowmaps, Event system and more!

## 👥 Team Members
| Name | Email |
| :--- | :--- |
| Segkesser Dimitris | csd5006@csd.uoc.gr |
| Fouskis Michalis | csd5076@csd.uoc.gr |

## ℹ️ About

This Project is an extension of the original CSD3D project, with the following added functionality and improvements:

- Exit Plans: The user can now request evacuation navigation to the nearest exit in case of an emergency.

- Baked Shadowmaps: The lighting of the scene has been improved with the use of baked shadowmaps and lighting, enhancing the visual quality and realism of the building. This also comes with performance gains, since no realtime lighting is computed anymore.

- Department events and posters: Events can be added to the virtual department, which are automatically displayed in posters. The user can access a special menu (UI) to view all current events, as well as request navigation to the event location.

- Bonus (Requested by the Department): Update on the personnel information as well as room naming.

- Extra (Not requested explicitly): Corrections to the department mesh (like inverted faces), chairs with wheels have been separated from the building mesh and largely remeshed by hand for better visual quality, and baking behaviour. Also replaced poster model with hand-made one for better visuals.

## How to use the Extension

- Exit Plans: Use 'E' to access the main menu when in the simulation. You'll notice a 'Emergency Exit' button. Clicking this will guide you to the nearest exit. Clicking 'R' will refresh the exit to ensure you're always directed to the closest one (not automatic to avoid confusion during navigation).

The next image shows how you can access the exit plan feature from the main menu. The main menu can be opened with the 'E' key.
![Exit Plan 1](./Example%20Images/exit_plan1.png)

![Exit Plan 2](./Example%20Images/exit_plan2.png)

- Baked Shadowmaps: No user action needed. Just enjoy the improved lighting and performance!

![Baked Shadowmaps and Lighting](./Example%20Images/shadowmap1.png)

![Baked Shadowmaps and Lighting](./Example%20Images/shadowmap2.png)

- Department Events and Posters: You can access the events menu by pressing the 'E' key to open the main menu, then clicking on the 'Set Destination' button. This will display a list of current events in the department. You can click on any event to get navigation assistance to its location. Banners in the department will also display current events.

![Events](./Example%20Images/events1.png)
![Events](./Example%20Images/events2.png)
![Events](./Example%20Images/events3.png)

## Note

In order for the csv files to be read correctly, they must be available in the main CSD3D repository inside the "Excel Files" folder. At the time of writing this README, this branch has not been merged with the main CSD3D repository, so the files must be copied manually. Once merged, this step will no longer be necessary.

## For future development

If you ever need to modify the current structure of the CSV files, or add new ones (like in the case of the [images extension](./Excel%20Files/IDToImage.csv)), you can use the the local CSVs in the "Excel Files" folder. For this purpose this extension added a new option to CSV-fetching scripts (namely: 'Is Local URL'), which allows you to specify whether the CSV file should be fetched from the local "Excel Files" folder, or from GitHub. That way, you can easily test changes to the config CSV files without affecting the main ones until you're ready to merge.

> __Useful debugging TIP__: After a painful debugging session, you may find that the original implementation doesn't work well without an internet connection, resulting in UI elements such as timetables remaining stuck in a visible state on game start. If you ever encounter such an issue, ensure you have a working internet connection when starting the game, as the original implementation relies on fetching CSV files from GitHub. This is a crucial step to ensure all UI elements function correctly.

## Also see

[Images Extension](./Excel%20Files/ImageGuide.md) - A config file that allows adding images to the different physical room descriptions in the department, without modifying the codebase.