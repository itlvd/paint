# Project 2: Paint

# A. Introduction
Members: 
- 19127363 - Lê Văn Đông
- 20127004 - Huỳnh Minh Bảo
- 20127600 - Lưu Tuấn Quân

<br> Project Paint allows users to draw shapes, delete, undo, redo and save them as binary or as a picture (png).

# B. Completed Features:

## Technical details

- Design patterns: Singleton, Factory, Abstract factory, prototype
- Plugin architecture
- Delegate & event

## Core requirements (7 points)

- Basic graphic objects: Line, Rectangle, Ecllipse
1. Dynamically load all graphic objects that can be drawn from external DLL files -> <b>Lê Văn Đông</b>
2. The user can choose which object to draw (rectangle, ecllipse and line) -> <b>Lê Văn Đông</b>
3. The user can see the preview of the object they want to draw -> <b>Lê Văn Đông</b>
4. The user can finish the drawing preview and their change becomes permanent with previously drawn objects -> <b>Lê Văn Đông</b>
5. The list of drawn objects can be saved and loaded again for continuing later in BINARY format -> <b>Lưu Tuấn Quân</b>
6. Save and load all drawn objects as an image in png format (rasterization). -> <b>Lưu Tuấn Quân</b>

## Improvements:
- <b>Allow the user to change the color, pen width, stroke type (dash, dot, dash dot dot...) -> <b>Lê Văn Đông</b>
- Fill color by boundaries -> <b>Lê Văn Đông</b>
- Erase each object individually -> <b>Lê Văn Đông</b>
- Zooming -> <b>Huỳnh Minh Bảo</b>
- Undo (Ctrl Z), Redo (Ctrl Y) for each action -> <b>Huỳnh Minh Bảo</b>


# C. Suggested Score
- 19127363 - Lê Văn Đông    - 10
- 20127004 - Huỳnh Minh Bảo - 10
- 20127600 - Lưu Tuấn Quân  - 10